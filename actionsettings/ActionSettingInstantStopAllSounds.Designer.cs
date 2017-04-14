namespace TataBuilder.actionsettings
{
    partial class ActionSettingInstantStopAllSounds
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
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkBGM = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkEffect = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkVoice = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(99, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Stop All Sounds";
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
            // chkBGM
            // 
            this.chkBGM.AutoSize = true;
            this.chkBGM.Location = new System.Drawing.Point(131, 25);
            this.chkBGM.Name = "chkBGM";
            this.chkBGM.Size = new System.Drawing.Size(15, 14);
            this.chkBGM.TabIndex = 5;
            this.chkBGM.UseVisualStyleBackColor = true;
            this.chkBGM.CheckedChanged += new System.EventHandler(this.SaveData);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Background Music";
            // 
            // chkEffect
            // 
            this.chkEffect.AutoSize = true;
            this.chkEffect.Location = new System.Drawing.Point(131, 50);
            this.chkEffect.Name = "chkEffect";
            this.chkEffect.Size = new System.Drawing.Size(15, 14);
            this.chkEffect.TabIndex = 7;
            this.chkEffect.UseVisualStyleBackColor = true;
            this.chkEffect.CheckedChanged += new System.EventHandler(this.SaveData);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Effect Music";
            // 
            // chkVoice
            // 
            this.chkVoice.AutoSize = true;
            this.chkVoice.Location = new System.Drawing.Point(131, 75);
            this.chkVoice.Name = "chkVoice";
            this.chkVoice.Size = new System.Drawing.Size(15, 14);
            this.chkVoice.TabIndex = 9;
            this.chkVoice.UseVisualStyleBackColor = true;
            this.chkVoice.CheckedChanged += new System.EventHandler(this.SaveData);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Voice Music";
            // 
            // ActionSettingInstantStopAllSounds
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkVoice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkEffect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkBGM);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label2);
            this.Name = "ActionSettingInstantStopAllSounds";
            this.Size = new System.Drawing.Size(320, 180);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkBGM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkEffect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkVoice;
        private System.Windows.Forms.Label label3;
    }
}
