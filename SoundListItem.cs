using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Media;
using System.IO;
using NAudio.Wave;

namespace TataBuilder
{
    public partial class SoundListItem : UserControl
    {
        const int WM_LBUTTONDOWN = 0x0201;

        public string filePath { get; set; }
        public bool selected { get; set; }

        //Declarations required for audio out and the MP3 stream
        IWavePlayer waveOutDevice;
        AudioFileReader audioFileReader;

        public SoundListItem(string filePath)
        {
            InitializeComponent();
            this.Dock = DockStyle.Top;

            this.filePath = filePath;
            this.selected = false;

            this.lblFileName.Text = Path.GetFileName(filePath);

            try {
                audioFileReader = new AudioFileReader(filePath);
                if (audioFileReader != null) {
                    this.lblTime.Text = audioFileReader.TotalTime.ToString("h':'mm':'ss");
                    audioFileReader.Close();
                    audioFileReader = null;
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            WireAllControls(this);
        }

        public void select()
        {
            selected = true;
            updateBackgroundImage();

            // call event listener
            SelectablePanel container = this.Parent as SelectablePanel;
            if (container != null)
                container.FireSelectionChanged();
        }

        public void deselect()
        {
            selected = false;
            updateBackgroundImage();

            // call event listener
            SelectablePanel container = this.Parent as SelectablePanel;
            if (container != null)
                container.FireSelectionChanged();
        }

        public void updateBackgroundImage()
        {
            if (selected) {

                Bitmap bm = new Bitmap(this.Width, this.Height);
                Pen pen1;
                Brush brush1, brush2;
                if (this.Focused || (this.Parent != null && this.Parent.Focused) || (this.Parent != null && this.ownerForm().ActiveControl.isChildOf(this.Parent))) {
                    pen1 = new Pen(Color.FromArgb(132, 172, 221));
                    brush1 = new LinearGradientBrush(new Point(0, 0), new Point(0, bm.Height), Color.FromArgb(235, 244, 254), Color.FromArgb(207, 228, 254));
                    brush2 = new LinearGradientBrush(new Point(0, 0), new Point(0, this.Height), Color.FromArgb(241, 247, 254), Color.FromArgb(221, 235, 254));
                } else {
                    pen1 = new Pen(Color.FromArgb(217, 217, 217));
                    brush1 = new LinearGradientBrush(new Point(0, 0), new Point(0, bm.Height), Color.FromArgb(247, 247, 247), Color.FromArgb(230, 230, 230));
                    brush2 = new LinearGradientBrush(new Point(0, 0), new Point(0, this.Height), Color.FromArgb(250, 250, 250), Color.FromArgb(240, 240, 240));
                }

                Graphics g = Graphics.FromImage(bm);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawRoundedRectangle(pen1, 0, 0, bm.Width - 1, bm.Height - 1, 2);
                g.FillRoundedRectangle(brush1, 1, 1, bm.Width - 3, bm.Height - 3, 2);
                g.DrawRoundedRectangle(new Pen(brush2), 1, 1, bm.Width - 3, bm.Height - 3, 2);

                this.BackgroundImage = bm;
            } else {
                this.BackgroundImage = null;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (waveOutDevice != null && waveOutDevice.PlaybackState == PlaybackState.Playing) {
                
                if (waveOutDevice != null) {
                    waveOutDevice.Stop();
                    waveOutDevice = null;
                }

                if (audioFileReader != null) {
                    audioFileReader.Close();
                    audioFileReader = null;
                }

                // change to play icon
                this.btnPlay.ImageIndex = 0;

            } else {

                try {
                    waveOutDevice = new WaveOut();
                    audioFileReader = new AudioFileReader(filePath);
                    waveOutDevice.Init(audioFileReader);
                    waveOutDevice.PlaybackStopped += onPlaybackStopped;
                    waveOutDevice.Play();

                    // change to stop icon
                    this.btnPlay.ImageIndex = 2;

                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("Could not play the selected sound.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        public void onPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (waveOutDevice != null) {
                waveOutDevice.Stop();
                waveOutDevice = null;
            }

            if (audioFileReader != null) {
                audioFileReader.Close();
                audioFileReader = null;
            }

            // change to play icon
            this.btnPlay.ImageIndex = 0;
        }

        private void WireAllControls(Control cont)
        {
            foreach (Control ctl in cont.Controls)
            {
                ctl.Click += ctl_Click;
                if (ctl.HasChildren)
                {
                    WireAllControls(ctl);
                }
            }
        }

        private void ctl_Click(object sender, EventArgs e)
        {
            this.InvokeOnClick(this, EventArgs.Empty);
        }

        private void AnimationListItem_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                foreach (Control item in this.Parent.Controls)
                {
                    if (item is SoundListItem)
                    {
                        if (item != this)
                            ((SoundListItem)item).deselect();
                    }
                }
            }

            this.select();
            this.Focus();
        }

        private void btnPlay_MouseHover(object sender, EventArgs e)
        {
            this.btnPlay.ImageIndex = (this.btnPlay.ImageIndex / 2) * 2 + 1;
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e)
        {
            this.btnPlay.ImageIndex = (this.btnPlay.ImageIndex / 2) * 2;
        }

        private void btnPlay_MouseDown(object sender, MouseEventArgs e)
        {
            this.btnPlay.ImageIndex = (this.btnPlay.ImageIndex / 2) * 2;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void btnEdit_MouseLeave(object sender, EventArgs e)
        {

        }

        private void btnEdit_MouseHover(object sender, EventArgs e)
        {

        }

    }
}
