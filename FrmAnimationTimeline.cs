using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TataBuilder.actionsettings;

namespace TataBuilder
{
    public partial class FrmAnimationTimeline : Form
    {

        #region Win32API

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int SetWindowTheme(IntPtr hWnd, string appName, string partList);

        const int LVM_FIRST = 0x1000;
        const int LVM_SETICONSPACING = LVM_FIRST + 53;

        public int MakeLong(short lowPart, short highPart)
        {
            return (int)(((ushort)lowPart) | (uint)(highPart << 16));
        }

        #endregion

        public TDocument document { get; set; }
        public TAnimation animation { get; set; }

        Cursor draggingCursor;

        public FrmAnimationTimeline()
        {
            InitializeComponent();

            SetWindowTheme(lvwSequenceActions.Handle, "explorer", null);
        }

        private void FrmAnimationTimeline_Load(object sender, EventArgs e)
        {
            // show target
            lblTarget.Text = String.Format("{0} ({1} - {2})", animation.layer.name, animation.eventu, animation.state);

            // set data of time line view
            timeLineView.DataSource = animation;
            timeLineView.NotifyDataSourceChanged();

            // initialize interval actions list
            SendMessage(lvwIntervalActions.Handle, LVM_SETICONSPACING, IntPtr.Zero, (IntPtr)MakeLong(70, 70));
            addActionsListItem(lvwIntervalActions, new TActionIntervalBezier());
            addActionsListItem(lvwIntervalActions, new TActionIntervalDelay());
            addActionsListItem(lvwIntervalActions, new TActionIntervalFade());
            addActionsListItem(lvwIntervalActions, new TActionIntervalMove());
            addActionsListItem(lvwIntervalActions, new TActionIntervalRotate());
            addActionsListItem(lvwIntervalActions, new TActionIntervalScale());
            addActionsListItem(lvwIntervalActions, new TActionIntervalAnimate());

            // initialize instant actions list
            SendMessage(lvwInstantActions.Handle, LVM_SETICONSPACING, IntPtr.Zero, (IntPtr)MakeLong(70, 70));
            addActionsListItem(lvwInstantActions, new TActionInstantDispatchEvent());
            addActionsListItem(lvwInstantActions, new TActionInstantChangeState());
            addActionsListItem(lvwInstantActions, new TActionInstantEnableActor());
            addActionsListItem(lvwInstantActions, new TActionInstantStopAnimation());
            addActionsListItem(lvwInstantActions, new TActionInstantGoScene());
            addActionsListItem(lvwInstantActions, new TActionInstantReadMode());
            addActionsListItem(lvwInstantActions, new TActionInstantZIndex());
            addActionsListItem(lvwInstantActions, new TActionInstantMakeAvatar());
            addActionsListItem(lvwInstantActions, new TActionInstantClearAvatar());

            // initialize sound actions list
            SendMessage(lvwSoundActions.Handle, LVM_SETICONSPACING, IntPtr.Zero, (IntPtr)MakeLong(70, 70));
            addActionsListItem(lvwSoundActions, new TActionInstantPlaySound());
            addActionsListItem(lvwSoundActions, new TActionInstantStopSound());
            addActionsListItem(lvwSoundActions, new TActionInstantPlayVoice());
            addActionsListItem(lvwSoundActions, new TActionInstantStopVoice());
            addActionsListItem(lvwSoundActions, new TActionInstantEnableSound());
            addActionsListItem(lvwSoundActions, new TActionInstantEnableVoice());
            addActionsListItem(lvwSoundActions, new TActionInstantEnableBGM());
            addActionsListItem(lvwSoundActions, new TActionInstantChangeBGM());
            addActionsListItem(lvwSoundActions, new TActionInstantStopAllSounds());

            // initialize action settings panel
            timeLineView_SelectionChanged(this, null);
        }

        private void addActionsListItem(ListView listView, TAction action)
        {
            ListViewItem item = listView.Items.Add(action.name, 0);
            if (item != null) {
                item.Tag = action;
            }
        }

        private void btnAddSequence_Click(object sender, EventArgs e)
        {
            animation.addSequence();
            timeLineView.ClearSelection();
            timeLineView.NotifyDataSourceChanged();
        }

