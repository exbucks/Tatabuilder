using GuiLabs.Undo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder
{
    public partial class FrmWorkspace : Form
    {
        public TDocument document { get; set; }

        bool MousePressed = false;
        PointF MouseDownPos = new PointF();
        int MouseDownTool = TDocument.TOOL_NONE;
        int MouseDownPart = -1;
        RectangleF SelectRegion = RectangleF.Empty;

        ModifyActorAction modifyActorAction;

        public FrmWorkspace()
        {
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(pnlWorkspace_MouseWheel);
        }

        private void FrmWorkspace_Load(object sender, EventArgs e)
        {
            this.Text = this.document.filename;
        }

        private void FrmWorkspace_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.promtToSaveDocument()) {
                e.Cancel = true;
            }
        }

        private bool promtToSaveDocument()
        {
            if (!this.document.modified) return true;

            DialogResult dr = MessageBox.Show("Do you want to save '" + this.document.filename + "'?", Program.APP_NAME, MessageBoxButtons.YesNoCancel);

            switch (dr) {
                case DialogResult.Cancel:
                    return false;

                case DialogResult.No:
                    return true;

                case DialogResult.Yes:
                    return saveDocument(false);
            }

            throw new ApplicationException();
        }

        public bool saveDocument(bool showDialog)
        {
            bool ret;
            if (showDialog || this.document.directory == null) {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = this.document.filename;
                if (this.document.directory != null) dlg.InitialDirectory = this.document.directory;

                dlg.Filter = String.Format("Tata Builder Project File (*.{0})|*.{0}", Program.DOC_EXTENSION);

                DialogResult dr = dlg.ShowDialog();
                if (dr == DialogResult.Cancel) return false;
                if (dr != DialogResult.OK) throw new ApplicationException();

                ret = this.document.save(dlg.FileName);
            } else {
                ret = this.document.save();
            }

            if (ret) {
                this.document.modified = false;

                FrmMainContainer mainForm = (FrmMainContainer)this.MdiParent;
                mainForm.updateRecentDocument(this.document.filepath);
            }

            return ret;
        }

        public void showRuler(bool show)
        {
            if (show) {
                pnlHorizontalRuler.Visible = true;
                pnlVerticalRuler.Visible = true;
            } else {
                pnlHorizontalRuler.Visible = false;
                pnlVerticalRuler.Visible = false;
            }
        }

        public void updateRuler()
        {
            PointF offset = this.document.offset;
            float zoom = this.document.zoom;
            float width = this.pnlWorkspace.Width, height = this.pnlWorkspace.Height;

            Matrix m = new Matrix();
            m.Translate(width / 2 + offset.X, height / 2 + offset.Y);
            m.Scale(zoom, zoom);
            m.Translate(-0.5F * Program.BOOK_WIDTH, -0.5F * Program.BOOK_HEIGHT);
            m.Invert();

            PointF[] aPos = { new PointF(0, 0) };
            m.TransformPoints(aPos);

            rulerHorizontal.StartOffset = (int)(-aPos[0].X * zoom);
            rulerVertical.StartOffset = (int)(-aPos[0].Y * zoom);
            rulerHorizontal.ZoomFactor = rulerVertical.ZoomFactor = zoom;
            if (zoom >= 1)
                rulerHorizontal.MajorInterval = rulerVertical.MajorInterval = 100;
            else if (zoom >= 0.5)
                rulerHorizontal.MajorInterval = rulerVertical.MajorInterval = 200;
            else
                rulerHorizontal.MajorInterval = rulerVertical.MajorInterval = 400;
        }

        public void updateCursor()
        {
            if (MousePressed == false) {
                switch (this.document.activeTool()) {
                    case TDocument.TOOL_HAND:
                        this.Cursor = Cursors.Hand;
                        break;
                    case TDocument.TOOL_TEXT:
                        this.Cursor = Cursors.Cross;
                        break;
                    default:
                        this.Cursor = Cursors.Default;
                        break;
                }
            }
        }

        public Size getWorkArea()
        {
            return this.pnlWorkspace.Size;
        }

        private void pnlWorkspace_Paint(object sender, PaintEventArgs e)
        {
            updateRuler();

            // draw document
            this.document.drawWorkspace(e.Graphics, this.pnlWorkspace.Width, this.pnlWorkspace.Height);

            // draw selection region
            if (SelectRegion != RectangleF.Empty) {
                Pen pen = new Pen(Color.Black, 1);
                pen.DashStyle = DashStyle.Custom;
                pen.DashPattern = new float[] { 4, 4 };
                e.Graphics.DrawRectangle(pen, Math.Min(SelectRegion.Left, SelectRegion.Right), Math.Min(SelectRegion.Top, SelectRegion.Bottom), Math.Abs(SelectRegion.Width), Math.Abs(SelectRegion.Height));
            }
        }

        private void pnlWorkspace_DragEnter(object sender, DragEventArgs e)
        {
            if (this.document.currentScene() == null) 
                return;

            if (e.Data.GetDataPresent(DataFormats.Serializable, true) == true) {
                e.Effect = DragDropEffects.All;
            }
        }

        private void pnlWorkspace_DragDrop(object sender, DragEventArgs e)
        {
            if (this.document.currentScene() == null)
                return;

            if (e.Data.GetDataPresent(DataFormats.Serializable, true) == true) {
                // dragging item
                ListViewItem item = (ListViewItem)e.Data.GetData(DataFormats.Serializable);

                // convert screen coordinate to client coordinate
                Point pt = this.pnlWorkspace.PointToClient(new Point(e.X, e.Y));

                // item to current scene
                TImageActor actor = this.document.currentScene().pushImage(this.document.libraryManager.imageFileName(item.Index), pt.X, pt.Y);
                this.document.actionManager.RecordAction(new AddActorAction(this.document, actor));

                // set the document modified flag
                this.document.modified = true;

                // redraw scene
                this.pnlWorkspace.Refresh();

                // update panels
                FrmMainContainer mainForm = (FrmMainContainer)this.MdiParent;
                mainForm.updateScenesPanel(this.document.sceneManager.currentSceneIndex);
                mainForm.updateOutlinePanel();
                mainForm.updateHistoryPanel();
            }
        }

        private void pnlWorkspace_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.document.currentScene() == null)
                return;

            FrmMainContainer mainForm = (FrmMainContainer)this.MdiParent;

            if (e.Button == MouseButtons.Left) {
                // set flag that mouse is pressed and store position
                MousePressed = true;
                MouseDownPos = new PointF(e.X, e.Y);
                MouseDownTool = this.document.activeTool();

                if (MouseDownTool == TDocument.TOOL_SELECT || MouseDownTool == TDocument.TOOL_BOUNDING || MouseDownTool == TDocument.TOOL_PUZZLE) {

                    // if no current selection, create new selection
                    if (!this.document.haveSelection()) {

                        // target item
                        TActor target = this.document.actorAtPosition(e.X, e.Y, (MouseDownTool == TDocument.TOOL_BOUNDING));
                        if (target != null && (MouseDownTool != TDocument.TOOL_PUZZLE || target.puzzle)) {
                            // add target item to selection stack
                            this.document.toggleSelectedItem(target);

                            if (target.locked) {
                                MousePressed = false;
                                MouseDownTool = TDocument.TOOL_NONE;
                                MouseDownPart = -1;
                                SelectRegion = RectangleF.Empty;
                            } else {
                                // record modify action
                                modifyActorAction = new ModifyActorAction(document, target);

                                // calc which part of selection is mouse position
                                MouseDownPart = 0;
                                target.createBackup(); // when moving actor, need the original state before moving
                                this.Cursor = Cursors.SizeAll;
                            }
                        } else {
//                            this.SelectRegion = new RectangleF(e.X, e.Y, 0, 0);
                        }

                    } else {
                        // calc which part of selection is mouse position
                        int cursor;
                        MouseDownPart = this.document.partOfSelection(e.X, e.Y, out cursor);

                        if (MouseDownPart == -1) { // if click at outside of selection bound, deselect current selection.

                            // clear selection stack
                            this.document.clearSelectedItems();

                            // clear modify action
                            modifyActorAction = null;

                            // target item
                            TActor target = this.document.actorAtPosition(e.X, e.Y, MouseDownTool == TDocument.TOOL_BOUNDING);
                            if (target != null) {
                                // add target item to selection stack
                                this.document.toggleSelectedItem(target);

                                if (target.locked) {
                                    MousePressed = false;
                                    MouseDownTool = TDocument.TOOL_NONE;
                                    MouseDownPart = -1;
                                    SelectRegion = RectangleF.Empty;
                                } else {
                                    // record modify action
                                    modifyActorAction = new ModifyActorAction(document, target);

                                    MouseDownPart = 0;
                                    this.Cursor = Cursors.SizeAll;
                                }
                            } else {
                                //                                this.SelectRegion = new RectangleF(e.X, e.Y, 0, 0);
                            }
                        } else {
                            if (this.document.selectedActor().locked) {
                                MousePressed = false;
                                MouseDownTool = TDocument.TOOL_NONE;
                                MouseDownPart = -1;
                                SelectRegion = RectangleF.Empty;
                            } else {
                                if (MouseDownPart == 0) {
                                    // record modify action
                                    modifyActorAction = new ModifyActorAction(document, this.document.selectedActor());
                                }
                            }
                        }
                        
                        if (MouseDownPart != -1) {
                            foreach (TActor actor in this.document.selectedItems) {
                                actor.createBackup();
                            }
                        }
                    }

                    // redraw workspace
                    this.pnlWorkspace.Refresh();

                    // fire mainform's selected item changed event
                    mainForm.selectedItemChanged();

                } else if (MouseDownTool == TDocument.TOOL_TEXT || MouseDownTool == TDocument.TOOL_AVATAR) {

                    // if no current selection, create new selection
                    if (!this.document.haveSelection()) {
                        // start selection
                        this.SelectRegion = new RectangleF(e.X, e.Y, 0, 0);
                    } else {
                        // calc which part of selection is mouse position
                        int cursor;
                        MouseDownPart = this.document.partOfSelection(e.X, e.Y, out cursor);

                        if (MouseDownPart == -1 || MouseDownPart == 9 || MouseDownTool == TDocument.TOOL_AVATAR) { // if click at outside of selection bound, deselect current selection.
                            // start selection
                            this.SelectRegion = new RectangleF(e.X, e.Y, 0, 0);
                            MouseDownPart = -1;
                        }
                    }

                    // redraw workspace
                    this.pnlWorkspace.Refresh();
                }
            }

            // for moving by space key, set focus the control in the workspace form
            lblFocusTarget.Focus();
        }

        private void pnlWorkspace_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.document.currentScene() == null)
                return;

            if (e.Button == MouseButtons.Left && MousePressed) {

                if (MouseDownTool == TDocument.TOOL_SELECT) {
                    // if have selection, move it
                    if (this.document.haveSelection()) {
                        if (MouseDownPart == 0) {
                            // move selection items
                            this.document.moveSelectedItems(e.X - MouseDownPos.X, e.Y - MouseDownPos.Y, (Control.ModifierKeys & Keys.Shift) != 0);
                        } else if (MouseDownPart >= 1 && MouseDownPart <= 8) {
                            // scale selection items
                            this.document.scaleSelectedItems(MouseDownPart, e.X - MouseDownPos.X, e.Y - MouseDownPos.Y, (Control.ModifierKeys & Keys.Shift) != 0);
                        } else if (MouseDownPart == 10) {
                            // move anchor point
                            this.document.moveAnchorOfSelectedItem(e.X - MouseDownPos.X, e.Y - MouseDownPos.Y, (Control.ModifierKeys & Keys.Shift) != 0);
                        } else if (MouseDownPart == 9) {
                            // rotate
                            TActor item = this.document.selectedItems[0];
                            PointF p = item.parent.logicalToScreen(item.position);
                            float a = TUtil.angleBetweenVectors(new PointF(MouseDownPos.X - p.X, MouseDownPos.Y - p.Y), new PointF(e.X - p.X, e.Y - p.Y));
                            this.document.rotateSelectedItems((float)(a * 180 / Math.PI), (Control.ModifierKeys & Keys.Shift) != 0);
                        }
                    }

                } else if (MouseDownTool == TDocument.TOOL_HAND) {
                    PointF offset = this.document.offset;
                    offset.X += e.X - MouseDownPos.X;
                    offset.Y += e.Y - MouseDownPos.Y;

                    this.document.offset = offset;

                    // update mouse position
                    MouseDownPos.X = e.X; MouseDownPos.Y = e.Y;

                } else if (MouseDownTool == TDocument.TOOL_TEXT || MouseDownTool == TDocument.TOOL_AVATAR) {
                    // if have selection, move it
                    if (!this.document.haveSelection() || MouseDownPart == -1 || MouseDownPart == 9) {
                        this.SelectRegion.Width = e.X - this.SelectRegion.X;
                        this.SelectRegion.Height = e.Y - this.SelectRegion.Y;
                    } else if (MouseDownPart >= 1 && MouseDownPart <= 8 && MouseDownTool == TDocument.TOOL_TEXT) {

                        // resize text actor
                        if (this.document.resizeSelectedTextActor(MouseDownPart, e.X - MouseDownPos.X, e.Y - MouseDownPos.Y)) {
                            // update mouse position
                            MouseDownPos.X = e.X; MouseDownPos.Y = e.Y;
                        }
                    }
                } else if (MouseDownTool == TDocument.TOOL_BOUNDING) {
                    if (this.document.haveSelection()) {
                        if (MouseDownPart == 0) {
                            // move the interaction bound
                            this.document.moveInteractionBound(e.X - MouseDownPos.X, e.Y - MouseDownPos.Y, (Control.ModifierKeys & Keys.Shift) != 0);
                        } else if (MouseDownPart >= 1 && MouseDownPart <= 8) {
                            // scale the interaction bound
                            this.document.scaleInteractionBound(MouseDownPart, e.X - MouseDownPos.X, e.Y - MouseDownPos.Y, (Control.ModifierKeys & Keys.Shift) != 0);
                        }
                    }
                } else if (MouseDownTool == TDocument.TOOL_PUZZLE) {
                    if (this.document.haveSelection()) {
                        if (MouseDownPart == 0) {
                            // move the puzzle area
                            this.document.movePuzzleArea(e.X - MouseDownPos.X, e.Y - MouseDownPos.Y, (Control.ModifierKeys & Keys.Shift) != 0);
                        } else if (MouseDownPart >= 1 && MouseDownPart <= 8) {
                            // scale the puzzle area
                            this.document.scalePuzzleArea(MouseDownPart, e.X - MouseDownPos.X, e.Y - MouseDownPos.Y, (Control.ModifierKeys & Keys.Shift) != 0);
                        }
                    }
                }

                // redraw workspace
                this.pnlWorkspace.Refresh();

            } else {
                if (this.document.activeTool() == TDocument.TOOL_SELECT) {
                    if (this.document.haveSelection()) {

                        // calc which part of selection is mouse position
                        int cursor;
                        int part = this.document.partOfSelection(e.X, e.Y, out cursor);
                        if (part == -1)
                            this.Cursor = Cursors.Default;
                        else if (part == 0)
                            this.Cursor = Cursors.SizeAll;
                        else if (part == 9)
                            this.Cursor = new Cursor(Properties.Resources.cursor_rotation.Handle);
                        else if (part == 10)
                            this.Cursor = Cursors.NoMove2D;
                        else if (cursor == 0)
                            this.Cursor = Cursors.SizeNS;
                        else if (cursor == 1)
                            this.Cursor = Cursors.SizeNWSE;
                        else if (cursor == 2)
                            this.Cursor = Cursors.SizeWE;
                        else if (cursor == 3)
                            this.Cursor = Cursors.SizeNESW;

                    }
                } else if (this.document.activeTool() == TDocument.TOOL_TEXT) {
                    if (this.document.haveSelection() && (this.document.selectedItems[0] is TTextActor)) {

                        // calc which part of selection is mouse position
                        int cursor;
                        int part = this.document.partOfSelection(e.X, e.Y, out cursor);
                        if (part == -1 || part == 9)
                            this.Cursor = Cursors.Cross;
                        else if (part == 0)
                            this.Cursor = Cursors.IBeam;
                        else if (cursor == 0)
                            this.Cursor = Cursors.SizeNS;
                        else if (cursor == 1)
                            this.Cursor = Cursors.SizeNWSE;
                        else if (cursor == 2)
                            this.Cursor = Cursors.SizeWE;
                        else if (cursor == 3)
                            this.Cursor = Cursors.SizeNESW;
                    }
                } else if (this.document.activeTool() == TDocument.TOOL_BOUNDING || this.document.activeTool() == TDocument.TOOL_PUZZLE) {
                    if (this.document.haveSelection()) {

                        // calc which part of selection is mouse position
                        int cursor;
                        int part = this.document.partOfSelection(e.X, e.Y, out cursor);
                        if (part == -1)
                            this.Cursor = Cursors.Default;
                        else if (part == 0)
                            this.Cursor = Cursors.SizeAll;
                        else if (cursor == 0)
                            this.Cursor = Cursors.SizeNS;
                        else if (cursor == 1)
                            this.Cursor = Cursors.SizeNWSE;
                        else if (cursor == 2)
                            this.Cursor = Cursors.SizeWE;
                        else if (cursor == 3)
                            this.Cursor = Cursors.SizeNESW;

                    }
                } else if (this.document.activeTool() == TDocument.TOOL_AVATAR) {
                    this.Cursor = Cursors.Cross;
                }
            }
        }

        private void pnlWorkspace_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.document.currentScene() == null)
                return;

            // main form
            FrmMainContainer mainForm = (FrmMainContainer)this.MdiParent;

            if (e.Button == MouseButtons.Left && MousePressed) {
                if (MouseDownTool == TDocument.TOOL_TEXT) {

                    if (this.document.haveSelection() && MouseDownPart == 0) {
                        mainForm.focusTextActorContent();
                    } else if (!this.document.haveSelection() || MouseDownPart == -1) {
                        // positivation
                        RectangleF bound = TUtil.positiveRectangle(SelectRegion);
                        if (bound.Width > 1 && bound.Height > 1) {

                            // item to current scene
                            TTextActor actor = this.document.currentScene().pushText("", bound);
                            this.document.actionManager.RecordAction(new AddActorAction(this.document, actor));

                            // set the document modified flag
                            this.document.modified = true;

                            // select new added text actor
                            this.document.clearSelectedItems();
                            this.document.toggleSelectedItem(actor);

                            // update panels
                            mainForm.updateScenesPanel(this.document.sceneManager.currentSceneIndex);
                            mainForm.updateOutlinePanel();
                            mainForm.updateHistoryPanel();

                            // fire mainform's selected item changed event
                            mainForm.selectedItemChanged();

                            // show textbox of ribbon bar and make let user to edit content
                            mainForm.focusTextActorContent();

                            // record modify action
                            modifyActorAction = new ModifyActorAction(document, actor);
                        }
                    } else if (this.document.haveSelection() && modifyActorAction != null) {
                        // this is the case when text box was resized with text tool
                        modifyActorAction.setFinalData(this.document.selectedActor());
                        if (modifyActorAction.isModified()) {
                            this.document.actionManager.RecordAction(modifyActorAction);

                            // set the document modified flag
                            this.document.modified = true;

                            // ready to new modification action
                            modifyActorAction = new ModifyActorAction(document, document.selectedActor());

                            // update history
                            mainForm.updateHistoryPanel();
                        }

                        // update panels
                        this.document.sceneManager.updateThumbnail(this.document.sceneManager.currentSceneIndex);
                        mainForm.updateScenesPanel(this.document.sceneManager.currentSceneIndex);
                        mainForm.updateToolbarSceneSettings();
                        mainForm.updateOutlinePanel();
                    }
                } else if (MouseDownTool == TDocument.TOOL_AVATAR) {
                    if (!this.document.haveSelection() || MouseDownPart == -1) {
                        // positivation
                        RectangleF bound = TUtil.positiveRectangle(SelectRegion);
                        if (bound.Width > 1 && bound.Height > 1) {

                            // item to current scene
                            TAvatarActor actor = this.document.currentScene().pushAvatar(bound);
                            this.document.actionManager.RecordAction(new AddActorAction(this.document, actor));

                            // set the document modified flag
                            this.document.modified = true;

                            // select new added avatar actor
                            this.document.clearSelectedItems();
                            this.document.toggleSelectedItem(actor);

                            // update panels
                            mainForm.updateScenesPanel(this.document.sceneManager.currentSceneIndex);
                            mainForm.updateOutlinePanel();
                            mainForm.updateHistoryPanel();

                            // fire mainform's selected item changed event
                            mainForm.selectedItemChanged();

                            // record modify action
                            modifyActorAction = new ModifyActorAction(document, actor);
                        }
                    }
                } else {

                    if (this.document.haveSelection() && modifyActorAction != null) {
                        // this is the case when text box was resized with text tool
                        modifyActorAction.setFinalData(this.document.selectedActor());
                        if (modifyActorAction.isModified()) {
                            this.document.actionManager.RecordAction(modifyActorAction);

                            // set the document modified flag
                            this.document.modified = true;

                            // ready to new modification action
                            modifyActorAction = new ModifyActorAction(document, document.selectedActor());

                            // update history
                            mainForm.updateHistoryPanel();
                        }
                    }

                    // update panels
                    this.document.sceneManager.updateThumbnail(this.document.sceneManager.currentSceneIndex);
                    mainForm.updateScenesPanel(this.document.sceneManager.currentSceneIndex);
                    mainForm.updateToolbarSceneSettings();
                    mainForm.updateOutlinePanel();
                }

                foreach (TActor actor in this.document.selectedItems) {
                    actor.deleteBackup();
                }

                MousePressed = false;
                MouseDownTool = TDocument.TOOL_NONE;
                MouseDownPart = -1;
                SelectRegion = RectangleF.Empty;

                // reset cursor
                this.updateCursor();

                // redraw workspace
                this.pnlWorkspace.Refresh();
            }
        }

        private void pnlWorkspace_MouseWheel(object sender, MouseEventArgs e)
        {
            if (this.document.currentScene() == null)
                return;

            // change zoom
            float newZoom = this.document.zoom + (float)e.Delta / 1000;
            if (newZoom >= 0.25 && newZoom <= 4) {
                this.document.zoom = newZoom;
                this.pnlWorkspace.Refresh();

                // main form
                FrmMainContainer mainForm = (FrmMainContainer)this.MdiParent;
                mainForm.updateToolbarSceneSettings();
            }
        }
    }

}
