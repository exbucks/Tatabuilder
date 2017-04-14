namespace TataBuilder
{
    partial class SoundListItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundListItem));
            this.lblTime = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imlPlayStop = new System.Windows.Forms.ImageList(this.components);
            this.btnPlay = new TataBuilder.FocuslessButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTime
            // 
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(219, 8);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(45, 24);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "00:00";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFileName
            // 
            this.lblFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFileName.Location = new System.Drawing.Point(32, 8);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblFileName.Size = new System.Drawing.Size(187, 24);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "Scene_1.mp3";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::TataBuilder.Properties.Resources.resources_sound_note;
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // imlPlayStop
            // 
            this.imlPlayStop.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlPlayStop.ImageStream")));
            this.imlPlayStop.TransparentColor = System.Drawing.Color.Transparent;
            this.imlPlayStop.Images.SetKeyName(0, "btn_play_sound.png");
            this.imlPlayStop.Images.SetKeyName(1, "btn_play_sound_over.png");
            this.imlPlayStop.Images.SetKeyName(2, "btn_stop_sound.png");
            this.imlPlayStop.Images.SetKeyName(3, "btn_stop_sound_over.png");
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPlay.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.ImageIndex = 0;
            this.btnPlay.ImageList = this.imlPlayStop;
            this.btnPlay.Location = new System.Drawing.Point(264, 8);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(30, 24);
            this.btnPlay.TabIndex = 9;
            this.btnPlay.TabStop = false;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            this.btnPlay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPlay_MouseDown);
            this.btnPlay.MouseLeave += new System.EventHandler(this.btnPlay_MouseLeave);
            this.btnPlay.MouseHover += new System.EventHandler(this.btnPlay_MouseHover);
            // 
            // SoundListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.pictureBox1);
            this.Name = "SoundListItem";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Size = new System.Drawing.Size(302, 40);
            this.Click += new System.EventHandler(this.AnimationListItem_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTime;
        private FocuslessButton btnPlay;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.ImageList imlPlayStop;

    }
}