        private void btnRemoveSequence_Click(object sender, EventArgs e)
        {
            int selectedSequence = timeLineView.SelectedRowIndex;
            if (selectedSequence != -1) {
                animation.removeSequence(selectedSequence);
                timeLineView.ClearSelection();
                timeLineView.NotifyDataSourceChanged();
            }
        }

        private void btnDeleteAction_Click(object sender, EventArgs e)
        {
            int selectedSequence = timeLineView.SelectedRowIndex;
            int selectedItem = timeLineView.SelectedItemIndexInRow;
            if (selectedSequence != -1 && selectedItem != -1) {
                animation.deleteAction(selectedSequence, selectedItem);
                timeLineView.ClearSelection();
                timeLineView.NotifyDataSourceChanged();
            }
        }

        private void trcTimeLineViewZoom_Scroll(object sender, EventArgs e)
        {
            timeLineView.Zoom = trcTimeLineViewZoom.Value;
            timeLineView.NotifyDataSourceChanged();
        }

        private void actionsList_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            ListView listView = (ListView)sender;
            Image itemImage = e.Item.ImageList.Images[e.Item.ImageIndex];

            if (e.Item.Tag != null) {
                TAction action = (TAction)e.Item.Tag;

                if ((e.State & ListViewItemStates.Selected) != 0) {
                    e.Graphics.DrawImage(Properties.Resources.action_list_cell_selected, e.Bounds.X + e.Bounds.Width / 2 - itemImage.Width / 2, e.Bounds.Y + 2);
                    if (action.icon != null)
                        e.Graphics.DrawImage(action.icon, e.Bounds.X + e.Bounds.Width / 2 - action.icon.Width / 2, e.Bounds.Y + 2 + itemImage.Height / 2 - action.icon.Height / 2);
                } else {
                    Bitmap icon = action.iconWithFrame();
                    if (icon != null)
                        e.Graphics.DrawImage(icon, e.Bounds.X + e.Bounds.Width / 2 - icon.Width / 2, e.Bounds.Y + 2);
                }

            }

            // Draw the item text for views other than the Details view. 
            if (listView.View != View.Details) {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;

                e.Graphics.DrawString(e.Item.Text, listView.Font, Brushes.Black, new Rectangle(e.Bounds.X, e.Bounds.Y + 2 + itemImage.Height + 2, e.Bounds.Width, 25), sf);
            }
        }

