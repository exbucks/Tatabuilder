namespace TataBuilder.actionsettings
{
    partial class ActionSettingInstantGoScene
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
            this.cmbScene = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.radPreviousScene = new System.Windows.Forms.RadioButton();
            this.radNextScene = new System.Windows.Forms.RadioButton();
            this.radCoverScene = new System.Windows.Forms.RadioButton();
            this.radSpecificScene = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // cmbScene
            // 
            this.cmbScene.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbScene.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbScene.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbScene.FormattingEnabled = true;
            this.cmbScene.Location = new System.Drawing.Point(143, 122);
            this.cmbScene.Name = "cmbScene";
            this.cmbScene.Size = new System.Drawing.Size(92, 21);
            this.cmbScene.TabIndex = 8;
            this.cmbScene.SelectedIndexChanged += new System.EventHandler(this.SaveData);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(99, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Go to Scene";
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
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(99, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Scene";
            // 
            // radPreviousScene
            // 
            this.radPreviousScene.AutoSize = true;
            this.radPreviousScene.Checked = true;
            this.radPreviousScene.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radPreviousScene.Location = new System.Drawing.Point(102, 23);
            this.radPreviousScene.Name = "radPreviousScene";
            this.radPreviousScene.Size = new System.Drawing.Size(100, 17);
            this.radPreviousScene.TabIndex = 3;
            this.radPreviousScene.TabStop = true;
            this.radPreviousScene.Text = "Previous Scene";
            this.radPreviousScene.UseVisualStyleBackColor = true;
            this.radPreviousScene.CheckedChanged += new System.EventHandler(this.SaveData);
            // 
            // radNextScene
            // 
            this.radNextScene.AutoSize = true;
            this.radNextScene.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radNextScene.Location = new System.Drawing.Point(102, 48);
            this.radNextScene.Name = "radNextScene";
            this.radNextScene.Size = new System.Drawing.Size(81, 17);
            this.radNextScene.TabIndex = 4;
            this.radNextScene.Text = "Next Scene";
            this.radNextScene.UseVisualStyleBackColor = true;
            this.radNextScene.CheckedChanged += new System.EventHandler(this.SaveData);
            // 
            // radCoverScene
            // 
            this.radCoverScene.AutoSize = true;
            this.radCoverScene.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radCoverScene.Location = new System.Drawing.Point(102, 73);
            this.radCoverScene.Name = "radCoverScene";
            this.radCoverScene.Size = new System.Drawing.Size(87, 17);
            this.radCoverScene.TabIndex = 5;
            this.radCoverScene.Text = "Cover Scene";
            this.radCoverScene.UseVisualStyleBackColor = true;
            this.radCoverScene.CheckedChanged += new System.EventHandler(this.SaveData);
            // 
            // radSpecificScene
            // 
            this.radSpecificScene.AutoSize = true;
            this.radSpecificScene.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radSpecificScene.Location = new System.Drawing.Point(102, 98);
            this.radSpecificScene.Name = "radSpecificScene";
            this.radSpecificScene.Size = new System.Drawing.Size(97, 17);
            this.radSpecificScene.TabIndex = 6;
            this.radSpecificScene.Text = "Specific Scene";
            this.radSpecificScene.UseVisualStyleBackColor = true;
            this.radSpecificScene.CheckedChanged += new System.EventHandler(this.SaveData);
            // 
            // ActionSettingInstantGoScene
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radSpecificScene);
            this.Controls.Add(this.radCoverScene);
            this.Controls.Add(this.radNextScene);
            this.Controls.Add(this.radPreviousScene);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbScene);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Name = "ActionSettingInstantGoScene";
            this.Size = new System.Drawing.Size(320, 180);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbScene;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radPreviousScene;
        private System.Windows.Forms.RadioButton radNextScene;
        private System.Windows.Forms.RadioButton radCoverScene;
        private System.Windows.Forms.RadioButton radSpecificScene;
    }
}
