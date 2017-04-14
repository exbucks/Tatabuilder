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
    public class TTextActor : TActor
    {
        public string text { get; set; }
        public Font font { get; set; }
        public Color color { get; set; }

        protected SizeF BoxSize;
        public SizeF boxSize { get { return BoxSize; } set { BoxSize = value; refreshMatrix(); } }

        public TTextActor(TDocument doc)
            : base(doc)
        {
            this.text = "";
            this.font = new Font("Arial", 12);
            this.color = Color.Black;

            this.BoxSize = new SizeF();
        }

        public TTextActor(TDocument doc, string text, float x, float y, float width, float height, TLayer parent, string actorName)
            : base(doc, x, y, parent, actorName)
        {
            this.text = text;
            this.font = new Font("Arial", 12);
            this.color = Color.Black;

            this.BoxSize = new SizeF(width, height);
            this.refreshMatrix();
        }

        protected override void clone(TLayer target)
        {
            base.clone(target);

            TTextActor targetLayer = (TTextActor)target;
            targetLayer.text = this.text;
            targetLayer.font = (Font)this.font.Clone();
            targetLayer.color = this.color;
            targetLayer.BoxSize = this.boxSize;
            refreshMatrix();
        }

        public override bool parseXml(XElement xml, TLayer parentLayer)
        {
            if (xml == null || xml.Name != "TextActor")
                return false;

            if (!base.parseXml(xml, parentLayer))
                return false;

            try {
                text = xml.Element("Text").Value;
                FontFamily family = Program.findFontFamily(xml.Element("FontFamilyName").Value);
                float fontSize = TUtil.parseFloatXElement(xml.Element("FontSize"), 12);
                FontStyle fontStyle = (FontStyle)TUtil.parseIntXElement(xml.Element("FontStyle"), 0);
                if (family != null)
                    font = new Font(family, fontSize, fontStyle, GraphicsUnit.Point);
                else
                    font = new Font("Arial", fontSize, fontStyle, GraphicsUnit.Point);
                color = Color.FromArgb(int.Parse(xml.Element("Color").Value));
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
            xml.Name = "TextActor";
            xml.Add(
                new XElement("Text", text),
                new XElement("FontFamilyName", font.Name),
                new XElement("FontSize", font.Size),
                new XElement("FontStyle", (int)font.Style),
                new XElement("Color", color.ToArgb()),
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
            // alpha
            float al = this.alphaFromScreen();

            if (al > 1e-10) {  // alpha > 0
                // save graphics
                GraphicsState gs = g.Save();

                // apply matrix
                g.MultiplyTransform(matrix);

                // background
                g.FillRectangle(new SolidBrush(Color.FromArgb((int)(al * this.backgroundColor.A), this.backgroundColor)), this.bound());

                // draw text
                TScene scene = this.ownerScene();
                if (scene == null || scene.run_emulator == null || scene.run_emulator.textOn) {
                    if (1 - al < 1e-10) { // if alpha == 1
                        g.DrawString(this.text, this.font, new SolidBrush(this.color), this.bound());
                    } else {
                        g.DrawString(this.text, this.font, new SolidBrush(Color.FromArgb((int)(al * 255), this.color)), this.bound());
                    }
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

        public override RectangleF bound()
        {
            return new RectangleF(0, 0, BoxSize.Width, BoxSize.Height);
        }

    }
}
