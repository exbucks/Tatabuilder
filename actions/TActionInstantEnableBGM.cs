using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantEnableBGM : TActionInstant
    {
        public bool enabled { get; set; }

        public TActionInstantEnableBGM()
        {
            name = "Ena./Dis. BG Music";
            icon = Properties.Resources.icon_action_enablebgm;

            enabled = false;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantEnableBGM targetAction = (TActionInstantEnableBGM)target;
            targetAction.enabled = this.enabled;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantEnableBGM")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                enabled = bool.Parse(xml.Element("Enabled").Value);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionInstantEnableBGM";
            xml.Add(
                new XElement("Enabled", enabled)
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
            emulator.toggleBGM(false, enabled);
            return base.step(emulator, time);
        }

        #endregion
    }
}
