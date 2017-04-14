using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TImageActor : TActor
    {
        // image file name
        public string image { get; set; }

        // Image object to load and draw this actor
        protected Image ImgTexture;

        public TImageActor(TDocument doc)
            : base(doc)
        {
            image = "";
            ImgTexture = null;
        }

        public TImageActor(TDocument doc, string img, float x, float y, TLayer parent, string actorName) 
            : base(doc, x, y, parent, actorName)
        {
            // save file path
            image = img;

            // load image
            this.loadImage();

            this.refreshMatrix();
        }

        public TImageActor(TDocument doc, Image texture, float x, float y, TLayer parent, string actorName)
            : base(doc, x, y, parent, actorName)
        {
            // save file path
            image = "";

            // load image
            this.loadImage(texture);

            this.refreshMatrix();
        }

        protected override void clone(TLayer target)
        {
            base.clone(target);

            TImageActor targetLayer = (TImageActor)target;
            targetLayer.image = this.image;
            targetLayer.loadImage();
            targetLayer.refreshMatrix();
        }

        public override bool parseXml(XElement xml, TLayer parentLayer)
        {
            if (xml == null || xml.Name != "ImageActor")
                return false;

            if (!base.parseXml(xml, parentLayer))
                return false;

            try {
                image = xml.Element("Image").Value;
                loadImage();
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
            xml.Name = "ImageActor";
            xml.Add(
                new XElement("Image", image)
            );

            return xml;
        }

        public void loadImage(Image img)
        {
            ImgTexture = img;
            refreshMatrix();
        }

        public void loadImage()
        {
            try {
                ImgTexture = Image.FromFile(document.libraryManager.imageFilePath(document.libraryManager.imageIndex(image)));
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            refreshMatrix();
        }

        public override void refreshMatrix()
        {
            matrix.Reset();
            matrix.Translate(Position.X, Position.Y);
            matrix.Rotate(Rotation);
            matrix.Scale(Scale.Width, Scale.Height);
            matrix.Shear(Skew.Width, Skew.Height);

            if (ImgTexture != null)
                matrix.Translate(-Anchor.X * ImgTexture.Width, -Anchor.Y * ImgTexture.Height);
        }

        public override void draw(Graphics g)
        {
            if (ImgTexture != null) {
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
                        g.DrawImage(ImgTexture, 0, 0, ImgTexture.Width, ImgTexture.Height);
                    } else {
                        ColorMatrix cm = new ColorMatrix();
                        cm.Matrix33 = al;
                        ImageAttributes ia = new ImageAttributes();
                        ia.SetColorMatrix(cm);
                        g.DrawImage(ImgTexture, new Rectangle(0, 0, ImgTexture.Width, ImgTexture.Height), 0, 0, ImgTexture.Width, ImgTexture.Height, GraphicsUnit.Pixel, ia);
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
            if (this.ImgTexture != null)
                return new RectangleF(0, 0, this.ImgTexture.Width, this.ImgTexture.Height);
            else
                return new RectangleF(0, 0, 0, 0);
        }

        public override bool isUsingImage(string img)
        {
            if (image.Equals(img))
                return true;

            return base.isUsingImage(img);
        }
    }
}
