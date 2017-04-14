namespace TataBuilder
{
    partial class FrmWorkspace
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
            if (disposing && (components != null))
            {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmWorkspace));
            this.lblFocusTarget = new System.Windows.Forms.Label();
            this.pnlVerticalRuler = new System.Windows.Forms.Panel();
            this.pnlHorizontalRuler = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlWorkspace = new TataBuilder.DoubleBufferedPanel();
            this.rulerHorizontal = new Lyquidity.UtilityLibrary.Controls.RulerControl();
            this.rulerVertical = new Lyquidity.UtilityLibrary.Controls.RulerControl();
            this.pnlVerticalRuler.SuspendLayout();
            this.pnlHorizontalRuler.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFocusTarget
            // 
            this.lblFocusTarget.AutoSize = true;
            this.lblFocusTarget.Location = new System.Drawing.Point(0, 0);
            this.lblFocusTarget.Name = "lblFocusTarget";
            this.lblFocusTarget.Size = new System.Drawing.Size(0, 13);
            this.lblFocusTarget.TabIndex = 0;
            this.lblFocusTarget.Visible = false;
            // 
            // pnlVerticalRuler
            // 
            this.pnlVerticalRuler.Controls.Add(this.rulerVertical);
            this.pnlVerticalRuler.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlVerticalRuler.Location = new System.Drawing.Point(0, 0);
            this.pnlVerticalRuler.Name = "pnlVerticalRuler";
            this.pnlVerticalRuler.Padding = new System.Windows.Forms.Padding(5, 25, 5, 5);
            this.pnlVerticalRuler.Size = new System.Drawing.Size(25, 730);
            this.pnlVerticalRuler.TabIndex = 0;
            // 
            // pnlHorizontalRuler
            // 
            this.pnlHorizontalRuler.Controls.Add(this.rulerHorizontal);
            this.pnlHorizontalRuler.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHorizontalRuler.Location = new System.Drawing.Point(25, 0);
            this.pnlHorizontalRuler.Name = "pnlHorizontalRuler";
            this.pnlHorizontalRuler.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.pnlHorizontalRuler.Size = new System.Drawing.Size(983, 25);
            this.pnlHorizontalRuler.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pnlWorkspace);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(25, 25);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.panel3.Size = new System.Drawing.Size(983, 705);
            this.panel3.TabIndex = 0;
            // 
            // pnlWorkspace
            // 
            this.pnlWorkspace.AllowDrop = true;
            this.pnlWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWorkspace.Location = new System.Drawing.Point(0, 0);
            this.pnlWorkspace.Name = "pnlWorkspace";
            this.pnlWorkspace.Size = new System.Drawing.Size(978, 700);
            this.pnlWorkspace.TabIndex = 0;
            this.pnlWorkspace.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlWorkspace_DragDrop);
            this.pnlWorkspace.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlWorkspace_DragEnter);
            this.pnlWorkspace.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlWorkspace_Paint);
            this.pnlWorkspace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlWorkspace_MouseDown);
            this.pnlWorkspace.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlWorkspace_MouseMove);
            this.pnlWorkspace.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlWorkspace_MouseUp);
            // 
            // rulerHorizontal
            // 
            this.rulerHorizontal.ActualSize = true;
            this.rulerHorizontal.DivisionMarkFactor = 10;
            this.rulerHorizontal.Divisions = 10;
            this.rulerHorizontal.Dock = System.Windows.Forms.DockStyle.Top;
            this.rulerHorizontal.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.rulerHorizontal.ForeColor = System.Drawing.Color.Black;
            this.rulerHorizontal.Location = new System.Drawing.Point(0, 5);
            this.rulerHorizontal.MajorInterval = 100;
            this.rulerHorizontal.MiddleMarkFactor = 3;
            this.rulerHorizontal.MouseTrackingOn = false;
            this.rulerHorizontal.Name = "rulerHorizontal";
            this.rulerHorizontal.Orientation = Lyquidity.UtilityLibrary.Controls.enumOrientation.orHorizontal;
            this.rulerHorizontal.RulerAlignment = Lyquidity.UtilityLibrary.Controls.enumRulerAlignment.raMiddle;
            this.rulerHorizontal.ScaleMode = Lyquidity.UtilityLibrary.Controls.enumScaleMode.smPixels;
            this.rulerHorizontal.Size = new System.Drawing.Size(978, 15);
            this.rulerHorizontal.StartOffset = 0;
            this.rulerHorizontal.StartValue = 0D;
            this.rulerHorizontal.TabIndex = 1;
            this.rulerHorizontal.VerticalNumbers = false;
            this.rulerHorizontal.ZoomFactor = 1D;
            // 
            // rulerVertical
            // 
            this.rulerVertical.ActualSize = true;
            this.rulerVertical.DivisionMarkFactor = 10;
            this.rulerVertical.Divisions = 10;
            this.rulerVertical.Dock = System.Windows.Forms.DockStyle.Left;
            this.rulerVertical.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.rulerVertical.ForeColor = System.Drawing.Color.Black;
            this.rulerVertical.Location = new System.Drawing.Point(5, 25);
            this.rulerVertical.MajorInterval = 100;
            this.rulerVertical.MiddleMarkFactor = 3;
            this.rulerVertical.MouseTrackingOn = false;
            this.rulerVertical.Name = "rulerVertical";
            this.rulerVertical.Orientation = Lyquidity.UtilityLibrary.Controls.enumOrientation.orVertical;
            this.rulerVertical.RulerAlignment = Lyquidity.UtilityLibrary.Controls.enumRulerAlignment.raMiddle;
            this.rulerVertical.ScaleMode = Lyquidity.UtilityLibrary.Controls.enumScaleMode.smPixels;
            this.rulerVertical.Size = new System.Drawing.Size(15, 700);
            this.rulerVertical.StartOffset = 0;
            this.rulerVertical.StartValue = 0D;
            this.rulerVertical.TabIndex = 2;
            this.rulerVertical.VerticalNumbers = true;
            this.rulerVertical.ZoomFactor = 1D;
            // 
            // FrmWorkspace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pnlHorizontalRuler);
            this.Controls.Add(this.pnlVerticalRuler);
            this.Controls.Add(this.lblFocusTarget);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmWorkspace";
            this.Text = "Untitled";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmWorkspace_FormClosing);
            this.Load += new System.EventHandler(this.FrmWorkspace_Load);
            this.pnlVerticalRuler.ResumeLayout(false);
            this.pnlHorizontalRuler.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFocusTarget;
        private DoubleBufferedPanel pnlWorkspace;
        private Lyquidity.UtilityLibrary.Controls.RulerControl rulerHorizontal;
        private Lyquidity.UtilityLibrary.Controls.RulerControl rulerVertical;
        private System.Windows.Forms.Panel pnlVerticalRuler;
        private System.Windows.Forms.Panel pnlHorizontalRuler;
        private System.Windows.Forms.Panel panel3;

    }
}