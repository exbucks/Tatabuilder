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
    public class TActionIntervalDelay : TActionInterval
    {
        public TActionIntervalDelay()
        {
            name = "Delay";
            startingColor = Color.FromArgb(255, 222, 222);
            endingColor = Color.FromArgb(255, 212, 212);
            icon = Properties.Resources.icon_action_delay;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionIntervalDelay")
                return false;

            if (!base.parseXml(xml))
                return false;

            return true;
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionIntervalDelay";

            return xml;
        }

        #region Launch Methods

        public override void reset(long time)
        {
            base.reset(time);
        }

        // execute action for every frame
        // if action is finished, return true;
        public override bool step(FrmEmulator emulator, long time)
        {
            return base.step(emulator, time);
        }

        #endregion
    }
}
