using Newtonsoft.Json.Linq;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder
{
    public partial class FrmPublish : Form
    {
        public TDocument document { get; set; }
        private int step = 0;
        private Thread task = null;
        private int uploadProgressPercentage;

        public FrmPublish() 
        {
            InitializeComponent();
        }

        private void FrmPublish_Load(object sender, EventArgs e) 
        {
            lblIdentifier.Text = document.identifier;
            txtLog.Text = "";
            prgStatus.Value = 0;
        }

        private void FrmPublish_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (task != null && task.IsAlive) {
                // Request that oThread be stopped
                task.Abort();

                // Wait until oThread finishes. Join also has overloads that take a millisecond interval or a TimeSpan object.
                task.Join();
            }

            // Clear web client event handler
            Program.webClient.UploadProgressChanged -= WebClientUploadProgressChanged;
            Program.webClient.UploadFileCompleted -= WebClientUploadCompleted;
        }

        private void btnOK_Click(object sender, EventArgs e) 
        {
            if (step == 0) {
                btnOK.Enabled = false;
                txtLog.Text = "Starting publishing...\r\n";

                task = new Thread(publish);
                task.Start();
            } else {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        
        private void publish() 
        {
            NameValueCollection values;
            byte[] response;
            string responseString;

            //=================================== export package to temporary file ============================================
            txtLog.Invoke((Action)delegate {
                txtLog.Text += "Creating temporary package file...\r\n";
            });

            string tempFile = Path.GetTempFileName();
            document.export(tempFile);

            prgStatus.Invoke((Action)delegate {
                prgStatus.Value = 10;
            });

            //=================================== check the status of book of the server ===================================
            txtLog.Invoke((Action)delegate {
                txtLog.Text += "Checking the status of book of the server...\r\n";
            });

            try {
                values = new NameValueCollection {
                    { "id", document.identifier }
                };

                response = Program.webClient.UploadValues(Program.URL_CHECK_READY, "POST", values);
                responseString = Encoding.ASCII.GetString(response);
                try {
                    dynamic result = JObject.Parse(responseString);
                    if (String.Compare(result.status.Value, "Success", true) == 0) {
                        txtLog.Invoke((Action)delegate {
                            txtLog.Text += "Checking the status of book of the server was succeeded.\r\n";
                        });
                    } else {
                        txtLog.Invoke((Action)delegate {
                            txtLog.Text += "Checking the status of book of the server was failed.\r\n";
                            MessageBox.Show(result.description.Value, Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            resetStatus();
                        });

                        return;
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                    txtLog.Invoke((Action)delegate {
                        txtLog.Text += "Checking the status of book of the server was failed.\r\n";
                        MessageBox.Show("Could not check the status. Please try again.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        resetStatus();
                    });

                    return;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                txtLog.Invoke((Action)delegate {
                    txtLog.Text += "Checking the status of book of the server was failed.\r\n";
                    MessageBox.Show("Could not connect the server. Please check your internet connection.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resetStatus();
                });

                return;
            }

            prgStatus.Invoke((Action)delegate {
                prgStatus.Value = 20;
            });

            //=================================== upload binary to the server ===================================
            txtLog.Invoke((Action)delegate {
                txtLog.Text += "Uploading binary to server...\r\n";
            });

            try {
                string url = Program.URL_UPLOAD_BINARY;
                if (url.Contains("?"))
                    url += "&id=" + document.identifier;
                else
                    url += "?id=" + document.identifier;

                uploadProgressPercentage = 0;
                Program.webClient.UploadProgressChanged += WebClientUploadProgressChanged;
                Program.webClient.UploadFileCompleted += WebClientUploadCompleted;

                Program.webClient.UploadFileAsync(new Uri(url), "POST", tempFile);
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                txtLog.Invoke((Action)delegate {
                    txtLog.Text += "Uploading binary to the server was failed.\r\n";
                    MessageBox.Show("Uploading binary to the server was failed. Please try again.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resetStatus();
                });

                return;
            }
        }

        void WebClientUploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage - uploadProgressPercentage >= 5) {
                uploadProgressPercentage = e.ProgressPercentage;
                prgStatus.Invoke((Action)delegate {
                    prgStatus.Value = 20 + 80 * e.ProgressPercentage / 100;
                });
            }
        }

        void WebClientUploadCompleted(object sender, UploadFileCompletedEventArgs e)
        {

            try {
                string responseString = Encoding.ASCII.GetString(e.Result);
                dynamic result = JObject.Parse(responseString);
                if (String.Compare(result.status.Value, "Success", true) == 0) {
                    txtLog.Invoke((Action)delegate {
                        txtLog.Text += "Uploading binary to the server was succeeded.\r\n";
                    });
                } else {
                    txtLog.Invoke((Action)delegate {
                        txtLog.Text += "Uploading binary to the server was failed.\r\n";
                        MessageBox.Show(result.description.Value, Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        resetStatus();
                    });

                    return;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                txtLog.Invoke((Action)delegate {
                    txtLog.Text += "Uploading binary to the server was failed.\r\n";
                    MessageBox.Show("Uploading binary to the server was failed. Please try again.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resetStatus();
                });

                return;
            }

            // Clear web client event handler
            Program.webClient.UploadProgressChanged -= WebClientUploadProgressChanged;
            Program.webClient.UploadFileCompleted -= WebClientUploadCompleted;

            // update step
            step = 1;

            // finish button
            prgStatus.Invoke((Action)delegate {
                prgStatus.Value = 100;
                btnOK.Text = "Finish";
                btnOK.Enabled = true;
                btnCancel.Enabled = false;
            });
        }

        private void resetStatus()
        {
            prgStatus.Value = 0;
            btnOK.Text = "Upload";
            btnOK.Enabled = true;
            btnCancel.Enabled = true;
        }
    }
}
