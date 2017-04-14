namespace TataBuilder.actionsettings
{
    partial class ActionSettingIntervalBezier
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
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.nudControlPoint3Y = new System.Windows.Forms.NumericUpDown();
            this.nudControlPoint3X = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.nudControlPoint2Y = new System.Windows.Forms.NumericUpDown();
            this.nudControlPoint2X = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudControlPoint1Y = new System.Windows.Forms.NumericUpDown();
            this.nudControlPoint1X = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbEasingMode = new System.Windows.Forms.ComboBox();
            this.cmbEasingType = new System.Windows.Forms.ComboBox();
            this.nudDuration = new System.Windows.Forms.NumericUpDown();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint3Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint3X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint2Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint2X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint1Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint1X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(178, 150);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(14, 13);
            this.label14.TabIndex = 20;
            this.label14.Text = "Y";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(82, 150);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(14, 13);
            this.label15.TabIndex = 18;
            this.label15.Text = "X";
            // 
            // nudControlPoint3Y
            // 
            this.nudControlPoint3Y.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudControlPoint3Y.Location = new System.Drawing.Point(198, 148);
            this.nudControlPoint3Y.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudControlPoint3Y.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudControlPoint3Y.Name = "nudControlPoint3Y";
            this.nudControlPoint3Y.Size = new System.Drawing.Size(70, 20);
            this.nudControlPoint3Y.TabIndex = 21;
            this.nudControlPoint3Y.ValueChanged += new System.EventHandler(this.SaveData);
            // 
            // nudControlPoint3X
            // 
            this.nudControlPoint3X.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudControlPoint3X.Location = new System.Drawing.Point(102, 148);
            this.nudControlPoint3X.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudControlPoint3X.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudControlPoint3X.Name = "nudControlPoint3X";
            this.nudControlPoint3X.Size = new System.Drawing.Size(70, 20);
            this.nudControlPoint3X.TabIndex = 19;
            this.nudControlPoint3X.ValueChanged += new System.EventHandler(this.SaveData);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(13, 150);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(66, 13);
            this.label16.TabIndex = 17;
            this.label16.Text = "End Position";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(178, 125);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Y";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(82, 125);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "X";
            // 
            // nudControlPoint2Y
            // 
            this.nudControlPoint2Y.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudControlPoint2Y.Location = new System.Drawing.Point(198, 123);
            this.nudControlPoint2Y.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudControlPoint2Y.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudControlPoint2Y.Name = "nudControlPoint2Y";
            this.nudControlPoint2Y.Size = new System.Drawing.Size(70, 20);
            this.nudControlPoint2Y.TabIndex = 16;
            this.nudControlPoint2Y.ValueChanged += new System.EventHandler(this.SaveData);
            // 
            // nudControlPoint2X
            // 
            this.nudControlPoint2X.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudControlPoint2X.Location = new System.Drawing.Point(102, 123);
            this.nudControlPoint2X.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudControlPoint2X.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudControlPoint2X.Name = "nudControlPoint2X";
            this.nudControlPoint2X.Size = new System.Drawing.Size(70, 20);
            this.nudControlPoint2X.TabIndex = 14;
            this.nudControlPoint2X.ValueChanged += new System.EventHandler(this.SaveData);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(13, 125);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Control P. 2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(178, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(82, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "X";
            // 
            // nudControlPoint1Y
            // 
            this.nudControlPoint1Y.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudControlPoint1Y.Location = new System.Drawing.Point(198, 98);
            this.nudControlPoint1Y.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudControlPoint1Y.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudControlPoint1Y.Name = "nudControlPoint1Y";
            this.nudControlPoint1Y.Size = new System.Drawing.Size(70, 20);
            this.nudControlPoint1Y.TabIndex = 11;
            this.nudControlPoint1Y.ValueChanged += new System.EventHandler(this.SaveData);
            // 
            // nudControlPoint1X
            // 
            this.nudControlPoint1X.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudControlPoint1X.Location = new System.Drawing.Point(102, 98);
            this.nudControlPoint1X.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudControlPoint1X.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudControlPoint1X.Name = "nudControlPoint1X";
            this.nudControlPoint1X.Size = new System.Drawing.Size(70, 20);
            this.nudControlPoint1X.TabIndex = 9;
            this.nudControlPoint1X.ValueChanged += new System.EventHandler(this.SaveData);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(13, 100);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "Control P. 1";
            // 
            // cmbEasingMode
            // 
            this.cmbEasingMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEasingMode.FormattingEnabled = true;
            this.cmbEasingMode.Items.AddRange(new object[] {
            "In",
            "Out",
            "InOut"});
            this.cmbEasingMode.Location = new System.Drawing.Point(208, 172);
            this.cmbEasingMode.Name = "cmbEasingMode";
            this.cmbEasingMode.Size = new System.Drawing.Size(50, 21);
            this.cmbEasingMode.TabIndex = 24;
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
            this.cmbEasingType.Location = new System.Drawing.Point(102, 172);
            this.cmbEasingType.Name = "cmbEasingType";
            this.cmbEasingType.Size = new System.Drawing.Size(100, 21);
            this.cmbEasingType.TabIndex = 23;
            this.cmbEasingType.Text = "None";
            this.cmbEasingType.SelectedIndexChanged += new System.EventHandler(this.SaveData);
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
            this.label9.Size = new System.Drawing.Size(104, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Bezier Movement";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Ease Function";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Bezier Positions";
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
            // ActionSettingIntervalBezier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.nudControlPoint3Y);
            this.Controls.Add(this.nudControlPoint3X);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.nudControlPoint2Y);
            this.Controls.Add(this.nudControlPoint2X);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudControlPoint1Y);
            this.Controls.Add(this.nudControlPoint1X);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cmbEasingMode);
            this.Controls.Add(this.cmbEasingType);
            this.Controls.Add(this.nudDuration);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Name = "ActionSettingIntervalBezier";
            this.Size = new System.Drawing.Size(320, 238);
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint3Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint3X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint2Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint2X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint1Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudControlPoint1X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbEasingMode;
        private System.Windows.Forms.ComboBox cmbEasingType;
        private System.Windows.Forms.NumericUpDown nudDuration;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudControlPoint1Y;
        private System.Windows.Forms.NumericUpDown nudControlPoint1X;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nudControlPoint2Y;
        private System.Windows.Forms.NumericUpDown nudControlPoint2X;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown nudControlPoint3Y;
        private System.Windows.Forms.NumericUpDown nudControlPoint3X;
        private System.Windows.Forms.Label label16;
    }
}
