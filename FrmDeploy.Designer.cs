namespace TataBuilder
{
    partial class FrmDeploy
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
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.prgStatus = new System.Windows.Forms.ProgressBar();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lvwDevices = new System.Windows.Forms.ListView();
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.address = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imlDeviceIcons = new System.Windows.Forms.ImageList(this.components);
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
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(364, 265);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 25);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Deploy";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Device List";
            // 
            // prgStatus
            // 
            this.prgStatus.Location = new System.Drawing.Point(27, 239);
            this.prgStatus.Name = "prgStatus";
            this.prgStatus.Size = new System.Drawing.Size(503, 20);
            this.prgStatus.TabIndex = 14;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(455, 13);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 25);
            this.btnRefresh.TabIndex = 15;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lvwDevices
            // 
            this.lvwDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name,
            this.address});
            this.lvwDevices.FullRowSelect = true;
            this.lvwDevices.Location = new System.Drawing.Point(27, 41);
            this.lvwDevices.Name = "lvwDevices";
            this.lvwDevices.Size = new System.Drawing.Size(503, 192);
            this.lvwDevices.SmallImageList = this.imlDeviceIcons;
            this.lvwDevices.TabIndex = 16;
            this.lvwDevices.UseCompatibleStateImageBehavior = false;
            this.lvwDevices.View = System.Windows.Forms.View.Details;
            this.lvwDevices.SelectedIndexChanged += new System.EventHandler(this.lvwDevices_SelectedIndexChanged);
            // 
            // name
            // 
            this.name.Text = "Device Name";
            this.name.Width = 250;
            // 
            // address
            // 
            this.address.Text = "IP Address";
            this.address.Width = 150;
            // 
            // imlDeviceIcons
            // 
            this.imlDeviceIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imlDeviceIcons.ImageSize = new System.Drawing.Size(1, 32);
            this.imlDeviceIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FrmDeploy
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(556, 307);
            this.Controls.Add(this.lvwDevices);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.prgStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDeploy";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Deploy to Device";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDeploy_FormClosing);
            this.Load += new System.EventHandler(this.FrmDeploy_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar prgStatus;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ListView lvwDevices;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader address;
        private System.Windows.Forms.ImageList imlDeviceIcons;
    }
}