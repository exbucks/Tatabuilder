using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using GuiLabs.Undo;
using Ionic.Zip;

namespace TataBuilder
{
    public class TDocument
    {
        public TSceneManager sceneManager { get; set; }
        public TLibraryManager libraryManager { get; set; }
        public ActionManager actionManager { get; set; }

        public string identifier { get; set; }

        public string backgroundMusic { get; set; }
        public int backgroundMusicVolume { get; set; }
        public int navigationButtonDelayTime { get; set; }
        public bool navigationLeftButtonRender { get; set; }
        public bool navigationRightButtonRender { get; set; }


        public string prevSceneButton { get; set; }
        public string nextSceneButton { get; set; }

        public string avatarDefault { get; set; }
        public string avatarFrame { get; set; }
        public string avatarMask { get; set; }

        /*================================================== Properties ======================================================================*/

        public bool modified { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public string directory { get; set; }

        public const int TOOL_NONE = 0;
        public const int TOOL_SELECT = 1;
        public const int TOOL_HAND = 2;
        public const int TOOL_TEXT = 3;
        public const int TOOL_BOUNDING = 4;
        public const int TOOL_AVATAR = 5;
        public const int TOOL_PUZZLE = 6;

        public int currentTool { get; set; }
        public int currentTempTool { get; set; }
        public float zoom { get; set; }
        public PointF offset { get; set; }

        public List<TActor> selectedItems { get; set; }
        public Matrix workspaceMatrix { get; set; }

        public Image run_avatar { get; set; }

        public TDocument()
        {
            // create managers
            sceneManager = new TSceneManager(this);
            libraryManager = new TLibraryManager(this);
            actionManager = new ActionManager();

            // setting
            identifier = "";

            backgroundMusic = "";
            backgroundMusicVolume = 100;
            navigationButtonDelayTime = 5;
            navigationLeftButtonRender = true;
            navigationRightButtonRender = true;

            prevSceneButton = "";
            nextSceneButton = "";
            avatarDefault = "";
            avatarFrame = "";
            avatarMask = "";

            // initialize properties
            modified = false;
            filepath = null;
            filename = "Untitled";
            directory = null;

            currentTool = TOOL_SELECT;
            currentTempTool = TOOL_NONE;
            zoom = Program.DEFAULT_ZOOM;
            offset = new Point(0, 0);

            selectedItems = new List<TActor>();
            workspaceMatrix = new Matrix();

            run_avatar = null;
        }

        public bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "Document")
                return false;

            identifier = TUtil.parseStringXElement(xml.Element("Identifier"), "");
            backgroundMusic = xml.Element("BackgroundMusic").Value;
            backgroundMusicVolume = TUtil.parseIntXElement(xml.Element("BackgroundMusicVolume"), 100);
            navigationButtonDelayTime = TUtil.parseIntXElement(xml.Element("NavigationButtonDelayTime"), 5);
            navigationLeftButtonRender = TUtil.parseBoolXElement(xml.Element("NavigationLeftButtonRender"), true);
            navigationRightButtonRender = TUtil.parseBoolXElement(xml.Element("NavigationRightButtonRender"), true);
            prevSceneButton = xml.Element("PrevSceneButton").Value;
            nextSceneButton = xml.Element("NextSceneButton").Value;
            avatarDefault = xml.Element("AvatarDefault").Value;
            avatarFrame = xml.Element("AvatarFrame").Value;
            avatarMask = xml.Element("AvatarMask").Value;

            return
                libraryManager.parseXml(xml.Element("Libraries")) &&
                sceneManager.parseXml(xml.Element("Scenes"));
        }

        public XElement toXml()
        {
            return
                new XElement("Document",
                    new XElement("Version", Program.getVersion()),
                    new XElement("Identifier", identifier),
                    new XElement("BackgroundMusic", backgroundMusic),
                    new XElement("BackgroundMusicVolume", backgroundMusicVolume),
                    new XElement("NavigationButtonDelayTime", navigationButtonDelayTime),
                    new XElement("NavigationLeftButtonRender", navigationLeftButtonRender),
                    new XElement("NavigationRightButtonRender", navigationLeftButtonRender),
                    new XElement("PrevSceneButton", prevSceneButton),
                    new XElement("NextSceneButton", nextSceneButton),
                    new XElement("AvatarDefault", avatarDefault),
                    new XElement("AvatarFrame", avatarFrame),
                    new XElement("AvatarMask", avatarMask),
                    libraryManager.toXml(),
                    sceneManager.toXml()
                );
        }

        public string getImagesDirectoryPath()
        {
            return this.directory + "\\images";
        }

        public string getSoundsDirectoryPath()
        {
            return this.directory + "\\sounds";
        }

