using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantStopAllSounds : TActionInstant
    {
        public bool bgm { get; set; }
        public bool effect { get; set; }
        public bool voice { get; set; }

        public TActionInstantStopAllSounds()
        {
            name = "Stop All Sounds";
            icon = Properties.Resources.icon_action_stopallsounds;

            bgm = true;
            effect = true;
            voice = true;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantStopAllSounds targetAction = (TActionInstantStopAllSounds)target;
            targetAction.bgm = this.bgm;
            targetAction.effect = this.effect;
            targetAction.voice = this.voice;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantStopAllSounds")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                bgm = TUtil.parseBoolXElement(xml.Element("BGM"), true);
                effect = TUtil.parseBoolXElement(xml.Element("Effect"), true);
                voice = TUtil.parseBoolXElement(xml.Element("Voice"), true);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionInstantStopAllSounds";
            xml.Add(
                new XElement("BGM", bgm),
                new XElement("Effect", effect),
                new XElement("Voice", voice)
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
            emulator.stopAllSounds(bgm, effect, voice);

            return base.step(emulator, time);
        }

        #endregion
    }
}
