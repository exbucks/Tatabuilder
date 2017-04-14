namespace TataBuilder.actionsettings
{
    partial class ActionSettingIntervalRotate
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
            this.nudAngle = new System.Windows.Forms.NumericUpDown();
            this.nudDuration = new System.Windows.Forms.NumericUpDown();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbEasingMode = new System.Windows.Forms.ComboBox();
            this.cmbEasingType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // nudAngle
            // 
            this.nudAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudAngle.Location = new System.Drawing.Point(102, 73);
            this.nudAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudAngle.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.nudAngle.Name = "nudAngle";
            this.nudAngle.Size = new System.Drawing.Size(70, 20);
            this.nudAngle.TabIndex = 7;
            this.nudAngle.ValueChanged += new System.EventHandler(this.SaveData);
            // 
            // nudDuration
            // 
            this.nudDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDuration.Location = new System.Drawing.Point(102, 48);
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
            // cmbType
            // 
            this.cmbType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "TO",
            "BY"});
            this.cmbType.Location = new System.Drawing.Point(102, 22);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(92, 21);
            this.cmbType.TabIndex = 3;
            this.cmbType.Text = "TO";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.SaveData);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(99, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Rotate";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Angle";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Duration (ms)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Type";
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
            // cmbEasingMode
            // 
            this.cmbEasingMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEasingMode.FormattingEnabled = true;
            this.cmbEasingMode.Items.AddRange(new object[] {
            "In",
            "Out",
            "InOut"});
            this.cmbEasingMode.Location = new System.Drawing.Point(208, 97);
            this.cmbEasingMode.Name = "cmbEasingMode";
            this.cmbEasingMode.Size = new System.Drawing.Size(50, 21);
            this.cmbEasingMode.TabIndex = 10;
            this.cmbEasingMode.Text = "In";
            this.cmbEasingMode.SelectedIndexChanged += new System.EventHandler(this.SaveData);
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
            this.cmbEasingType.Location = new System.Drawing.Point(102, 97);
            this.cmbEasingType.Name = "cmbEasingType";
            this.cmbEasingType.Size = new System.Drawing.Size(100, 21);
            this.cmbEasingType.TabIndex = 9;
            this.cmbEasingType.Text = "None";
            this.cmbEasingType.SelectedIndexChanged += new System.EventHandler(this.SaveData);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Ease Function";
            // 
            // ActionSettingIntervalRotate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbEasingMode);
            this.Controls.Add(this.cmbEasingType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nudAngle);
            this.Controls.Add(this.nudDuration);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Name = "ActionSettingIntervalRotate";
            this.Size = new System.Drawing.Size(320, 180);
            ((System.ComponentModel.ISupportInitialize)(this.nudAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudAngle;
        private System.Windows.Forms.NumericUpDown nudDuration;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbEasingMode;
        private System.Windows.Forms.ComboBox cmbEasingType;
        private System.Windows.Forms.Label label7;
    }
}
