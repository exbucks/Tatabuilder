using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public abstract class TAction
    {
        [NonSerialized]
        private TSequence _sequence;
        public TSequence sequence { get { return _sequence; } set { _sequence = value; }}

        public string name { get; set; }

        public bool isInstant { get; set; }

        // milliseconds
        public virtual long duration { get; set; }

        public Color startingColor { get; set; }

        public Color endingColor { get; set; }

        public Bitmap icon { get; set; }

        public Size iconFrame { get; set; }

        [NonSerialized]
        protected long run_startTime;

        public TAction()
        {
            this.sequence = null;
            this.name = "";
            this.isInstant = false;
            this.duration = 0;
            this.startingColor = Color.FromArgb(250, 250, 250);
            this.endingColor = Color.FromArgb(247, 247, 247);
            this.icon = null;
            this.iconFrame = new Size(37, 37);

            this.run_startTime = 0;
        }

        public virtual TAction clone()
        {
            TAction action = (TAction)Activator.CreateInstance(this.GetType());
            this.clone(action);

            return action;
        }

        protected virtual void clone(TAction target)
        {
            target.sequence = this.sequence;
            target.name = this.name;
            target.isInstant = this.isInstant;
            target.duration = this.duration;
            target.startingColor = this.startingColor;
            target.endingColor = this.endingColor;
            target.icon = this.icon;
            target.iconFrame = this.iconFrame;
        }

        public virtual bool parseXml(XElement xml)
        {
            if (xml == null)
                return false;

            try {
                duration = long.Parse(xml.Element("Duration").Value);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public virtual XElement toXml()
        {
            return
                new XElement("Action",
                    new XElement("Duration", duration)
                );
        }

        public virtual Bitmap iconWithFrame()
        {
            Bitmap bmp = new Bitmap(iconFrame.Width, iconFrame.Height);
            Graphics g = Graphics.FromImage(bmp);

            drawRoundedRectangle(g, new Rectangle(0, 0, iconFrame.Width, iconFrame.Height), 3, new Pen(Color.FromArgb(195, 195, 217)), startingColor, endingColor);

            if (icon != null)
                g.DrawImage(icon, iconFrame.Width / 2 - icon.Width / 2,iconFrame.Height / 2 - icon.Height / 2);

            g.Dispose();

            return bmp;
        }

        private void drawRoundedRectangle(Graphics gfx, Rectangle bounds, int cornerRadius, Pen drawPen, Color topColor, Color bottomColor)
        {
            int strokeOffset = Convert.ToInt32(Math.Ceiling(drawPen.Width));
            bounds = new Rectangle(bounds.X, bounds.Y, bounds.Width - strokeOffset, bounds.Height - strokeOffset);

            drawPen.EndCap = drawPen.StartCap = LineCap.Round;

            GraphicsPath gfxPath = new GraphicsPath();
            gfxPath.AddArc(bounds.X, bounds.Y, cornerRadius, cornerRadius, 180, 90);
            gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y, cornerRadius, cornerRadius, 270, 90);
            gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            gfxPath.AddArc(bounds.X, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            gfxPath.CloseAllFigures();

            LinearGradientBrush bgBrush = new LinearGradientBrush(bounds, topColor, bottomColor, LinearGradientMode.Vertical);
            gfx.FillPath(bgBrush, gfxPath);
            gfx.DrawPath(drawPen, gfxPath);
        }

        public virtual bool isUsingImage(string img)
        {
            return false;
        }

        public virtual bool isUsingSound(string snd)
        {
            return false;
        }

        #region Launch Methods

        public virtual void reset(long time)
        {
            run_startTime = time;
        }

        // execute action for every frame
        // if action is finished, return true;
        public virtual bool step(FrmEmulator emulator, long time)
        {
            if (isInstant || run_startTime + duration <= time)
                return true;
            return false;
        }

        #endregion
    }
}
