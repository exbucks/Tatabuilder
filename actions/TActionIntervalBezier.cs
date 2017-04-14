using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionIntervalBezier : TActionInterval
    {
        public enum ActionType { TO, BY };

        public ActionType type { get; set; }
        public Point point1 { get; set; }
        public Point point2 { get; set; }
        public Point point3 { get; set; }
        public TEasingFunction.EasingType easingType { get; set; }
        public TEasingFunction.EasingMode easingMode { get; set; }

        [NonSerialized]
        private PointF run_point0;
        [NonSerialized]
        private PointF run_point1;
        [NonSerialized]
        private PointF run_point2;
        [NonSerialized]
        private PointF run_point3;
        [NonSerialized]
        private TEasingFunction run_easingFunction;

        public TActionIntervalBezier()
        {
            name = "Bezier";
            startingColor = Color.FromArgb(255, 230, 205);
            endingColor = Color.FromArgb(255, 222, 189);
            icon = Properties.Resources.icon_action_bezier;

            type = ActionType.TO;
            point1 = new Point();
            point2 = new Point();
            point3 = new Point();
            easingType = TEasingFunction.EasingType.None;
            easingMode = TEasingFunction.EasingMode.In;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionIntervalBezier targetAction = (TActionIntervalBezier)target;
            targetAction.type = this.type;
            targetAction.point1 = this.point1;
            targetAction.point2 = this.point2;
            targetAction.point3 = this.point3;
            targetAction.easingType = this.easingType;
            targetAction.easingMode = this.easingMode;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionIntervalBezier")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                type = (ActionType)int.Parse(xml.Element("Type").Value);
                point1 = new Point(int.Parse(xml.Element("Point1X").Value), int.Parse(xml.Element("Point1Y").Value));
                point2 = new Point(int.Parse(xml.Element("Point2X").Value), int.Parse(xml.Element("Point2Y").Value));
                point3 = new Point(int.Parse(xml.Element("Point3X").Value), int.Parse(xml.Element("Point3Y").Value));
                easingType = (TEasingFunction.EasingType)int.Parse(xml.Element("EasingType").Value);
                easingMode = (TEasingFunction.EasingMode)int.Parse(xml.Element("EasingMode").Value);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionIntervalBezier";
            xml.Add(
                new XElement("Type", (int)type),
                new XElement("Point1X", point1.X),
                new XElement("Point1Y", point1.Y),
                new XElement("Point2X", point2.X),
                new XElement("Point2Y", point2.Y),
                new XElement("Point3X", point3.X),
                new XElement("Point3Y", point3.Y),
                new XElement("EasingType", (int)easingType),
                new XElement("EasingMode", (int)easingMode)
            );

            return xml;
        }

        #region Launch Methods

        public override void reset(long time)
        {
            base.reset(time);

            TLayer layer = sequence.animation.layer;
            if (layer is TActor) {
                TActor target = (TActor)layer;
                if (type == ActionType.TO) {
                    run_point0 = target.position;
                    run_point1 = this.point1;
                    run_point2 = this.point2;
                    run_point3 = this.point3;
                } else if (type == ActionType.BY) {
                    run_point0 = target.position;
                    run_point1 = new PointF(target.position.X + this.point1.X, target.position.Y + this.point1.Y);
                    run_point2 = new PointF(target.position.X + this.point2.X, target.position.Y + this.point2.Y);
                    run_point3 = new PointF(target.position.X + this.point3.X, target.position.Y + this.point3.Y);
                }
                run_easingFunction = new TEasingFunction();
            }
        }

        // execute action for every frame
        // if action is finished, return true;
        public override bool step(FrmEmulator emulator, long time)
        {
            float elapsed = time - run_startTime;
            if (elapsed > duration)
                elapsed = duration;

            TLayer layer = sequence.animation.layer;
            if (layer is TActor) {
                TActor target = (TActor)layer;
                float t = run_easingFunction.ease(easingType, easingMode, duration, elapsed, 0, 1);
                target.position = bezier(t);
            }

            return base.step(emulator, time);
        }

        private PointF bezier(float t)
        {
            double x = Math.Pow(1 - t, 3) * run_point0.X + 3 * Math.Pow(1 - t, 2) * t * run_point1.X + 3 * (1 - t) * Math.Pow(t, 2) * run_point2.X + Math.Pow(t, 3) * run_point3.X;
            double y = Math.Pow(1 - t, 3) * run_point0.Y + 3 * Math.Pow(1 - t, 2) * t * run_point1.Y + 3 * (1 - t) * Math.Pow(t, 2) * run_point2.Y + Math.Pow(t, 3) * run_point3.Y;
            return new PointF((float)x, (float)y);
        }

        #endregion
    }
}
