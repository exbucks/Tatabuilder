using C1.Win.C1Input;
using C1.Win.C1Ribbon;
using GuiLabs.Undo;
using Ionic.Zip;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
        
namespace TataBuilder
{

    public partial class FrmMainContainer : C1.Win.C1Ribbon.C1RibbonForm
    {
        Cursor resourceDraggingCursor;
        Cursor sceneDraggingCursor;

        const string CLIPBOARD_ACTOR_FORMAT = "TataActorFormat";

        #region TextBoxHost

        public class TextBoxHost : C1.Win.C1Ribbon.RibbonControlHost
        {
            public TextBoxHost()
                : base(new C1TextBox())
            {
                C1TextBox textBox = (C1TextBox)this.Control;
                textBox.BackColorChanged += this.TextBox_BackColorChanged;
                textBox.BorderColorChanged += this.TextBox_BorderColorChanged;
                textBox.Multiline = true;
                textBox.ScrollBars = ScrollBars.Vertical;
                textBox.Width = 300;
                textBox.Height = 55;
            }

            protected virtual void TextBox_BackColorChanged(object sender, EventArgs e)
            {
                C1TextBox textBox = (C1TextBox)this.Control;
                if (textBox != null) {
                    if (textBox.Enabled) {
                        this.BackColor = Color.FromArgb(255, 254, 254, 254);
                        textBox.BorderColor = Color.FromArgb(255, 211, 214, 217);
                    } else {
                        this.BackColor = Color.FromArgb(255, 250, 250, 250);
                        textBox.BorderColor = Color.FromArgb(255, 228, 231, 235);
                    }
                }
            }

            protected virtual void TextBox_BorderColorChanged(object sender, EventArgs e)
            {
                C1TextBox textBox = (C1TextBox)this.Control;
                if (textBox != null) {
                    if (textBox.Enabled)
                        textBox.BorderColor = Color.FromArgb(255, 211, 214, 217);
                    else
                        textBox.BorderColor = Color.FromArgb(255, 228, 231, 235);
                }
            }
        }

        #endregion

        #region External DLL

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int SetWindowTheme(IntPtr hWnd, string appName, string partList);

        const int LVM_FIRST = 0x1000;
        const int LVM_SETICONSPACING = LVM_FIRST + 53;

        public int MakeLong(short lowPart, short highPart) {
            return (int)(((ushort)lowPart) | (uint)(highPart << 16));
        }

        #endregion

        #region MDIContainer

        public FrmMainContainer()
        {
            InitializeComponent();

            // scrolling timer for outline treeview drag-drop
            this.scrollingTimer.Interval = 200;
            this.scrollingTimer.Tick += new EventHandler(scrollingTimer_Tick);

            // fill font family combobox
            foreach (string font in Program.getFontFamilies()) {
                this.cmbFontFace.Items.Add(new RibbonButton(font));
            }

            // Populate the Font Size combobox with some typical font sizes.
            foreach (int size in new int[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 }) {
                this.cmbFontSize.Items.Add(new RibbonButton(size.ToString()));
            }

            initializeRecentDocumentList();

            SetWindowTheme(tvwSceneOutline.Handle, "explorer", null);
            SetWindowTheme(lvwImageList.Handle, "explorer", null);
        }

        private void FrmMainContainer_Load(object sender, EventArgs e)
        {
            // workspace's border
            MDIClientSupport.SetBevel(this, false);

            // scenes panel's height
            this.lstScenes.ItemHeight = Program.SCENE_THUMBNAIL_OUTER_HEIGHT + 10;

            // image library item space
            SendMessage(this.lvwImageList.Handle, LVM_SETICONSPACING, IntPtr.Zero, (IntPtr)MakeLong((short)(TLibraryManager.LARGE_IMAGE_LIST_THUMBNAIL_WIDTH + 10), (short)(TLibraryManager.LARGE_IMAGE_LIST_THUMBNAIL_HEIGHT + 30)));

            FrmMainContainer_MdiChildActivate(sender, e);

            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() >= 2) {
                this.openDocument(args[1]);
            }
        }

        private void FrmMainContainer_MdiChildActivate(object sender, EventArgs e) {
            // Determine the active document.
            TDocument doc = this.activeDocument();

            rbtnSave.Enabled = (doc != null);
            rbtnSaveAs.Enabled = (doc != null);
            rbtnUndo.Enabled = (doc != null);
            rbtnRedo.Enabled = (doc != null);
            rbtnEmulateTablet.Enabled = (doc != null);
            rbtnEmulatePhone.Enabled = (doc != null);
            rbtnExport.Enabled = (doc != null);
            rbtnPublish.Enabled = (doc != null);
            rbtnDeployProject.Enabled = (doc != null);
            rbtnDeployScene.Enabled = (doc != null);
            rbtnClose.Enabled = (doc != null);

            rtabHome.Enabled = (doc != null);
            rtabView.Enabled = (doc != null);

            rtabDocument.Visible = (doc != null);
            rtabResources.Visible = (doc != null);

            rtabAnimations.Visible = (doc != null && doc.currentScene() != null);
            rctgSceneProperties.Visible = (doc != null && doc.currentScene() != null && !doc.haveSelection());
            rctgActorProperties.Visible = (doc != null && doc.currentScene() != null && doc.haveSelection());
            rctgTextActorProperties.Visible = (doc != null && doc.currentScene() != null && doc.selectedActor() is TTextActor);

            pnlRightBar.Enabled = (doc != null);

            foreach (RibbonItem x in c1StatusBar.RightPaneItems)
                x.Enabled = (doc != null);

            this.updateAllPanels();
        }

        private void FrmMainContainer_KeyDown(object sender, KeyEventArgs e)
        {
            // Determine the active child form.
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                IntPtr handle = GetFocus();
                if (handle != IntPtr.Zero) {
                    Control focused = Control.FromHandle(handle);
                    if (!(focused is TextBox)) {
                        if (e.KeyCode == Keys.Space) {
                            e.SuppressKeyPress = true;
                            if (doc.currentTempTool != TDocument.TOOL_HAND) {
                                doc.currentTempTool = TDocument.TOOL_HAND;
                                activeWorkspace.updateCursor();
                            }
                        } else if (e.KeyCode == Keys.V) {
                            e.SuppressKeyPress = true;
                            rbtnSelectTool_Click(null, null);
                        } else if (e.KeyCode == Keys.H) {
                            e.SuppressKeyPress = true;
                            rbtnHandTool_Click(null, null);
                        } else if (e.KeyCode == Keys.T) {
                            e.SuppressKeyPress = true;
                            rbtnTextTool_Click(null, null);
                        } else if (e.KeyCode == Keys.B) {
                            e.SuppressKeyPress = true;
                            rbtnBoundingTool_Click(null, null);
                        } else if (e.KeyCode == Keys.A) {
                            e.SuppressKeyPress = true;
                            rbtnAvatarTool_Click(null, null);
                        } else if (e.KeyCode == Keys.P) {
                            e.SuppressKeyPress = true;
                            rbtnPuzzleTool_Click(null, null);
                        } else if (e.KeyCode == Keys.Delete) {
                            rbtnDelete_Click(null, null);
                        } else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right) {
                            if (doc.currentScene() != null && doc.haveSelection()) {
                                if (doc.activeTool() == TDocument.TOOL_SELECT) {

                                    TActor target = doc.selectedActor();

                                    // record modify action
                                    ModifyActorAction modifyActorAction = new ModifyActorAction(doc, target);
                                    target.createBackup();

                                    // move actor
                                    float step = ((Control.ModifierKeys & Keys.Shift) != 0) ? 10 * doc.zoom : 1 * doc.zoom;
                                    if (e.KeyCode == Keys.Up)
                                        doc.moveSelectedItems(0, -step, false);
                                    else if (e.KeyCode == Keys.Down)
                                        doc.moveSelectedItems(0, step, false);
                                    else if (e.KeyCode == Keys.Left)
                                        doc.moveSelectedItems(-step, 0, false);
                                    else if (e.KeyCode == Keys.Right)
                                        doc.moveSelectedItems(step, 0, false);

                                    // record history
                                    modifyActorAction.setFinalData(target);
                                    if (modifyActorAction.isModified()) {
                                        doc.actionManager.RecordAction(modifyActorAction);

                                        // set the document modified flag
                                        doc.modified = true;

                                        // ready to new modification action
                                        modifyActorAction = new ModifyActorAction(doc, target);

                                        // update history
                                        this.updateHistoryPanel();
                                    }

                                    // update panels
                                    doc.sceneManager.updateThumbnail(doc.sceneManager.currentSceneIndex);
                                    this.updateScenesPanel(doc.sceneManager.currentSceneIndex);
                                    this.updateToolbarSceneSettings();

                                    foreach (TActor actor in doc.selectedItems) {
                                        actor.deleteBackup();
                                    }

                                    // redraw workspace
                                    activeWorkspace.Refresh();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FrmMainContainer_KeyUp(object sender, KeyEventArgs e)
        {
            // Determine the active child form.
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                if (e.KeyCode == Keys.Space) {
                    IntPtr handle = GetFocus();
                    if (handle != IntPtr.Zero) {
                        Control focused = Control.FromHandle(handle);
                        if (!(focused is TextBox)) {
                            e.SuppressKeyPress = true;
                            doc.currentTempTool = TDocument.TOOL_NONE;
                            activeWorkspace.updateCursor();
                        }
                    }
                }
            }
        }

        private void splMainRightDivider_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(250, 250, 250));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(212, 212, 212)), 2, 0, 2, splMainRightDivider.Height);
        }

