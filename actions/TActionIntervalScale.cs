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
    public class TActionIntervalScale : TActionInterval
    {
        public enum ActionType { TO, BY };

        public ActionType type { get; set; }
        public SizeF scale { get; set; }
        public TEasingFunction.EasingType easingType { get; set; }
        public TEasingFunction.EasingMode easingMode { get; set; }

        [NonSerialized]
        private SizeF run_startScale;
        [NonSerialized]
        private SizeF run_endScale;
        [NonSerialized]
        private TEasingFunction run_easingFunction;

        public TActionIntervalScale()
        {
            name = "Scale";
            startingColor = Color.FromArgb(255, 247, 221);
            endingColor = Color.FromArgb(255, 245, 220);
            icon = Properties.Resources.icon_action_scale;

            type = ActionType.TO;
            scale = new SizeF(1, 1);
            easingType = TEasingFunction.EasingType.None;
            easingMode = TEasingFunction.EasingMode.In;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionIntervalScale targetAction = (TActionIntervalScale)target;
            targetAction.type = this.type;
            targetAction.scale = this.scale;
            targetAction.easingType = this.easingType;
            targetAction.easingMode = this.easingMode;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionIntervalScale")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                type = (ActionType)int.Parse(xml.Element("Type").Value);
                scale = new SizeF(float.Parse(xml.Element("ScaleWidth").Value), float.Parse(xml.Element("ScaleHeight").Value));
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
            xml.Name = "ActionIntervalScale";
            xml.Add(
                new XElement("Type", (int)type),
                new XElement("ScaleWidth", scale.Width),
                new XElement("ScaleHeight", scale.Height),
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
                run_startScale = target.scale;
                if (type == ActionType.TO)
                    run_endScale = this.scale;
                else if (type == ActionType.BY)
                    run_endScale = new SizeF(target.scale.Width * this.scale.Width, target.scale.Height * this.scale.Height);
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
                float width = run_easingFunction.ease(easingType, easingMode, duration, elapsed, run_startScale.Width, run_endScale.Width);
                float height = run_easingFunction.ease(easingType, easingMode, duration, elapsed, run_startScale.Height, run_endScale.Height);
                target.scale = new SizeF(width, height);
            }

            return base.step(emulator, time);
        }

        #endregion
    }
}
