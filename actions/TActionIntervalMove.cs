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
    public class TActionIntervalMove : TActionInterval
    {
        public enum ActionType { TO, BY };

        public ActionType type { get; set; }
        public PointF position { get; set; }
        public TEasingFunction.EasingType easingType { get; set; }
        public TEasingFunction.EasingMode easingMode { get; set; }

        [NonSerialized]
        private PointF run_startPos;
        [NonSerialized]
        private PointF run_endPos;
        [NonSerialized]
        private TEasingFunction run_easingFunction;

        public TActionIntervalMove()
        {
            name = "Move";
            startingColor = Color.FromArgb(205, 243, 255);
            endingColor = Color.FromArgb(188, 239, 255);
            icon = Properties.Resources.icon_action_move;

            type = ActionType.TO;
            position = new PointF();
            easingType = TEasingFunction.EasingType.None;
            easingMode = TEasingFunction.EasingMode.In;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionIntervalMove targetAction = (TActionIntervalMove)target;
            targetAction.type = this.type;
            targetAction.position = this.position;
            targetAction.easingType = this.easingType;
            targetAction.easingMode = this.easingMode;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionIntervalMove")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                type = (ActionType)int.Parse(xml.Element("Type").Value);
                position = new PointF(float.Parse(xml.Element("PositionX").Value), float.Parse(xml.Element("PositionY").Value));
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
            xml.Name = "ActionIntervalMove";
            xml.Add(
                new XElement("Type", (int)type),
                new XElement("PositionX", position.X),
                new XElement("PositionY", position.Y),
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
                run_startPos = target.position;
                if (type == ActionType.TO)
                    run_endPos = this.position;
                else if (type == ActionType.BY)
                    run_endPos = new PointF(target.position.X + this.position.X, target.position.Y + this.position.Y);
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
                float x = run_easingFunction.ease(easingType, easingMode, duration, elapsed, run_startPos.X, run_endPos.X);
                float y = run_easingFunction.ease(easingType, easingMode, duration, elapsed, run_startPos.Y, run_endPos.Y);
                target.position = new PointF(x, y);
            }

            return base.step(emulator, time);
        }

        #endregion
    }
}
