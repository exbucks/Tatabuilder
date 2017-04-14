using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    public class TScene : TLayer
    {
        public bool touchIndication { get; set; }
        public bool prevButtonVisible { get; set; }
        public bool nextButtonVisible { get; set; }
        public string backgroundMusic { get; set; }
        public int backgroundMusicVolume { get; set; }

        public FrmEmulator  run_emulator { get; set; }
        public Matrix       run_matrix { get; set; }
        public List<TActor> run_extraActors { get; set; }

        public TScene(TDocument doc)
            : base(doc, null, "")
        {
            // default background color of new scene
            backgroundColor = Color.White;

            // default values
            touchIndication = true;
            prevButtonVisible = true;
            nextButtonVisible = true;

            // bgm
            backgroundMusic = "";
            backgroundMusicVolume = 100;

            // store scene manager
            document = doc;

            // initialize runtime variables
            run_matrix = null;
            run_extraActors = null;
        }

        public TScene(TDocument doc, string sceneName)
            : base(doc, null, sceneName)
        {
            // default background color of new scene
            backgroundColor = Color.White;

            // default values
            touchIndication = true;
            prevButtonVisible = true;
            nextButtonVisible = true;

            // BGM
            backgroundMusic = "";
            backgroundMusicVolume = 100;
        }

        protected override void clone(TLayer target)
        {
            base.clone(target);

            TScene targetLayer = (TScene)target;
            targetLayer.touchIndication = this.touchIndication;
            targetLayer.prevButtonVisible = this.prevButtonVisible;
            targetLayer.nextButtonVisible = this.nextButtonVisible;
            targetLayer.backgroundMusic = this.backgroundMusic;
            targetLayer.backgroundMusicVolume = this.backgroundMusicVolume;
        }

        public override bool parseXml(XElement xml, TLayer parentLayer)
        {
            if (xml == null || xml.Name != "Scene")
                return false;

            if (!base.parseXml(xml, null))
                return false;

            try {
                touchIndication = bool.Parse(xml.Element("TouchIndication").Value);
                prevButtonVisible = bool.Parse(xml.Element("PrevButtonVisible").Value);
                nextButtonVisible = bool.Parse(xml.Element("NextButtonVisible").Value);
                backgroundMusic = xml.Element("BackgroundMusic").Value;
                backgroundMusicVolume = TUtil.parseIntXElement(xml.Element("BackgroundMusicVolume"), 100);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "Scene";
            xml.Add(
                new XElement("TouchIndication", touchIndication),
                new XElement("PrevButtonVisible", prevButtonVisible),
                new XElement("NextButtonVisible", nextButtonVisible),
                new XElement("BackgroundMusic", backgroundMusic),
                new XElement("BackgroundMusicVolume", backgroundMusicVolume)
            );

            return xml;
        }

        public Image thumbnailImage()
        {
            // create thumbnail bitmap
            Bitmap thumbnail = new Bitmap(Program.SCENE_THUMBNAIL_WIDTH, Program.SCENE_THUMBNAIL_HEIGHT);

            // draw background
            Graphics g = Graphics.FromImage(thumbnail);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, Program.SCENE_THUMBNAIL_WIDTH, Program.SCENE_THUMBNAIL_HEIGHT);

            // calc matrix
            Matrix m = new Matrix();
            m.Scale((float)Program.SCENE_THUMBNAIL_WIDTH / Program.BOOK_WIDTH, (float)Program.SCENE_THUMBNAIL_HEIGHT / Program.BOOK_HEIGHT);

            // save graphics and apply matrix
            GraphicsState gs = g.Save();
            g.MultiplyTransform(m);

            // draw scene
            this.draw(g);

            // restore graphics
            g.Restore(gs);

            return thumbnail;
        }

        public TImageActor pushImage(string image, float x, float y)
        {
            TImageActor actor;

            // new name
            string actorName = this.newLayerName("Actor_");

            // selected layer
            if (document.haveSelection()) {

                TLayer selectedLayer = document.selectedItems[0];
                PointF pt = selectedLayer.parent.screenToLogical(new PointF(x, y));
                actor = new TImageActor(document, image, pt.X, pt.Y, selectedLayer.parent, actorName);

            } else {

                // create image actor
                PointF pt = this.screenToLogical(new PointF(x, y));
                actor = new TImageActor(document, image, pt.X, pt.Y, this, actorName);
            }

            return actor;
        }

        public TTextActor pushText(string text, RectangleF region)
        {
            TTextActor actor = null;

            // new name
            string actorName = this.newLayerName("Actor_");

            // selected layer
            if (document.haveSelection()) {

                TLayer selectedLayer = document.selectedItems[0];
                PointF pt = selectedLayer.parent.screenToLogical(new PointF(region.X + region.Width / 2, region.Y + region.Height / 2));
                PointF sz = selectedLayer.parent.screenVectorToLogical(new PointF(region.Width, region.Height));
                actor = new TTextActor(document, text, pt.X, pt.Y, sz.X, sz.Y, selectedLayer.parent, actorName);

            } else {

                // text box should have the size at leat 100x30 initially
                PointF minSz = this.logicalVectorToScreen(new PointF(100, 30));
                if (region.Width < minSz.X)
                    region.Width = minSz.X;
                if (region.Height < minSz.Y)
                    region.Height = minSz.Y;

                // create text actor
                PointF pt = this.screenToLogical(new PointF(region.X + region.Width / 2, region.Y + region.Height / 2));
                PointF sz = this.screenVectorToLogical(new PointF(region.Width, region.Height));
                actor = new TTextActor(document, text, pt.X, pt.Y, sz.X, sz.Y, this, actorName);
            }

            return actor;
        }

        public TAvatarActor pushAvatar(RectangleF region)
        {
            TAvatarActor actor = null;

            // new name
            string actorName = this.newLayerName("Actor_");

            // selected layer
            if (document.haveSelection()) {

                TLayer selectedLayer = document.selectedItems[0];
                PointF pt = selectedLayer.parent.screenToLogical(new PointF(region.X + region.Width / 2, region.Y + region.Height / 2));
                PointF sz = selectedLayer.parent.screenVectorToLogical(new PointF(region.Width, region.Height));
                actor = new TAvatarActor(document, pt.X, pt.Y, sz.X, sz.Y, selectedLayer.parent, actorName);

            } else {

                // create text actor
                PointF pt = this.screenToLogical(new PointF(region.X + region.Width / 2, region.Y + region.Height / 2));
                PointF sz = this.screenVectorToLogical(new PointF(region.Width, region.Height));
                actor = new TAvatarActor(document, pt.X, pt.Y, sz.X, sz.Y, this, actorName);
            }

            return actor;
        }

        public override void draw(Graphics g)
        {
            // alpha
            float al = this.alphaFromScreen();

            // scene background
            g.FillRectangle(new SolidBrush(Color.FromArgb((int)(al * 255), this.backgroundColor)), 0, 0, Program.BOOK_WIDTH, Program.BOOK_HEIGHT);

            // draw actors
            List<TActor> items = this.sortedChilds();
            for (int i = 0; i < items.Count; i++) {
                items[i].draw(g);
            }

            // draw extra actors
            if (run_extraActors != null) {
                foreach (TActor actor in run_extraActors) {
                    actor.draw(g);
                }
            }
        }

        public override TLayer findLayer(string targetName)
        {
            if (run_extraActors != null) {
                foreach (TActor actor in run_extraActors) {
                    TLayer ret = actor.findLayer(targetName);
                    if (ret != null)
                        return ret;
                }
            }

            return base.findLayer(targetName);
        }

        // matrix mutliplied the matrix from top scene to this
        public override Matrix matrixFromScreen()
        {
            if (run_matrix != null)
                return run_matrix.Clone();
            else
                return document.workspaceMatrix.Clone();
        }

        public override TActor actorAtPosition(Matrix m, PointF screenPos, bool withinInteraction)
        {
            if (run_extraActors != null) {
                for (int i = run_extraActors.Count - 1; i >= 0; i--) {
                    TActor ret = run_extraActors[i].actorAtPosition(m, screenPos, withinInteraction);
                    if (ret != null)
                        return ret;
                }
            }

            List<TActor> items = this.sortedChilds();
            for (int i = items.Count - 1; i >= 0; i--) {
                TActor ret = items[i].actorAtPosition(m, screenPos, withinInteraction);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        public override RectangleF bound()
        {
            return new RectangleF(0, 0, Program.BOOK_WIDTH, Program.BOOK_HEIGHT);
        }

        public override bool isUsingSound(string snd)
        {
            // check document properties
            if (backgroundMusic.Equals(snd))
                return true;

            return base.isUsingSound(snd);
        }

        //==================================== Animation =================================================//

        public override string[] getDefaultEvents()
        {
            return new string[] { Program.DEFAULT_EVENT_ENTER, Program.DEFAULT_EVENT_AUTOPLAY };
        }

        public override string[] getDefaultStates()
        {
            return new string[] { Program.DEFAULT_STATE_DEFAULT };
        }

        public override void step(FrmEmulator emulator, long time)
        {
            base.step(emulator, time);

            // step of extra actors
            if (run_extraActors != null) {
                foreach (TActor actor in run_extraActors) {
                    actor.step(emulator, time);
                }
            }
        }

    }
}
