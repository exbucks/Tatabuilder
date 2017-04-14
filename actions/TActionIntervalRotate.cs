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
    public class TActionIntervalRotate : TActionInterval
    {
        public enum ActionType { TO, BY };

        public ActionType type { get; set; }
        public int angle { get; set; }                // rotation angle in degree
        public TEasingFunction.EasingType easingType { get; set; }
        public TEasingFunction.EasingMode easingMode { get; set; }

        [NonSerialized]
        private float run_startAngle;
        [NonSerialized]
        private float run_endAngle;
        [NonSerialized]
        private TEasingFunction run_easingFunction;

        public TActionIntervalRotate()
        {
            name = "Rotate";
            startingColor = Color.FromArgb(221, 255, 249);
            endingColor = Color.FromArgb(209, 255, 247);
            icon = Properties.Resources.icon_action_rotate;

            type = ActionType.TO;
            angle = 0;
            easingType = TEasingFunction.EasingType.None;
            easingMode = TEasingFunction.EasingMode.In;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionIntervalRotate targetAction = (TActionIntervalRotate)target;
            targetAction.type = this.type;
            targetAction.angle = this.angle;
            targetAction.easingType = this.easingType;
            targetAction.easingMode = this.easingMode;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionIntervalRotate")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                type = (ActionType)int.Parse(xml.Element("Type").Value);
                angle = int.Parse(xml.Element("Angle").Value);
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
            xml.Name = "ActionIntervalRotate";
            xml.Add(
                new XElement("Type", (int)type),
                new XElement("Angle", angle),
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
                run_startAngle = target.rotation;
                if (type == ActionType.TO)
                    run_endAngle = this.angle;
                else if (type == ActionType.BY)
                    run_endAngle = target.rotation + this.angle;
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
                target.rotation = TUtil.normalizeDegreeAngle(run_easingFunction.ease(easingType, easingMode, duration, elapsed, run_startAngle, run_endAngle));
            }

            return base.step(emulator, time);
        }

        #endregion
    }
}
