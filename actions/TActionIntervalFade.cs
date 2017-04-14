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
    public class TActionIntervalFade : TActionInterval
    {
        public enum ActionType { TO, FROMTO, IN, OUT };

        public ActionType type { get; set; }
        public float startAlpha { get; set; }
        public float endAlpha { get; set; }
        public TEasingFunction.EasingType easingType { get; set; }
        public TEasingFunction.EasingMode easingMode { get; set; }

        [NonSerialized]
        private float run_startAlpha;
        [NonSerialized]
        private float run_endAlpha;
        [NonSerialized]
        private TEasingFunction run_easingFunction;

        public TActionIntervalFade()
        {
            name = "Fade";
            startingColor = Color.FromArgb(226, 226, 226);
            endingColor = Color.FromArgb(216, 216, 216);
            icon = Properties.Resources.icon_action_fade;

            type = ActionType.TO;
            startAlpha = 0;
            endAlpha = 1;
            easingType = TEasingFunction.EasingType.None;
            easingMode = TEasingFunction.EasingMode.In;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionIntervalFade targetAction = (TActionIntervalFade)target;
            targetAction.type = this.type;
            targetAction.startAlpha = this.startAlpha;
            targetAction.endAlpha = this.endAlpha;
            targetAction.easingType = this.easingType;
            targetAction.easingMode = this.easingMode;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionIntervalFade")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                type = (ActionType)int.Parse(xml.Element("Type").Value);
                startAlpha = float.Parse(xml.Element("StartAlpha").Value);
                endAlpha = float.Parse(xml.Element("EndAlpha").Value);
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
            xml.Name = "ActionIntervalFade";
            xml.Add(
                new XElement("Type", (int)type),
                new XElement("StartAlpha", startAlpha),
                new XElement("EndAlpha", endAlpha),
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
            switch (type) {
                case ActionType.TO:
                    run_startAlpha = layer.alpha;
                    run_endAlpha = this.endAlpha;
                    break;
                case ActionType.FROMTO:
                    run_startAlpha = this.startAlpha;
                    run_endAlpha = this.endAlpha;
                    break;
                case ActionType.IN:
                    run_startAlpha = layer.alpha;
                    run_endAlpha = 1;
                    break;
                case ActionType.OUT:
                    run_startAlpha = layer.alpha;
                    run_endAlpha = 0;
                    break;
            }
            run_easingFunction = new TEasingFunction();
        }

        // execute action for every frame
        // if action is finished, return true;
        public override bool step(FrmEmulator emulator, long time)
        {
            float elapsed = time - run_startTime;
            if (elapsed > duration)
                elapsed = duration;

            TLayer layer = sequence.animation.layer;
            layer.alpha = run_easingFunction.ease(easingType, easingMode, duration, elapsed, run_startAlpha, run_endAlpha);

            return base.step(emulator, time);
        }

        #endregion
    }
}