        public void checkProjectDirectories()
        {
            // project folder
            if (!Directory.Exists(this.directory))
                Directory.CreateDirectory(this.directory);

            if (!Directory.Exists(this.directory + "\\images"))
                Directory.CreateDirectory(this.getImagesDirectoryPath());

            if (!Directory.Exists(this.directory + "\\sounds"))
                Directory.CreateDirectory(this.getSoundsDirectoryPath());
        }

        public bool open(string filePathName)
        {
            FileInfo fileInfo = new FileInfo(filePathName);
            this.filepath = filePathName;
            this.filename = fileInfo.Name;
            this.directory = fileInfo.DirectoryName;

            // check project directories
            this.checkProjectDirectories();

            try {
                XElement xDoc = XElement.Load(this.filepath);
                return this.parseXml(xDoc);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            
            return false;
        }

        public bool save()
        {
            // check project directories
            this.checkProjectDirectories();

            try {
                XElement xDoc = this.toXml();
                xDoc.Save(this.filepath);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public bool save(string filePathName)
        {
            string oldDirectory = this.directory;

            FileInfo fileInfo = new FileInfo(filePathName);
            this.filepath = filePathName;
            this.filename = fileInfo.Name;
            this.directory = fileInfo.DirectoryName;

            // check project directories
            this.checkProjectDirectories();

            if (oldDirectory != null && !this.directory.Equals(oldDirectory))
            {
                for (int i = 0; i < libraryManager.imageCount(); i++) {
                    try {
                        File.Copy(oldDirectory + "\\images\\" + libraryManager.imageFileName(i), libraryManager.imageFilePath(i));
                    } catch (Exception) {
                        MessageBox.Show("Can't save the image " + libraryManager.imageFileName(i), Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                for (int i = 0; i < libraryManager.soundCount(); i++) {
                    try {
                        File.Copy(oldDirectory + "\\sounds\\" + libraryManager.soundFileName(i), libraryManager.soundFilePath(i));
                    } catch (Exception) {
                        MessageBox.Show("Can't save the sound " + libraryManager.soundFileName(i), Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            try {
                XElement xDoc = this.toXml();
                xDoc.Save(this.filepath);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public void export(string filePathName) 
        {
            using (ZipFile zip = new ZipFile()) {
                //zip.UseUnicodeAsNecessary = true;
                zip.AlternateEncodingUsage = ZipOption.Always;
                zip.AlternateEncoding = Encoding.UTF8;

                // add project file
                ZipEntry mainFile = zip.AddFile(this.filepath, "");
                mainFile.FileName = "main." + Program.DOC_EXTENSION;

                // add image resources
                for (int i = 0; i < libraryManager.imageCount(); i++) 
                    zip.AddFile(libraryManager.imageFilePath(i), "images");

                // add image resources
                for (int i = 0; i < libraryManager.soundCount(); i++)
                    zip.AddFile(libraryManager.soundFilePath(i), "sounds");

                // save zip file
                zip.Save(filePathName);
            }
        }

        public void drawWorkspace(Graphics g, float width, float height) 
        {
            // clear workspace
            g.Clear(Color.FromArgb(250, 250, 250));

            TScene scene = this.currentScene();
            if (scene != null) {
                // calc matrix
                workspaceMatrix.Reset();
                workspaceMatrix.Translate(width / 2 + offset.X, height / 2 + offset.Y);
                workspaceMatrix.Scale(zoom, zoom);
                workspaceMatrix.Translate(-0.5F * Program.BOOK_WIDTH, -0.5F * Program.BOOK_HEIGHT);

                // border of work area
                PointF[] aPos = { new PointF(0, 0), new PointF(Program.BOOK_WIDTH, Program.BOOK_HEIGHT) };
                workspaceMatrix.TransformPoints(aPos);
                aPos[0].X = (float)Math.Ceiling(aPos[0].X - 1);
                aPos[0].Y = (float)Math.Ceiling(aPos[0].Y - 1);
                aPos[1].X = (float)Math.Ceiling(aPos[1].X + 1);
                aPos[1].Y = (float)Math.Ceiling(aPos[1].Y + 1);
                g.DrawRectangle(new Pen(Color.FromArgb(198, 198, 198)), aPos[0].X, aPos[0].Y, aPos[1].X - aPos[0].X - 1, aPos[1].Y - aPos[0].Y - 1);

                // apply matrix
                GraphicsState gs = g.Save();
                g.MultiplyTransform(workspaceMatrix);

                // draw scene
                scene.draw(g);

                // restore graphics
                g.Restore(gs);

                // draw selected bound
                this.drawSelectedBound(g);
            }
        }

        private void drawSelectedBound(Graphics g)
        {
            PointF[] bound = null;
            TActor actor = this.selectedActor();
            if (currentTool == TDocument.TOOL_BOUNDING) {
                if (actor != null)
                    bound = actor.interactionBoundOnScreen();
            } else if (currentTool == TDocument.TOOL_PUZZLE) {
                if (actor != null && actor.puzzle)
                    bound = actor.puzzleAreaOnScreen();
            } else {
                bound = this.selectedBound();
            }

            if (bound != null) {
                Color boundColor;
                switch (currentTool) {
                    case TDocument.TOOL_BOUNDING:
                        boundColor = Color.Red;
                        break;
                    case TDocument.TOOL_PUZZLE:
                        boundColor = Color.Blue;
                        break;
                    default:
                        boundColor = Color.Black;
                        break;
                }

                // draw rectangle
                Pen pen = new Pen(boundColor, 1);
                pen.DashStyle = DashStyle.Custom;
                pen.DashPattern = new float[] { 4, 4 };
                g.DrawPolygon(pen, bound);

                // draw control point
                const int ctrl_size =  6;
                Pen ctrl_pen = new Pen(boundColor, 1);
                Brush ctrl_brush = new SolidBrush(Color.White);
                Rectangle ctrl_rect;
                for (int i = 0; i < bound.Length; i++) {

                    // corner
                    ctrl_rect = new Rectangle((int)(bound[i].X - ctrl_size / 2), (int)(bound[i].Y - ctrl_size / 2), ctrl_size, ctrl_size);
                    g.FillRectangle(ctrl_brush, ctrl_rect);
                    g.DrawRectangle(ctrl_pen, ctrl_rect);
                    
                    // edge
                    int j = (i + 1) % bound.Length;
                    ctrl_rect = new Rectangle((int)((bound[i].X + bound[j].X - ctrl_size) / 2), (int)((bound[i].Y + bound[j].Y - ctrl_size) / 2), ctrl_size, ctrl_size);
                    g.FillRectangle(ctrl_brush, ctrl_rect);
                    g.DrawRectangle(ctrl_pen, ctrl_rect);
                }

                // draw anchor point
                if (actor != null && (currentTool != TDocument.TOOL_BOUNDING && currentTool != TDocument.TOOL_PUZZLE)) {
                    const int anchor_line_size = 10;
                    const int anchor_circle_size = 6;

                    RectangleF actorBound = actor.bound();
                    PointF anchorPosF = actor.logicalToScreen(new PointF(actor.anchor.X * actorBound.Width, actor.anchor.Y * actorBound.Height));
                    Point anchorPos = new Point((int)anchorPosF.X, (int)anchorPosF.Y);
                    g.DrawLine(ctrl_pen, anchorPos.X - anchor_line_size / 2, anchorPos.Y, anchorPos.X - anchor_circle_size / 2, anchorPos.Y);
                    g.DrawLine(ctrl_pen, anchorPos.X + anchor_line_size / 2, anchorPos.Y, anchorPos.X + anchor_circle_size / 2, anchorPos.Y);
                    g.DrawLine(ctrl_pen, anchorPos.X, anchorPos.Y - anchor_line_size / 2, anchorPos.X, anchorPos.Y - anchor_circle_size / 2);
                    g.DrawLine(ctrl_pen, anchorPos.X, anchorPos.Y + anchor_line_size / 2, anchorPos.X, anchorPos.Y + anchor_circle_size / 2);
                    g.DrawEllipse(ctrl_pen, anchorPos.X - anchor_circle_size / 2, anchorPos.Y - anchor_circle_size / 2, anchor_circle_size, anchor_circle_size);
                    g.FillRectangle(Brushes.Black, anchorPos.X, anchorPos.Y, 1, 1);
                }
            }
        }

        public TScene currentScene() 
        {
            return sceneManager.scene(sceneManager.currentSceneIndex);
        }

        public TScene prevScene(TScene scene)
        {
            int sceneIndex = sceneManager.indexOfScene(scene.name);
            if (sceneIndex > 0)
                return sceneManager.scene(sceneIndex - 1);
            else
                return sceneManager.scene(sceneManager.sceneCount() - 1);
        }

        public TScene nextScene(TScene scene)
        {
            int sceneIndex = sceneManager.indexOfScene(scene.name);
            if (sceneIndex + 1 < sceneManager.sceneCount())
                return sceneManager.scene(sceneIndex + 1);
            else
                return sceneManager.scene(0);
        }

        public TScene findScene(string sceneName)
        {
            return sceneManager.scene(sceneManager.indexOfScene(sceneName));
        }
        
        // check selection
        public bool haveSelection()
        {
            return selectedItems.Count > 0;
        }

        public TActor selectedActor()
        {
            if (selectedItems.Count == 1)
                return selectedItems[0];

            return null;
        }

        public TLayer selectedLayer()
        {
            if (selectedItems.Count == 0)
                return this.currentScene();
            else if (selectedItems.Count == 1)
                return selectedItems[0];
            return null;
        }

        // get the bound that contain the selected items based on real drawing canvas coordinates
        public PointF[] selectedBound()
        {
            if (selectedItems.Count == 0)
                return null;
            if (selectedItems.Count == 1)
                return selectedItems[0].boundOnScreen();

            RectangleF bound = selectedItems[0].boundStraightOnScreen();
            for (int i = 1; i < selectedItems.Count; i++) {
                bound = RectangleF.Union(bound, selectedItems[i].boundStraightOnScreen());
            }

            return new PointF[] { new PointF(bound.Left, bound.Top), new PointF(bound.Left, bound.Bottom), new PointF(bound.Right, bound.Bottom), new PointF(bound.Right, bound.Top) };
        }

        public Image getAvatarImage()
        {
            if (run_avatar != null)
                return run_avatar;

            int avatarIndex = libraryManager.imageIndex(avatarDefault);
            if (avatarIndex == -1)
                return Properties.Resources.avatar_default;
            else
                return Image.FromFile(libraryManager.imageFilePath(avatarIndex));
        }

        public Image getAvatarFrameImage()
        {
            int index = libraryManager.imageIndex(avatarFrame);
            if (index == -1)
                return Properties.Resources.avatar_frame;
            else
                return Image.FromFile(libraryManager.imageFilePath(index));
        }

        public Image getAvatarMaskImage()
        {
            int index = libraryManager.imageIndex(avatarMask);
            if (index == -1)
                return Properties.Resources.avatar_mask;
            else
                return Image.FromFile(libraryManager.imageFilePath(index));
        }

        public bool containsInSelection(float x, float y)
        {
            return TUtil.isInPolygon(this.selectedBound(), new PointF(x, y));
        }

        //============== return value ===============//
        //
        //                    -1
        //      9                           9
        //        ┌───────────────────────┐
        //        │ 1         8         4 │
        //        │                       │
        //  -1    │ 5         0         7 │    -1
        //        │                       │
        //        │ 2         6         3 │
        //        └───────────────────────┘
        //      9                           9
        //                    -1
        //
        //
        // Anchor Point : 10
        //============================================//
        public int partOfSelection(float x, float y, out int cursor)
        {
            cursor = -1;

            if (selectedItems.Count == 0)
                return -1;
            if (selectedItems.Count > 1)
                return containsInSelection(x, y) ? 0 : 9;

            TActor actor = selectedItems[0];
            if (currentTool == TDocument.TOOL_PUZZLE && !actor.puzzle)
                return -1;

            PointF screenPos = new PointF(x, y);
            PointF logicalPos = currentTool != TDocument.TOOL_PUZZLE ? actor.screenToLogical(new PointF(x, y)) : actor.ownerScene().screenToLogical(new PointF(x, y));

            RectangleF actorBound = actor.bound();
            PointF anchorPosOnScreen = actor.logicalToScreen(new PointF(actorBound.Width * actor.anchor.X, actorBound.Height * actor.anchor.Y));

            RectangleF bound;
            if (currentTool == TDocument.TOOL_BOUNDING) {
                bound = actor.interactionBound;
            } else if (currentTool == TDocument.TOOL_PUZZLE) {
                bound = actor.puzzleArea;
            } else {
                bound = actorBound;
            }

            PointF[] boundOnScreen;
            if (currentTool == TDocument.TOOL_BOUNDING) {
                boundOnScreen = actor.interactionBoundOnScreen();
            } else if (currentTool == TDocument.TOOL_PUZZLE) {
                boundOnScreen = actor.puzzleAreaOnScreen();
            } else {
                boundOnScreen = actor.boundOnScreen();
            }
            int ctrl_size = 6;

            bool leftEdge       = TUtil.distanceBetweenPointLine(screenPos, boundOnScreen[0], boundOnScreen[1]) <= ctrl_size;
            bool bottomEdge     = TUtil.distanceBetweenPointLine(screenPos, boundOnScreen[1], boundOnScreen[2]) <= ctrl_size;
            bool rightEdge      = TUtil.distanceBetweenPointLine(screenPos, boundOnScreen[2], boundOnScreen[3]) <= ctrl_size;
            bool topEdge        = TUtil.distanceBetweenPointLine(screenPos, boundOnScreen[3], boundOnScreen[0]) <= ctrl_size;
            bool insideBound    = bound.Contains(logicalPos);

            int first_cursor = 0; // cursor form part 5
            double angle = -Math.Atan2(boundOnScreen[1].Y - boundOnScreen[0].Y, boundOnScreen[1].X - boundOnScreen[0].X) * 180 / Math.PI;
            if (angle < 0) angle += 360;
            first_cursor = (int)((angle + 22.5) / 45) % 4;

            int part = -1;

            if (leftEdge && topEdge) {
                part = 1; cursor = (first_cursor - 1) % 4;
            } else if (leftEdge && bottomEdge) {
                part = 2; cursor = (first_cursor + 1) % 4;
            } else if (rightEdge && bottomEdge) {
                part = 3; cursor = (first_cursor + 3) % 4;
            } else if (rightEdge && topEdge) {
                part = 4; cursor = (first_cursor + 5) % 4;
            } else if (leftEdge && TUtil.isPointProjectionInLineSegment(screenPos, boundOnScreen[0], boundOnScreen[1])) {
                part = 5; cursor = (first_cursor + 0) % 4;
            } else if (bottomEdge && TUtil.isPointProjectionInLineSegment(screenPos, boundOnScreen[1], boundOnScreen[2])) {
                part = 6; cursor = (first_cursor + 2) % 4;
            } else if (rightEdge && TUtil.isPointProjectionInLineSegment(screenPos, boundOnScreen[2], boundOnScreen[3])) {
                part = 7; cursor = (first_cursor + 4) % 4;
            } else if (topEdge && TUtil.isPointProjectionInLineSegment(screenPos, boundOnScreen[3], boundOnScreen[0])) {
                part = 8; cursor = (first_cursor + 6) % 4;
            } else if (TUtil.distanceBetweenPoints(screenPos, anchorPosOnScreen) <= ctrl_size) {
                part = 10;
            } else if (insideBound) {
                part = 0;
            } else if (currentTool != TDocument.TOOL_BOUNDING && currentTool != TDocument.TOOL_PUZZLE) {
                if (TUtil.distanceBetweenPoints(screenPos, boundOnScreen[0]) <= ctrl_size * 3 ||
                    TUtil.distanceBetweenPoints(screenPos, boundOnScreen[1]) <= ctrl_size * 3 ||
                    TUtil.distanceBetweenPoints(screenPos, boundOnScreen[2]) <= ctrl_size * 3 ||
                    TUtil.distanceBetweenPoints(screenPos, boundOnScreen[3]) <= ctrl_size * 3)
                    part = 9;
            }

            return part;
        }

        public void clearSelectedItems()
        {
            this.selectedItems.Clear();
        }

        public void toggleSelectedItem(TActor item)
        {
            if (this.selectedItems.Contains(item))
                this.selectedItems.Remove(item);
            else
                this.selectedItems.Add(item);
        }

        // move the selected items the specified delta, parameters are based on real drawing canvas coordinates
        public void moveSelectedItems(float dx, float dy, bool fixedMove)
        {
            if (fixedMove) {
                double al = Math.Atan2(dy, dx) * 180 / Math.PI;
                float d = Math.Min(Math.Abs(dx), Math.Abs(dy));
                if (al >= -22.5 && al < 22.5) {
                    dy = 0;
                } else if (al >= 22.5 && al < 67.5) {
                    dx = d; dy = d;
                } else if (al >= 67.5 && al < 112.5) {
                    dx = 0;
                } else if (al >= 112.5 && al < 157.5) {
                    dx = -d; dy = d;
                } else if (al >= 157.5 || al < -157.5) {
                    dy = 0;
                } else if (al >= -157.5 && al < -112.5) {
                    dx = -d; dy = -d;
                } else if (al >= -112.5 && al < -67.5) {
                    dx = 0;
                } else if (al >= 67.5 && al < 22.5) {
                    dx = d; dy =- d;
                }
            }

            for (int i = 0; i < selectedItems.Count; i++) {
                TActor item = this.selectedItems[i];
                TActor origin = item.backupActor;
                PointF p = origin.parent.logicalToScreen(origin.position);
                p.X += dx; p.Y += dy;
                item.position = origin.parent.screenToLogical(p);
            }
        }

        // move the anchor point of selected item the specified delta, parameters are based on real drawing canvas coordinates
        public void moveAnchorOfSelectedItem(float dx, float dy, bool fixedMove)
        {
            if (fixedMove) {
                double al = Math.Atan2(dy, dx) * 180 / Math.PI;
                float d = Math.Min(Math.Abs(dx), Math.Abs(dy));
                if (al >= -22.5 && al < 22.5) {
                    dy = 0;
                } else if (al >= 22.5 && al < 67.5) {
                    dx = d; dy = d;
                } else if (al >= 67.5 && al < 112.5) {
                    dx = 0;
                } else if (al >= 112.5 && al < 157.5) {
                    dx = -d; dy = d;
                } else if (al >= 157.5 || al < -157.5) {
                    dy = 0;
                } else if (al >= -157.5 && al < -112.5) {
                    dx = -d; dy = -d;
                } else if (al >= -112.5 && al < -67.5) {
                    dx = 0;
                } else if (al >= 67.5 && al < 22.5) {
                    dx = d; dy = -d;
                }
            }

            for (int i = 0; i < selectedItems.Count; i++) {
                TActor item = this.selectedItems[i];
                TActor origin = item.backupActor;

                PointF screenPos = origin.parent.logicalToScreen(origin.position);
                screenPos.X += dx; screenPos.Y += dy;
                PointF logicalPos = origin.screenToLogical(screenPos);
                RectangleF bound = origin.bound();

                item.position = origin.parent.screenToLogical(screenPos);
                item.anchor = new PointF(logicalPos.X / bound.Width, logicalPos.Y / bound.Height);
            }
        }

        // scale the selected items the specified delta, parameters are based on real drawing canvas coordinates
        public void scaleSelectedItems(int part, float dx, float dy, bool fixedRatio)
        {
            for (int i = 0; i < selectedItems.Count; i++) {

                TActor item = this.selectedItems[i];
                TActor origin = item.backupActor;
                PointF d = origin.screenVectorToLogical(new PointF(dx, dy));

                float sx = origin.scale.Width, sy = origin.scale.Height;
                RectangleF bound = origin.bound();
                float px = origin.anchor.X * bound.Width, py = origin.anchor.Y * bound.Height;

                if (fixedRatio) {
                    float w = bound.Width * sx, h = bound.Height * sy;
                    float z;
                    if (part == 1) {
                        z = Math.Max(-d.X / w, -d.Y / h);
                        d = new PointF(-z * w, -z * h);
                    } else if (part == 2) {
                        z = Math.Max(-d.X / w, d.Y / h);
                        d = new PointF(-z * w, z * h);
                    } else if (part == 3) {
                        z = Math.Max(d.X / w, d.Y / h);
                        d = new PointF(z * w, z * h);
                    } else if (part == 4) {
                        z = Math.Max(d.X / w, -d.Y / h);
                        d = new PointF(z * w, -z * h);
                    }
                }

                if (part == 1 || part == 2 || part == 5) {
                    sx -= sx * d.X / bound.Width;
                    px = d.X + (bound.Width - d.X) * origin.anchor.X;
                }
                if (part == 3 || part == 4 || part == 7) {
                    sx += sx * d.X / bound.Width;
                    px = origin.anchor.X * (bound.Width + d.X);
                }
                if (part == 1 || part == 4 || part == 8) {
                    sy -= sy * d.Y / bound.Height;
                    py = d.Y + (bound.Height - d.Y) * origin.anchor.Y;
                }
                if (part == 2 || part == 3 || part == 6) {
                    sy += sy * d.Y / bound.Height;
                    py = origin.anchor.Y * (bound.Height + d.Y);
                }

                PointF[] p0 = { new PointF(px, py) };
                origin.matrix.TransformPoints(p0);
                item.position = p0[0];
                item.scale = new SizeF(sx, sy);

//                PointF p0 = item.logicalToScreen(new PointF(px, py));
//                item.position = item.parent.screenToLogical(p0);
//                item.scale = new SizeF(sx, sy);
            }
        }

        // rotate the selected item the specified angle, the angle is degree
        public void rotateSelectedItems(float angle, bool fixedAngle)
        {
            if (selectedItems.Count == 0)
                return;

            if (fixedAngle) {
                angle =(float)(Math.Floor(angle / 15) + 1) * 15;
            }

            if (selectedItems.Count == 1) {
                TActor item = (TActor)this.selectedItems[0];
                TActor origin = item.backupActor;
                item.rotation = TUtil.normalizeDegreeAngle(origin.rotation + angle);
            } else {

                // center of selection
                PointF[] bound = this.selectedBound();
                PointF c = new PointF((bound[0].X + bound[2].X) / 2, (bound[0].Y + bound[2].Y) / 2);

                // rotate each selected item
                for (int i = 0; i < selectedItems.Count; i++) {

                    // adjust rotation value
                    TActor item = this.selectedItems[0];
                    TActor origin = item.backupActor;
                    item.rotation = TUtil.normalizeDegreeAngle(origin.rotation + angle);

                    // item position
                    PointF p = item.parent.logicalToScreen(item.position);
                    p = TUtil.rotatePositionAround(c, p, angle);
                    item.position = item.parent.screenToLogical(p);
                }
            }

        }

        // rotate the selected item to the specified angle, the angle is degree
        public void rotateSelectedItemsTo(float angle)
        {
            if (selectedItems.Count == 0)
                return;

            // rotate each selected item
            for (int i = 0; i < selectedItems.Count; i++) {
                this.selectedItems[i].rotation = angle;
            }
        }

        // scale the selected text actor the specified delta, parameters are based on real drawing canvas coordinates
        public bool resizeSelectedTextActor(int part, float dx, float dy)
        {
            TActor actor = this.selectedActor();
            if (actor != null && actor is TTextActor) {

                TTextActor item = (TTextActor)actor;
                PointF d = item.screenVectorToLogical(new PointF(dx, dy));

                float w = item.boxSize.Width, h = item.boxSize.Height;
                float px = item.anchor.X * w, py = item.anchor.Y * h;

                if (part == 1 || part == 2 || part == 5) {
                    w -= d.X;
                    px = d.X + w * item.anchor.X;
                }
                if (part == 3 || part == 4 || part == 7) {
                    w += d.X;
                    px = item.anchor.X * w;
                }
                if (part == 1 || part == 4 || part == 8) {
                    h -= d.Y;
                    py = d.Y + h * item.anchor.Y;
                }
                if (part == 2 || part == 3 || part == 6) {
                    h += d.Y;
                    py = item.anchor.Y * h;
                }

                if (w > 0 && h > 0) {

                    PointF[] p0 = { new PointF(px, py) };
                    item.matrix.TransformPoints(p0);
                    item.position = p0[0];
                    item.boxSize = new SizeF(w, h);

                    return true;
                }
            }

            return false;
        }

        // move the selected items the specified delta, parameters are based on real drawing canvas coordinates
        public void moveInteractionBound(float dx, float dy, bool fixedMove)
        {
            if (fixedMove) {
                double al = Math.Atan2(dy, dx) * 180 / Math.PI;
                float d = Math.Min(Math.Abs(dx), Math.Abs(dy));
                if (al >= -22.5 && al < 22.5) {
                    dy = 0;
                } else if (al >= 22.5 && al < 67.5) {
                    dx = d; dy = d;
                } else if (al >= 67.5 && al < 112.5) {
                    dx = 0;
                } else if (al >= 112.5 && al < 157.5) {
                    dx = -d; dy = d;
                } else if (al >= 157.5 || al < -157.5) {
                    dy = 0;
                } else if (al >= -157.5 && al < -112.5) {
                    dx = -d; dy = -d;
                } else if (al >= -112.5 && al < -67.5) {
                    dx = 0;
                } else if (al >= 67.5 && al < 22.5) {
                    dx = d; dy = -d;
                }
            }

            for (int i = 0; i < selectedItems.Count; i++) {
                TActor item = this.selectedItems[i];
                TActor origin = item.backupActor;
                PointF p = origin.logicalToScreen(origin.interactionBound.Location);
                p.X += dx; p.Y += dy;
                item.interactionBound = new RectangleF(origin.screenToLogical(p), origin.interactionBound.Size);
            }
        }

        // scale the selected items the specified delta, parameters are based on real drawing canvas coordinates
        public void scaleInteractionBound(int part, float dx, float dy, bool fixedRatio)
        {
            for (int i = 0; i < selectedItems.Count; i++) {

                TActor item = this.selectedItems[i];
                TActor origin = item.backupActor;

                PointF d = origin.screenVectorToLogical(new PointF(dx, dy));
                RectangleF bound = origin.interactionBound;

                if (fixedRatio) {
                    float z;
                    if (part == 1) {
                        z = Math.Max(-d.X / bound.Width, -d.Y / bound.Height);
                        d = new PointF(-z * bound.Width, -z * bound.Height);
                    } else if (part == 2) {
                        z = Math.Max(-d.X / bound.Width, d.Y / bound.Height);
                        d = new PointF(-z * bound.Width, z * bound.Height);
                    } else if (part == 3) {
                        z = Math.Max(d.X / bound.Width, d.Y / bound.Height);
                        d = new PointF(z * bound.Width, z * bound.Height);
                    } else if (part == 4) {
                        z = Math.Max(d.X / bound.Width, -d.Y / bound.Height);
                        d = new PointF(z * bound.Width, -z * bound.Height);
                    }
                }

                float x1 = bound.Left, y1 = bound.Top, x2 = bound.Right, y2 = bound.Bottom;
                if (part == 1 || part == 2 || part == 5) 
                    x1 += d.X;
                if (part == 3 || part == 4 || part == 7) 
                    x2 += d.X;
                if (part == 1 || part == 4 || part == 8)
                    y1 += d.Y;
                if (part == 2 || part == 3 || part == 6)
                    y2 += d.Y;

                item.interactionBound = new RectangleF(x1, y1, x2 - x1, y2 - y1);
            }
        }

        // move the puzzle area of selected items the specified delta, parameters are based on real drawing canvas coordinates
        public void movePuzzleArea(float dx, float dy, bool fixedMove)
        {
            if (fixedMove) {
                double al = Math.Atan2(dy, dx) * 180 / Math.PI;
                float d = Math.Min(Math.Abs(dx), Math.Abs(dy));
                if (al >= -22.5 && al < 22.5) {
                    dy = 0;
                } else if (al >= 22.5 && al < 67.5) {
                    dx = d; dy = d;
                } else if (al >= 67.5 && al < 112.5) {
                    dx = 0;
                } else if (al >= 112.5 && al < 157.5) {
                    dx = -d; dy = d;
                } else if (al >= 157.5 || al < -157.5) {
                    dy = 0;
                } else if (al >= -157.5 && al < -112.5) {
                    dx = -d; dy = -d;
                } else if (al >= -112.5 && al < -67.5) {
                    dx = 0;
                } else if (al >= 67.5 && al < 22.5) {
                    dx = d; dy = -d;
                }
            }

            for (int i = 0; i < selectedItems.Count; i++) {
                TActor item = this.selectedItems[i];
                TActor origin = item.backupActor;
                PointF p = origin.ownerScene().logicalToScreen(origin.puzzleArea.Location);
                p.X += dx; p.Y += dy;
                item.puzzleArea = new RectangleF(origin.ownerScene().screenToLogical(p), origin.puzzleArea.Size);
            }
        }

        // scale the puzzle area of selected items the specified delta, parameters are based on real drawing canvas coordinates
        public void scalePuzzleArea(int part, float dx, float dy, bool fixedRatio)
        {
            for (int i = 0; i < selectedItems.Count; i++) {

                TActor item = this.selectedItems[i];
                TActor origin = item.backupActor;

                PointF d = origin.ownerScene().screenVectorToLogical(new PointF(dx, dy));
                RectangleF bound = origin.puzzleArea;

                if (fixedRatio) {
                    float z;
                    if (part == 1) {
                        z = Math.Max(-d.X / bound.Width, -d.Y / bound.Height);
                        d = new PointF(-z * bound.Width, -z * bound.Height);
                    } else if (part == 2) {
                        z = Math.Max(-d.X / bound.Width, d.Y / bound.Height);
                        d = new PointF(-z * bound.Width, z * bound.Height);
                    } else if (part == 3) {
                        z = Math.Max(d.X / bound.Width, d.Y / bound.Height);
                        d = new PointF(z * bound.Width, z * bound.Height);
                    } else if (part == 4) {
                        z = Math.Max(d.X / bound.Width, -d.Y / bound.Height);
                        d = new PointF(z * bound.Width, -z * bound.Height);
                    }
                }

                float x1 = bound.Left, y1 = bound.Top, x2 = bound.Right, y2 = bound.Bottom;
                if (part == 1 || part == 2 || part == 5) 
                    x1 += d.X;
                if (part == 3 || part == 4 || part == 7) 
                    x2 += d.X;
                if (part == 1 || part == 4 || part == 8)
                    y1 += d.Y;
                if (part == 2 || part == 3 || part == 6)
                    y2 += d.Y;

                item.puzzleArea = new RectangleF(x1, y1, x2 - x1, y2 - y1);
            }
        }

        // find top layer at specified position, parameters are based on real drawing canvas coordinates
        public TActor actorAtPosition(float x, float y, bool withinInteraction)
        {
            return this.currentScene().actorAtPosition(workspaceMatrix, new PointF(x, y), withinInteraction);
        }

        public int activeTool()
        {
            if (this.currentTempTool != TDocument.TOOL_NONE)
                return this.currentTempTool;
            return this.currentTool;
        }

        public void transferLayer(TActor item, TLayer target)
        {
            // item's position based on new parent
            PointF pt = item.parent.logicalToScreen(item.position);
            pt = target.screenToLogical(pt);

            // item's rotation based on new parent
            float angle = item.rotationOnScreen();
            if (target is TActor)
                angle -= ((TActor)target).rotationOnScreen();
            TUtil.normalizeDegreeAngle(angle);

            // for scale
            RectangleF bound = item.bound();
            PointF s = item.logicalVectorToScreen(new PointF(bound.Width, bound.Height));

            // new properties
            item.position = pt;
            item.rotation = angle;
            item.scale = new Size(1, 1);

            Matrix m = target.matrixFromScreen();
            m.Multiply(item.matrix);
            if (m.IsInvertible) {
                PointF[] aPos = { s };
                m.Invert();
                m.TransformVectors(aPos);
                s = aPos[0];
                item.scale = new SizeF(s.X / bound.Width, s.Y / bound.Height);
            }

            item.parent.childs.Remove(item);
            target.childs.Add(item);
            item.parent = target;
        }

        public bool isUsingImage(string img)
        {
            // check document properties
            if (prevSceneButton.Equals(img) || nextSceneButton.Equals(img))
                return true;
            if (avatarDefault.Equals(img) || avatarFrame.Equals(img) || avatarMask.Equals(img))
                return true;

            // check scenes
            for (int i = 0; i < sceneManager.sceneCount(); i++) {
                TScene scene = sceneManager.scene(i);
                if (scene.isUsingImage(img))
                    return true;
            }

            return false;
        }

        public bool isUsingSound(string snd)
        {
            // check document properties
            if (backgroundMusic.Equals(snd))
                return true;

            // check scenes
            for (int i = 0; i < sceneManager.sceneCount(); i++) {
                TScene scene = sceneManager.scene(i);
                if (scene.isUsingSound(snd))
                    return true;
            }

            return false;
        }
    }

}
