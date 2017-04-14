using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TAvatarActor : TActor
    {

        protected SizeF BoxSize;
        public SizeF boxSize { get { return BoxSize; } set { BoxSize = value; refreshMatrix(); } }

        public TAvatarActor(TDocument doc)
            : base(doc)
        {
            this.BoxSize = new SizeF();
        }

        public TAvatarActor(TDocument doc, float x, float y, float width, float height, TLayer parent, string actorName)
            : base(doc, x, y, parent, actorName)
        {
            this.BoxSize = new SizeF(width, height);
            this.refreshMatrix();
        }

        protected override void clone(TLayer target)
        {
            base.clone(target);

            TAvatarActor targetLayer = (TAvatarActor)target;
            targetLayer.BoxSize = this.BoxSize;
            targetLayer.refreshMatrix();
        }

        public override bool parseXml(XElement xml, TLayer parentLayer)
        {
            if (xml == null || xml.Name != "AvatarActor")
                return false;

            if (!base.parseXml(xml, parentLayer))
                return false;

            try {
                BoxSize.Width = float.Parse(xml.Element("SizeWidth").Value);
                BoxSize.Height = float.Parse(xml.Element("SizeHeight").Value);
                refreshMatrix();
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "AvatarActor";
            xml.Add(
                new XElement("SizeWidth", boxSize.Width),
                new XElement("SizeHeight", boxSize.Height)
            );

            return xml;
        }

        public override void refreshMatrix()
        {
            matrix.Reset();
            matrix.Translate(Position.X, Position.Y);
            matrix.Rotate(Rotation);
            matrix.Scale(Scale.Width, Scale.Height);
            matrix.Shear(Skew.Width, Skew.Height);
            matrix.Translate(-Anchor.X * BoxSize.Width, -Anchor.Y * BoxSize.Height);
        }

        public override void draw(Graphics g)
        {
            Image avata = document.sceneManager.document.getAvatarImage();
            if (avata != null) {
                // alpha
                float al = this.alphaFromScreen();

                if (al > 1e-10) {  // alpha > 0
                    // save graphics
                    GraphicsState gs = g.Save();

                    // apply matrix
                    g.MultiplyTransform(matrix);

                    // background
                    g.FillRectangle(new SolidBrush(Color.FromArgb((int)(al * this.backgroundColor.A), this.backgroundColor)), this.bound());

                    // draw image
                    if (1 - al < 1e-10) { // if alpha == 1
                        g.DrawImage(avata, 0, 0, BoxSize.Width, BoxSize.Height);
                    } else {
                        ColorMatrix cm = new ColorMatrix();
                        cm.Matrix33 = al;
                        ImageAttributes ia = new ImageAttributes();
                        ia.SetColorMatrix(cm);
                        g.DrawImage(avata, new PointF[] { new PointF(0, 0), new PointF(BoxSize.Width, 0), new PointF(0, BoxSize.Height) }, new RectangleF(0, 0, avata.Width, avata.Height), GraphicsUnit.Pixel, ia);
                    }

                    // draw childs
                    List<TActor> items = this.sortedChilds();
                    for (int i = 0; i < items.Count; i++) {
                        items[i].draw(g);
                    }

                    // restore graphics
                    g.Restore(gs);
                }
            }
        }

        public override RectangleF bound()
        {
            return new RectangleF(0, 0, BoxSize.Width, BoxSize.Height);
        }
    }
}
