namespace TataBuilder.actionsettings
{
    partial class ActionSettingIntervalFade
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
            this.panel6 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbEasingType = new System.Windows.Forms.ComboBox();
            this.cmbEasingMode = new System.Windows.Forms.ComboBox();
            this.pnlEndAlphaRow = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.nudEndAlpha = new System.Windows.Forms.NumericUpDown();
            this.pnlStartAlphaRow = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.nudStartAlpha = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.nudDuration = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel6.SuspendLayout();
            this.pnlEndAlphaRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndAlpha)).BeginInit();
            this.pnlStartAlphaRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartAlpha)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label7);
            this.panel6.Controls.Add(this.cmbEasingType);
            this.panel6.Controls.Add(this.cmbEasingMode);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 123);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(320, 25);
            this.panel6.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Ease Function";
            // 
            // cmbEasingType
            // 
            this.cmbEasingType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEasingType.FormattingEnabled = true;
            this.cmbEasingType.Items.AddRange(new object[] {
            "None",
            "Exponential",
            "Sine",
            "Elastic",
            "Bounce",
            "Back"});
            this.cmbEasingType.Location = new System.Drawing.Point(102, 1);
            this.cmbEasingType.Name = "cmbEasingType";
            this.cmbEasingType.Size = new System.Drawing.Size(100, 21);
            this.cmbEasingType.TabIndex = 11;
            this.cmbEasingType.Text = "None";
            this.cmbEasingType.SelectedIndexChanged += new System.EventHandler(this.SaveData);
            // 
            // cmbEasingMode
            // 
            this.cmbEasingMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEasingMode.FormattingEnabled = true;
            this.cmbEasingMode.Items.AddRange(new object[] {
            "In",
            "Out",
            "InOut"});
            this.cmbEasingMode.Location = new System.Drawing.Point(208, 1);
            this.cmbEasingMode.Name = "cmbEasingMode";
            this.cmbEasingMode.Size = new System.Drawing.Size(50, 21);
            this.cmbEasingMode.TabIndex = 12;
            this.cmbEasingMode.Text = "In";
            this.cmbEasingMode.SelectedIndexChanged += new System.EventHandler(this.SaveData);
            // 
            // pnlEndAlphaRow
            // 
            this.pnlEndAlphaRow.Controls.Add(this.label1);
            this.pnlEndAlphaRow.Controls.Add(this.nudEndAlpha);
            this.pnlEndAlphaRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEndAlphaRow.Location = new System.Drawing.Point(0, 98);
            this.pnlEndAlphaRow.Name = "pnlEndAlphaRow";
            this.pnlEndAlphaRow.Size = new System.Drawing.Size(320, 25);
            this.pnlEndAlphaRow.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "End Alpha";
            // 
            // nudEndAlpha
            // 
            this.nudEndAlpha.DecimalPlaces = 2;
            this.nudEndAlpha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudEndAlpha.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudEndAlpha.Location = new System.Drawing.Point(102, 0);
            this.nudEndAlpha.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEndAlpha.Name = "nudEndAlpha";
            this.nudEndAlpha.Size = new System.Drawing.Size(60, 20);
            this.nudEndAlpha.TabIndex = 9;
            this.nudEndAlpha.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEndAlpha.ValueChanged += new System.EventHandler(this.SaveData);
            // 
            // pnlStartAlphaRow
            // 
            this.pnlStartAlphaRow.Controls.Add(this.label6);
            this.pnlStartAlphaRow.Controls.Add(this.nudStartAlpha);
            this.pnlStartAlphaRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStartAlphaRow.Location = new System.Drawing.Point(0, 73);
            this.pnlStartAlphaRow.Name = "pnlStartAlphaRow";
            this.pnlStartAlphaRow.Size = new System.Drawing.Size(320, 25);
            this.pnlStartAlphaRow.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Start Alpha";
            // 
            // nudStartAlpha
            // 
            this.nudStartAlpha.DecimalPlaces = 2;
            this.nudStartAlpha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudStartAlpha.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudStartAlpha.Location = new System.Drawing.Point(102, 0);
            this.nudStartAlpha.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStartAlpha.Name = "nudStartAlpha";
            this.nudStartAlpha.Size = new System.Drawing.Size(60, 20);
            this.nudStartAlpha.TabIndex = 7;
            this.nudStartAlpha.ValueChanged += new System.EventHandler(this.SaveData);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.nudDuration);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 48);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(320, 25);
            this.panel3.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Duration (ms)";
            // 
            // nudDuration
            // 
            this.nudDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDuration.Location = new System.Drawing.Point(102, 0);
            this.nudDuration.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.nudDuration.Name = "nudDuration";
            this.nudDuration.Size = new System.Drawing.Size(70, 20);
            this.nudDuration.TabIndex = 5;
            this.nudDuration.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudDuration.ValueChanged += new System.EventHandler(this.SaveData);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.cmbType);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 22);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(320, 26);
            this.panel2.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Type";
            // 
            // cmbType
            // 
            this.cmbType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "TO",
            "FROMTO",
            "IN",
            "OUT"});
            this.cmbType.Location = new System.Drawing.Point(102, 0);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(92, 21);
            this.cmbType.TabIndex = 3;
            this.cmbType.Text = "TO";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(320, 22);
            this.panel1.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Action";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(99, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Fade";
            // 
            // ActionSettingIntervalFade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.pnlEndAlphaRow);
            this.Controls.Add(this.pnlStartAlphaRow);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ActionSettingIntervalFade";
            this.Size = new System.Drawing.Size(320, 180);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.pnlEndAlphaRow.ResumeLayout(false);
            this.pnlEndAlphaRow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndAlpha)).EndInit();
            this.pnlStartAlphaRow.ResumeLayout(false);
            this.pnlStartAlphaRow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartAlpha)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudStartAlpha;
        private System.Windows.Forms.NumericUpDown nudDuration;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudEndAlpha;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbEasingMode;
        private System.Windows.Forms.ComboBox cmbEasingType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnlStartAlphaRow;
        private System.Windows.Forms.Panel pnlEndAlphaRow;
        private System.Windows.Forms.Panel panel6;
    }
}
