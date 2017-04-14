using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantChangeBGM : TActionInstant
    {
        public string sound { get; set; }
        public int volume { get; set; }

        public TActionInstantChangeBGM()
        {
            name = "Change BG Music";
            icon = Properties.Resources.icon_action_changebgm;

            sound = "";
            volume = 100;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantChangeBGM targetAction = (TActionInstantChangeBGM)target;
            targetAction.sound = this.sound;
            targetAction.volume = this.volume;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantChangeBGM")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                sound = xml.Element("Sound").Value;
                volume = TUtil.parseIntXElement(xml.Element("Volume"), 100);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionInstantChangeBGM";
            xml.Add(
                new XElement("Sound", sound),
                new XElement("Volume", volume)
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
            emulator.playBGM(sound, volume);

            return base.step(emulator, time);
        }

        #endregion
    }
}