        private void splRightPanelDivider_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(250, 250, 250));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(212, 212, 212)), 0, 2, splRightPanelDivider.Width, 2);
        }

        #endregion

        #region Scenes Panel

        public ListBox getScenesList()
        {
            return lstScenes;
        }

        private void lstScenes_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                // Make sure we're not trying to draw something that isn't there.
                if (e.Index >= lstScenes.Items.Count || e.Index <= -1)
                    return;

                // Get the item object.
                object item = lstScenes.Items[e.Index];
                if (item == null)
                    return;

                Bitmap offScreenBmp = new Bitmap(e.Bounds.Width, e.Bounds.Height);
                Graphics offScreenDC = Graphics.FromImage(offScreenBmp);

                // calc rects
                Rectangle outerRect = new Rectangle((e.Bounds.Width - Program.SCENE_THUMBNAIL_OUTER_WIDTH) / 2, (e.Bounds.Height - Program.SCENE_THUMBNAIL_OUTER_HEIGHT) / 2, Program.SCENE_THUMBNAIL_OUTER_WIDTH, Program.SCENE_THUMBNAIL_OUTER_HEIGHT);
                Rectangle innerRect = new Rectangle((e.Bounds.Width - Program.SCENE_THUMBNAIL_WIDTH) / 2, (e.Bounds.Height - Program.SCENE_THUMBNAIL_HEIGHT) / 2, Program.SCENE_THUMBNAIL_WIDTH, Program.SCENE_THUMBNAIL_HEIGHT);
                Rectangle borderRect = new Rectangle(innerRect.X - 1, innerRect.Y - 1, innerRect.Width + 1, innerRect.Height + 1);

                // Draw the background image depending on  if the item is selected or not.
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                    // The item is selected.
                    offScreenDC.DrawImage(Properties.Resources.scene_sel_bg, outerRect);
                } else {
                    // The item is NOT selected.
                    offScreenDC.FillRectangle(new SolidBrush(this.lstScenes.BackColor), 0, 0, e.Bounds.Width, e.Bounds.Height);
                    offScreenDC.DrawRectangle(new Pen(Color.FromArgb(120, 200, 120)), borderRect);
                }

                // image list
                ImageList imageList = doc.sceneManager.thumbnailImageList();
                offScreenDC.DrawImage(imageList.Images[e.Index], innerRect);

                e.Graphics.DrawImage(offScreenBmp, e.Bounds.X, e.Bounds.Y);
            }
        }

        private void lstScenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstScenes.Tag != null && (bool)lstScenes.Tag == true) 
                return;

            // Determine the active child form.
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace == null) // || lstScenes.SelectedIndex < 0)
                return;

            TDocument doc = activeWorkspace.document;
            doc.sceneManager.currentSceneIndex = lstScenes.SelectedIndex;
            doc.clearSelectedItems();
            activeWorkspace.Refresh();

            this.updateToolbarSceneSettings();
            this.updateOutlinePanel();
            this.updateSceneButtons();
        }

        private void lstScenes_MouseDown(object sender, MouseEventArgs e)
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null && e.Button == MouseButtons.Left) {
                if (lstScenes.SelectedItem == null) return;
                if (lstScenes.IndexFromPoint(e.Location) == -1) return;

                ImageList imageList = doc.sceneManager.thumbnailImageList();
                Bitmap cursorBmp = new Bitmap(Program.SCENE_THUMBNAIL_WIDTH + 2, Program.SCENE_THUMBNAIL_HEIGHT + 2);
                Graphics g = Graphics.FromImage(cursorBmp);
                g.FillRectangle(new SolidBrush(lstScenes.BackColor), 0, 0, Program.SCENE_THUMBNAIL_WIDTH + 2, Program.SCENE_THUMBNAIL_HEIGHT + 2);
                g.DrawRectangle(new Pen(Color.FromArgb(120, 200, 120)), 0, 0, Program.SCENE_THUMBNAIL_WIDTH, Program.SCENE_THUMBNAIL_HEIGHT);
                g.DrawImage(imageList.Images[lstScenes.SelectedIndex], 1, 1, Program.SCENE_THUMBNAIL_WIDTH, Program.SCENE_THUMBNAIL_HEIGHT);
                g.Dispose();

                if (cursorBmp != null) {
                    sceneDraggingCursor = CursorUtil.CreateCursor(cursorBmp, cursorBmp.Width / 2, cursorBmp.Height / 2);
                } else {
                    sceneDraggingCursor = null;
                }

                lstScenes_SelectedIndexChanged(sender, e);
                lstScenes.DoDragDrop(Program.DRAG_SCENE_PREFIX + lstScenes.SelectedIndex, DragDropEffects.Move);
            }
        }

        private void lstScenes_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (sceneDraggingCursor != null) {
                e.UseDefaultCursors = false;
                Cursor.Current = sceneDraggingCursor;
            } else {
                e.UseDefaultCursors = true;
            }
        }

        private void lstScenes_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void lstScenes_DragDrop(object sender, DragEventArgs e)
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                if (e.Data.GetDataPresent(DataFormats.StringFormat, true) == true) {
                    string data = (string)e.Data.GetData(typeof(string));
                    if (data.StartsWith(Program.DRAG_SCENE_PREFIX)) {
                        int selectedIndex;
                        if (int.TryParse(data.Substring(Program.DRAG_SCENE_PREFIX.Length), out selectedIndex)) {
                            Point point = lstScenes.PointToClient(new Point(e.X, e.Y));
                            point.Y += lstScenes.ItemHeight / 2;
                            int index = lstScenes.IndexFromPoint(point);
                            if (index < 0) index = lstScenes.Items.Count;

                            if (selectedIndex != index && selectedIndex != index - 1) {
                                // move scene
                                doc.actionManager.RecordAction(new ReorderSceneAction(doc, this, selectedIndex, index));

                                // set the document modified flag
                                doc.modified = true;

                                // update panels
                                lstScenes.Invalidate();
                                updateHistoryPanel();
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Library Panel

        private void btnImageLibraryHandle_CheckedChanged(object sender, EventArgs e)
        {
            pnlImageLibrary.Visible = btnImageLibraryHandle.Checked;
        }

        private void btnSoundLibraryHandle_CheckedChanged(object sender, EventArgs e)
        {
            pnlSoundLibrary.Visible = btnSoundLibraryHandle.Checked;
        }

        private void btnCloseResources_Click(object sender, EventArgs e)
        {
            pnlResources.Visible = false;
            rchkVisibleResources.Checked = false; ;
        }

        private void pnlResources_VisibleChanged(object sender, EventArgs e)
        {
            splRightPanelDivider.Visible = pnlResources.Visible && pnlLayers.Visible;
            pnlRightBar.Visible = pnlResources.Visible || pnlLayers.Visible;
            splMainRightDivider.Visible = pnlRightBar.Visible;
        }

        private void btnImageListMode_Click(object sender, EventArgs e)
        {
            switch (lvwImageList.View) {
                case View.LargeIcon:
                    lvwImageList.View = View.List;
                    btnImageListMode.Image = Properties.Resources.btn_resources_grid;
                    break;
                case View.List:
                    lvwImageList.View = View.LargeIcon;
                    btnImageListMode.Image = Properties.Resources.btn_resources_list;
                    break;
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                TLibraryManager manager = doc.libraryManager;

                // show open dialog
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png|All files (*.*)|*.*";
                dialog.Multiselect = true;
                dialog.Title = "Select files to add to the Image assets";

                //Present to the user.
                if (dialog.ShowDialog() == DialogResult.OK) {
                    // Read the files 
                    foreach (String srcFile in dialog.FileNames) {

                        string fileName = Path.GetFileName(srcFile);
                        string srcFolder = Path.GetDirectoryName(srcFile);
                        string dstfolder = doc.getImagesDirectoryPath();
                        string dstFile = dstfolder + "\\" + fileName;

                        if (!String.Equals(srcFolder, dstfolder, StringComparison.OrdinalIgnoreCase)) {
                            if (Directory.Exists(dstFile)) {
                                MessageBox.Show("Could not import the image " + fileName + ", because this project's images folder already contains the folder with the same name.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }

                            if (File.Exists(dstFile)) {
                                if (MessageBox.Show("Project's images folder already contains the file was named as " + fileName + ".\nAre you sure to overwrite this file?", Program.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) 
                                    continue;
                            }

                            try {
                                File.Copy(srcFile, dstFile, true);
                            } catch (Exception) {
                                MessageBox.Show("Could not import the image " + fileName, Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }
                        }

                        // check for duplication of image name
                        if (doc.libraryManager.imageIndex(fileName) == -1) {
                            // add image data to image library manager
                            bool ret = doc.libraryManager.addImage(fileName);
                            if (ret) {
                                this.lvwImageList.Items.Add(fileName, doc.libraryManager.imageCount() - 1);
                            } else {
                                // Could not load the image - probably related to Windows file system permissions.
                                MessageBox.Show("Cannot load the image: " + fileName + ".\nYou may not have permission to read the file, or it may be corrupt.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        } else {
                            // Already exist file name
                            MessageBox.Show("Image: " + fileName + " already exists.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    // set the document modified flag
                    doc.modified = true;
                }
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null && lvwImageList.SelectedItems.Count > 0) {

                foreach (ListViewItem item in lvwImageList.SelectedItems) {
                    string img = doc.libraryManager.imageFileName(item.Index);
                    if (doc.isUsingImage(img)) {
                        MessageBox.Show(String.Format("Image [{0}] couldn't be deleted because it's been used.", img), Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (MessageBox.Show("Are you sure you want to remove the selected images?", Program.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    foreach (ListViewItem item in lvwImageList.SelectedItems) {
                        int index = item.Index;

                        for (int i = index + 1; i < lvwImageList.Items.Count; i++)
                            lvwImageList.Items[i].ImageIndex--;

                        doc.libraryManager.removeImage(index);

                        lvwImageList.Items.Remove(item);

                        // set the document modified flag
                        doc.modified = true;
                    }
                }
            }
        }

        private void lvwImageList_EnabledChanged(object sender, EventArgs e)
        {
            lvwImageList.Visible = lvwImageList.Enabled;
        }

        private void lvwImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.updateLibraryButtons();
        }

        private void lvwImageList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null) {
                string imagePath = doc.libraryManager.imageFilePath(((ListViewItem)e.Item).Index);
                Image cursorImg = Image.FromFile(imagePath);
                Bitmap cursorBmp = new Bitmap(cursorImg, (int)(cursorImg.Width * doc.zoom), (int)(cursorImg.Height * doc.zoom));

                if (cursorBmp != null) {
                    resourceDraggingCursor = CursorUtil.CreateCursor(cursorBmp, cursorBmp.Width / 2, cursorBmp.Height / 2);
                } else {
                    resourceDraggingCursor = null;
                }

                lvwImageList.DoDragDrop(e.Item, DragDropEffects.Copy);
            }
        }

        private void lvwImageList_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (resourceDraggingCursor != null) {
                e.UseDefaultCursors = false;
                Cursor.Current = resourceDraggingCursor;
            } else {
                e.UseDefaultCursors = true;
            }
        }

        private void btnAddSound_Click(object sender, EventArgs e)
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                TLibraryManager manager = doc.libraryManager;

                // show open dialog
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Sound files (*.mp3, *.wav)|*.mp3;*.wav|All files (*.*)|*.*";
                dialog.Multiselect = true;
                dialog.Title = "Select files to add to the Sound assets";

                //Present to the user.
                if (dialog.ShowDialog() == DialogResult.OK) {
                    // Read the files 
                    foreach (String srcFile in dialog.FileNames) {

                        string fileName = Path.GetFileName(srcFile);
                        string srcFolder = Path.GetDirectoryName(srcFile);
                        string dstfolder = doc.getSoundsDirectoryPath();
                        string dstFile = dstfolder + "\\" + fileName;

                        if (!String.Equals(srcFolder, dstfolder, StringComparison.OrdinalIgnoreCase)) {
                            if (Directory.Exists(dstFile)) {
                                MessageBox.Show("Could not import the sound " + fileName + ", because this project's sounds folder already contains the folder with the same name.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }

                            if (File.Exists(dstFile)) {
                                if (MessageBox.Show("Project's sounds folder already contains the file was named as " + fileName + ".\nAre you sure to overwrite this file?", Program.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                                    continue;
                            }

                            try {
                                File.Copy(srcFile, dstFile, true);
                            } catch (Exception) {
                                MessageBox.Show("Could not import the sound " + fileName, Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }
                        }

                        // add sound data to sound library manager
                        bool ret = doc.libraryManager.addSound(fileName);
                        if (ret) {
                            try {
                                SoundListItem item = new SoundListItem(dstFile);
                                pnlSoundList.Controls.Add(item);
                            } catch (Exception) {
                                MessageBox.Show("Could not load the sound: " + fileName, Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        } else {
                            // Could not load the image - probably related to Windows file system permissions.
                            MessageBox.Show("Could not load the sound: " + fileName + ". You may not have permission to read the file, or " + "it may be corrupt.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    // set the document modified flag
                    doc.modified = true;
                }
            }
        }

        private void btnRemoveSound_Click(object sender, EventArgs e)
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null && pnlSoundList.SelectedItem() != null) {

                SoundListItem item = pnlSoundList.SelectedItem() as SoundListItem;
                string snd = Path.GetFileName(item.filePath);
                if (doc.isUsingSound(snd)) {
                    MessageBox.Show(String.Format("Sound [{0}] couldn't be deleted because it's been used.", snd), Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to remove the selected sound?", Program.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    pnlSoundList.Controls.Remove(item);
                    doc.libraryManager.removeSound(Path.GetFileName(item.filePath));

                    // set the document modified flag
                    doc.modified = true;

                    // update panels
                    updateLibraryButtons();
                }
            }
        }

        private void pnlSoundList_Enter(object sender, EventArgs e)
        {
            foreach (SoundListItem item in pnlSoundList.Controls) {
                if (item.selected)
                    item.updateBackgroundImage();
            }
        }

        private void pnlSoundList_Leave(object sender, EventArgs e)
        {
            foreach (SoundListItem item in pnlSoundList.Controls) {
                if (item.selected)
                    item.updateBackgroundImage();
            }
        }

        private void pnlSoundList_SelectionChanged(object sender, EventArgs e)
        {
            this.updateLibraryButtons();
        }

        #endregion

        #region Outline Panel

        // Timer for scrolling
        private Timer scrollingTimer = new Timer();

        // Node being dragged
        private TreeNode dragNode = null;

        // Temporary drop node for selection
        private TreeNode tempDropNode = null;

        private void btnCloseLayers_Click(object sender, EventArgs e)
        {
            pnlLayers.Visible = false;
            rchkVisibleLayers.Checked = false;
        }

        private void pnlLayers_VisibleChanged(object sender, EventArgs e)
        {
            splRightPanelDivider.Visible = pnlResources.Visible && pnlLayers.Visible;
            pnlRightBar.Visible = pnlResources.Visible || pnlLayers.Visible;
            splMainRightDivider.Visible = pnlRightBar.Visible;
            if (pnlLayers.Visible)
                pnlResources.Dock = DockStyle.Top;
            else
                pnlResources.Dock = DockStyle.Fill;
        }

        private void chkLockedLayer_CheckedChanged(object sender, EventArgs e)
        {
            // Determine the active form
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                // document
                TDocument doc = activeWorkspace.document;
                if (doc != null && doc.currentScene() != null) {
                    TActor actor = doc.selectedActor();
                    if (actor != null) {
                        actor.locked = chkLockedLayer.Checked;
                    }
                }
            }
        }

        private void addChildsToOutline(TreeNode node)
        {
            TLayer layer = node.Tag as TLayer;
            for (int i = layer.childs.Count - 1; i >= 0; i--) {
                TLayer childLayer = layer.childs[i];
//                TreeNode childNode = new TreeNode(childLayer.name, 2, 3);
                TreeNode childNode = new TreeNode(childLayer.name);
                childNode.Tag = childLayer;
                node.Nodes.Add(childNode);

                // add childs recursivly
                this.addChildsToOutline(childNode);
            }
        }

        private void btnChangeImageActor_Click(object sender, EventArgs e)
        {
            // Determine the active form
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                // document
                TDocument doc = activeWorkspace.document;
                if (doc != null && doc.currentScene() != null) {
                    TActor actor = doc.selectedActor();
                    if (actor != null && actor is TImageActor) {
                        TImageActor imageActor = (TImageActor)actor;

                        FrmImagesPicker imagesPicker = new FrmImagesPicker();
                        imagesPicker.Text = "Select an image to change";
                        imagesPicker.libraryManager = doc.libraryManager;
                        imagesPicker.multiSelect = false;
                        if (imagesPicker.ShowDialog() == DialogResult.OK) {
                            string img = "";
                            if (imagesPicker.selectedImages.Count > 0)
                                img = imagesPicker.selectedImages[0];

                            // change image of Image Actor
                            if (!imageActor.image.Equals(img)) {
                                // add new actor to parent
                                doc.actionManager.RecordAction(new ChangeImageActorAction(doc, imageActor, img));

                                // set the document modified flag
                                doc.modified = true;

                                // redraw document
                                activeWorkspace.Refresh();

                                // update other windows
                                doc.sceneManager.updateThumbnail(doc.sceneManager.currentSceneIndex);
                                updateScenesPanel(doc.sceneManager.currentSceneIndex);
                                updateHistoryPanel();
                            }
                        }
                    }
                }
            }
        }

        private void btnCloneActor_Click(object sender, EventArgs e)
        {
            // Determine the active form
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                // document
                TDocument doc = activeWorkspace.document;
                if (doc != null && doc.currentScene() != null) {
                    TActor actor = doc.selectedActor();
                    if (actor != null) {
                        // determine the parent layer to insert the new actor
                        TActor newActor = (TActor)actor.clone();
                        newActor.name = doc.currentScene().newLayerName("Actor_");
                        
                        // add new actor to parent
                        doc.actionManager.RecordAction(new AddActorAction(doc, newActor));

                        // set the document modified flag
                        doc.modified = true;

                        // select the new actor
                        doc.selectedItems.Clear();
                        doc.selectedItems.Add(newActor);

                        // redraw document
                        activeWorkspace.Refresh();

                        // update other windows
                        doc.sceneManager.updateThumbnail(doc.sceneManager.currentSceneIndex);
                        updateScenesPanel(doc.sceneManager.currentSceneIndex);
                        updateOutlinePanel();
                        updateToolbarSceneSettings();
                        updateToolbarSelection();
                        updateHistoryPanel();
                    }
                }
            }
        }

        private void btnRemoveActor_Click(object sender, EventArgs e)
        {
            rbtnDelete_Click(sender, e);
        }

        private TreeNode findOutlineNodeByTag(TLayer layer, Object rootNode)
        {
            TreeNodeCollection nodes = null;
            if (rootNode is TreeNode) {
                if (((TreeNode)rootNode).Tag == layer)
                    return (TreeNode)rootNode;
                else
                    nodes = ((TreeNode)rootNode).Nodes;
            } else if (rootNode is TreeView) {
                nodes = ((TreeView)rootNode).Nodes;
            }

            if (nodes != null) {
                foreach (TreeNode node in nodes) {
                    TreeNode next = findOutlineNodeByTag(layer, node);
                    if (next != null)
                        return next;
                }
            }

            return null;
        }

        private void tvwSceneOutline_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.tvwSceneOutline.Tag != null && (bool)this.tvwSceneOutline.Tag == true)
                return;

            // Determine the active form
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                // document
                TDocument doc = activeWorkspace.document;

                // selected node
                TreeNode node = tvwSceneOutline.SelectedNode;
                doc.clearSelectedItems();
                if (node.Tag is TActor)
                    doc.toggleSelectedItem((TActor)node.Tag);

                selectedItemChanged();
                activeWorkspace.Refresh();
            }
        }

        private void tvwSceneOutline_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            TDocument doc = this.activeDocument();
            if (doc != null) {
                if (e.Label != null) {
                    if (e.Label.Length > 0) {
                        TLayer layer = (TLayer)e.Node.Tag;

                        if (layer is TScene) {
                            bool validated = true;
                            TScene scene = layer as TScene;

                            for (int i = 0; i < doc.sceneManager.sceneCount(); i++) {
                                TScene one = doc.sceneManager.scene(i);
                                if (one != scene && one.name == e.Label) {
                                    validated = false;
                                    MessageBox.Show("There is already another scene with same name!", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    break;
                                }
                            }

                            if (validated) {
                                // ready for modification of layer
                                ModifySceneAction layerNameAction = new ModifySceneAction(doc, scene);

                                // change the layer name
                                layer.name = e.Label;

                                // record and execute modification
                                layerNameAction.setFinalData(scene);
                                doc.actionManager.RecordAction(layerNameAction);

                                // set the document modified flag
                                doc.modified = true;

                                updateToolbarSceneProperties();
                                updateHistoryPanel();
                            } else {
                                e.CancelEdit = true;
                            }
                        } else {
                            bool validated = true;
                            TScene scene = doc.currentScene();
                            TActor actor = layer as TActor;

                            List<TLayer> allActors = scene.getAllChilds();
                            foreach (TLayer one in allActors) {
                                if (one != actor && one.name == e.Label) {
                                    validated = false;
                                    MessageBox.Show("There is already another actor with same name!", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    break;
                                }
                            }

                            if (validated) {
                                // ready for modification of layer
                                ModifyActorAction layerNameAction = new ModifyActorAction(doc, actor);

                                // change the layer name
                                layer.name = e.Label;

                                // record and execute modification
                                layerNameAction.setFinalData(actor);
                                doc.actionManager.RecordAction(layerNameAction);

                                // set the document modified flag
                                doc.modified = true;

                                updateToolbarActorProperties();
                                updateHistoryPanel();
                            } else {
                                e.CancelEdit = true;
                            }
                        }
                    } else {
                        if (e.Node.Parent == null)
                            MessageBox.Show("The scene name cann't be the empty string.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            MessageBox.Show("The actor name cann't be the empty string.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        e.CancelEdit = true;
                    }
                }
            }
        }

        private void tvwSceneOutline_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Get drag node and select it
            this.dragNode = (TreeNode)e.Item;
            this.tvwSceneOutline.SelectedNode = this.dragNode;

            // Reset image list used for drag image
            this.imlDragItems.Images.Clear();
            this.imlDragItems.ImageSize = new Size(this.dragNode.Bounds.Size.Width + this.tvwSceneOutline.Indent, this.dragNode.Bounds.Height);

            // Create new bitmap
            // This bitmap will contain the tree node image to be dragged
            Bitmap bmp = new Bitmap(this.dragNode.Bounds.Width + this.tvwSceneOutline.Indent, this.dragNode.Bounds.Height);

            // Get graphics from bitmap
            Graphics gfx = Graphics.FromImage(bmp);

            // Draw node icon into the bitmap
            if (this.tvwSceneOutline.ImageList != null)
                gfx.DrawImage(this.imlSceneOutline.Images[this.dragNode.ImageIndex], 0, 0);

            // Draw node label into bitmap
            gfx.DrawString(this.dragNode.Text,
                this.tvwSceneOutline.Font,
                new SolidBrush(this.tvwSceneOutline.ForeColor),
                (float)this.tvwSceneOutline.Indent, 1.0f);

            // Add bitmap to imagelist
            this.imlDragItems.Images.Add(bmp);

            // Get mouse position in client coordinates
            Point p = this.tvwSceneOutline.PointToClient(Control.MousePosition);

            // Compute delta between mouse position and node bounds
            int dx = p.X + this.tvwSceneOutline.Indent - this.dragNode.Bounds.Left;
            int dy = p.Y - this.dragNode.Bounds.Top;

            // Begin dragging image
            if (DragHelper.ImageList_BeginDrag(this.imlDragItems.Handle, 0, dx, dy)) {
                // Begin dragging
                this.tvwSceneOutline.DoDragDrop(bmp, DragDropEffects.Move);
                // End dragging image
                DragHelper.ImageList_EndDrag();
            }
        }

        private void tvwSceneOutline_DragOver(object sender, DragEventArgs e)
        {
            // Compute drag position and move image
            Point p = this.tvwSceneOutline.PointToClient(new Point(e.X, e.Y));
            DragHelper.ImageList_DragMove(p.X, p.Y);

            // Get actual drop node
            TreeNode dropNode = this.tvwSceneOutline.GetNodeAt(this.tvwSceneOutline.PointToClient(new Point(e.X, e.Y)));
            if (dropNode == null) {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;

            // if mouse is on a new node select it
            if (this.tempDropNode != dropNode) {
                DragHelper.ImageList_DragShowNolock(false);
                if (this.tvwSceneOutline.SelectedNode != dropNode) {
                    this.tvwSceneOutline.Tag = true;
                    this.tvwSceneOutline.SelectedNode = dropNode;
                    this.tvwSceneOutline.Tag = null;
                }
                DragHelper.ImageList_DragShowNolock(true);
                tempDropNode = dropNode;
            }

            // Avoid that drop node is child of drag node 
            TreeNode tmpNode = dropNode;
            while (tmpNode.Parent != null) {
                if (tmpNode.Parent == this.dragNode) e.Effect = DragDropEffects.None;
                tmpNode = tmpNode.Parent;
            }
        }

        private void tvwSceneOutline_DragDrop(object sender, DragEventArgs e)
        {
            // Unlock updates
            DragHelper.ImageList_DragLeave(this.tvwSceneOutline.Handle);

            // Get drop node
            TreeNode dropNode = this.tvwSceneOutline.GetNodeAt(this.tvwSceneOutline.PointToClient(new Point(e.X, e.Y)));

            // If drop node isn't equal to drag node, add drag node as child of drop node
            if (this.dragNode != dropNode) {
                // Remove drag node from parent
                if (this.dragNode.Parent == null) {
                    this.tvwSceneOutline.Nodes.Remove(this.dragNode);
                } else {
                    this.dragNode.Parent.Nodes.Remove(this.dragNode);
                }

                // Add drag node to drop node
                dropNode.Nodes.Add(this.dragNode);
                dropNode.ExpandAll();

                if (this.tvwSceneOutline.SelectedNode != this.dragNode) {
                    this.tvwSceneOutline.Tag = true;
                    this.tvwSceneOutline.SelectedNode = this.dragNode;
                    this.tvwSceneOutline.Tag = null;
                }

                // Determine the active child form.
                FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
                if (activeWorkspace != null && this.dragNode.Tag != null && dropNode.Tag != null && (this.dragNode.Tag is TActor) && (dropNode.Tag is TLayer)) {
                    TDocument doc = activeWorkspace.document;

                    doc.actionManager.RecordAction(new TransferActorAction(doc, (TActor)this.dragNode.Tag, (TLayer)dropNode.Tag));

                    // set the document modified flag
                    doc.modified = true;

                    // update workspace
                    activeWorkspace.Refresh();
                    updateHistoryPanel();
                }

                // Set drag node to null
                this.dragNode = null;

                // Disable scroll timer
                this.scrollingTimer.Enabled = false;
            }
        }

        private void tvwSceneOutline_DragEnter(object sender, DragEventArgs e)
        {
            Point p = this.tvwSceneOutline.PointToClient(new Point(e.X, e.Y));
            DragHelper.ImageList_DragEnter(this.tvwSceneOutline.Handle, p.X, p.Y);

            // Enable timer for scrolling dragged item
            this.scrollingTimer.Enabled = true;
        }

        private void tvwSceneOutline_DragLeave(object sender, EventArgs e)
        {
            DragHelper.ImageList_DragLeave(this.tvwSceneOutline.Handle);

            // Disable timer for scrolling dragged item
            this.scrollingTimer.Enabled = false;
        }

        private void tvwSceneOutline_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move) {
                // Show pointer cursor while dragging
                e.UseDefaultCursors = false;
                this.tvwSceneOutline.Cursor = Cursors.Default;
            } else {
                e.UseDefaultCursors = true;
            }
        }

        private void scrollingTimer_Tick(object sender, EventArgs e)
        {
            // get node at mouse position
            Point pt = PointToClient(Control.MousePosition);
            TreeNode node = this.tvwSceneOutline.GetNodeAt(pt);

            if (node == null) return;

            // if mouse is near to the top, scroll up
            if (pt.Y < 30) {
                // set actual node to the upper one
                if (node.PrevVisibleNode != null) {
                    node = node.PrevVisibleNode;

                    // hide drag image
                    DragHelper.ImageList_DragShowNolock(false);
                    // scroll and refresh
                    node.EnsureVisible();
                    this.tvwSceneOutline.Refresh();
                    // show drag image
                    DragHelper.ImageList_DragShowNolock(true);

                }
            }
                // if mouse is near to the bottom, scroll down
            else if (pt.Y > this.tvwSceneOutline.Size.Height - 30) {
                if (node.NextVisibleNode != null) {
                    node = node.NextVisibleNode;

                    DragHelper.ImageList_DragShowNolock(false);
                    node.EnsureVisible();
                    this.tvwSceneOutline.Refresh();
                    DragHelper.ImageList_DragShowNolock(true);
                }
            }
        }

        #endregion

        #region ApplicationMenu

        private void rbtnNew_Click(object sender, EventArgs e)
        {
            FrmNewProject frmNewProject = new FrmNewProject();
            if (frmNewProject.ShowDialog() == DialogResult.Cancel)
                return;

            // create new document
            TDocument doc = new TDocument();

            // add new scene to new document
            TScene scene = new TScene(doc, doc.sceneManager.newSceneName());
            doc.sceneManager.addScene(scene);

            // save new document
            string filePath = frmNewProject.txtLocation.Text + "\\" + frmNewProject.txtName.Text + "\\" + frmNewProject.txtName.Text + "." + Program.DOC_EXTENSION;
            doc.save(filePath);
            doc.identifier = frmNewProject.txtIdentifier.Text;

            // recent
            this.updateRecentDocument(filePath);

            // update scenes panel
            lstScenes.Items.Add("");

            // show new form
            FrmWorkspace frmWorkspace = new FrmWorkspace();
            // assign document
            frmWorkspace.document = doc;

            frmWorkspace.MdiParent = this;
            if (this.ActiveMdiChild == null)
                frmWorkspace.WindowState = FormWindowState.Maximized;
            else
                frmWorkspace.WindowState = this.ActiveMdiChild.WindowState;

            frmWorkspace.showRuler(rchkVisibleRuler.Checked);
            frmWorkspace.Show();
        }

        private bool openDocument(string file)
        {
            if (!File.Exists(file)) {
                MessageBox.Show("File does not exist: " + file);
                return false;
            }

            if (file.EndsWith("." + Program.PACKAGE_EXTENSION)) {
                if (ZipFile.IsZipFile(file)) {
                    string path = TUtil.getTemporaryDirectory();
                    if (path != null) {
                        try {
                            using (ZipFile zip = new ZipFile(file)) {
                                zip.ExtractAll(path, ExtractExistingFileAction.OverwriteSilently);
                                file = Path.Combine(path, "main." + Program.DOC_EXTENSION);
                            }
                        } catch (Exception) {
                            MessageBox.Show("Could not open Tata Package File.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    } else {
                        MessageBox.Show("Could not open Tata Package File.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                } else {
                    MessageBox.Show("Invalid Tata Package File.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // open document
            TDocument doc = new TDocument();
            if (!doc.open(file)) {
                MessageBox.Show("Could not open the file because it is not the right kind of document.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // recent
            this.updateRecentDocument(file);

            // open workspace
            FrmWorkspace frmWorkspace = new FrmWorkspace();

            // assign document
            frmWorkspace.document = doc;

            frmWorkspace.MdiParent = this;
            if (this.ActiveMdiChild == null)
                frmWorkspace.WindowState = FormWindowState.Maximized;
            else
                frmWorkspace.WindowState = this.ActiveMdiChild.WindowState;

            frmWorkspace.showRuler(rchkVisibleRuler.Checked);
            frmWorkspace.Show();

            return true;
        }

        private void rbtnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = String.Format("Tata Builder Files (*.{0}, *.{1})|*.{0};*.{1}|Tata Builder Project Files (*.{0})|*.{0}|Tata Builder Package Files (*.{1})|*.{1}|All files (*.*)|*.*", Program.DOC_EXTENSION, Program.PACKAGE_EXTENSION);

            DialogResult dr = dlg.ShowDialog(this);
            if (dr == DialogResult.Cancel) return;
            if (dr != DialogResult.OK) throw new ApplicationException();

            this.openDocument(dlg.FileName);
        }

        private void rbtnSave_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                activeWorkspace.saveDocument(false);
            }
        }

        private void rbtnSaveAs_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                activeWorkspace.saveDocument(true);
            }
        }

        private void rbtnExport_Click(object sender, EventArgs e) {

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                if (!doc.save()) {
                    MessageBox.Show("Could not save the project.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SaveFileDialog dlg = new SaveFileDialog();
                int k = -1;
                if ((k = doc.filename.LastIndexOf(".")) == -1)
                    dlg.FileName = doc.filename;
                else
                    dlg.FileName = doc.filename.Substring(0, k);
                if (doc.directory != null) dlg.InitialDirectory = doc.directory;

                dlg.Filter = String.Format("Tata Builder Package Files (*.{0})|*.{0}", Program.PACKAGE_EXTENSION);

                DialogResult dr = dlg.ShowDialog();
                if (dr == DialogResult.OK) {
                    doc.export(dlg.FileName);
                }
            }
        }

        private void rbtnPublish_Click(object sender, EventArgs e) {

            if (Program.signed == false) {
                // signin
                rbtnSignIn_Click(sender, e);

                // check result
                if (Program.signed == false) {
                    return;
                }
            }

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {

                // save document before publish
                if (!doc.save()) {
                    MessageBox.Show("Could not save the project.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                FrmPublish frmPublish = new FrmPublish();
                frmPublish.document = doc;
                frmPublish.ShowDialog();
            }            
        }

        private void rbtnDeployProject_Click(object sender, EventArgs e) {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null) {

                // save document before publish
                if (!doc.save()) {
                    MessageBox.Show("Could not save the project.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                FrmDeploy frmDeploy = new FrmDeploy();
                frmDeploy.document = doc;
                frmDeploy.ShowDialog();
            }
        }

        private void rbtnDeployScene_Click(object sender, EventArgs e) {

        }

        private void rbtnClose_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null)
                activeWorkspace.Close();
        }

        private void rbtnOptions_Click(object sender, EventArgs e)
        {
            string path = Assembly.GetExecutingAssembly().CodeBase;
            string adminPath = Path.Combine(Path.GetDirectoryName(path), "TtbAdmin.exe");
            var processInfo = new ProcessStartInfo(adminPath);

            // The following properties run the new process as administrator
            processInfo.UseShellExecute = true;
            processInfo.Verb = "runas";
            processInfo.WindowStyle = ProcessWindowStyle.Hidden;

            // Start the new process
            try {
                Process.Start(processInfo);
            } catch (Exception) {
            }
        }

        private void rbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbtnSignIn_Click(object sender, EventArgs e) 
        {
            rlblAppStatus.Text = "Sigining...";

            FrmSignIn frmSignIn = new FrmSignIn();
            frmSignIn.ShowDialog();

            if (Program.signed) {
                rbtnSignIn.Visible = false;
                rmnuUser.Text = String.Format("Welcome, {0}!", Program.author);
                rmnuUser.Visible = true;
            }

            rlblAppStatus.Text = "Ready";
        }

        private void rbtnSignOut_Click(object sender, EventArgs e) 
        {
            try {
                rlblAppStatus.Text = "Sigouting...";
                var response = Program.webClient.UploadString(Program.URL_LOGOUT, "POST", "");

                rmnuUser.Visible = false;
                rbtnSignIn.Visible = true;

                Program.signed = false;
                return ;
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Could not connect the server. Please check your internet connection.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                rlblAppStatus.Text = "Ready";
            }

            return;
        }

        #endregion

        #region RibbonBar

        #region Quick Menu Bar

        private void rbtnUndo_Click(object sender, EventArgs e)
        {
            // Determine the active form
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                // document
                TDocument doc = activeWorkspace.document;
                doc.actionManager.Undo();

                // set the document modified flag
                doc.modified = true;
                
                // redraw all panel
                activeWorkspace.Refresh();

                this.updateToolbar();
                this.updateScenesPanel();
                this.updateOutlinePanel();
                this.updateHistoryPanel();
            }
        }

        private void rbtnRedo_Click(object sender, EventArgs e)
        {
            // Determine the active form
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                // document
                TDocument doc = activeWorkspace.document;
                doc.actionManager.Redo();

                // set the document modified flag
                doc.modified = true;

                // redraw all panel
                activeWorkspace.Refresh();

                this.updateToolbar();
                this.updateScenesPanel();
                this.updateOutlinePanel();
                this.updateHistoryPanel();
            }
        }

        private void rbtnEmulatePhone_Click(object sender, EventArgs e)
        {
            TDocument doc = this.activeDocument();
            if (doc != null) {
                FrmEmulator emulator = new FrmEmulator(FrmEmulator.EmulatorType.Phone);
                emulator.document = doc;
                emulator.ShowDialog();
            }
        }

        private void rbtnEmulateTablet_Click(object sender, EventArgs e)
        {
            TDocument doc = this.activeDocument();
            if (doc != null) {
                FrmEmulator emulator = new FrmEmulator(FrmEmulator.EmulatorType.Tablet);
                emulator.document = doc;
                emulator.ShowDialog();
            }
        }

        #endregion

        #region Home Tab

        private void rbtnDelete_Click(object sender, EventArgs e)
        {
            // Determine the active form
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                // document
                TDocument doc = activeWorkspace.document;
                if (doc != null && doc.currentScene() != null) {
                    if (doc.haveSelection()) {
                        // remove actor
                        doc.actionManager.RecordAction(new DeleteActorsAction(doc, doc.selectedItems));

                        // set the document modified flag
                        doc.modified = true;

                        // redraw document
                        activeWorkspace.Refresh();

                        // update other windows
                        doc.sceneManager.updateThumbnail(doc.sceneManager.currentSceneIndex);
                        updateScenesPanel(doc.sceneManager.currentSceneIndex);
                        updateOutlinePanel();
                        updateToolbarSceneSettings();
                        updateToolbarSelection();
                        updateHistoryPanel();
                    }
                }
            }
        }

        private void rbtnCut_Click(object sender, EventArgs e)
        {
            // Determine the active form
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                // document
                TDocument doc = activeWorkspace.document;
                if (doc != null && doc.currentScene() != null) {
                    TActor actor = doc.selectedActor();
                    if (actor != null) {
                        Clipboard.Clear();
                        Clipboard.SetData(CLIPBOARD_ACTOR_FORMAT, actor);

                        // remove actor
                        doc.actionManager.RecordAction(new DeleteActorsAction(doc, doc.selectedItems));

                        // set the document modified flag
                        doc.modified = true;

                        // redraw document
                        activeWorkspace.Refresh();

                        // update other windows
                        doc.sceneManager.updateThumbnail(doc.sceneManager.currentSceneIndex);
                        updateScenesPanel(doc.sceneManager.currentSceneIndex);
                        updateOutlinePanel();
                        updateToolbarSceneSettings();
                        updateToolbarSelection();
                        updateHistoryPanel();
                    }
                }
            }
        }

        private void rbtnCopy_Click(object sender, EventArgs e)
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {
                TActor actor = doc.selectedActor();
                if (actor != null) {
                    Clipboard.Clear();
                    Clipboard.SetData(CLIPBOARD_ACTOR_FORMAT, actor);
                    updateToolbarSelection();
                }
            }
        }

        private void rbtnPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(CLIPBOARD_ACTOR_FORMAT)) {
                // Determine the active form
                FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
                if (activeWorkspace != null) {
                    // document
                    TDocument doc = activeWorkspace.document;

                    if (doc != null && doc.currentScene() != null) {
                        // determine the parent layer to insert the new actor
                        TLayer parent = null;
                        if (doc.haveSelection())
                            parent = doc.selectedActor().parent;
                        else
                            parent = doc.currentScene();

                        // pull the actor from clipboard
                        TActor actor = (TActor)Clipboard.GetDataObject().GetData(CLIPBOARD_ACTOR_FORMAT);
                        if (actor != null) {
                            // fix child-parent relationsip and nonserialized fields
                            actor.fixRelationship();
                            actor.document = doc;
                            if (doc.currentScene().findLayer(actor.name) != null)
                                actor.name = doc.currentScene().newLayerName("Actor_");
                            actor.parent = parent;

                            // add new actor to parent
                            doc.actionManager.RecordAction(new AddActorAction(doc, actor));

                            // set the document modified flag
                            doc.modified = true;

                            // select the new actor
                            doc.selectedItems.Clear();
                            doc.selectedItems.Add(actor);

                            // redraw document
                            activeWorkspace.Refresh();

                            // update other windows
                            doc.sceneManager.updateThumbnail(doc.sceneManager.currentSceneIndex);
                            updateScenesPanel(doc.sceneManager.currentSceneIndex);
                            updateOutlinePanel();
                            updateToolbarSceneSettings();
                            updateToolbarSelection();
                            updateHistoryPanel();
                        }
                    }
                }
            }
        }

        private void rbtnNewScene_Click(object sender, EventArgs e)
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                // add new scene to document with history
                TScene scene = new TScene(doc, doc.sceneManager.newSceneName());
                AddSceneAction addSceneAction = new AddSceneAction(doc, this, scene);
                doc.actionManager.RecordAction(addSceneAction);

                // set the document modified flag
                doc.modified = true;

                // select the new scene
                this.lstScenes.SelectedIndex = doc.sceneManager.sceneCount() - 1;

                // update history
                updateHistoryPanel();
            }
        }

        private void rbtnCloneScene_Click(object sender, EventArgs e)
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                // clone the specified scene
                TScene scene = (TScene)doc.currentScene().clone();
                scene.name = doc.sceneManager.newSceneName();

                // add the cloned scene
                AddSceneAction addSceneAction = new AddSceneAction(doc, this, scene);
                doc.actionManager.RecordAction(addSceneAction);

                // set the document modified flag
                doc.modified = true;

                // select the duplicated scene
                this.lstScenes.SelectedIndex = doc.sceneManager.sceneCount() - 1;

                // update history
                updateHistoryPanel();
            }
        }

        private void rbtnDeleteScene_Click(object sender, EventArgs e)
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                if (doc.currentScene() != null) {

                    // remove scene
                    DeleteSceneAction deleteSceneAction = new DeleteSceneAction(doc, this, doc.sceneManager.currentSceneIndex, doc.currentScene());
                    doc.actionManager.RecordAction(deleteSceneAction);

                    // set the document modified flag
                    doc.modified = true;

                    // update history
                    updateHistoryPanel();
                }
            }
        }

        private void rbtnSelectTool_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                doc.currentTool = TDocument.TOOL_SELECT;

                this.updateToolbarTools();

                activeWorkspace.updateCursor();
                activeWorkspace.Refresh();
            }
        }

        private void rbtnHandTool_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                doc.currentTool = TDocument.TOOL_HAND;

                this.updateToolbarTools();

                activeWorkspace.updateCursor();
                activeWorkspace.Refresh();
            }
        }

        private void rbtnTextTool_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                doc.currentTool = TDocument.TOOL_TEXT;

                this.updateToolbarTools();

                activeWorkspace.updateCursor();
                activeWorkspace.Refresh();
            }
        }

        private void rbtnBoundingTool_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                doc.currentTool = TDocument.TOOL_BOUNDING;

                this.updateToolbarTools();

                activeWorkspace.updateCursor();
                activeWorkspace.Refresh();
            }
        }

        private void rbtnAvatarTool_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                doc.currentTool = TDocument.TOOL_AVATAR;

                this.updateToolbarTools();

                activeWorkspace.updateCursor();
                activeWorkspace.Refresh();
            }
        }

        private void rbtnPuzzleTool_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null)
            {
                TDocument doc = activeWorkspace.document;
                doc.currentTool = TDocument.TOOL_PUZZLE;

                this.updateToolbarTools();

                activeWorkspace.updateCursor();
                activeWorkspace.Refresh();
            }
        }

        #endregion

        #region View Tab

        private void rchkVisibleRuler_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.MdiChildren.Length; i++) {
                FrmWorkspace workspace = this.MdiChildren[i] as FrmWorkspace;
                workspace.showRuler(rchkVisibleRuler.Checked);
            }
        }

        private void rchkVisibleResources_CheckedChanged(object sender, EventArgs e)
        {
            if (rchkVisibleResources.Checked) {
                pnlRightBar.Visible = true;
                splMainRightDivider.Visible = true;
            }

            pnlResources.Visible = rchkVisibleResources.Checked;
        }

        private void rchkVisibleLayers_CheckedChanged(object sender, EventArgs e)
        {
            if (rchkVisibleLayers.Checked) {
                pnlRightBar.Visible = true;
                splMainRightDivider.Visible = true;
            }

            pnlLayers.Visible = rchkVisibleLayers.Checked;
        }

        private void rbtnZoom_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;

                FrmZoom dlgZoom = new FrmZoom(doc.zoom);
                if (dlgZoom.ShowDialog() == DialogResult.OK) {

                    changeDocumentZoom(dlgZoom.zoom);
                    activeWorkspace.Refresh();

                    // status bar
                    updateStatusBar();
                }
            }
        }

        private void rbtnZoomIn_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                if (doc.zoom + 0.05f <= 4) {

                    changeDocumentZoom(doc.zoom + 0.05f);
                    activeWorkspace.Refresh();

                    // status bar
                    updateStatusBar();
                }
            }
        }

        private void rbtnZoomOut_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                if (doc.zoom - 0.05f >= 0.25) {

                    changeDocumentZoom(doc.zoom - 0.05f);
                    activeWorkspace.Refresh();

                    // status bar
                    updateStatusBar();
                }
            }
        }

        private void rbtnZoomActualSize_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                changeDocumentZoom(1);
                doc.offset = new PointF(0, 0);
                activeWorkspace.Refresh();

                // status bar
                updateStatusBar();
            }
        }

        private void rbtnZoomFitWindow_Click(object sender, EventArgs e)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                Size workarea = activeWorkspace.getWorkArea();
                changeDocumentZoom(Math.Min((float)(workarea.Width - 30) / Program.BOOK_WIDTH, (float)(workarea.Height - 30) / Program.BOOK_HEIGHT));
                doc.offset = new PointF(0, 0);
                activeWorkspace.Refresh();

                // status bar
                updateStatusBar();
            }
        }

        private void rbtnWindowLayoutArrange_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void rbtnWindowLayoutCascade_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void rmnuSwitchWindows_DropDown(object sender, EventArgs e)
        {
            rmnuSwitchWindows.Items.Clear();
            for (int i = 0; i < this.MdiChildren.Length; i++) {
                FrmWorkspace workspace = this.MdiChildren[i] as FrmWorkspace;

                RibbonButton item = new RibbonButton(workspace.Text);
                item.Tag = i;
                item.Click += switchWindow;
                if (workspace == this.ActiveMdiChild)
                    item.SmallImage = Properties.Resources.toolbar_windows_check;

                rmnuSwitchWindows.Items.Add(item);
            }
        }

        private void switchWindow(object sender, EventArgs e)
        {
            RibbonButton item = sender as RibbonButton;
            FrmWorkspace workspace = this.MdiChildren[(int)item.Tag] as FrmWorkspace;
            this.ActivateMdiChild(workspace);
            workspace.WindowState = FormWindowState.Maximized;
        }

        #endregion

        #region Document Tab

        private void rtxtDocumentIdentifier_ChangeCommitted(object sender, EventArgs e) {

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                doc.identifier = rtxtDocumentIdentifier.Text;

                // set the document modified flag
                doc.modified = true;
            }
        }

        private void rcmbDocumentBGM_DropDown(object sender, EventArgs e)
        {
            // clear
            rcmbDocumentBGM.Items.Clear();
            rcmbDocumentBGM.Items.Add(Program.NONE_ITEM);

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                int soundCount = doc.libraryManager.soundCount();
                for (int i = 0; i < soundCount; i++) {
                    rcmbDocumentBGM.Items.Add(doc.libraryManager.soundFileName(i));
                }
            }
        }

        private void rcmbDocumentBGM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rcmbDocumentBGM.Text == Program.NONE_ITEM)
                rcmbDocumentBGM.Text = "";

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                doc.actionManager.RecordAction(new ChangeDocumentBGMAction(doc, this, doc.backgroundMusic, rcmbDocumentBGM.Text));

                // set the document modified flag
                doc.modified = true;

                // update panels
                updateHistoryPanel();
            }
        }

        private void rtrbDocumentBGMVolume_ValueChanged(object sender, EventArgs e)
        {
            if (rtrbDocumentBGMVolume.Tag != null && (bool)rtrbDocumentBGMVolume.Tag == true)
                return;

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                doc.actionManager.RecordAction(new ChangeDocumentBGMVolumeAction(doc, this, doc.backgroundMusicVolume, rtrbDocumentBGMVolume.Value));

                // set the document modified flag
                doc.modified = true;

                // update panels
                updateHistoryPanel();
            }
        }

        private void rtrbDocumentNVBDelay_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void rtxtDocumentNVBDelay_ChnageCommitted(object sender, EventArgs e)
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null)
            {
                doc.actionManager.RecordAction(new ChangeDocumentNVBDelayAction(doc, this, doc.navigationButtonDelayTime, Int32.Parse(rtxtDocumentNVBDelay.Text)));

                // set the document modified flag
                doc.modified = true;

                // update panels
                updateHistoryPanel();
            }
        }

        private void rchkRenderNavigationLeftButton_CheckedChanged(object sender, EventArgs e)
        {
            TDocument doc = this.activeDocument();
            if (doc != null)
            {
                
                doc.actionManager.RecordAction(new ChangeDocumentNVBLeftRenderAction(doc, this, doc.navigationLeftButtonRender, rchkRenderNavigationLeftButton.Checked));

                // set the document modified flag
                doc.modified = true;

                // update panels
                updateHistoryPanel();
            }
        }

        private void rchkRenderNavigationRightButton_CheckedChanged(object sender, EventArgs e)
        {
            TDocument doc = this.activeDocument();
            if (doc != null)
            {
                doc.actionManager.RecordAction(new ChangeDocumentNVBRightRenderAction(doc, this, doc.navigationRightButtonRender, rchkRenderNavigationRightButton.Checked));

                // set the document modified flag
                doc.modified = true;

                // update panels
                updateHistoryPanel();
            }
        }

        private void rgryPrevSceneButton_DropDown(object sender, EventArgs e)
        {
            // clear
            rgryPrevSceneButton.Items.Clear();

            rgryPrevSceneButton.Items.Add(new RibbonGalleryItem("", Properties.Resources.toolbar_project_prev));

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                int imageCount = doc.libraryManager.imageCount();
                for (int i = 0; i < imageCount; i++) {
                    Image img = doc.libraryManager.imageForToolbarAtIndex(i);
                    RibbonGalleryItem item = new RibbonGalleryItem("", img);
                    rgryPrevSceneButton.Items.Add(item);
                }

                // current selection
                manualSceneButtonChanged = true;
                rgryPrevSceneButton.SelectedIndex = doc.libraryManager.imageIndex(doc.prevSceneButton) + 1;
                manualSceneButtonChanged = false;
            }
        }

        private void rgryNextSceneButton_DropDown(object sender, EventArgs e)
        {
            // clear
            rgryNextSceneButton.Items.Clear();

            rgryNextSceneButton.Items.Add(new RibbonGalleryItem("", Properties.Resources.toolbar_project_next));

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                int imageCount = doc.libraryManager.imageCount();
                for (int i = 0; i < imageCount; i++) {
                    Image img = doc.libraryManager.imageForToolbarAtIndex(i);
                    RibbonGalleryItem item = new RibbonGalleryItem("", img);
                    rgryNextSceneButton.Items.Add(item);
                }

                // current selection
                manualSceneButtonChanged = true;
                rgryNextSceneButton.SelectedIndex = doc.libraryManager.imageIndex(doc.nextSceneButton);
                manualSceneButtonChanged = false;
            }
        }

        private bool manualSceneButtonChanged = false;

        private void rgryPrevSceneButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manualSceneButtonChanged)
                return;

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                string newPrevSceneButton = doc.libraryManager.imageFileName(rgryPrevSceneButton.SelectedIndex - 1);
                doc.actionManager.RecordAction(new ChangeDocumentPrevSceneButton(doc, this, doc.prevSceneButton, newPrevSceneButton));

                // set the document modified flag
                doc.modified = true;

                // update panels
                updateHistoryPanel();
            }
        }

        private void rgryNextSceneButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manualSceneButtonChanged)
                return;

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                string newNextSceneButton = doc.libraryManager.imageFileName(rgryNextSceneButton.SelectedIndex - 1);
                doc.actionManager.RecordAction(new ChangeDocumentNextSceneButton(doc, this, doc.nextSceneButton, newNextSceneButton));

                // set the document modified flag
                doc.modified = true;

                // update panels
                updateHistoryPanel();
            }
        }

        private bool manualVatarImageChanged = false;

        private void rgryAvatarDefault_DropDown(object sender, EventArgs e)
        {
            // clear
            rgryAvatarDefault.Items.Clear();
            rgryAvatarDefault.Items.Add(new RibbonGalleryItem("", Properties.Resources.toolbar_avatar_default));

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                int imageCount = doc.libraryManager.imageCount();
                for (int i = 0; i < imageCount; i++) {
                    Image img = doc.libraryManager.imageForToolbarAtIndex(i);
                    RibbonGalleryItem item = new RibbonGalleryItem("", img);
                    rgryAvatarDefault.Items.Add(item);
                }

                // current selection
                manualVatarImageChanged = true;
                rgryAvatarDefault.SelectedIndex = doc.libraryManager.imageIndex(doc.avatarDefault) + 1;
                manualVatarImageChanged = false;
            }
        }

        private void rgryAvatarFrame_DropDown(object sender, EventArgs e)
        {
            // clear
            rgryAvatarFrame.Items.Clear();
            rgryAvatarFrame.Items.Add(new RibbonGalleryItem("", Properties.Resources.toolbar_avatar_frame));

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                int imageCount = doc.libraryManager.imageCount();
                for (int i = 0; i < imageCount; i++) {
                    Image img = doc.libraryManager.imageForToolbarAtIndex(i);
                    RibbonGalleryItem item = new RibbonGalleryItem("", img);
                    rgryAvatarFrame.Items.Add(item);
                }

                // current selection
                manualVatarImageChanged = true;
                rgryAvatarFrame.SelectedIndex = doc.libraryManager.imageIndex(doc.avatarFrame) + 1;
                manualVatarImageChanged = false;
            }
        }

        private void rgryAvatarMask_DropDown(object sender, EventArgs e)
        {
            // clear
            rgryAvatarMask.Items.Clear();
            rgryAvatarMask.Items.Add(new RibbonGalleryItem("", Properties.Resources.toolbar_avatar_mask));

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                int imageCount = doc.libraryManager.imageCount();
                for (int i = 0; i < imageCount; i++) {
                    Image img = doc.libraryManager.imageForToolbarAtIndex(i);
                    RibbonGalleryItem item = new RibbonGalleryItem("", img);
                    rgryAvatarMask.Items.Add(item);
                }

                // current selection
                manualVatarImageChanged = true;
                rgryAvatarMask.SelectedIndex = doc.libraryManager.imageIndex(doc.avatarMask) + 1;
                manualVatarImageChanged = false;
            }
        }

        private void rgryAvatarDefault_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manualVatarImageChanged)
                return;

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                string newAvatarDefault = doc.libraryManager.imageFileName(rgryAvatarDefault.SelectedIndex - 1);
                doc.actionManager.RecordAction(new ChangeDocumentAvatarDefault(doc, this, doc.avatarDefault, newAvatarDefault));

                // set the document modified flag
                doc.modified = true;

                // update panels
                updateHistoryPanel();
            }
        }

        private void rgryAvatarFrame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manualVatarImageChanged)
                return;

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                string newAvatarFrame = doc.libraryManager.imageFileName(rgryAvatarFrame.SelectedIndex - 1);
                doc.actionManager.RecordAction(new ChangeDocumentAvatarFrame(doc, this, doc.avatarFrame, newAvatarFrame));

                // set the document modified flag
                doc.modified = true;

                // update panels
                updateHistoryPanel();
            }
        }

        private void rgryAvatarMask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manualVatarImageChanged)
                return;

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                string newAvatarMask = doc.libraryManager.imageFileName(rgryAvatarMask.SelectedIndex - 1);
                doc.actionManager.RecordAction(new ChangeDocumentAvatarMask(doc, this, doc.avatarMask, newAvatarMask));

                // set the document modified flag
                doc.modified = true;

                // update panels
                updateHistoryPanel();
            }
        }

        #endregion

        #region Resources Tab

        private void rbtnImportImages_Click(object sender, EventArgs e)
        {
            btnAddImage_Click(sender, e);
        }

        private void rbtnImportSounds_Click(object sender, EventArgs e)
        {
            btnAddSound_Click(sender, e);
        }

        #endregion

        #region Animations Tab

        private void rgryEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            rbtnRenameEvent.Enabled = rgryEvents.SelectedIndex != -1;
            rbtnDeleteEvent.Enabled = rgryEvents.SelectedIndex != -1;
        }

        private void rbtnNewEvent_Click(object sender, EventArgs e)
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {

                // active layer
                TLayer layer = doc.selectedLayer();

                string eventu = Interaction.InputBox("Please input the name of new event", Program.APP_NAME);
                if (eventu != "") {
                    if (!layer.addEvent(eventu)) {
                        MessageBox.Show("Event [" + eventu + "] already exists", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    rgryEvents.Items.Add(eventu);
                    rcmbAnimationEvent.Items.Add(eventu);

                    // set the document modified flag
                    doc.modified = true;
                }
            }
        }

        private void rbtnRenameEvent_Click(object sender, EventArgs e)
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {

                // active layer
                TLayer selectedLayer = doc.selectedLayer();

                if (rgryEvents.SelectedItem != null) {
                    string eventu = rgryEvents.SelectedItem.Text;
                    string newEventu = Interaction.InputBox("Please input new name of event you want to change.", Program.APP_NAME, eventu);

                    // case when click cancel button
                    if (newEventu == "")
                        return;

                    // check if it is default event
                    if (selectedLayer.getDefaultEvents().Contains(eventu)) {
                        MessageBox.Show("Event [" + eventu + "] is default event, so it can'be renamed.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // check if new name exists
                    if (selectedLayer.getEvents().Contains(newEventu)) {
                        MessageBox.Show("Event [" + newEventu + "] already exists", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // rename all references in animations of selected layer
                    foreach (TAnimation animation in selectedLayer.animations) {
                        if (animation.eventu == eventu)
                            animation.eventu = newEventu;
                    }

                    // rename all references in animations of all layers of current scene
                    List<TLayer> allLayers = doc.currentScene().getAllChilds();
                    foreach (TLayer layer in allLayers) {
                        foreach (TAnimation animation in layer.animations) {
                            for (int i = 0; i < animation.numberOfSequences(); i++) {
                                for (int j = 0; j < animation.numberOfActionsInSequence(i); j++) {
                                    TAction action = animation.actionAtIndex(i, j);
                                    if (action is TActionInstantDispatchEvent) {
                                        TActionInstantDispatchEvent typedAction = (TActionInstantDispatchEvent)action;
                                        if (typedAction.actor == selectedLayer.name && typedAction.eventu == eventu)
                                            typedAction.eventu = newEventu;
                                    }

                                }
                            }
                        }
                    }

                    selectedLayer.renameEvent(eventu, newEventu);
                    rgryEvents.SelectedItem.Text = newEventu;
                    updateToolbarAnimationPart(selectedLayer);

                    // set the document modified flag
                    doc.modified = true;
                }

                rbtnRenameEvent.Enabled = rgryEvents.SelectedIndex != -1;
                rbtnDeleteEvent.Enabled = rgryEvents.SelectedIndex != -1;
            }
        }

        private void rbtnDeleteEvent_Click(object sender, EventArgs e)
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {

                // active layer
                TLayer selectedLayer = doc.selectedLayer();

                if (rgryEvents.SelectedItem != null) {
                    string eventu = rgryEvents.SelectedItem.Text;

                    // check if it is default event
                    if (selectedLayer.getDefaultEvents().Contains(eventu)) {
                        MessageBox.Show("Event [" + eventu + "] is default event, so it can'be deleted.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // rename all references in animations of selected layer
                    foreach (TAnimation animation in selectedLayer.animations) {
                        if (animation.eventu == eventu) {
                            MessageBox.Show("Event [" + eventu + "] couldn't be deleted because it's been used.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // rename all references in animations of all layers of current scene
                    List<TLayer> allLayers = doc.currentScene().getAllChilds();
                    foreach (TLayer layer in allLayers) {
                        foreach (TAnimation animation in layer.animations) {
                            for (int i = 0; i < animation.numberOfSequences(); i++) {
                                for (int j = 0; j < animation.numberOfActionsInSequence(i); j++) {
                                    TAction action = animation.actionAtIndex(i, j);
                                    if (action is TActionInstantDispatchEvent) {
                                        TActionInstantDispatchEvent typedAction = (TActionInstantDispatchEvent)action;
                                        if (typedAction.actor == selectedLayer.name && typedAction.eventu == eventu) {
                                            MessageBox.Show("Event [" + eventu + "] couldn't be deleted because it's been used.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                    }

                                }
                            }
                        }
                    }

                    selectedLayer.deleteEvent(eventu);
                    rgryEvents.Items.Remove(rgryEvents.SelectedItem);
                    rcmbAnimationEvent.Items.Remove(eventu);

                    // set the document modified flag
                    doc.modified = true;
                }

                rbtnRenameEvent.Enabled = rgryEvents.SelectedIndex != -1;
                rbtnDeleteEvent.Enabled = rgryEvents.SelectedIndex != -1;
            }
        }

        private void rgryStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            rbtnRenameState.Enabled = rgryStates.SelectedIndex != -1;
            rbtnDeleteState.Enabled = rgryStates.SelectedIndex != -1;
        }

        private void rbtnNewState_Click(object sender, EventArgs e)
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {

                // active layer
                TLayer layer = doc.selectedLayer();

                string state = Interaction.InputBox("Please input the name of new state", Program.APP_NAME);
                if (state != "") {
                    if (!layer.addState(state)) {
                        MessageBox.Show("State [" + state + "] already exists", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    rgryStates.Items.Add(state);
                    rcmbAnimationState.Items.Add(state);

                    // set the document modified flag
                    doc.modified = true;
                }
            }
        }

        private void rbtnRenameState_Click(object sender, EventArgs e)
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {

                // active layer
                TLayer selectedLayer = doc.selectedLayer();

                if (rgryStates.SelectedItem != null) {
                    string state = rgryStates.SelectedItem.Text;
                    string newState = Interaction.InputBox("Please input new name of state you want to change.", Program.APP_NAME, state);

                    // case when click cancel button
                    if (newState == "")
                        return;

                    // check if it is default state
                    if (selectedLayer.getDefaultStates().Contains(state)) {
                        MessageBox.Show("State [" + state + "] is default state, so it can'be renamed.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // check if new name exists
                    if (selectedLayer.getStates().Contains(newState)) {
                        MessageBox.Show("State [" + newState + "] already exists", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // rename all references in animations of selected layer
                    foreach (TAnimation animation in selectedLayer.animations) {
                        if (animation.state == state)
                            animation.state = newState;
                    }

                    // rename all references in animations of all layers of current scene
                    List<TLayer> allLayers = doc.currentScene().getAllChilds();
                    foreach (TLayer layer in allLayers) {
                        foreach (TAnimation animation in layer.animations) {
                            for (int i = 0; i < animation.numberOfSequences(); i++) {
                                for (int j = 0; j < animation.numberOfActionsInSequence(i); j++) {
                                    TAction action = animation.actionAtIndex(i, j);
                                    if (action is TActionInstantChangeState) {
                                        TActionInstantChangeState typedAction = (TActionInstantChangeState)action;
                                        if (typedAction.actor == selectedLayer.name && typedAction.state == state)
                                            typedAction.state = newState;
                                    }

                                }
                            }
                        }
                    }

                    selectedLayer.renameState(state, newState);
                    rgryStates.SelectedItem.Text = newState;
                    updateToolbarAnimationPart(selectedLayer);

                    // set the document modified flag
                    doc.modified = true;
                }

                rbtnRenameState.Enabled = rgryStates.SelectedIndex != -1;
                rbtnDeleteState.Enabled = rgryStates.SelectedIndex != -1;
            }
        }

        private void rbtnDeleteState_Click(object sender, EventArgs e)
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {

                // active layer
                TLayer selectedLayer = doc.selectedLayer();

                if (rgryStates.SelectedItem != null) {
                    string state = rgryStates.SelectedItem.Text;

                    // check if it is default state
                    if (selectedLayer.getDefaultStates().Contains(state)) {
                        MessageBox.Show("State [" + state + "] is default state, so it can'be deleted.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // rename all references in animations of selected layer
                    foreach (TAnimation animation in selectedLayer.animations) {
                        if (animation.state == state) {
                            MessageBox.Show("Event [" + state + "] couldn't be deleted because it's been used.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // rename all references in animations of all layers of current scene
                    List<TLayer> allLayers = doc.currentScene().getAllChilds();
                    foreach (TLayer layer in allLayers) {
                        foreach (TAnimation animation in layer.animations) {
                            for (int i = 0; i < animation.numberOfSequences(); i++) {
                                for (int j = 0; j < animation.numberOfActionsInSequence(i); j++) {
                                    TAction action = animation.actionAtIndex(i, j);
                                    if (action is TActionInstantChangeState) {
                                        TActionInstantChangeState typedAction = (TActionInstantChangeState)action;
                                        if (typedAction.actor == selectedLayer.name && typedAction.state == state) {
                                            MessageBox.Show("Event [" + state + "] couldn't be deleted because it's been used.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    selectedLayer.deleteState(state);
                    rgryStates.Items.Remove(rgryStates.SelectedItem);
                    rcmbAnimationState.Items.Remove(state);

                    // set the document modified flag
                    doc.modified = true;
                }

                rbtnRenameState.Enabled = rgryStates.SelectedIndex != -1;
                rbtnDeleteState.Enabled = rgryStates.SelectedIndex != -1;
            }
        }

        private void rgryAnimations_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateToolbarAnimationProperties();
        }

        public void addAnimationListItem(TAnimation animation)
        {
            // add item into gallery list
            RibbonGalleryItem item = new RibbonGalleryItem("", imageOfAnimation(animation));
            item.Tag = animation;
            rgryAnimations.Items.Add(item);
        }

        public void insertAnimationListItem(int index, TAnimation animation)
        {
            // add item into gallery list
            RibbonGalleryItem item = new RibbonGalleryItem("", imageOfAnimation(animation));
            item.Tag = animation;
            rgryAnimations.Items.Insert(index, item);
        }

        public void removeAnimationListItem(int index)
        {
            if (index == -1)
                // remove last item
                rgryAnimations.Items.RemoveAt(rgryAnimations.Items.Count - 1);
            else
                // remove at the specified index
                rgryAnimations.Items.RemoveAt(index);
        }

        public void updateAnimationListItem(int index, TAnimation animation)
        {
            rgryAnimations.Items[index].Tag = animation;
        }

        public void updateAnimationListItemImage(int index, TAnimation animation)
        {
            rgryAnimations.Items[index].LargeImage = imageOfAnimation(animation);
        }

        private void rbtnNewAnimation_Click(object sender, EventArgs e)
        {
            TDocument doc = this.activeDocument();
            if (doc != null) {
                TLayer layer = doc.selectedLayer();
                TAnimation animation = new TAnimation(layer);
                animation.eventu = (sender as RibbonButton).Text;

                // add default empty sequence
                animation.addSequence();

                // add animation to animation list of layer
                doc.actionManager.RecordAction(new AddAnimationAction(this, layer, animation));

                // set the document modified flag
                doc.modified = true;

                Application.DoEvents();
                rgryAnimations.SelectedIndex = rgryAnimations.Items.Count - 1;

                this.updateHistoryPanel();
            }
        }

        private void rmnuNewAnimation_DropDown(object sender, EventArgs e)
        {
            // clear
            rmnuNewAnimation.Items.Clear();

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                TLayer layer = doc.selectedLayer();
                rmnuNewAnimation.Items.Clear();
                foreach (string eventu in layer.getEvents()) {
                    RibbonButton button = new RibbonButton(eventu);
                    button.Click += rbtnNewAnimation_Click;
                    rmnuNewAnimation.Items.Add(button);
                }
            }
        }

        private void rbtnEditAnimation_Click(object sender, EventArgs e)
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {

                // active layer
                TLayer selectedLayer = doc.selectedLayer();

                if (rgryAnimations.SelectedItem != null) {
                    TAnimation animation = (TAnimation)rgryAnimations.SelectedItem.Tag;

                    FrmAnimationTimeline dlg = new FrmAnimationTimeline();
                    dlg.document = doc;
                    dlg.animation = animation.clone();
                    if (dlg.ShowDialog() == DialogResult.OK) {
                        // replace the original animation with new animation
                        doc.actionManager.RecordAction(new ChangeAnimationAction(this, selectedLayer, animation, dlg.animation));

                        // set the document modified flag
                        doc.modified = true;
                    }
                }

                rbtnEditAnimation.Enabled = rgryAnimations.SelectedIndex != -1;
                rbtnDeleteAnimation.Enabled = rgryAnimations.SelectedIndex != -1;

                this.updateHistoryPanel();
            }
        }

        private void rbtnDeleteAnimation_Click(object sender, EventArgs e)
        {
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {
                TLayer layer = doc.selectedLayer();

                if (rgryAnimations.SelectedItem != null) {
                    TAnimation animation = (TAnimation)rgryAnimations.SelectedItem.Tag;
                    int index = layer.animations.IndexOf(animation);

                    doc.actionManager.RecordAction(new DeleteAnimationAction(this, layer, index));

                    // set the document modified flag
                    doc.modified = true;

                    this.updateToolbarAnimationProperties();
                    this.updateHistoryPanel();
                }
            }
        }

        private void rcmbAnimationEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            // manual change
            if (rcmbAnimationEvent.Tag != null)
                return;

            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {

                // active layer
                TLayer selectedLayer = doc.selectedLayer();

                if (rgryAnimations.SelectedItem != null) {
                    TAnimation animation = (TAnimation)rgryAnimations.SelectedItem.Tag;
                    string newEvent;
                    if (rcmbAnimationEvent.SelectedIndex < 1) // if none is selected or undefined is selected
                        newEvent = Program.DEFAULT_EVENT_UNDEFINED;
                    else
                        newEvent = rcmbAnimationEvent.Text;

                    doc.actionManager.RecordAction(new ChangeAnimationEventAction(this, rgryAnimations.SelectedIndex, animation, newEvent));
                    this.updateHistoryPanel();
                }
            }
        }

        private void rcmbAnimationState_SelectedIndexChanged(object sender, EventArgs e)
        {
            // manual change
            if (rcmbAnimationState.Tag != null)
                return;

            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {

                // active layer
                TLayer selectedLayer = doc.selectedLayer();

                if (rgryAnimations.SelectedItem != null) {
                    TAnimation animation = (TAnimation)rgryAnimations.SelectedItem.Tag;
                    string newState;
                    if (rcmbAnimationState.SelectedIndex == -1) // if none is selected
                        newState = Program.DEFAULT_STATE_DEFAULT;
                    else
                        newState = rcmbAnimationState.Text;

                    doc.actionManager.RecordAction(new ChangeAnimationStateAction(this, rgryAnimations.SelectedIndex, animation, newState));

                    // set the document modified flag
                    doc.modified = true;

                    this.updateHistoryPanel();
                }
            }
        }

        #endregion 

        #region Scene Tab

        private void rcmbSceneBGM_DropDown(object sender, EventArgs e)
        {
            // clear
            rcmbSceneBGM.Items.Clear();
            rcmbSceneBGM.Items.Add(Program.NONE_ITEM);

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                int soundCount = doc.libraryManager.soundCount();
                for (int i = 0; i < soundCount; i++) {
                    rcmbSceneBGM.Items.Add(doc.libraryManager.soundFileName(i));
                }
            }
        }

        private void txtSceneName_ChangeCommitted(object sender, EventArgs e)
        {
            if (manualScenePropertiesChanged)
                return;

            // Determine the active child form.
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                TScene scene = doc.currentScene();

                bool validated = true;
                if (txtSceneName.Text.Length > 0) {
                    for (int i = 0; i < doc.sceneManager.sceneCount(); i++) {
                        TScene one = doc.sceneManager.scene(i);
                        if (one != scene && one.name == txtSceneName.Text) {
                            validated = false;
                            MessageBox.Show("There is already another scene with same name!", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                } else {
                    validated = false;
                    MessageBox.Show("The scene name cann't be the empty string.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (validated) {
                    // ready for modification of scene
                    ModifySceneAction modifySceneAction = new ModifySceneAction(doc, scene);

                    // change the scene name
                    scene.name = txtSceneName.Text;

                    // record and execute modification
                    modifySceneAction.setFinalData(scene);
                    doc.actionManager.RecordAction(modifySceneAction);

                    // set the document modified flag
                    doc.modified = true;

                    updateOutlinePanel();
                    updateHistoryPanel();
                } else {
                    manualScenePropertiesChanged = true;
                    txtSceneName.Text = scene.name;
                    manualScenePropertiesChanged = false;
                }
            }
        }

        private void SceneProperties_Changed(object sender, EventArgs e)
        {
            if (manualScenePropertiesChanged)
                return;

            // Determine the active child form.
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                // current scene
                TScene scene = doc.currentScene();
                if (scene != null) {
                    // ready for modification of scene
                    ModifySceneAction modifySceneAction = new ModifySceneAction(doc, scene);

                    // change the scene properties
                    scene.backgroundColor = rcpSceneBackColor.Color;
                    scene.touchIndication = rchkSceneTouchIndication.Checked;
                    scene.prevButtonVisible = rchkScenePrevButton.Checked;
                    scene.nextButtonVisible = rchkSceneNextButton.Checked;

                    if (rcmbSceneBGM.Text == Program.NONE_ITEM)
                        rcmbSceneBGM.Text = "";
                    scene.backgroundMusic = rcmbSceneBGM.Text;
                    scene.backgroundMusicVolume = rtrbSceneBGMVolume.Value;

                    // record and execute modification
                    modifySceneAction.setFinalData(scene);
                    doc.actionManager.RecordAction(modifySceneAction);

                    // set the document modified flag
                    doc.modified = true;

                    // redraw text actor
                    activeWorkspace.Refresh();
                    updateScenesPanel(doc.sceneManager.currentSceneIndex);
                    updateHistoryPanel();
                }
            }

        }

        #endregion

        #region Actor Tab

        private void txtActorName_ChangeCommitted(object sender, EventArgs e)
        {
            if (manualActorPropertiesChanged)
                return;

            // Determine the active child form.
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                TActor actor = doc.selectedActor();
                TScene scene = doc.currentScene();

                bool validated = true;
                if (txtActorName.Text.Length > 0) {
                    List<TLayer> allActors = scene.getAllChilds();
                    foreach (TLayer one in allActors) {
                        if (one != actor && one.name == txtActorName.Text) {
                            validated = false;
                            MessageBox.Show("There is already another actor with same name!", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                } else {
                    validated = false;
                    MessageBox.Show("The actor name cann't be the empty string.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (validated) {
                    ModifyActorAction modifyActorAction = new ModifyActorAction(doc, actor);
                    actor.name = txtActorName.Text;
                    modifyActorAction.setFinalData(actor);
                    doc.actionManager.RecordAction(modifyActorAction);

                    // set the document modified flag
                    doc.modified = true;

                    updateOutlinePanel();
                    updateHistoryPanel();
                } else {
                    manualActorPropertiesChanged = true;
                    txtActorName.Text = actor.name;
                    manualActorPropertiesChanged = false;
                }
            }
        }

        private void rchkActorAutoBounding_CheckedChanged(object sender, EventArgs e)
        {
            numActorBoundingX.Enabled = !rchkActorAutoBounding.Checked;
            numActorBoundingY.Enabled = !rchkActorAutoBounding.Checked;
            numActorBoundingWidth.Enabled = !rchkActorAutoBounding.Checked;
            numActorBoundingHeight.Enabled = !rchkActorAutoBounding.Checked;

            ActorProperties_Changed(sender, e);
        }

        private void ActorProperties_Changed(object sender, EventArgs e)
        {
            if (manualActorPropertiesChanged)
                return;

            // Determine the active child form.
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                TActor actor = doc.selectedActor();

                ModifyActorAction modifyActorAction = new ModifyActorAction(doc, actor);

                actor.draggable = rchkActorDraggable.Checked;
                actor.acceleratorSensibility = rchkActorAcceleratorSensibility.Checked;
                actor.anchor = new PointF((float)numActorAnchorX.Value, (float)numActorAnchorY.Value);
                actor.position = new PointF((float)numActorPositionX.Value, (float)numActorPositionY.Value);
                actor.scale = new SizeF((float)numActorScaleX.Value, (float)numActorScaleY.Value);
                actor.rotation = (float)numActorRotation.Value;
                actor.backgroundColor = rcpActorBackColor.Color;
                actor.alpha = (float)numActorAlpha.Value;
                actor.zIndex = (int)numActorZIndex.Value;
                actor.interactionBound = new RectangleF((float)numActorBoundingX.Value, (float)numActorBoundingY.Value, (float)numActorBoundingWidth.Value, (float)numActorBoundingHeight.Value);
                actor.autoInteractionBound = rchkActorAutoBounding.Checked;

                actor.puzzleArea = new RectangleF((float)numBehaviorPuzzleX.Value, (float)numBehaviorPuzzleY.Value, (float)numBehaviorPuzzleWidth.Value, (float)numBehaviorPuzzleHeight.Value);
                actor.puzzle = rchkBehaviorPuzzleActor.Checked;


                modifyActorAction.setFinalData(actor);
                doc.actionManager.RecordAction(modifyActorAction);

                // set the document modified flag
                doc.modified = true;

                // redraw text actor
                activeWorkspace.Refresh();

                updateHistoryPanel();
            }

        }

        #endregion

        #region Behavior Tab

        private void rchkBehaviorPuzzleActor_CheckedChanged(object sender, EventArgs e)
        {
            numBehaviorPuzzleX.Enabled = rchkBehaviorPuzzleActor.Checked;
            numBehaviorPuzzleY.Enabled = rchkBehaviorPuzzleActor.Checked;
            numBehaviorPuzzleWidth.Enabled = rchkBehaviorPuzzleActor.Checked;
            numBehaviorPuzzleHeight.Enabled = rchkBehaviorPuzzleActor.Checked;

            ActorProperties_Changed(sender, e);
        }

        #endregion

        #region TextActor Tab

        private void TextActorProperties_Changed(object sender, EventArgs e)
        {
            if (manualTextActorPropertiesChanged)
                return;

            // Determine the active child form.
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                TActor actor = doc.selectedActor();
                if (actor is TTextActor) {
                    // get selected text actor
                    TTextActor textActor = (TTextActor)actor;

                    ModifyActorAction modifyActorAction = new ModifyActorAction(doc, textActor);

                    // change text of text actor
                    textActor.text = txtTextActorContent.Text;

                    // font size
                    float size;
                    if (!float.TryParse(cmbFontSize.Text, out size))
                        size = textActor.font.Size;

                    // font style
                    FontStyle style = FontStyle.Regular;
                    if (rbtnFontBold.Pressed) style |= FontStyle.Bold;
                    if (rbtnFontItalic.Pressed) style |= FontStyle.Italic;
                    if (rbtnFontUnderline.Pressed) style |= FontStyle.Underline;
                    if (rbtnFontStrikeout.Pressed) style |= FontStyle.Strikeout;

                    // change font of text actor
                    FontFamily family = Program.findFontFamily(cmbFontFace.Text);
                    if (family != null)
                        textActor.font = new Font(family, size, style, GraphicsUnit.Point);
                    else
                        textActor.font = new Font("Loto", size, style, GraphicsUnit.Point);

                    modifyActorAction.setFinalData(textActor);
                    doc.actionManager.RecordAction(modifyActorAction);

                    // color
                    textActor.color = rcpFontColor.Color;

                    // set the document modified flag
                    doc.modified = true;

                    // redraw text actor
                    activeWorkspace.Refresh();
                }
            }

        }

        #endregion

        #endregion

        #region StatusBar

        private void rtrbZoomSlider_Scroll(object sender, EventArgs e)
        {
            int val = rtrbZoomSlider.Value;
            rlblZoomStatus.Text = val.ToString() + "%";

            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                changeDocumentZoom((float)val / 100);
                activeWorkspace.Refresh();
            }
        }

        #endregion

        #region Recent Document List

        private RecentDocumentList recentDocuments;

        private void initializeRecentDocumentList()
        {
            // Create a new collection if it was not serialized before.
            if (Properties.Settings.Default.RecentDocuments == null) {
                Properties.Settings.Default.RecentDocuments = new StringCollection();
            }

            this.recentDocuments = new RecentDocumentList(
                this.c1RibbonBar.ApplicationMenu.RightPaneItems,
                Properties.Settings.Default.RecentDocuments,
                this.loadRecentDocument);
        }

        private void loadRecentDocument(string filename)
        {
            this.openDocument(filename);
        }

        public void updateRecentDocument(string filename)
        {
            this.recentDocuments.Update(filename);
            Properties.Settings.Default.Save();
        }

        private class RecentDocumentList
        {
            public delegate void LoadDocumentDelegate(string filename);

            public RecentDocumentList(
                RibbonItemCollection rightPaneItems,
                StringCollection files,
                LoadDocumentDelegate loadDocument)
            {
                this.rightPaneItems = rightPaneItems;
                this.files = files;
                this.loadDocument = loadDocument;

                // first create a header and make sure it's not selectable
                RibbonListItem listItem = new RibbonListItem(new RibbonLabel("Recent Documents"));
                listItem.AllowSelection = false;
                rightPaneItems.Add(listItem);
                rightPaneItems.Add(new RibbonListItem(new RibbonSeparator()));

                this.listTopIndex = rightPaneItems.Count;

                // create the recently used document list
                foreach (string document in this.files) {
                    RecentDocumentItem item = new RecentDocumentItem(document, false, loadDocument);
                    rightPaneItems.Add(item);
                }
            }

            readonly RibbonItemCollection rightPaneItems;
            readonly StringCollection files;
            readonly LoadDocumentDelegate loadDocument;
            readonly int listTopIndex;

            class RecentDocumentItem : RibbonListItem
            {
                public RecentDocumentItem(string fullFileName, bool pinned, LoadDocumentDelegate loadDocument)
                {
                    this.fullFileName = fullFileName;

                    FileInfo fileInfo = new FileInfo(fullFileName);
                    string documentName = fileInfo.Directory.Name + "\\" + fileInfo.Name;

                    // each item consists of the name of the document and a push pin
                    this.label = new RibbonLabel(documentName);
                    this.pin = new RibbonToggleButton();

                    // set max width of label to avoid the pin icon go out border
                    this.label.MaxTextWidth = 170;

                    // allow the button to be selectable so we can toggle it
                    this.pin.AllowSelection = true;

                    this.pin.Pressed = pinned;
                    this.pin.PressedChanged += delegate { this.SetPinImage(); };
                    this.SetPinImage();

                    this.Items.Add(this.label);
                    this.Items.Add(this.pin);

                    this.ToolTip = fullFileName;

                    this.Click += delegate { loadDocument(this.FullFileName); };
                }

                readonly RibbonLabel label;
                readonly RibbonToggleButton pin;
                readonly string fullFileName;

                void SetPinImage()
                {
                    this.pin.SmallImage = this.pin.Pressed
                        ? Properties.Resources.Pinned
                        : Properties.Resources.Unpinned;
                }

                public string FullFileName
                {
                    get { return this.fullFileName; }
                }

                public bool Pinned
                {
                    get { return this.pin.Pressed; }
                }
            }

            /// <summary>
            /// Adds or moves the file to the top of the list.
            /// </summary>
            /// <param name="filename"> Absolule or relative file path and name. </param>
            public void Update(string filename)
            {
                string fullFileName = new FileInfo(filename).FullName;

                int i = this.IndexOf(fullFileName);
                if (i >= 0) {
                    if (this[i].Pinned) return; // do not move pinned items

                    this.RemoveAt(i);
                }

                this.Insert(0, new RecentDocumentItem(fullFileName, false, this.loadDocument));
            }

            private int Count
            {
                get { return this.rightPaneItems.Count - this.listTopIndex; }
            }

            private RecentDocumentItem this[int i]
            {
                get { return (RecentDocumentItem)this.rightPaneItems[this.listTopIndex + i]; }
            }

            private int IndexOf(string fullFileName)
            {
                for (int i = 0; i < this.Count; ++i) {
                    if (this[i].FullFileName == fullFileName) return i;
                }
                return -1;
            }

            private void RemoveAt(int i)
            {
                this.rightPaneItems.RemoveAt(this.listTopIndex + i);
                this.files.RemoveAt(i);
            }

            private void Insert(int i, RecentDocumentItem item)
            {
                this.rightPaneItems.Insert(this.listTopIndex + i, item);
                this.files.Insert(i, item.FullFileName);
            }
        }

        #endregion

        #region MDIContainer Public Interface

        public TDocument activeDocument()
        {
            // Determine the active child form
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null)
                return activeWorkspace.document;
            return null;
        }

        public void updateAllPanels()
        {
            this.updateToolbar();
            this.updateScenesPanel();
            this.updateOutlinePanel();
            this.updateLibraryPanel();
            this.updateHistoryPanel();
        }

        public void updateToolbar()
        {
            this.updateToolbarDocumentSettings();
            this.updateToolbarSelection();
            this.updateToolbarTools();
            this.updateToolbarSceneSettings();
            this.updateStatusBar();
        }

        public void updateToolbarDocumentSettings()
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                // document tab
                rtxtDocumentIdentifier.Text = doc.identifier;
                rcmbDocumentBGM.Text = doc.backgroundMusic;

                rtrbDocumentBGMVolume.Tag = true;
                rtrbDocumentBGMVolume.Value = doc.backgroundMusicVolume;
                rtrbDocumentBGMVolume.Tag = null;

                // scene navigation buttons
                int prevSceneButtonImage = doc.libraryManager.imageIndex(doc.prevSceneButton);
                if (prevSceneButtonImage == -1)
                    rgryPrevSceneButton.LargeImage = Properties.Resources.toolbar_project_prev;
                else
                    rgryPrevSceneButton.LargeImage = doc.libraryManager.imageForToolbarAtIndex(prevSceneButtonImage);

                int nextSceneButtonImage = doc.libraryManager.imageIndex(doc.nextSceneButton);
                if (nextSceneButtonImage == -1)
                    rgryNextSceneButton.LargeImage = Properties.Resources.toolbar_project_next;
                else
                    rgryNextSceneButton.LargeImage = doc.libraryManager.imageForToolbarAtIndex(nextSceneButtonImage);

                // avatar images
                int avatarDefault = doc.libraryManager.imageIndex(doc.avatarDefault);
                if (avatarDefault == -1)
                    rgryAvatarDefault.LargeImage = Properties.Resources.toolbar_avatar_default;
                else
                    rgryAvatarDefault.LargeImage = doc.libraryManager.imageForToolbarAtIndex(avatarDefault);

                int avatarFrame = doc.libraryManager.imageIndex(doc.avatarFrame);
                if (avatarFrame == -1)
                    rgryAvatarFrame.LargeImage = Properties.Resources.toolbar_avatar_frame;
                else
                    rgryAvatarFrame.LargeImage = doc.libraryManager.imageForToolbarAtIndex(avatarFrame);

                int avatarMask = doc.libraryManager.imageIndex(doc.avatarMask);
                if (avatarMask == -1)
                    rgryAvatarMask.LargeImage = Properties.Resources.toolbar_avatar_mask;
                else
                    rgryAvatarMask.LargeImage = doc.libraryManager.imageForToolbarAtIndex(avatarMask);
            }
        }

        public void updateToolbarSceneSettings()
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                // properties
                updateToolbarAnimations();
                updateToolbarSceneProperties();
                updateToolbarActorProperties();
                updateToolbarTextActorProperties();
            }
        }

        public void updateToolbarAnimationProperties()
        {
            if (rgryAnimations.SelectedItem != null) {
                TAnimation animation = (TAnimation)rgryAnimations.SelectedItem.Tag;
                rcmbAnimationEvent.Text = animation.eventu;
                rcmbAnimationState.Text = animation.state;
            }

            rbtnEditAnimation.Enabled = rgryAnimations.SelectedIndex != -1;
            rbtnDeleteAnimation.Enabled = rgryAnimations.SelectedIndex != -1;
            rcmbAnimationEvent.Enabled = rgryAnimations.SelectedIndex != -1;
            rcmbAnimationState.Enabled = rgryAnimations.SelectedIndex != -1;
        }

        public void updateScenesPanel()
        {
            this.lstScenes.BeginUpdate();

            // clear scenes panel
            this.lstScenes.Items.Clear();

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                int sceneCount = doc.sceneManager.sceneCount();
                if (sceneCount > 0) {
                    for (int i = 0; i < sceneCount; i++) {
                        this.lstScenes.Items.Add("");
                    }

                    this.lstScenes.Tag = true;
                    this.lstScenes.SelectedIndex = doc.sceneManager.currentSceneIndex;
                    this.lstScenes.Tag = null;
                }
            }

            this.lstScenes.EndUpdate();

            this.updateSceneButtons();
        }

        public void updateScenesPanel(int index)
        {
            this.lstScenes.Invalidate(this.lstScenes.GetItemRectangle(index));
        }

        public void updateOutlinePanel()
        {
            this.tvwSceneOutline.BeginUpdate();

            // remove all nodes from treeview control
            this.tvwSceneOutline.Nodes.Clear();

            TDocument doc = this.activeDocument();
            if (doc != null) {
                // current scene
                TScene scene = doc.currentScene();

                if (scene != null) {
                    // add scene to treeview control
//                    TreeNode sceneNode = new TreeNode(scene.name, 0, 1);
                    TreeNode sceneNode = new TreeNode(scene.name);
                    sceneNode.Tag = scene;
                    this.tvwSceneOutline.Nodes.Add(sceneNode);

                    // add layers to treeview control
                    this.addChildsToOutline(sceneNode);

                    // expand all children
                    this.tvwSceneOutline.ExpandAll();

                    // highlight selection
                    this.highlightOutlineSelection();
                }
            }

            this.tvwSceneOutline.EndUpdate();

            this.updateOutlineButtons();
        }

        public void updateLibraryPanel()
        {
            // clear library panel
            this.lvwImageList.Items.Clear();
            this.pnlSoundList.Controls.Clear();

            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                this.lvwImageList.LargeImageList = doc.libraryManager.largeImageListThumbnails();
                this.lvwImageList.SmallImageList = doc.libraryManager.smallImageListThumbnails();

                int imageCount = doc.libraryManager.imageCount();
                for (int i = 0; i < imageCount; i++) {
                    this.lvwImageList.Items.Add(doc.libraryManager.imageFileName(i), i);
                }

                int soundCount = doc.libraryManager.soundCount();
                for (int i = 0; i < soundCount; i++) {
                    SoundListItem item = new SoundListItem(doc.libraryManager.soundFilePath(i));
                    pnlSoundList.Controls.Add(item);
                }
            }

            this.updateLibraryButtons();
        }

        public void updateHistoryPanel()
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                rbtnUndo.Enabled = doc.actionManager.CanUndo;
                rbtnRedo.Enabled = doc.actionManager.CanRedo;
            } else {
                rbtnUndo.Enabled = false;
                rbtnRedo.Enabled = false;
            }
        }

        public void highlightOutlineSelection()
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {

                // active layer
                TLayer layer = doc.selectedLayer();

                // update selected node of outline treeview
                TreeNode node = this.findOutlineNodeByTag(layer, this.tvwSceneOutline);
                if (node != null && this.tvwSceneOutline.SelectedNode != node) {
                    this.tvwSceneOutline.Tag = true;
                    this.tvwSceneOutline.SelectedNode = this.findOutlineNodeByTag(layer, this.tvwSceneOutline);
                    this.tvwSceneOutline.Tag = null;
                }
            }
        }

        public void focusTextActorContent()
        {
            rtabTextActor.Selected = true;
            Application.DoEvents();
            txtTextActorContent.Focus();
        }

        // event handler is called when selected item is changed
        public void selectedItemChanged()
        {
            this.updateToolbarSelection();
            this.updateToolbarSceneSettings();
            this.highlightOutlineSelection();
            this.updateOutlineButtons();
        }

        #endregion

        #region MDIContainer Private Interface

        private void updateToolbarSelection()
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {
                rbtnDelete.Enabled = doc.haveSelection();
                rbtnCut.Enabled = rbtnCopy.Enabled = doc.haveSelection();
                rbtnPaste.Enabled = Clipboard.GetDataObject().GetDataPresent(CLIPBOARD_ACTOR_FORMAT);
            }
        }

        private void updateToolbarTools()
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {
                // tools
                rbtnSelectTool.Pressed = (doc.currentTool == TDocument.TOOL_SELECT);
                rbtnHandTool.Pressed = (doc.currentTool == TDocument.TOOL_HAND);
                rbtnTextTool.Pressed = (doc.currentTool == TDocument.TOOL_TEXT);
                rbtnBoundingTool.Pressed = (doc.currentTool == TDocument.TOOL_BOUNDING);
                rbtnBoundingTool2.Pressed = (doc.currentTool == TDocument.TOOL_BOUNDING);
                rbtnAvatarTool.Pressed = (doc.currentTool == TDocument.TOOL_AVATAR);
                rbtnPuzzleTool.Pressed = (doc.currentTool == TDocument.TOOL_PUZZLE);
                rbtnPuzzleTool2.Pressed = (doc.currentTool == TDocument.TOOL_PUZZLE);
            }
        }

        private void updateToolbarAnimationPart(TLayer layer)
        {
            // animation events
            rcmbAnimationEvent.Items.Clear();
            rcmbAnimationEvent.Items.Add(Program.DEFAULT_EVENT_UNDEFINED);
            foreach (string eventu in layer.getEvents())
                rcmbAnimationEvent.Items.Add(eventu);
            rcmbAnimationEvent.Tag = true; // for manual change
            rcmbAnimationEvent.SelectedIndex = 0;
            rcmbAnimationEvent.Tag = null;

            // animation states
            rcmbAnimationState.Items.Clear();
            foreach (string state in layer.getStates())
                rcmbAnimationState.Items.Add(state);
            rcmbAnimationState.Tag = true; // for manual change
            rcmbAnimationState.SelectedIndex = 0;
            rcmbAnimationState.Tag = null; // for manual change

            // animation list
            rgryAnimations.Items.Clear();
            foreach (TAnimation animation in layer.animations) {
                // add item into gallery list
                RibbonGalleryItem item = new RibbonGalleryItem("", imageOfAnimation(animation));
                item.Tag = animation;
                rgryAnimations.Items.Add(item);
            }

            rbtnEditAnimation.Enabled = false;
            rbtnDeleteAnimation.Enabled = false;
            rcmbAnimationEvent.Enabled = false;
            rcmbAnimationState.Enabled = false;
        }

        private void updateToolbarAnimations()
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null) {
                // show animations tab
                rtabAnimations.Visible = true;

                // active layer
                TLayer layer = doc.selectedLayer();

                // events list
                rbtnRenameEvent.Enabled = false;
                rbtnDeleteEvent.Enabled = false;
                rgryEvents.Items.Clear();
                foreach (string eventu in layer.getEvents()) {
                    rgryEvents.Items.Add(eventu);
                }

                // states list
                rbtnRenameState.Enabled = false;
                rbtnDeleteState.Enabled = false;
                rgryStates.Items.Clear();
                foreach (string state in layer.getStates()) 
                    rgryStates.Items.Add(state);

                updateToolbarAnimationPart(layer);
            } else {
                // hide animations tab
                rtabAnimations.Visible = false;
            }
        }

        private bool manualScenePropertiesChanged = false;

        private void updateToolbarSceneProperties()
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null && !doc.haveSelection()) {
                rctgSceneProperties.Visible = true;

                TScene scene = doc.currentScene();

                // set manual flag
                manualScenePropertiesChanged = true;

                txtSceneName.Text = scene.name;
                rcpSceneBackColor.DefaultColor = scene.backgroundColor;
                rcpSceneBackColor.Color = scene.backgroundColor;
                rchkSceneTouchIndication.Checked = scene.touchIndication;
                rchkScenePrevButton.Checked = scene.prevButtonVisible;
                rchkSceneNextButton.Checked = scene.nextButtonVisible;
                rcmbSceneBGM.Text = scene.backgroundMusic;
                rtrbSceneBGMVolume.Value = scene.backgroundMusicVolume;

                // clear manual flag
                manualScenePropertiesChanged = false;

            } else {
                rctgSceneProperties.Visible = false;
            }
        }

        private bool manualActorPropertiesChanged = false;

        private void updateToolbarActorProperties()
        {
            // Determine the active document
            TDocument doc = this.activeDocument();
            if (doc != null && doc.currentScene() != null && doc.haveSelection()) {
                // show actor properties tab
                rctgActorProperties.Visible = true;

                // actor instance
                TActor actor = (TActor)doc.selectedActor();

                // set flag to avoid event handler when properties are changed by code
                manualActorPropertiesChanged = true;

                txtActorName.Text = actor.name;
                rcpActorBackColor.DefaultColor = actor.backgroundColor;
                rcpActorBackColor.Color = actor.backgroundColor;
                rchkActorDraggable.Checked = actor.draggable;
                rchkActorAcceleratorSensibility.Checked = actor.acceleratorSensibility;
                numActorAnchorX.Value = (decimal)actor.anchor.X;
                numActorAnchorY.Value = (decimal)actor.anchor.Y;
                numActorPositionX.Value = (decimal)actor.position.X;
                numActorPositionY.Value = (decimal)actor.position.Y;
                numActorScaleX.Value = (decimal)actor.scale.Width;
                numActorScaleY.Value = (decimal)actor.scale.Height;
                numActorRotation.Value = (decimal)actor.rotation;
                numActorAlpha.Value = (decimal)actor.alpha;
                numActorZIndex.Value = (decimal)actor.zIndex;
                rchkActorAutoBounding.Checked = actor.autoInteractionBound;
                RectangleF bounding = actor.interactionBound;
                numActorBoundingX.Value = (decimal)bounding.X;
                numActorBoundingY.Value = (decimal)bounding.Y;
                numActorBoundingWidth.Value = (decimal)bounding.Width;
                numActorBoundingHeight.Value = (decimal)bounding.Height;

                // puzzle properties
                rchkBehaviorPuzzleActor.Checked = actor.puzzle;
                RectangleF puzzleArea = actor.puzzleArea;
                numBehaviorPuzzleX.Value = (decimal)puzzleArea.X;
                numBehaviorPuzzleY.Value = (decimal)puzzleArea.Y;
                numBehaviorPuzzleWidth.Value = (decimal)puzzleArea.Width;
                numBehaviorPuzzleHeight.Value = (decimal)puzzleArea.Height;

                // clear flag
                manualActorPropertiesChanged = false;
            } else {
                // hide actor properties tab
                rctgActorProperties.Visible = false;
            }
        }

        private bool manualTextActorPropertiesChanged = false;

        private void updateToolbarTextActorProperties()
        {
            TDocument doc = this.activeDocument();

            if (doc.selectedActor() is TTextActor) {
                // show textactor tabs
                rctgTextActorProperties.Visible = true;

                // text actor instance
                TTextActor actor = (TTextActor)doc.selectedActor();

                // set flag to avoid event handler when properties are changed by code 
                manualTextActorPropertiesChanged = true;

                Regex regex = new Regex("(?<!\r)\n");
                txtTextActorContent.Text = regex.Replace(actor.text, "\r\n");
                cmbFontFace.Text = actor.font.FontFamily.Name;
                cmbFontSize.Text = actor.font.Size.ToString();
                rbtnFontBold.Pressed = actor.font.Bold;
                rbtnFontItalic.Pressed = actor.font.Italic;
                rbtnFontUnderline.Pressed = actor.font.Underline;
                rbtnFontStrikeout.Pressed = actor.font.Strikeout;
                rcpFontColor.Color = actor.color;
                rcpFontColor.DefaultColor = actor.color;

                // clear flag
                manualTextActorPropertiesChanged = false;
            } else {
                // hide textactor tabs
                rctgTextActorProperties.Visible = false;
            }
        }

        private void updateStatusBar()
        {
            // Determine the active document.
            TDocument doc = this.activeDocument();
            if (doc != null) {
                if (doc.zoom >= 0.25 && doc.zoom <= 4)
                    rtrbZoomSlider.Value = (int)(doc.zoom * 100);
                else if (doc.zoom < 0.25)
                    rtrbZoomSlider.Value = 25;
                else if (doc.zoom > 4)
                    rtrbZoomSlider.Value = 400;
                rlblZoomStatus.Text = (int)(doc.zoom * 100) + "%";
            }
        }

        private void updateLibraryButtons()
        {
            TDocument doc = this.activeDocument();
            if (doc != null) {
                btnAddImage.Enabled = true;
                btnRemoveImage.Enabled = lvwImageList.SelectedItems.Count > 0;
                btnAddSound.Enabled = true;
                btnRemoveSound.Enabled = pnlSoundList.SelectedItem() != null;
            } else {
                btnAddImage.Enabled = false;
                btnRemoveImage.Enabled = false;
                btnAddSound.Enabled = false;
                btnRemoveSound.Enabled = false;
            }
        }

        private void updateOutlineButtons()
        {
            TDocument doc = this.activeDocument();
            if (doc != null) {
                TreeNode node = tvwSceneOutline.SelectedNode;
                btnChangeImageActor.Enabled = (node != null) && (node.Tag is TImageActor);
                btnCloneActor.Enabled = btnRemoveActor.Enabled = (node != null) && (node.Tag is TActor);

                if (node != null && node.Tag is TActor) {
                    chkLockedLayer.Visible = true;
                    chkLockedLayer.Checked = ((TLayer)node.Tag).locked;
                } else {
                    chkLockedLayer.Visible = false;
                }
            } else {
                btnChangeImageActor.Enabled = false;
                btnCloneActor.Enabled = false;
                btnRemoveActor.Enabled = false;
            }
        }

        private void updateSceneButtons()
        {
            TDocument doc = this.activeDocument();
            if (doc != null) {
                rbtnNewScene.Enabled = true;
                rbtnCloneScene.Enabled = rbtnDeleteScene.Enabled = lstScenes.SelectedIndex != -1;
            } else {
                rbtnNewScene.Enabled = false;
                rbtnCloneScene.Enabled = false;
                rbtnDeleteScene.Enabled = false;
            }
        }

        private void changeDocumentZoom(float zoom)
        {
            FrmWorkspace activeWorkspace = (FrmWorkspace)this.ActiveMdiChild;
            if (activeWorkspace != null) {
                TDocument doc = activeWorkspace.document;
                doc.zoom = zoom;
            }
        }

        // create and return the item image for animation gallery of ribbon bar
        private Image imageOfAnimation(TAnimation animation)
        {
            // margin is 8px
            Bitmap bm = new Bitmap(rgryAnimations.ItemSize.Width - 8, rgryAnimations.ItemSize.Height - 8);
            Graphics g = Graphics.FromImage(bm);

            g.DrawImage(Properties.Resources.toolbar_animations_animation_item, new Rectangle(0, 0, bm.Width, bm.Height));

            Font font = c1RibbonBar.Font;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            g.DrawString(animation.eventu, font, Brushes.White, new Rectangle(0, 0, bm.Width, bm.Height / 2), sf);
            g.DrawString(animation.state, font, Brushes.Black, new Rectangle(0, bm.Height / 2, bm.Width, bm.Height / 2), sf);

            g.Dispose();

            return bm;
        }

        #endregion
    }
}
