using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder
{
    class ToggleButton : System.Windows.Forms.CheckBox
    {

        private Bitmap uncheckedImage;
        [Browsable(true), Category("Appearance"), Description("Bitmap for unchecked status")]
        public Bitmap UncheckedImage { get { return uncheckedImage; } set { uncheckedImage = value; this.Refresh(); } }
        
        private Bitmap checkedImage;
        [Browsable(true), Category("Appearance"), Description("Bitmap for checked status")]
        public Bitmap CheckedImage { get { return checkedImage; } set { checkedImage = value; this.Refresh(); } }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Image img = this.Checked ? this.CheckedImage : this.UncheckedImage;
            if (img != null) {
                int x = 0, y = 0;
                if (this.ImageAlign == ContentAlignment.TopLeft || this.ImageAlign == ContentAlignment.MiddleLeft || this.ImageAlign == ContentAlignment.BottomLeft)
                    x = 0;
                else if (this.ImageAlign == ContentAlignment.TopCenter || this.ImageAlign == ContentAlignment.MiddleCenter || this.ImageAlign == ContentAlignment.BottomCenter)
                    x = (this.Width - img.Width) / 2;
                else if (this.ImageAlign == ContentAlignment.TopRight || this.ImageAlign == ContentAlignment.MiddleRight || this.ImageAlign == ContentAlignment.BottomRight)
                    x = this.Width - img.Width;

                if (this.ImageAlign == ContentAlignment.TopLeft || this.ImageAlign == ContentAlignment.TopCenter || this.ImageAlign == ContentAlignment.TopRight)
                    y = 0;
                else if (this.ImageAlign == ContentAlignment.MiddleLeft || this.ImageAlign == ContentAlignment.MiddleCenter || this.ImageAlign == ContentAlignment.MiddleRight)
                    y = (this.Height - img.Height) / 2;
                else if (this.ImageAlign == ContentAlignment.BottomLeft || this.ImageAlign == ContentAlignment.BottomCenter || this.ImageAlign == ContentAlignment.BottomRight)
                    y = this.Height - img.Height;

                g.DrawImage(img, x, y, img.Width, img.Height);
            }
        }
    }
}
