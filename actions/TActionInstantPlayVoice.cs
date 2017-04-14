using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantPlayVoice : TActionInstant
    {
        public string sound { get; set; }
        public int volume { get; set; }
        public bool loop { get; set; }

        public TActionInstantPlayVoice()
        {
            name = "Play Voice";
            icon = Properties.Resources.icon_action_playvoice;

            sound = "";
            volume = 100;
            loop = false;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantPlayVoice targetAction = (TActionInstantPlayVoice)target;
            targetAction.sound = this.sound;
            targetAction.volume = this.volume;
            targetAction.loop = this.loop;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantPlayVoice")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                sound = xml.Element("Sound").Value;
                volume = TUtil.parseIntXElement(xml.Element("Volume"), 100);
                loop = bool.Parse(xml.Element("Loop").Value);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionInstantPlayVoice";
            xml.Add(
                new XElement("Sound", sound),
                new XElement("Volume", volume),
                new XElement("Loop", loop)
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
            emulator.playVoice(sound, volume, loop);

            return base.step(emulator, time);
        }

        #endregion
    }
}
