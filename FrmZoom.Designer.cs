namespace TataBuilder
{
    partial class FrmZoom
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
            this.radZoomSpecific = new System.Windows.Forms.RadioButton();
            this.radZoom400 = new System.Windows.Forms.RadioButton();
            this.radZoom200 = new System.Windows.Forms.RadioButton();
            this.radZoom100 = new System.Windows.Forms.RadioButton();
            this.radZoom66 = new System.Windows.Forms.RadioButton();
            this.radZoom50 = new System.Windows.Forms.RadioButton();
            this.radZoom33 = new System.Windows.Forms.RadioButton();
            this.nudZoomPercent = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudZoomPercent)).BeginInit();
            this.SuspendLayout();
            // 
            // radZoomSpecific
            // 
            this.radZoomSpecific.AutoSize = true;
            this.radZoomSpecific.Location = new System.Drawing.Point(24, 26);
            this.radZoomSpecific.Name = "radZoomSpecific";
            this.radZoomSpecific.Size = new System.Drawing.Size(63, 17);
            this.radZoomSpecific.TabIndex = 0;
            this.radZoomSpecific.TabStop = true;
            this.radZoomSpecific.Text = "Specific";
            this.radZoomSpecific.UseVisualStyleBackColor = true;
            // 
            // radZoom400
            // 
            this.radZoom400.AutoSize = true;
            this.radZoom400.Location = new System.Drawing.Point(24, 49);
            this.radZoom400.Name = "radZoom400";
            this.radZoom400.Size = new System.Drawing.Size(51, 17);
            this.radZoom400.TabIndex = 3;
            this.radZoom400.TabStop = true;
            this.radZoom400.Text = "400%";
            this.radZoom400.UseVisualStyleBackColor = true;
            // 
            // radZoom200
            // 
            this.radZoom200.AutoSize = true;
            this.radZoom200.Location = new System.Drawing.Point(24, 72);
            this.radZoom200.Name = "radZoom200";
            this.radZoom200.Size = new System.Drawing.Size(51, 17);
            this.radZoom200.TabIndex = 4;
            this.radZoom200.TabStop = true;
            this.radZoom200.Text = "200%";
            this.radZoom200.UseVisualStyleBackColor = true;
            // 
            // radZoom100
            // 
            this.radZoom100.AutoSize = true;
            this.radZoom100.Location = new System.Drawing.Point(24, 95);
            this.radZoom100.Name = "radZoom100";
            this.radZoom100.Size = new System.Drawing.Size(51, 17);
            this.radZoom100.TabIndex = 5;
            this.radZoom100.TabStop = true;
            this.radZoom100.Text = "100%";
            this.radZoom100.UseVisualStyleBackColor = true;
            // 
            // radZoom66
            // 
            this.radZoom66.AutoSize = true;
            this.radZoom66.Location = new System.Drawing.Point(24, 118);
            this.radZoom66.Name = "radZoom66";
            this.radZoom66.Size = new System.Drawing.Size(45, 17);
            this.radZoom66.TabIndex = 6;
            this.radZoom66.TabStop = true;
            this.radZoom66.Text = "66%";
            this.radZoom66.UseVisualStyleBackColor = true;
            // 
            // radZoom50
            // 
            this.radZoom50.AutoSize = true;
            this.radZoom50.Location = new System.Drawing.Point(24, 141);
            this.radZoom50.Name = "radZoom50";
            this.radZoom50.Size = new System.Drawing.Size(45, 17);
            this.radZoom50.TabIndex = 7;
            this.radZoom50.TabStop = true;
            this.radZoom50.Text = "50%";
            this.radZoom50.UseVisualStyleBackColor = true;
            // 
            // radZoom33
            // 
            this.radZoom33.AutoSize = true;
            this.radZoom33.Location = new System.Drawing.Point(24, 164);
            this.radZoom33.Name = "radZoom33";
            this.radZoom33.Size = new System.Drawing.Size(45, 17);
            this.radZoom33.TabIndex = 8;
            this.radZoom33.TabStop = true;
            this.radZoom33.Text = "33%";
            this.radZoom33.UseVisualStyleBackColor = true;
            // 
            // nudZoomPercent
            // 
            this.nudZoomPercent.Location = new System.Drawing.Point(128, 26);
            this.nudZoomPercent.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.nudZoomPercent.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudZoomPercent.Name = "nudZoomPercent";
            this.nudZoomPercent.Size = new System.Drawing.Size(55, 20);
            this.nudZoomPercent.TabIndex = 1;
            this.nudZoomPercent.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(189, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "%";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(38, 200);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 25);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(124, 200);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmZoom
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(225, 242);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudZoomPercent);
            this.Controls.Add(this.radZoom33);
            this.Controls.Add(this.radZoom50);
            this.Controls.Add(this.radZoom66);
            this.Controls.Add(this.radZoom100);
            this.Controls.Add(this.radZoom200);
            this.Controls.Add(this.radZoom400);
            this.Controls.Add(this.radZoomSpecific);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmZoom";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Zoom";
            ((System.ComponentModel.ISupportInitialize)(this.nudZoomPercent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radZoomSpecific;
        private System.Windows.Forms.RadioButton radZoom400;
        private System.Windows.Forms.RadioButton radZoom200;
        private System.Windows.Forms.RadioButton radZoom100;
        private System.Windows.Forms.RadioButton radZoom66;
        private System.Windows.Forms.RadioButton radZoom50;
        private System.Windows.Forms.RadioButton radZoom33;
        private System.Windows.Forms.NumericUpDown nudZoomPercent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}