namespace TataBuilder
{
    partial class TimeLineView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlVScrollBarWrapper = new System.Windows.Forms.Panel();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.pnlHScrollBarWrapper = new System.Windows.Forms.Panel();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.pnlVScrollBarWrapper.SuspendLayout();
            this.pnlHScrollBarWrapper.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlVScrollBarWrapper
            // 
            this.pnlVScrollBarWrapper.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlVScrollBarWrapper.Controls.Add(this.vScrollBar);
            this.pnlVScrollBarWrapper.Location = new System.Drawing.Point(774, 1);
            this.pnlVScrollBarWrapper.Name = "pnlVScrollBarWrapper";
            this.pnlVScrollBarWrapper.Padding = new System.Windows.Forms.Padding(0, 0, 0, 17);
            this.pnlVScrollBarWrapper.Size = new System.Drawing.Size(17, 258);
            this.pnlVScrollBarWrapper.TabIndex = 2;
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 241);
            this.vScrollBar.TabIndex = 2;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            // 
            // pnlHScrollBarWrapper
            // 
            this.pnlHScrollBarWrapper.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHScrollBarWrapper.Controls.Add(this.hScrollBar);
            this.pnlHScrollBarWrapper.Location = new System.Drawing.Point(1, 242);
            this.pnlHScrollBarWrapper.Name = "pnlHScrollBarWrapper";
            this.pnlHScrollBarWrapper.Size = new System.Drawing.Size(773, 17);
            this.pnlHScrollBarWrapper.TabIndex = 3;
            // 
            // hScrollBar
            // 
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hScrollBar.Location = new System.Drawing.Point(0, 0);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(773, 17);
            this.hScrollBar.TabIndex = 1;
            this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
            // 
            // TimeLineView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlHScrollBarWrapper);
            this.Controls.Add(this.pnlVScrollBarWrapper);
            this.Name = "TimeLineView";
            this.Size = new System.Drawing.Size(792, 260);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TimeLineView_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TimeLineView_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TimeLineView_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimeLineView_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TimeLineView_MouseUp);
            this.pnlVScrollBarWrapper.ResumeLayout(false);
            this.pnlHScrollBarWrapper.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlVScrollBarWrapper;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.Panel pnlHScrollBarWrapper;
        private System.Windows.Forms.HScrollBar hScrollBar;
    }
}
