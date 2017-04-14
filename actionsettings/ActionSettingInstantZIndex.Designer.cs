namespace TataBuilder.actionsettings
{
    partial class ActionSettingInstantZIndex
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
            this.nudZIndex = new System.Windows.Forms.NumericUpDown();
            this.cmbActor = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudZIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // nudZIndex
            // 
            this.nudZIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudZIndex.Location = new System.Drawing.Point(102, 48);
            this.nudZIndex.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudZIndex.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudZIndex.Name = "nudZIndex";
            this.nudZIndex.Size = new System.Drawing.Size(70, 20);
            this.nudZIndex.TabIndex = 5;
            this.nudZIndex.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudZIndex.ValueChanged += new System.EventHandler(this.SaveData);
            // 
            // cmbActor
            // 
            this.cmbActor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbActor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbActor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbActor.FormattingEnabled = true;
            this.cmbActor.Location = new System.Drawing.Point(102, 22);
            this.cmbActor.Name = "cmbActor";
            this.cmbActor.Size = new System.Drawing.Size(92, 21);
            this.cmbActor.TabIndex = 3;
            this.cmbActor.SelectedIndexChanged += new System.EventHandler(this.SaveData);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(99, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Change Z-Index";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "New Z-Index";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Actor";
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
            // ActionSettingInstantZIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudZIndex);
            this.Controls.Add(this.cmbActor);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Name = "ActionSettingInstantZIndex";
            this.Size = new System.Drawing.Size(320, 180);
            ((System.ComponentModel.ISupportInitialize)(this.nudZIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudZIndex;
        private System.Windows.Forms.ComboBox cmbActor;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;

    }
}
