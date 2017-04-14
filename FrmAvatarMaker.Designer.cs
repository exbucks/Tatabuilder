namespace TataBuilder
{
    partial class FrmAvatarMaker
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAvatarMaker));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTakeCamera = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.pnlSelectPicture = new System.Windows.Forms.Panel();
            this.pnlAdjustPicture = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.btnUseThis = new System.Windows.Forms.Button();
            this.btnTryAgain = new System.Windows.Forms.Button();
            this.trbZoom = new System.Windows.Forms.TrackBar();
            this.picAvataPreview = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlSelectPicture.SuspendLayout();
            this.pnlAdjustPicture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAvataPreview)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 475);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(699, 1);
            this.panel1.TabIndex = 1;
            // 
            // btnTakeCamera
            // 
            this.btnTakeCamera.Location = new System.Drawing.Point(589, 6);
            this.btnTakeCamera.Name = "btnTakeCamera";
            this.btnTakeCamera.Size = new System.Drawing.Size(100, 30);
            this.btnTakeCamera.TabIndex = 2;
            this.btnTakeCamera.Text = "Take a picture";
            this.btnTakeCamera.UseVisualStyleBackColor = true;
            this.btnTakeCamera.Click += new System.EventHandler(this.btnTakeCamera_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(483, 6);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 30);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // pnlSelectPicture
            // 
            this.pnlSelectPicture.Controls.Add(this.btnTakeCamera);
            this.pnlSelectPicture.Controls.Add(this.btnBrowse);
            this.pnlSelectPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSelectPicture.Location = new System.Drawing.Point(0, 476);
            this.pnlSelectPicture.Name = "pnlSelectPicture";
            this.pnlSelectPicture.Size = new System.Drawing.Size(699, 42);
            this.pnlSelectPicture.TabIndex = 7;
            // 
            // pnlAdjustPicture
            // 
            this.pnlAdjustPicture.Controls.Add(this.pictureBox4);
            this.pnlAdjustPicture.Controls.Add(this.pictureBox5);
            this.pnlAdjustPicture.Controls.Add(this.btnUseThis);
            this.pnlAdjustPicture.Controls.Add(this.btnTryAgain);
            this.pnlAdjustPicture.Controls.Add(this.trbZoom);
            this.pnlAdjustPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAdjustPicture.Location = new System.Drawing.Point(0, 476);
            this.pnlAdjustPicture.Name = "pnlAdjustPicture";
            this.pnlAdjustPicture.Size = new System.Drawing.Size(699, 42);
            this.pnlAdjustPicture.TabIndex = 8;
            this.pnlAdjustPicture.Visible = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::TataBuilder.Properties.Resources.emulator_img_zoom_small;
            this.pictureBox4.Location = new System.Drawing.Point(232, 9);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(24, 24);
            this.pictureBox4.TabIndex = 5;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::TataBuilder.Properties.Resources.emulator_img_zoom_big;
            this.pictureBox5.Location = new System.Drawing.Point(436, 9);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(24, 24);
            this.pictureBox5.TabIndex = 6;
            this.pictureBox5.TabStop = false;
            // 
            // btnUseThis
            // 
            this.btnUseThis.Location = new System.Drawing.Point(587, 6);
            this.btnUseThis.Name = "btnUseThis";
            this.btnUseThis.Size = new System.Drawing.Size(100, 30);
            this.btnUseThis.TabIndex = 2;
            this.btnUseThis.Text = "Use this picture";
            this.btnUseThis.UseVisualStyleBackColor = true;
            this.btnUseThis.Click += new System.EventHandler(this.btnUseThis_Click);
            // 
            // btnTryAgain
            // 
            this.btnTryAgain.Location = new System.Drawing.Point(481, 6);
            this.btnTryAgain.Name = "btnTryAgain";
            this.btnTryAgain.Size = new System.Drawing.Size(100, 30);
            this.btnTryAgain.TabIndex = 3;
            this.btnTryAgain.Text = "Try again";
            this.btnTryAgain.UseVisualStyleBackColor = true;
            this.btnTryAgain.Click += new System.EventHandler(this.btnTryAgain_Click);
            // 
            // trbZoom
            // 
            this.trbZoom.AutoSize = false;
            this.trbZoom.Location = new System.Drawing.Point(256, 9);
            this.trbZoom.Maximum = 200;
            this.trbZoom.Minimum = 100;
            this.trbZoom.Name = "trbZoom";
            this.trbZoom.Size = new System.Drawing.Size(180, 24);
            this.trbZoom.TabIndex = 4;
            this.trbZoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trbZoom.Value = 100;
            this.trbZoom.Scroll += new System.EventHandler(this.trbZoom_Scroll);
            // 
            // picAvataPreview
            // 
            this.picAvataPreview.BackColor = System.Drawing.Color.Transparent;
            this.picAvataPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picAvataPreview.Location = new System.Drawing.Point(369, 141);
            this.picAvataPreview.Name = "picAvataPreview";
            this.picAvataPreview.Size = new System.Drawing.Size(246, 262);
            this.picAvataPreview.TabIndex = 9;
            this.picAvataPreview.TabStop = false;
            this.picAvataPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picAvataPreview_MouseDown);
            this.picAvataPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picAvataPreview_MouseMove);
            this.picAvataPreview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picAvataPreview_MouseUp);
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::TataBuilder.Properties.Resources.emulator_img_avatar_maker;
            this.panel2.Controls.Add(this.picAvataPreview);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(699, 475);
            this.panel2.TabIndex = 10;
            // 
            // FrmAvatarMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(699, 518);
            this.Controls.Add(this.pnlAdjustPicture);
            this.Controls.Add(this.pnlSelectPicture);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAvatarMaker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Make Avatar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAvatarMaker_FormClosing);
            this.Load += new System.EventHandler(this.FrmAvatarMaker_Load);
            this.pnlSelectPicture.ResumeLayout(false);
            this.pnlAdjustPicture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAvataPreview)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnTakeCamera;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Panel pnlSelectPicture;
        private System.Windows.Forms.Panel pnlAdjustPicture;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Button btnUseThis;
        private System.Windows.Forms.Button btnTryAgain;
        private System.Windows.Forms.TrackBar trbZoom;
        private System.Windows.Forms.PictureBox picAvataPreview;
        private System.Windows.Forms.Panel panel2;
    }
}