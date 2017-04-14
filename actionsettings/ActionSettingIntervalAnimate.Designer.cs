namespace TataBuilder.actionsettings
{
    partial class ActionSettingIntervalAnimate
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
            this.nudDuration = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lvwFrames = new System.Windows.Forms.ListView();
            this.btnMoveUpFrame = new System.Windows.Forms.Button();
            this.btnMoveDownFrame = new System.Windows.Forms.Button();
            this.btnAddFrame = new System.Windows.Forms.Button();
            this.btnRemoveFrame = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // nudDuration
            // 
            this.nudDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDuration.Location = new System.Drawing.Point(136, 29);
            this.nudDuration.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.nudDuration.Name = "nudDuration";
            this.nudDuration.Size = new System.Drawing.Size(93, 20);
            this.nudDuration.TabIndex = 5;
            this.nudDuration.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudDuration.ValueChanged += new System.EventHandler(this.nudDuration_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(133, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Animate";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Duration (ms)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Action";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Frames";
            // 
            // lvwFrames
            // 
            this.lvwFrames.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lvwFrames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwFrames.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwFrames.HideSelection = false;
            this.lvwFrames.Location = new System.Drawing.Point(7, 78);
            this.lvwFrames.Name = "lvwFrames";
            this.lvwFrames.Size = new System.Drawing.Size(304, 100);
            this.lvwFrames.TabIndex = 7;
            this.lvwFrames.UseCompatibleStateImageBehavior = false;
            this.lvwFrames.View = System.Windows.Forms.View.Tile;
            this.lvwFrames.SelectedIndexChanged += new System.EventHandler(this.lvwFrames_SelectedIndexChanged);
            // 
            // btnMoveUpFrame
            // 
            this.btnMoveUpFrame.BackgroundImage = global::TataBuilder.Properties.Resources.btn_small_left;
            this.btnMoveUpFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMoveUpFrame.FlatAppearance.BorderSize = 0;
            this.btnMoveUpFrame.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMoveUpFrame.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMoveUpFrame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveUpFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveUpFrame.Location = new System.Drawing.Point(7, 184);
            this.btnMoveUpFrame.Name = "btnMoveUpFrame";
            this.btnMoveUpFrame.Size = new System.Drawing.Size(30, 16);
            this.btnMoveUpFrame.TabIndex = 8;
            this.btnMoveUpFrame.UseVisualStyleBackColor = true;
            this.btnMoveUpFrame.Click += new System.EventHandler(this.btnMoveUpFrame_Click);
            // 
            // btnMoveDownFrame
            // 
            this.btnMoveDownFrame.BackgroundImage = global::TataBuilder.Properties.Resources.btn_small_right;
            this.btnMoveDownFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMoveDownFrame.FlatAppearance.BorderSize = 0;
            this.btnMoveDownFrame.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMoveDownFrame.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMoveDownFrame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveDownFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveDownFrame.Location = new System.Drawing.Point(43, 184);
            this.btnMoveDownFrame.Name = "btnMoveDownFrame";
            this.btnMoveDownFrame.Size = new System.Drawing.Size(30, 16);
            this.btnMoveDownFrame.TabIndex = 9;
            this.btnMoveDownFrame.UseVisualStyleBackColor = true;
            this.btnMoveDownFrame.Click += new System.EventHandler(this.btnMoveDownFrame_Click);
            // 
            // btnAddFrame
            // 
            this.btnAddFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFrame.BackgroundImage = global::TataBuilder.Properties.Resources.btn_small_add;
            this.btnAddFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAddFrame.FlatAppearance.BorderSize = 0;
            this.btnAddFrame.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddFrame.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddFrame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddFrame.Location = new System.Drawing.Point(245, 184);
            this.btnAddFrame.Name = "btnAddFrame";
            this.btnAddFrame.Size = new System.Drawing.Size(30, 16);
            this.btnAddFrame.TabIndex = 10;
            this.btnAddFrame.UseVisualStyleBackColor = true;
            this.btnAddFrame.Click += new System.EventHandler(this.btnAddFrame_Click);
            // 
            // btnRemoveFrame
            // 
            this.btnRemoveFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveFrame.BackgroundImage = global::TataBuilder.Properties.Resources.btn_small_delete;
            this.btnRemoveFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRemoveFrame.FlatAppearance.BorderSize = 0;
            this.btnRemoveFrame.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRemoveFrame.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRemoveFrame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveFrame.Location = new System.Drawing.Point(281, 184);
            this.btnRemoveFrame.Name = "btnRemoveFrame";
            this.btnRemoveFrame.Size = new System.Drawing.Size(30, 16);
            this.btnRemoveFrame.TabIndex = 11;
            this.btnRemoveFrame.UseVisualStyleBackColor = true;
            this.btnRemoveFrame.Click += new System.EventHandler(this.btnRemoveFrame_Click);
            // 
            // ActionSettingIntervalAnimate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnRemoveFrame);
            this.Controls.Add(this.btnAddFrame);
            this.Controls.Add(this.btnMoveDownFrame);
            this.Controls.Add(this.btnMoveUpFrame);
            this.Controls.Add(this.lvwFrames);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudDuration);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Name = "ActionSettingIntervalAnimate";
            this.Size = new System.Drawing.Size(320, 236);
            ((System.ComponentModel.ISupportInitialize)(this.nudDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudDuration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvwFrames;
        private System.Windows.Forms.Button btnMoveUpFrame;
        private System.Windows.Forms.Button btnMoveDownFrame;
        private System.Windows.Forms.Button btnAddFrame;
        private System.Windows.Forms.Button btnRemoveFrame;

    }
}
