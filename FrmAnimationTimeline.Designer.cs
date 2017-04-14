namespace TataBuilder
{
    partial class FrmAnimationTimeline
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
            if (disposing && (components != null))
            {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAnimationTimeline));
            this.lblTarget = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnDeleteAction = new TataBuilder.FocuslessButton();
            this.btnRemoveSequence = new TataBuilder.FocuslessButton();
            this.btnAddSequence = new TataBuilder.FocuslessButton();
            this.trcTimeLineViewZoom = new System.Windows.Forms.TrackBar();
            this.tabActionsList = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvwIntervalActions = new System.Windows.Forms.ListView();
            this.imlActionsList = new System.Windows.Forms.ImageList(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lvwInstantActions = new System.Windows.Forms.ListView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lvwSoundActions = new System.Windows.Forms.ListView();
            this.grpActionSettings = new System.Windows.Forms.GroupBox();
            this.actionSettingIntervalBezier = new TataBuilder.actionsettings.ActionSettingIntervalBezier();
            this.actionSettingIntervalMove = new TataBuilder.actionsettings.ActionSettingIntervalMove();
            this.actionSettingIntervalScale = new TataBuilder.actionsettings.ActionSettingIntervalScale();
            this.actionSettingIntervalRotate = new TataBuilder.actionsettings.ActionSettingIntervalRotate();
            this.actionSettingIntervalFade = new TataBuilder.actionsettings.ActionSettingIntervalFade();
            this.actionSettingIntervalDelay = new TataBuilder.actionsettings.ActionSettingIntervalDelay();
            this.actionSettingIntervalAnimate = new TataBuilder.actionsettings.ActionSettingIntervalAnimate();
            this.actionSettingInstantZIndex = new TataBuilder.actionsettings.ActionSettingInstantZIndex();
            this.actionSettingInstantMakeAvatar = new TataBuilder.actionsettings.ActionSettingInstantMakeAvatar();
            this.actionSettingInstantClearAvatar = new TataBuilder.actionsettings.ActionSettingInstantClearAvatar();
            this.actionSettingInstantPlaySound = new TataBuilder.actionsettings.ActionSettingInstantPlaySound();
            this.actionSettingInstantStopSound = new TataBuilder.actionsettings.ActionSettingInstantStopSound();
            this.actionSettingInstantPlayVoice = new TataBuilder.actionsettings.ActionSettingInstantPlayVoice();
            this.actionSettingInstantStopVoice = new TataBuilder.actionsettings.ActionSettingInstantStopVoice();
            this.actionSettingInstantStopAllSounds = new TataBuilder.actionsettings.ActionSettingInstantStopAllSounds();
            this.actionSettingInstantStopAnimation = new TataBuilder.actionsettings.ActionSettingInstantStopAnimation();
            this.actionSettingInstantReadMode = new TataBuilder.actionsettings.ActionSettingInstantReadMode();
            this.actionSettingInstantGoScene = new TataBuilder.actionsettings.ActionSettingInstantGoScene();
            this.actionSettingInstantEnableBGM = new TataBuilder.actionsettings.ActionSettingInstantEnableBGM();
            this.actionSettingInstantEnableActor = new TataBuilder.actionsettings.ActionSettingInstantEnableActor();
            this.actionSettingInstantDispatchEvent = new TataBuilder.actionsettings.ActionSettingInstantDispatchEvent();
            this.actionSettingInstantChangeState = new TataBuilder.actionsettings.ActionSettingInstantChangeState();
            this.actionSettingInstantChangeBGM = new TataBuilder.actionsettings.ActionSettingInstantChangeBGM();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvwSequenceActions = new System.Windows.Forms.ListView();
            this.imlSequenceActions = new System.Windows.Forms.ImageList(this.components);
            this.timeLineView = new TataBuilder.TimeLineView();
            this.actionSettingInstantEnableSound = new TataBuilder.actionsettings.ActionSettingInstantEnableSound();
            this.actionSettingInstantEnableVoice = new TataBuilder.actionsettings.ActionSettingInstantEnableVoice();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trcTimeLineViewZoom)).BeginInit();
            this.tabActionsList.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.grpActionSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTarget.Location = new System.Drawing.Point(12, 19);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(170, 16);
            this.lblTarget.TabIndex = 0;
            this.lblTarget.Text = "New Actor (Touch - Default)";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackgroundImage = global::TataBuilder.Properties.Resources.timeline_header;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btnDeleteAction);
            this.panel1.Controls.Add(this.btnRemoveSequence);
            this.panel1.Controls.Add(this.btnAddSequence);
            this.panel1.Controls.Add(this.trcTimeLineViewZoom);
            this.panel1.Location = new System.Drawing.Point(637, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(305, 31);
            this.panel1.TabIndex = 7;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.ErrorImage = null;
            this.pictureBox2.Image = global::TataBuilder.Properties.Resources.zoom_out;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(137, 7);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(18, 18);
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::TataBuilder.Properties.Resources.zoom_in;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(274, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(18, 18);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // btnDeleteAction
            // 
            this.btnDeleteAction.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteAction.FlatAppearance.BorderSize = 0;
            this.btnDeleteAction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnDeleteAction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnDeleteAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAction.Image = global::TataBuilder.Properties.Resources.action_delete;
            this.btnDeleteAction.Location = new System.Drawing.Point(95, 1);
            this.btnDeleteAction.Name = "btnDeleteAction";
            this.btnDeleteAction.Size = new System.Drawing.Size(30, 30);
            this.btnDeleteAction.TabIndex = 10;
            this.btnDeleteAction.UseVisualStyleBackColor = false;
            this.btnDeleteAction.Click += new System.EventHandler(this.btnDeleteAction_Click);
            // 
            // btnRemoveSequence
            // 
            this.btnRemoveSequence.BackColor = System.Drawing.Color.Transparent;
            this.btnRemoveSequence.FlatAppearance.BorderSize = 0;
            this.btnRemoveSequence.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnRemoveSequence.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnRemoveSequence.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveSequence.Image = global::TataBuilder.Properties.Resources.row_delete;
            this.btnRemoveSequence.Location = new System.Drawing.Point(50, 1);
            this.btnRemoveSequence.Name = "btnRemoveSequence";
            this.btnRemoveSequence.Size = new System.Drawing.Size(30, 30);
            this.btnRemoveSequence.TabIndex = 9;
            this.btnRemoveSequence.UseVisualStyleBackColor = false;
            this.btnRemoveSequence.Click += new System.EventHandler(this.btnRemoveSequence_Click);
            // 
            // btnAddSequence
            // 
            this.btnAddSequence.BackColor = System.Drawing.Color.Transparent;
            this.btnAddSequence.FlatAppearance.BorderSize = 0;
            this.btnAddSequence.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnAddSequence.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btnAddSequence.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSequence.Image = global::TataBuilder.Properties.Resources.row_add_after;
            this.btnAddSequence.Location = new System.Drawing.Point(12, 1);
            this.btnAddSequence.Name = "btnAddSequence";
            this.btnAddSequence.Size = new System.Drawing.Size(30, 30);
            this.btnAddSequence.TabIndex = 8;
            this.btnAddSequence.UseVisualStyleBackColor = false;
            this.btnAddSequence.Click += new System.EventHandler(this.btnAddSequence_Click);
            // 
            // trcTimeLineViewZoom
            // 
            this.trcTimeLineViewZoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.trcTimeLineViewZoom.Location = new System.Drawing.Point(153, 5);
            this.trcTimeLineViewZoom.Minimum = 1;
            this.trcTimeLineViewZoom.Name = "trcTimeLineViewZoom";
            this.trcTimeLineViewZoom.Size = new System.Drawing.Size(120, 45);
            this.trcTimeLineViewZoom.TabIndex = 6;
            this.trcTimeLineViewZoom.TabStop = false;
            this.trcTimeLineViewZoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trcTimeLineViewZoom.Value = 2;
            this.trcTimeLineViewZoom.Scroll += new System.EventHandler(this.trcTimeLineViewZoom_Scroll);
            // 
            // tabActionsList
            // 
            this.tabActionsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabActionsList.Controls.Add(this.tabPage1);
            this.tabActionsList.Controls.Add(this.tabPage2);
            this.tabActionsList.Controls.Add(this.tabPage3);
            this.tabActionsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabActionsList.Location = new System.Drawing.Point(15, 50);
            this.tabActionsList.Multiline = true;
            this.tabActionsList.Name = "tabActionsList";
            this.tabActionsList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabActionsList.SelectedIndex = 0;
            this.tabActionsList.Size = new System.Drawing.Size(175, 503);
            this.tabActionsList.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lvwIntervalActions);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(167, 477);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Interval";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lvwIntervalActions
            // 
            this.lvwIntervalActions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwIntervalActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwIntervalActions.LargeImageList = this.imlActionsList;
            this.lvwIntervalActions.Location = new System.Drawing.Point(3, 15);
            this.lvwIntervalActions.MultiSelect = false;
            this.lvwIntervalActions.Name = "lvwIntervalActions";
            this.lvwIntervalActions.OwnerDraw = true;
            this.lvwIntervalActions.Size = new System.Drawing.Size(161, 459);
            this.lvwIntervalActions.TabIndex = 0;
            this.lvwIntervalActions.UseCompatibleStateImageBehavior = false;
            this.lvwIntervalActions.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.actionsList_DrawItem);
            this.lvwIntervalActions.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.actionList_ItemDrag);
            this.lvwIntervalActions.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.actionList_GiveFeedback);
            // 
            // imlActionsList
            // 
            this.imlActionsList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlActionsList.ImageStream")));
            this.imlActionsList.TransparentColor = System.Drawing.Color.Transparent;
            this.imlActionsList.Images.SetKeyName(0, "action_list_cell.png");
            this.imlActionsList.Images.SetKeyName(1, "action_list_cell_selected.png");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lvwInstantActions);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(167, 477);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Instant";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lvwInstantActions
            // 
            this.lvwInstantActions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwInstantActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwInstantActions.LargeImageList = this.imlActionsList;
            this.lvwInstantActions.Location = new System.Drawing.Point(3, 15);
            this.lvwInstantActions.Name = "lvwInstantActions";
            this.lvwInstantActions.OwnerDraw = true;
            this.lvwInstantActions.Size = new System.Drawing.Size(161, 459);
            this.lvwInstantActions.TabIndex = 1;
            this.lvwInstantActions.UseCompatibleStateImageBehavior = false;
            this.lvwInstantActions.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.actionsList_DrawItem);
            this.lvwInstantActions.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.actionList_ItemDrag);
            this.lvwInstantActions.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.actionList_GiveFeedback);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lvwSoundActions);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.tabPage3.Size = new System.Drawing.Size(167, 477);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Sound";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lvwSoundActions
            // 
            this.lvwSoundActions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwSoundActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwSoundActions.LargeImageList = this.imlActionsList;
            this.lvwSoundActions.Location = new System.Drawing.Point(3, 15);
            this.lvwSoundActions.Name = "lvwSoundActions";
            this.lvwSoundActions.OwnerDraw = true;
            this.lvwSoundActions.Size = new System.Drawing.Size(161, 459);
            this.lvwSoundActions.TabIndex = 1;
            this.lvwSoundActions.UseCompatibleStateImageBehavior = false;
            this.lvwSoundActions.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.actionsList_DrawItem);
            this.lvwSoundActions.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.actionList_ItemDrag);
            this.lvwSoundActions.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.actionList_GiveFeedback);
            // 
            // grpActionSettings
            // 
            this.grpActionSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpActionSettings.Controls.Add(this.actionSettingIntervalBezier);
            this.grpActionSettings.Controls.Add(this.actionSettingIntervalMove);
            this.grpActionSettings.Controls.Add(this.actionSettingIntervalScale);
            this.grpActionSettings.Controls.Add(this.actionSettingIntervalRotate);
            this.grpActionSettings.Controls.Add(this.actionSettingIntervalFade);
            this.grpActionSettings.Controls.Add(this.actionSettingIntervalDelay);
            this.grpActionSettings.Controls.Add(this.actionSettingIntervalAnimate);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantZIndex);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantMakeAvatar);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantClearAvatar);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantPlaySound);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantStopSound);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantPlayVoice);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantStopVoice);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantStopAllSounds);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantStopAnimation);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantReadMode);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantGoScene);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantEnableBGM);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantEnableVoice);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantEnableSound);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantEnableActor);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantDispatchEvent);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantChangeState);
            this.grpActionSettings.Controls.Add(this.actionSettingInstantChangeBGM);
            this.grpActionSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpActionSettings.Location = new System.Drawing.Point(478, 271);
            this.grpActionSettings.Name = "grpActionSettings";
            this.grpActionSettings.Padding = new System.Windows.Forms.Padding(10, 15, 10, 15);
            this.grpActionSettings.Size = new System.Drawing.Size(464, 282);
            this.grpActionSettings.TabIndex = 12;
            this.grpActionSettings.TabStop = false;
            this.grpActionSettings.Text = "Action Setting";
            // 
            // actionSettingIntervalBezier
            // 
            this.actionSettingIntervalBezier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingIntervalBezier.Location = new System.Drawing.Point(10, 30);
            this.actionSettingIntervalBezier.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingIntervalBezier.Name = "actionSettingIntervalBezier";
            this.actionSettingIntervalBezier.Size = new System.Drawing.Size(444, 237);
            this.actionSettingIntervalBezier.TabIndex = 14;
            // 
            // actionSettingIntervalMove
            // 
            this.actionSettingIntervalMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingIntervalMove.Location = new System.Drawing.Point(10, 30);
            this.actionSettingIntervalMove.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingIntervalMove.Name = "actionSettingIntervalMove";
            this.actionSettingIntervalMove.Size = new System.Drawing.Size(444, 237);
            this.actionSettingIntervalMove.TabIndex = 0;
            // 
            // actionSettingIntervalScale
            // 
            this.actionSettingIntervalScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingIntervalScale.Location = new System.Drawing.Point(10, 30);
            this.actionSettingIntervalScale.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingIntervalScale.Name = "actionSettingIntervalScale";
            this.actionSettingIntervalScale.Size = new System.Drawing.Size(444, 237);
            this.actionSettingIntervalScale.TabIndex = 18;
            // 
            // actionSettingIntervalRotate
            // 
            this.actionSettingIntervalRotate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingIntervalRotate.Location = new System.Drawing.Point(10, 30);
            this.actionSettingIntervalRotate.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingIntervalRotate.Name = "actionSettingIntervalRotate";
            this.actionSettingIntervalRotate.Size = new System.Drawing.Size(444, 237);
            this.actionSettingIntervalRotate.TabIndex = 17;
            // 
            // actionSettingIntervalFade
            // 
            this.actionSettingIntervalFade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingIntervalFade.Location = new System.Drawing.Point(10, 30);
            this.actionSettingIntervalFade.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingIntervalFade.Name = "actionSettingIntervalFade";
            this.actionSettingIntervalFade.Size = new System.Drawing.Size(444, 237);
            this.actionSettingIntervalFade.TabIndex = 16;
            // 
            // actionSettingIntervalDelay
            // 
            this.actionSettingIntervalDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingIntervalDelay.Location = new System.Drawing.Point(10, 30);
            this.actionSettingIntervalDelay.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingIntervalDelay.Name = "actionSettingIntervalDelay";
            this.actionSettingIntervalDelay.Size = new System.Drawing.Size(444, 237);
            this.actionSettingIntervalDelay.TabIndex = 15;
            // 
            // actionSettingIntervalAnimate
            // 
            this.actionSettingIntervalAnimate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingIntervalAnimate.Location = new System.Drawing.Point(10, 30);
            this.actionSettingIntervalAnimate.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingIntervalAnimate.Name = "actionSettingIntervalAnimate";
            this.actionSettingIntervalAnimate.Size = new System.Drawing.Size(444, 237);
            this.actionSettingIntervalAnimate.TabIndex = 13;
            // 
            // actionSettingInstantZIndex
            // 
            this.actionSettingInstantZIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantZIndex.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantZIndex.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantZIndex.Name = "actionSettingInstantZIndex";
            this.actionSettingInstantZIndex.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantZIndex.TabIndex = 12;
            // 
            // actionSettingInstantMakeAvatar
            // 
            this.actionSettingInstantMakeAvatar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantMakeAvatar.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantMakeAvatar.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantMakeAvatar.Name = "actionSettingInstantMakeAvatar";
            this.actionSettingInstantMakeAvatar.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantMakeAvatar.TabIndex = 12;
            // 
            // actionSettingInstantClearAvatar
            // 
            this.actionSettingInstantClearAvatar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantClearAvatar.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantClearAvatar.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantClearAvatar.Name = "actionSettingInstantClearAvatar";
            this.actionSettingInstantClearAvatar.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantClearAvatar.TabIndex = 12;
            // 
            // actionSettingInstantPlaySound
            // 
            this.actionSettingInstantPlaySound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantPlaySound.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantPlaySound.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantPlaySound.Name = "actionSettingInstantPlaySound";
            this.actionSettingInstantPlaySound.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantPlaySound.TabIndex = 7;
            // 
            // actionSettingInstantStopSound
            // 
            this.actionSettingInstantStopSound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantStopSound.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantStopSound.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantStopSound.Name = "actionSettingInstantStopSound";
            this.actionSettingInstantStopSound.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantStopSound.TabIndex = 11;
            // 
            // actionSettingInstantPlayVoice
            // 
            this.actionSettingInstantPlayVoice.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantPlayVoice.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantPlayVoice.Name = "actionSettingInstantPlayVoice";
            this.actionSettingInstantPlayVoice.Size = new System.Drawing.Size(422, 237);
            this.actionSettingInstantPlayVoice.TabIndex = 19;
            // 
            // actionSettingInstantStopVoice
            // 
            this.actionSettingInstantStopVoice.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantStopVoice.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantStopVoice.Name = "actionSettingInstantStopVoice";
            this.actionSettingInstantStopVoice.Size = new System.Drawing.Size(422, 237);
            this.actionSettingInstantStopVoice.TabIndex = 20;
            // 
            // actionSettingInstantStopAllSounds
            // 
            this.actionSettingInstantStopAllSounds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantStopAllSounds.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantStopAllSounds.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantStopAllSounds.Name = "actionSettingInstantStopAllSounds";
            this.actionSettingInstantStopAllSounds.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantStopAllSounds.TabIndex = 10;
            // 
            // actionSettingInstantStopAnimation
            // 
            this.actionSettingInstantStopAnimation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantStopAnimation.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantStopAnimation.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantStopAnimation.Name = "actionSettingInstantStopAnimation";
            this.actionSettingInstantStopAnimation.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantStopAnimation.TabIndex = 9;
            // 
            // actionSettingInstantReadMode
            // 
            this.actionSettingInstantReadMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantReadMode.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantReadMode.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantReadMode.Name = "actionSettingInstantReadMode";
            this.actionSettingInstantReadMode.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantReadMode.TabIndex = 8;
            // 
            // actionSettingInstantGoScene
            // 
            this.actionSettingInstantGoScene.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantGoScene.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantGoScene.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantGoScene.Name = "actionSettingInstantGoScene";
            this.actionSettingInstantGoScene.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantGoScene.TabIndex = 6;
            // 
            // actionSettingInstantEnableBGM
            // 
            this.actionSettingInstantEnableBGM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantEnableBGM.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantEnableBGM.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantEnableBGM.Name = "actionSettingInstantEnableBGM";
            this.actionSettingInstantEnableBGM.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantEnableBGM.TabIndex = 5;
            // 
            // actionSettingInstantEnableSound
            // 
            this.actionSettingInstantEnableSound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantEnableSound.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantEnableSound.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.actionSettingInstantEnableSound.Name = "actionSettingInstantEnableSound";
            this.actionSettingInstantEnableSound.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantEnableSound.TabIndex = 21;
            // 
            // actionSettingInstantEnableVoice
            // 
            this.actionSettingInstantEnableVoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantEnableVoice.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantEnableVoice.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.actionSettingInstantEnableVoice.Name = "actionSettingInstantEnableVoice";
            this.actionSettingInstantEnableVoice.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantEnableVoice.TabIndex = 22;
            // 
            // actionSettingInstantEnableActor
            // 
            this.actionSettingInstantEnableActor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantEnableActor.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantEnableActor.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantEnableActor.Name = "actionSettingInstantEnableActor";
            this.actionSettingInstantEnableActor.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantEnableActor.TabIndex = 4;
            // 
            // actionSettingInstantDispatchEvent
            // 
            this.actionSettingInstantDispatchEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantDispatchEvent.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantDispatchEvent.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantDispatchEvent.Name = "actionSettingInstantDispatchEvent";
            this.actionSettingInstantDispatchEvent.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantDispatchEvent.TabIndex = 3;
            // 
            // actionSettingInstantChangeState
            // 
            this.actionSettingInstantChangeState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantChangeState.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantChangeState.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantChangeState.Name = "actionSettingInstantChangeState";
            this.actionSettingInstantChangeState.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantChangeState.TabIndex = 2;
            // 
            // actionSettingInstantChangeBGM
            // 
            this.actionSettingInstantChangeBGM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionSettingInstantChangeBGM.Location = new System.Drawing.Point(10, 30);
            this.actionSettingInstantChangeBGM.Margin = new System.Windows.Forms.Padding(4);
            this.actionSettingInstantChangeBGM.Name = "actionSettingInstantChangeBGM";
            this.actionSettingInstantChangeBGM.Size = new System.Drawing.Size(444, 237);
            this.actionSettingInstantChangeBGM.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(716, 564);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(110, 30);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(832, 564);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 30);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.lvwSequenceActions);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(207, 271);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 282);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actions of Selected Sequence";
            // 
            // lvwSequenceActions
            // 
            this.lvwSequenceActions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwSequenceActions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwSequenceActions.FullRowSelect = true;
            this.lvwSequenceActions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwSequenceActions.HideSelection = false;
            this.lvwSequenceActions.LargeImageList = this.imlSequenceActions;
            this.lvwSequenceActions.Location = new System.Drawing.Point(6, 25);
            this.lvwSequenceActions.MultiSelect = false;
            this.lvwSequenceActions.Name = "lvwSequenceActions";
            this.lvwSequenceActions.Size = new System.Drawing.Size(243, 251);
            this.lvwSequenceActions.SmallImageList = this.imlSequenceActions;
            this.lvwSequenceActions.TabIndex = 1;
            this.lvwSequenceActions.UseCompatibleStateImageBehavior = false;
            this.lvwSequenceActions.View = System.Windows.Forms.View.Tile;
            this.lvwSequenceActions.SelectedIndexChanged += new System.EventHandler(this.lvwSequenceActions_SelectedIndexChanged);
            // 
            // imlSequenceActions
            // 
            this.imlSequenceActions.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imlSequenceActions.ImageSize = new System.Drawing.Size(32, 32);
            this.imlSequenceActions.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // timeLineView
            // 
            this.timeLineView.AllowDrop = true;
            this.timeLineView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeLineView.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.timeLineView.FixedColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.timeLineView.GuidelineColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.timeLineView.GuidelineFixedColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.timeLineView.GuidelineSpacing = 40F;
            this.timeLineView.InstantMarkIcon = global::TataBuilder.Properties.Resources.icon_instant_item;
            this.timeLineView.ItemSplitColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.timeLineView.Location = new System.Drawing.Point(207, 50);
            this.timeLineView.Name = "timeLineView";
            this.timeLineView.RowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.timeLineView.RowBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.timeLineView.RowHeight = 60;
            this.timeLineView.RowMarkBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.timeLineView.RowMarkBarBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.timeLineView.RowSettingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(207)))), ((int)(((byte)(207)))));
            this.timeLineView.RowSettingBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(175)))), ((int)(((byte)(175)))));
            this.timeLineView.SelectedRowBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(175)))), ((int)(((byte)(255)))));
            this.timeLineView.Size = new System.Drawing.Size(735, 205);
            this.timeLineView.TabIndex = 8;
            this.timeLineView.WidthPerTimeScale = 40F;
            this.timeLineView.WorkgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.timeLineView.Zoom = 2;
            this.timeLineView.DrawRowSetting += new System.Windows.Forms.DrawItemEventHandler(this.timeLineView_DrawRowSetting);
            this.timeLineView.RowSettingClick += new System.EventHandler(this.timeLineView_RowSettingClick);
            this.timeLineView.ItemLengthChanged += new TataBuilder.LengthChangedEventHandler(this.timeLineView_ItemLengthChanged);
            this.timeLineView.ItemMoved += new TataBuilder.ItemMovedEventHandler(this.timeLineView_ItemMoved);
            this.timeLineView.RowSelectionChanged += new System.EventHandler(this.timeLineView_RowSelectionChanged);
            this.timeLineView.SelectionChanged += new System.EventHandler(this.timeLineView_SelectionChanged);
            this.timeLineView.DataChanged += new System.EventHandler(this.timeLineView_DataChanged);
            this.timeLineView.DragDrop += new System.Windows.Forms.DragEventHandler(this.timeLineView_DragDrop);
            this.timeLineView.DragOver += new System.Windows.Forms.DragEventHandler(this.timeLineView_DragOver);
            this.timeLineView.DragLeave += new System.EventHandler(this.timeLineView_DragLeave);
            // 
            // FrmAnimationTimeline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(955, 606);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabActionsList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpActionSettings);
            this.Controls.Add(this.timeLineView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTarget);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "FrmAnimationTimeline";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Animation Timeline";
            this.Load += new System.EventHandler(this.FrmAnimationTimeline_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trcTimeLineViewZoom)).EndInit();
            this.tabActionsList.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.grpActionSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.Panel panel1;
        private TimeLineView timeLineView;
        private System.Windows.Forms.TrackBar trcTimeLineViewZoom;
        private System.Windows.Forms.TabControl tabActionsList;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox grpActionSettings;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private FocuslessButton btnDeleteAction;
        private FocuslessButton btnRemoveSequence;
        private FocuslessButton btnAddSequence;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListView lvwIntervalActions;
        private System.Windows.Forms.ListView lvwInstantActions;
        private System.Windows.Forms.ListView lvwSoundActions;
        private System.Windows.Forms.ImageList imlActionsList;
        private actionsettings.ActionSettingIntervalMove actionSettingIntervalMove;
        private actionsettings.ActionSettingIntervalScale actionSettingIntervalScale;
        private actionsettings.ActionSettingIntervalRotate actionSettingIntervalRotate;
        private actionsettings.ActionSettingIntervalFade actionSettingIntervalFade;
        private actionsettings.ActionSettingIntervalDelay actionSettingIntervalDelay;
        private actionsettings.ActionSettingIntervalBezier actionSettingIntervalBezier;
        private actionsettings.ActionSettingIntervalAnimate actionSettingIntervalAnimate;
        private actionsettings.ActionSettingInstantZIndex actionSettingInstantZIndex;
        private actionsettings.ActionSettingInstantMakeAvatar actionSettingInstantMakeAvatar;
        private actionsettings.ActionSettingInstantClearAvatar actionSettingInstantClearAvatar;
        private actionsettings.ActionSettingInstantStopSound actionSettingInstantStopSound;
        private actionsettings.ActionSettingInstantStopAllSounds actionSettingInstantStopAllSounds;
        private actionsettings.ActionSettingInstantStopAnimation actionSettingInstantStopAnimation;
        private actionsettings.ActionSettingInstantReadMode actionSettingInstantReadMode;
        private actionsettings.ActionSettingInstantPlaySound actionSettingInstantPlaySound;
        private actionsettings.ActionSettingInstantGoScene actionSettingInstantGoScene;
        private actionsettings.ActionSettingInstantEnableBGM actionSettingInstantEnableBGM;
        private actionsettings.ActionSettingInstantEnableActor actionSettingInstantEnableActor;
        private actionsettings.ActionSettingInstantDispatchEvent actionSettingInstantDispatchEvent;
        private actionsettings.ActionSettingInstantChangeState actionSettingInstantChangeState;
        private actionsettings.ActionSettingInstantChangeBGM actionSettingInstantChangeBGM;
        private actionsettings.ActionSettingInstantPlayVoice actionSettingInstantPlayVoice;
        private actionsettings.ActionSettingInstantStopVoice actionSettingInstantStopVoice;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvwSequenceActions;
        private System.Windows.Forms.ImageList imlSequenceActions;
        private actionsettings.ActionSettingInstantEnableVoice actionSettingInstantEnableVoice;
        private actionsettings.ActionSettingInstantEnableSound actionSettingInstantEnableSound;
    }
}