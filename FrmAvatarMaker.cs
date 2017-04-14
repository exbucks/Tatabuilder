using AForge.Video.DirectShow;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WIA;

namespace TataBuilder
{
    public partial class FrmAvatarMaker : Form
    {
        public TDocument document { get; set; }
        public Image avatar { get; set; }

        //Create webcam object
        VideoCaptureDevice videoSource;
        Bitmap cameraImage;

        object lockobj = new object();
        PointF currentOffset = new PointF();
        float  currentZoom = 1;

        bool   currentMouseDown = false;
        PointF currentMousePos;

        public FrmAvatarMaker()
        {
            InitializeComponent();
        }

        private void FrmAvatarMaker_Load(object sender, EventArgs e)
        {
            picAvataPreview.BackgroundImage = document.getAvatarFrameImage();
            avatar = null;
            openDefaultDevice();
        }

        private void FrmAvatarMaker_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeDevice();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // show open dialog
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png|All files (*.*)|*.*";
            dialog.Multiselect = false;
            dialog.Title = "Select image file for avatar";

            //Present to the user.
            if (dialog.ShowDialog() == DialogResult.OK) {
                // close camera device
                closeDevice();

                // Read the files 
                lock (lockobj) {
                    cameraImage = new Bitmap(Image.FromFile(dialog.FileName));
                    updateAvataPreview();
                }

                pnlAdjustPicture.Visible = true;
                pnlSelectPicture.Visible = false;
            }
        }

        private void btnTakeCamera_Click(object sender, EventArgs e)
        {
            closeDevice();
            pnlAdjustPicture.Visible = true;
            pnlSelectPicture.Visible = false;
        }

        private void trbZoom_Scroll(object sender, EventArgs e)
        {
            currentZoom = (float)trbZoom.Value / 100;
            updateAvataPreview();
        }

        private void picAvataPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                currentMouseDown = true;
                currentMousePos = e.Location;
            }
        }

        private void picAvataPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && currentMouseDown) {
                currentOffset.X += e.X - currentMousePos.X;
                currentOffset.Y += e.Y - currentMousePos.Y;
                currentMousePos = e.Location;

                updateAvataPreview();
            }
        }

        private void picAvataPreview_MouseUp(object sender, MouseEventArgs e)
        {
            currentMouseDown = false;
        }

        private void btnTryAgain_Click(object sender, EventArgs e)
        {
            openDefaultDevice();
            pnlSelectPicture.Visible = true;
            pnlAdjustPicture.Visible = false;

            currentOffset = new PointF();
            currentZoom = 1;
            trbZoom.Value = 100;
        }

        private void btnUseThis_Click(object sender, EventArgs e)
        {
            this.avatar = picAvataPreview.BackgroundImage;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void openDefaultDevice()
        {
            //List all available video sources. (That can be webcams as well as tv cards, etc)
            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            //Check if atleast one video source is available
            if (videosources != null && videosources.Count > 0) {
                //For example use first video device. You may check if this is your webcam.
                videoSource = new VideoCaptureDevice(videosources[0].MonikerString);

                try {
                    //Check if the video device provides a list of supported resolutions
                    if (videoSource.VideoCapabilities.Length > 0) {
                        string highestSolution = "0;0";
                        //Search for the highest resolution
                        for (int i = 0; i < videoSource.VideoCapabilities.Length; i++) {
                            if (videoSource.VideoCapabilities[i].FrameSize.Width > Convert.ToInt32(highestSolution.Split(';')[0]))
                                highestSolution = videoSource.VideoCapabilities[i].FrameSize.Width.ToString() + ";" + i.ToString();
                        }
                        //Set the highest resolution as active
                        videoSource.VideoResolution = videoSource.VideoCapabilities[Convert.ToInt32(highestSolution.Split(';')[1])];
                    }
                } catch { }

                //Create NewFrame event handler
                //(This one triggers every time a new frame/image is captured
                videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(onCameraNewFrame);

                //Start recording
                videoSource.Start();
            }
        }

        private void closeDevice()
        {
            //Stop and free the webcam object if application is closing
            if (videoSource != null && videoSource.IsRunning) {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
            }
        }

        private void updateAvataPreview()
        {
            picAvataPreview.BackgroundImage = composeAvatarImage(cameraImage, currentOffset, currentZoom);
        }

        private void onCameraNewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            //Cast the frame as Bitmap object and don't forget to use ".Clone()" otherwise
            //you'll probably get access violation exceptions
            lock (lockobj) {
                cameraImage = (Bitmap)eventArgs.Frame.Clone();
                updateAvataPreview();
            }
        }

        private Image composeAvatarImage(Image camera, PointF offset, float zoom)
        {
            Image avatarMask = document.getAvatarMaskImage();
            Image avatarFrame = document.getAvatarFrameImage();

            int w = avatarMask.Width, h = avatarMask.Height;
            zoom = Math.Max((float)w / camera.Width, (float)h / camera.Height) * zoom;
            int cw = (int)(camera.Width * zoom), ch = (int)(camera.Height * zoom);

            Bitmap result = new Bitmap(avatarMask.Width, avatarMask.Height);
            Graphics g = Graphics.FromImage(result);

            g.DrawImage(camera, w / 2 - cw / 2 + offset.X, h / 2 - ch / 2 + offset.Y, cw, ch);
            g.DrawImage(avatarFrame, 0, 0, w, h);

            g.Dispose();

            return imageWithMask(result, new Bitmap(avatarMask));
        }

        private Image imageWithMask(Bitmap input, Bitmap mask)
        {
            Bitmap output = new Bitmap(input.Width, input.Height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, input.Width, input.Height);
            var bitsMask = mask.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bitsInput = input.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bitsOutput = output.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            unsafe {
                for (int y = 0; y < input.Height; y++) {
                    byte* ptrMask = (byte*)bitsMask.Scan0 + y * bitsMask.Stride;
                    byte* ptrInput = (byte*)bitsInput.Scan0 + y * bitsInput.Stride;
                    byte* ptrOutput = (byte*)bitsOutput.Scan0 + y * bitsOutput.Stride;
                    for (int x = 0; x < input.Width; x++) {
                        ptrOutput[4 * x] = ptrInput[4 * x];           // blue
                        ptrOutput[4 * x + 1] = ptrInput[4 * x + 1];   // green
                        ptrOutput[4 * x + 2] = ptrInput[4 * x + 2];   // red
                        ptrOutput[4 * x + 3] = ptrMask[4 * x + 3];        // alpha
                    }
                }
            }

            mask.UnlockBits(bitsMask);
            input.UnlockBits(bitsInput);
            output.UnlockBits(bitsOutput);

            return output;
        }
    }
}
