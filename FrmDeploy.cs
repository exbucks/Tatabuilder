using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder
{

    public partial class FrmDeploy : Form
    {

        #region External DLL

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int SetWindowTheme(IntPtr hWnd, string appName, string partList);

        #endregion

        const int       VIEWER_UDP_PORT             = 4863;
        const int       VIEWER_TCP_PORT             = 4864;
        const string    VIEWER_MSG_HELLO            = "Hello, TataViewer!";

        public TDocument document { get; set; }

        private Thread task = null;
        private long binarySize;
        private long totalSent;
        private byte[] response = new byte[1024];
        private int totalReceived;

        private Socket tcpClient;
        private AutoResetEvent tcpConnectDone = new AutoResetEvent(false);
        private ManualResetEvent tcpSendDone = new ManualResetEvent(false);
        private ManualResetEvent tcpReceiveDone = new ManualResetEvent(false);
        private bool tcpConnected = false;

        private UdpClient udpClient;

        public FrmDeploy() 
        {
            InitializeComponent();

            SetWindowTheme(lvwDevices.Handle, "explorer", null);
        }

        private void FrmDeploy_Load(object sender, EventArgs e) 
        {
            prgStatus.Value = 0;

            // udp client to get device list
            udpClient = new UdpClient(0);
            udpClient.BeginReceive(new AsyncCallback(listenDevices), null);

            // send broadcast to get device list
            refreshDevices();
        }

        private void FrmDeploy_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (task != null && task.IsAlive) {
                // Request that oThread be stopped
                task.Abort();

                // Wait until oThread finishes. Join also has overloads that take a millisecond interval or a TimeSpan object.
                task.Join();
            }

            if (udpClient != null) {
                udpClient.Close();
                udpClient = null;
            }
        }

        private void lvwDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = lvwDevices.SelectedIndices.Count > 0;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refreshDevices();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lvwDevices.Items.Count == 0) {
                MessageBox.Show("Any available devices could not be found.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (lvwDevices.SelectedItems.Count == 0) {
                MessageBox.Show("Please select the device you want to deploy.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try {
                // create tcp socket
                string address = lvwDevices.SelectedItems[0].SubItems[1].Text;
                IPEndPoint viewerEP = new IPEndPoint(IPAddress.Parse(address), VIEWER_TCP_PORT);
                tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.
                tcpConnected = false;
                tcpClient.BeginConnect(viewerEP, new AsyncCallback(tcpConnectCallback), tcpClient);
                tcpConnectDone.WaitOne();

                if (tcpConnected) {
                    btnOK.Enabled = false;
                    task = new Thread(() => deploy());
                    task.Start();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Could not deploy to device.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listenDevices(IAsyncResult res)
        {
            if (udpClient == null)
                return;

            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 0);

            try {
                byte[] received = udpClient.EndReceive(res, ref groupEP);
                string info = Encoding.UTF8.GetString(received);
                string address = groupEP.Address.ToString();

                lvwDevices.Invoke((Action)delegate {
                    int old = getItemIndexByAddress(address);
                    if (old != -1)
                        lvwDevices.Items.RemoveAt(old);

                    lvwDevices.Items.Add(new ListViewItem(new string[] { info, address }));
                    
                    if (lvwDevices.Items.Count == 1) {
                        lvwDevices.SelectedItems.Clear();
                        lvwDevices.SelectedIndices.Add(0);
                    }
                });

                udpClient.BeginReceive(new AsyncCallback(listenDevices), null);

            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void refreshDevices()
        {
            lvwDevices.Items.Clear();

            byte[] sendbuf = Encoding.UTF8.GetBytes(VIEWER_MSG_HELLO);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, VIEWER_UDP_PORT);
            udpClient.Send(sendbuf, sendbuf.Length, groupEP);
        }

        private int getItemIndexByAddress(string address)
        {
            foreach (ListViewItem item in lvwDevices.Items) {
                if (item.SubItems[1].Text.Equals(address))
                    return item.Index;
            }

            return -1;
        }

        private void deploy() 
        {
            //=================================== export package to temporary file ============================================
            string tempFile = Path.GetTempFileName();
            document.export(tempFile);

            //=================================== transfer info =============================================
            FileInfo fileInfo = new FileInfo(tempFile);
            binarySize = fileInfo.Length;
            totalSent = 0;

            byte[] identifierBytes = Encoding.UTF8.GetBytes(document.identifier);
            byte[] identifierLen = BitConverter.GetBytes(identifierBytes.Length);
            byte[] binarySizeBytes = BitConverter.GetBytes(binarySize);

            byte[] data = new byte[4 + 8 + identifierBytes.Length];
            identifierLen.CopyTo(data, 0);
            binarySizeBytes.CopyTo(data, 4);
            identifierBytes.CopyTo(data, 12);

            tcpClient.Send(data);

            //=================================== transfer binary to the device ===================================
            int readBytes = 0;
            int bufferSize = 1048576; // 1MB
            byte[] buffer = new byte[bufferSize];

            // Blocking read file and send to the clients asynchronously.
            using (FileStream stream = new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                do {
                    tcpSendDone.Reset();
                    stream.Flush();
                    readBytes = stream.Read(buffer, 0, bufferSize);

                    try {
                        tcpClient.BeginSend(buffer, 0, readBytes, SocketFlags.None, new AsyncCallback(tcpSendCallback), tcpClient);
                    } catch {
                        MessageBox.Show("Connection was lost. Please try again.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tcpClient = null;
                        return;
                    }

                    tcpSendDone.WaitOne();
                } while (readBytes > 0);
            }

            //=================================== receive response from the device ===================================
/*
            StateObject state = new StateObject();
            state.WorkSocket = tcpClient;

            tcpReceiveDone.Reset();
            totalReceived = 0;

            try {
                tcpClient.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(tcpReceiveCallback), state);
            } catch {
                if (!tcpClient.Connected) {
                    MessageBox.Show("Connection was lost. Please try again.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            tcpReceiveDone.WaitOne();
*/
            tcpClient.Shutdown(SocketShutdown.Both);
            tcpClient.Close();
            tcpClient = null;

            if (totalReceived > 0) {
                string msg = Encoding.UTF8.GetString(response);
                MessageBox.Show(msg, Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                this.Invoke((Action)delegate {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                });
            }
        }

        /// <summary>
        /// Callback when the client connect to the server successfully.
        /// </summary>
        /// <param name="ar"></param>
        private void tcpConnectCallback(IAsyncResult ar)
        {
            try {
                Socket clientSocket = (Socket)ar.AsyncState;

                clientSocket.EndConnect(ar);
            } catch {
                MessageBox.Show("Could not connect with device.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tcpConnectDone.Set();
                return;
            }

            tcpConnected = true;
            tcpConnectDone.Set();
        }

        /// <summary>
        /// Callback when a part of the file has been sent to the clients successfully.
        /// </summary>
        /// <param name="ar"></param>
        private void tcpSendCallback(IAsyncResult ar)
        {
            Socket handler = null;
            try {
                handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
                totalSent += bytesSent;

                if (binarySize > 0) {
                    prgStatus.Invoke((Action)delegate {
                        prgStatus.Value = (int)(((double)totalSent / binarySize) * 100);
                    });
                }

                // Close the socket when all the data has sent to the client.
                if (bytesSent == 0) {
//                    tcpAllDone.Set();
                }
            } catch (ArgumentException argEx) {
                MessageBox.Show(argEx.Message);
            } catch (SocketException) {
                // Close the socket if the client disconnected.
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            } finally {
                tcpSendDone.Set();
            }
        }

        /// <summary>
        /// Callback when receive a file chunk from the server successfully.
        /// </summary>
        /// <param name="ar"></param>
        private void tcpReceiveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket clientSocket = state.WorkSocket;

            int bytesRead = clientSocket.EndReceive(ar);
            if (bytesRead > 0) {
                state.Buffer.CopyTo(response, totalReceived);
                totalReceived += bytesRead;

                // Recursively receive the rest.
                try {
                    clientSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(tcpReceiveCallback), state);
                } catch {
                    if (!clientSocket.Connected) {
                        return;
                    }
                }
            } else {
                // Signal if all the file received.
                tcpReceiveDone.Set();
            }
        }

        private void resetStatus()
        {
            btnOK.Enabled = true;
            btnCancel.Enabled = true;
        }
    }

    class StateObject
    {
        public Socket WorkSocket = null;

        public const int BufferSize = 5242880;

        public byte[] Buffer = new byte[BufferSize];

        public bool Connected = false;
    }
}
