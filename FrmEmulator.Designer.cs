namespace TataBuilder
{
    partial class FrmEmulator
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
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRelaunch = new System.Windows.Forms.Button();
            this.btnSnapshoot = new System.Windows.Forms.Button();
            this.pnlDisplayBox = new TataBuilder.DoubleBufferedPanel();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = global::TataBuilder.Properties.Resources.emulator_phone_exit;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Location = new System.Drawing.Point(635, 28);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(73, 72);
            this.btnExit.TabIndex = 4;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRelaunch
            // 
            this.btnRelaunch.BackColor = System.Drawing.Color.Transparent;
            this.btnRelaunch.BackgroundImage = global::TataBuilder.Properties.Resources.emulator_phone_relaunch;
            this.btnRelaunch.FlatAppearance.BorderSize = 0;
            this.btnRelaunch.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRelaunch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRelaunch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRelaunch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelaunch.Location = new System.Drawing.Point(635, 147);
            this.btnRelaunch.Name = "btnRelaunch";
            this.btnRelaunch.Size = new System.Drawing.Size(73, 72);
            this.btnRelaunch.TabIndex = 5;
            this.btnRelaunch.UseVisualStyleBackColor = false;
            this.btnRelaunch.Click += new System.EventHandler(this.btnRelaunch_Click);
            // 
            // btnSnapshoot
            // 
            this.btnSnapshoot.BackColor = System.Drawing.Color.Transparent;
            this.btnSnapshoot.BackgroundImage = global::TataBuilder.Properties.Resources.emulator_phone_snapshoot;
            this.btnSnapshoot.FlatAppearance.BorderSize = 0;
            this.btnSnapshoot.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSnapshoot.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSnapshoot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSnapshoot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSnapshoot.Location = new System.Drawing.Point(635, 276);
            this.btnSnapshoot.Name = "btnSnapshoot";
            this.btnSnapshoot.Size = new System.Drawing.Size(73, 72);
            this.btnSnapshoot.TabIndex = 6;
            this.btnSnapshoot.UseVisualStyleBackColor = false;
            this.btnSnapshoot.Click += new System.EventHandler(this.btnSnapshoot_Click);
            // 
            // pnlDisplayBox
            // 
            this.pnlDisplayBox.BackColor = System.Drawing.Color.Black;
            this.pnlDisplayBox.Location = new System.Drawing.Point(133, 28);
            this.pnlDisplayBox.Name = "pnlDisplayBox";
            this.pnlDisplayBox.Size = new System.Drawing.Size(480, 320);
            this.pnlDisplayBox.TabIndex = 3;
            this.pnlDisplayBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDisplayBox_Paint);
            this.pnlDisplayBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlDisplayBox_MouseDown);
            this.pnlDisplayBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlDisplayBox_MouseMove);
            this.pnlDisplayBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlDisplayBox_MouseUp);
            // 
            // FrmEmulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::TataBuilder.Properties.Resources.emulator_phone;
            this.ClientSize = new System.Drawing.Size(744, 380);
            this.Controls.Add(this.btnSnapshoot);
            this.Controls.Add(this.btnRelaunch);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.pnlDisplayBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmEmulator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmEmulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEmulator_FormClosing);
            this.Load += new System.EventHandler(this.FrmEmulator_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPanel pnlDisplayBox;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRelaunch;
        private System.Windows.Forms.Button btnSnapshoot;
    }
}