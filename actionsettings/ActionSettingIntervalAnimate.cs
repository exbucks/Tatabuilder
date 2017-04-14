using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TataBuilder.actionsettings
{
    public partial class ActionSettingIntervalAnimate : ActionSettingBasePanel
    {

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int SetWindowTheme(IntPtr hWnd, string appName, string partList);

        public int MakeLong(short lowPart, short highPart)
        {
            return (int)(((ushort)lowPart) | (uint)(highPart << 16));
        }

        const int LVM_FIRST = 0x1000;
        const int LVM_SETICONSPACING = LVM_FIRST + 53;

        public ActionSettingIntervalAnimate()
        {
            InitializeComponent();

            SetWindowTheme(lvwFrames.Handle, "explorer", null);

            // image library item space
            SendMessage(lvwFrames.Handle, LVM_SETICONSPACING, IntPtr.Zero, (IntPtr)MakeLong((short)(TLibraryManager.LARGE_IMAGE_LIST_THUMBNAIL_WIDTH + 8), (short)(TLibraryManager.LARGE_IMAGE_LIST_THUMBNAIL_HEIGHT)));
            lvwFrames.TileSize = new Size(TLibraryManager.LARGE_IMAGE_LIST_THUMBNAIL_WIDTH + 8, TLibraryManager.LARGE_IMAGE_LIST_THUMBNAIL_HEIGHT + 8);
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // load action data
            TActionIntervalAnimate myAction = (TActionIntervalAnimate)this.action;
            nudDuration.Value = (decimal)myAction.duration;
            nudDuration.Enabled = false;

            // clear combo boxes
            lvwFrames.Items.Clear();

            // fill combo box
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            if (dlg != null && dlg.document != null) {
                TLibraryManager libraryManager = dlg.document.libraryManager;
                lvwFrames.LargeImageList = libraryManager.largeImageListThumbnails();
                for (int i = 0; i < myAction.frames.Count; i++) {
                    ListViewItem item = lvwFrames.Items.Add("", libraryManager.imageIndex(myAction.frames[i].image));
                    item.Tag = myAction.frames[i];
                }
            }

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionIntervalAnimate myAction = (TActionIntervalAnimate)this.action;

                myAction.frames.Clear();
                for (int i = 0; i < lvwFrames.Items.Count; i++) 
                    myAction.frames.Add((TAnimateFrame)lvwFrames.Items[i].Tag);

                base.SaveData();
            }
        }

        private long recalcTotalDuration()
        {
            long t = 0;
            for (int i = 0; i < lvwFrames.Items.Count; i++) {
                TAnimateFrame frame = (TAnimateFrame)lvwFrames.Items[i].Tag;
                t += frame.duration;
            }

            return t;
        }

        private void btnAddFrame_Click(object sender, EventArgs e)
        {
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            if (dlg != null && dlg.document != null) {
                TLibraryManager libraryManager = dlg.document.libraryManager;

                FrmImagesPicker imagesPicker = new FrmImagesPicker();
                imagesPicker.Text = "Select the images to add frames";
                imagesPicker.libraryManager = libraryManager;
                if (imagesPicker.ShowDialog() == DialogResult.OK) {
                    for (int i = 0; i < imagesPicker.selectedImages.Count; i++) {
                        ListViewItem item = lvwFrames.Items.Add("", libraryManager.imageIndex(imagesPicker.selectedImages[i]));
                        item.Tag = new TAnimateFrame { image = imagesPicker.selectedImages[i], duration = 1000 };
                    }

                    lvwFrames_SelectedIndexChanged(sender, e);
                    SaveData(sender, e);
                }
            }
        }

        private void btnRemoveFrame_Click(object sender, EventArgs e)
        {
            if (lvwFrames.SelectedItems.Count != 0) {
                int index = lvwFrames.SelectedItems[0].Index;
                foreach (ListViewItem item in lvwFrames.SelectedItems) {
                    lvwFrames.Items.Remove(item);
                }

                if (index > lvwFrames.Items.Count - 1)
                    index = lvwFrames.Items.Count - 1;

                lvwFrames.SelectedItems.Clear();
                if (index != -1) {
                    lvwFrames.SelectedIndices.Add(index);
                    lvwFrames.EnsureVisible(index);
                }

                SaveData(sender, e);
            }
        }

        private void btnMoveUpFrame_Click(object sender, EventArgs e)
        {
            if (lvwFrames.SelectedItems.Count > 0) {
                ListViewItem item = lvwFrames.SelectedItems[0];
                if (item.Index > 0) {
                    int index = item.Index - 1;
                    lvwFrames.Items.Remove(item);
                    lvwFrames.Items.Insert(index, item);
                    lvwFrames.SelectedItems.Clear();
                    lvwFrames.SelectedIndices.Add(index);
                    lvwFrames.View = View.Details;
                    lvwFrames.View = View.Tile;
                    lvwFrames.EnsureVisible(index);
                }
            }
        }

        private void btnMoveDownFrame_Click(object sender, EventArgs e)
        {
            if (lvwFrames.SelectedItems.Count > 0) {
                ListViewItem item = lvwFrames.SelectedItems[0];
                if (item.Index < lvwFrames.Items.Count - 1) {
                    int index = item.Index + 1;
                    lvwFrames.Items.Remove(item);
                    lvwFrames.Items.Insert(index, item);
                    lvwFrames.SelectedItems.Clear();
                    lvwFrames.SelectedIndices.Add(index);
                    lvwFrames.View = View.Details;
                    lvwFrames.View = View.Tile;
                    lvwFrames.EnsureVisible(index);
                }
            }
        }

        private void lvwFrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAddFrame.Enabled = true;
            btnRemoveFrame.Enabled = lvwFrames.SelectedItems.Count > 0;
            btnMoveUpFrame.Enabled = lvwFrames.SelectedItems.Count > 0 && lvwFrames.SelectedItems[0].Index > 0;
            btnMoveDownFrame.Enabled = lvwFrames.SelectedItems.Count > 0 && lvwFrames.SelectedItems[0].Index < lvwFrames.Items.Count - 1;

            if (lvwFrames.SelectedItems.Count > 0) {
                nudDuration.Enabled = true;
                nudDuration.Value = ((TAnimateFrame)lvwFrames.SelectedItems[0].Tag).duration;
            } else {
                nudDuration.Enabled = false;
                nudDuration.Value = recalcTotalDuration();
            }
        }

        private void nudDuration_ValueChanged(object sender, EventArgs e)
        {
            if (lvwFrames.SelectedItems.Count > 0) {
                ((TAnimateFrame)lvwFrames.SelectedItems[0].Tag).duration = (long)nudDuration.Value;
                SaveData(sender, e);
            }
        }
    }
}
