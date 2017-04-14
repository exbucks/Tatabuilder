using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantReadMode : TActionInstant
    {
        public enum ActionType { READTOME, READBYMYSELF, AUTOPLAY }

        public ActionType type;

        public TActionInstantReadMode()
        {
            name = "Read Mode";
            icon = Properties.Resources.icon_action_readmode;

            type = ActionType.READTOME;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantReadMode targetAction = (TActionInstantReadMode)target;
            targetAction.type = this.type;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantReadMode")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                type = (ActionType)int.Parse(xml.Element("Type").Value);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionInstantReadMode";
            xml.Add(
                new XElement("Type", (int)type)
            );

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
