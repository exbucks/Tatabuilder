namespace TataBuilder.actionsettings
{
    partial class ActionSettingInstantReadMode
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
            this.radAutoplay = new System.Windows.Forms.RadioButton();
            this.radReadByMyself = new System.Windows.Forms.RadioButton();
            this.radReadToMe = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // radAutoplay
            // 
            this.radAutoplay.AutoSize = true;
            this.radAutoplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radAutoplay.Location = new System.Drawing.Point(102, 72);
            this.radAutoplay.Name = "radAutoplay";
            this.radAutoplay.Size = new System.Drawing.Size(66, 17);
            this.radAutoplay.TabIndex = 5;
            this.radAutoplay.Text = "Autoplay";
            this.radAutoplay.UseVisualStyleBackColor = true;
            this.radAutoplay.CheckedChanged += new System.EventHandler(this.SaveData);
            // 
            // radReadByMyself
            // 
            this.radReadByMyself.AutoSize = true;
            this.radReadByMyself.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radReadByMyself.Location = new System.Drawing.Point(102, 48);
            this.radReadByMyself.Name = "radReadByMyself";
            this.radReadByMyself.Size = new System.Drawing.Size(97, 17);
            this.radReadByMyself.TabIndex = 4;
            this.radReadByMyself.Text = "Read by myself";
            this.radReadByMyself.UseVisualStyleBackColor = true;
            this.radReadByMyself.CheckedChanged += new System.EventHandler(this.SaveData);
            // 
            // radReadToMe
            // 
            this.radReadToMe.AutoSize = true;
            this.radReadToMe.Checked = true;
            this.radReadToMe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radReadToMe.Location = new System.Drawing.Point(102, 23);
            this.radReadToMe.Name = "radReadToMe";
            this.radReadToMe.Size = new System.Drawing.Size(80, 17);
            this.radReadToMe.TabIndex = 3;
            this.radReadToMe.TabStop = true;
            this.radReadToMe.Text = "Read to me";
            this.radReadToMe.UseVisualStyleBackColor = true;
            this.radReadToMe.CheckedChanged += new System.EventHandler(this.SaveData);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(99, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Change Read Mode";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Mode";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Action";
            // 
            // ActionSettingInstantReadMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radAutoplay);
            this.Controls.Add(this.radReadByMyself);
            this.Controls.Add(this.radReadToMe);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Name = "ActionSettingInstantReadMode";
            this.Size = new System.Drawing.Size(320, 180);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radAutoplay;
        private System.Windows.Forms.RadioButton radReadByMyself;
        private System.Windows.Forms.RadioButton radReadToMe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;

    }
}
