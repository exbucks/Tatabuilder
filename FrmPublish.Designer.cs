namespace TataBuilder
{
    partial class FrmPublish
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
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
        private void InitializeComponent() {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblIdentifier = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.prgStatus = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(450, 265);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(364, 265);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 25);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Upload";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Identifier :";
            // 
            // lblIdentifier
            // 
            this.lblIdentifier.Location = new System.Drawing.Point(104, 18);
            this.lblIdentifier.Name = "lblIdentifier";
            this.lblIdentifier.Size = new System.Drawing.Size(426, 20);
            this.lblIdentifier.TabIndex = 12;
            this.lblIdentifier.Text = "com.jeilsoft.lionking";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(27, 46);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(503, 187);
            this.txtLog.TabIndex = 13;
            // 
            // prgStatus
            // 
            this.prgStatus.Location = new System.Drawing.Point(27, 239);
            this.prgStatus.Name = "prgStatus";
            this.prgStatus.Size = new System.Drawing.Size(503, 20);
            this.prgStatus.TabIndex = 14;
            // 
            // FrmPublish
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(556, 307);
            this.Controls.Add(this.prgStatus);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblIdentifier);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPublish";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Publish";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPublish_FormClosing);
            this.Load += new System.EventHandler(this.FrmPublish_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblIdentifier;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ProgressBar prgStatus;
    }
}