        private void actionList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ListView listView = (ListView)sender;
            ListViewItem item = (ListViewItem)e.Item;
            if (item != null && item.Tag != null && item.Tag is TAction) {
                // corresponding action
                TAction action = (TAction)item.Tag;

                // icon of corresponding action
                Bitmap cursorBmp = ((TAction)item.Tag).iconWithFrame();

                draggingCursor = CursorUtil.CreateCursor(cursorBmp, cursorBmp.Width / 2, cursorBmp.Height / 2);
                listView.DoDragDrop("TActionDragItem:" + item.Tag.GetType().FullName, DragDropEffects.Copy);
            }
        }

        private void actionList_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Cursor.Current = draggingCursor;
        }

        private void timeLineView_DrawRowSetting(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            TSequence sequence = animation.sequenceAtIndex(e.Index);
            if (sequence != null) {
                if (sequence.repeat == -1) {
                    g.DrawImage(Properties.Resources.btn_repeat_loop, e.Bounds.X + 5, e.Bounds.Y + 19, 24, 24);
                } else if (sequence.repeat == 1) {
                    g.DrawImage(Properties.Resources.btn_repeat_one, e.Bounds.X + 5, e.Bounds.Y + 19, 24, 24);
                } else {
                    g.DrawImage(Properties.Resources.btn_repeat_some, e.Bounds.X + 5, e.Bounds.Y + 19, 24, 24);

                    Font font = new Font(timeLineView.Font.FontFamily, 7);
                    Brush repeatBrush = new SolidBrush(Color.FromArgb(78, 78, 80));
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Far;
                    g.DrawString(sequence.repeat.ToString(), font, repeatBrush, new RectangleF(e.Bounds.X, e.Bounds.Y + 31, 30, 10), sf);
                }
            }
        }

        private void timeLineView_RowSettingClick(object sender, EventArgs e)
        {
            int index = timeLineView.SelectedRowIndex;
            if (index != -1) {
                TSequence sequence = animation.sequenceAtIndex(index);
                if (sequence.repeat == 1) {
                    sequence.repeat = -1;
                } else if (sequence.repeat == -1) {
                    string num = Interaction.InputBox("Please input the number of repeat");
                    int n;
                    if (int.TryParse(num, out n) && n > 1)
                        sequence.repeat = n;
                    else
                        sequence.repeat = 1;
                } else {
                    sequence.repeat = 1;
                }

                timeLineView.Invalidate();
            }
        }

        private void timeLineView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) == true && ((string)e.Data.GetData(DataFormats.StringFormat)).StartsWith("TActionDragItem:")) {
                Point pt = timeLineView.PointToClient(new Point(e.X, e.Y));
                int rowIndex, itemIndex;
                if (timeLineView.HighlightPositionForInsert(pt, out rowIndex, out itemIndex)) {
                    e.Effect = DragDropEffects.All;
                    return;
                }
            }

            e.Effect = DragDropEffects.None;
        }

        private void timeLineView_DragLeave(object sender, EventArgs e)
        {
            timeLineView.ClearHighlight();
            timeLineView.Invalidate();
        }

        private void timeLineView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) == true && ((string)e.Data.GetData(DataFormats.StringFormat)).StartsWith("TActionDragItem:")) {

                // convert screen coordinate to client coordinate
                Point pt = timeLineView.PointToClient(new Point(e.X, e.Y));

                int rowIndex, itemIndex;
                if (timeLineView.HighlightPositionForInsert(pt, out rowIndex, out itemIndex)) {

                    // dragging item
                    string actionClass = ((string)e.Data.GetData(DataFormats.StringFormat)).Substring("TActionDragItem:".Length);
                    TAction action = (TAction)Activator.CreateInstance(Type.GetType(actionClass));

                    // insert action
                    animation.insertAction(rowIndex, itemIndex, action);
                    timeLineView.NotifyDataSourceChanged();
                    timeLineView.SetSelection(rowIndex, itemIndex);
                    return;
                }

                timeLineView.ClearHighlight();
                timeLineView.Invalidate();
            }
        }

        private void timeLineView_ItemLengthChanged(object sender, LengthChangedEventArgs e)
        {
            TAction action = animation.actionAtIndex(timeLineView.SelectedRowIndex, timeLineView.SelectedItemIndexInRow);
            if (action != null) {
                action.duration = (long)(e.Length * 1000);

                foreach (Control c in grpActionSettings.Controls) {
                    if (c.Visible == true && c is ActionSettingBasePanel) {
                        ((ActionSettingBasePanel)c).LoadData();
                    }
                }
            }
        }

        private bool timeLineView_ItemMoved(object sender, ItemMovedEventArgs e)
        {
            TSequence fromSequence = animation.sequenceAtIndex(e.FromRowIndex);
            TSequence toSequence = animation.sequenceAtIndex(e.ToRowIndex);
            TAction action = animation.actionAtIndex(e.FromRowIndex, e.FromItemIndex);

            if (fromSequence != null && toSequence != null && action != null) {
                toSequence.insertAction(e.ToItemIndex, action);
                if (e.FromRowIndex == e.ToRowIndex && e.ToItemIndex < e.FromItemIndex)
                    fromSequence.deleteAction(e.FromItemIndex + 1);
                else
                    fromSequence.deleteAction(e.FromItemIndex);
                return true;
            }

            return false;
        }

        private void timeLineView_DataChanged(object sender, EventArgs e)
        {
            updateSequenceActionsList();
        }

        private void timeLineView_RowSelectionChanged(object sender, EventArgs e)
        {
            updateSequenceActionsList();
        }

        private void timeLineView_SelectionChanged(object sender, EventArgs e)
        {
            //====================== changed selection of action list ================================//
            manualSequenceActionSelectedChanged = true;

            if (lvwSequenceActions.SelectedIndices.Count > 0) {
                for (int i = 0; i < lvwSequenceActions.SelectedIndices.Count; i++) 
                    lvwSequenceActions.Items[lvwSequenceActions.SelectedIndices[i]].Selected = false;
            }

            if (timeLineView.SelectedItemIndexInRow >= 0 && timeLineView.SelectedItemIndexInRow < lvwSequenceActions.Items.Count) {
                lvwSequenceActions.Items[timeLineView.SelectedItemIndexInRow].Selected = true;
                lvwSequenceActions.Items[timeLineView.SelectedItemIndexInRow].EnsureVisible();
            }

            manualSequenceActionSelectedChanged = false;

            //======================= show the setting panel =========================================//
            foreach (Control c in grpActionSettings.Controls) 
                c.Visible = false;

            TAction action = animation.actionAtIndex(timeLineView.SelectedRowIndex, timeLineView.SelectedItemIndexInRow);
            ActionSettingBasePanel settingPanel = null;
            if (action != null) {
                if (action is TActionIntervalBezier)
                    settingPanel = actionSettingIntervalBezier;
                else if (action is TActionIntervalDelay)
                    settingPanel = actionSettingIntervalDelay;
                else if (action is TActionIntervalFade)
                    settingPanel = actionSettingIntervalFade;
                else if (action is TActionIntervalMove)
                    settingPanel = actionSettingIntervalMove;
                else if (action is TActionIntervalRotate)
                    settingPanel = actionSettingIntervalRotate;
                else if (action is TActionIntervalScale)
                    settingPanel = actionSettingIntervalScale;
                else if (action is TActionIntervalAnimate)
                    settingPanel = actionSettingIntervalAnimate;
                else if (action is TActionInstantDispatchEvent)
                    settingPanel = actionSettingInstantDispatchEvent;
                else if (action is TActionInstantChangeState)
                    settingPanel = actionSettingInstantChangeState;
                else if (action is TActionInstantEnableActor)
                    settingPanel = actionSettingInstantEnableActor;
                else if (action is TActionInstantStopAnimation)
                    settingPanel = actionSettingInstantStopAnimation;
                else if (action is TActionInstantGoScene)
                    settingPanel = actionSettingInstantGoScene;
                else if (action is TActionInstantReadMode)
                    settingPanel = actionSettingInstantReadMode;
                else if (action is TActionInstantZIndex)
                    settingPanel = actionSettingInstantZIndex;
                else if (action is TActionInstantMakeAvatar)
                    settingPanel = actionSettingInstantMakeAvatar;
                else if (action is TActionInstantClearAvatar)
                    settingPanel = actionSettingInstantClearAvatar;
                else if (action is TActionInstantPlaySound)
                    settingPanel = actionSettingInstantPlaySound;
                else if (action is TActionInstantStopSound)
                    settingPanel = actionSettingInstantStopSound;
                else if (action is TActionInstantPlayVoice)
                    settingPanel = actionSettingInstantPlayVoice;
                else if (action is TActionInstantStopVoice)
                    settingPanel = actionSettingInstantStopVoice;
                else if (action is TActionInstantStopAllSounds)
                    settingPanel = actionSettingInstantStopAllSounds;
                else if (action is TActionInstantEnableSound)
                    settingPanel = actionSettingInstantEnableSound;
                else if (action is TActionInstantEnableVoice)
                    settingPanel = actionSettingInstantEnableVoice;
                else if (action is TActionInstantEnableBGM)
                    settingPanel = actionSettingInstantEnableBGM;
                else if (action is TActionInstantChangeBGM)
                    settingPanel = actionSettingInstantChangeBGM;

                if (settingPanel != null) {
                    settingPanel.timeLineView = this.timeLineView;
                    settingPanel.action = action;
                    settingPanel.LoadData();
                    settingPanel.Visible = true;
                }
            }

        }

        private bool manualSequenceActionSelectedChanged = false;

        private void lvwSequenceActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manualSequenceActionSelectedChanged == false) {
                if (lvwSequenceActions.SelectedIndices.Count > 0)
                    timeLineView.SetSelection(timeLineView.SelectedRowIndex, lvwSequenceActions.SelectedIndices[0]);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void updateSequenceActionsList()
        {
            lvwSequenceActions.Items.Clear();
            imlSequenceActions.Images.Clear();

            TSequence sequence = animation.sequenceAtIndex(timeLineView.SelectedRowIndex);
            if (sequence != null) {
                for (int i = 0; i < sequence.numberOfActions(); i++) {
                    TAction action = sequence.actionAtIndex(i);
                    imlSequenceActions.Images.Add(action.iconWithFrame());
                    lvwSequenceActions.Items.Add(action.name, i);
                }
            }
        }
    }
}
