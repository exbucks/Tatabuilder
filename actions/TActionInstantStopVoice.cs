using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantStopVoice : TActionInstant
    {
        public string sound { get; set; }

        public TActionInstantStopVoice()
        {
            name = "Stop Voice";
            icon = Properties.Resources.icon_action_stopvoice;

            sound = "";
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantStopVoice targetAction = (TActionInstantStopVoice)target;
            targetAction.sound = this.sound;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantStopVoice")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                sound = xml.Element("Sound").Value;
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionInstantStopVoice";
            xml.Add(
                new XElement("Sound", sound)
            );

            return xml;
        }

        public override bool isUsingSound(string snd)
        {
            return sound.Equals(snd);
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
            emulator.stopVoice(sound);

            return base.step(emulator, time);
        }

        #endregion
    }
}
