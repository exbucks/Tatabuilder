using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantStopAnimation : TActionInstant
    {
        public string actor { get; set; }
        public string eventu { get; set; }
        public string state { get; set; }

        public TActionInstantStopAnimation()
        {
            name = "Stop Animation";
            icon = Properties.Resources.icon_action_stop;

            actor = "";
            eventu = "";
            state = "";
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantStopAnimation targetAction = (TActionInstantStopAnimation)target;
            targetAction.actor = this.actor;
            targetAction.eventu = this.eventu;
            targetAction.state = this.state;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantStopAnimation")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                actor = xml.Element("Actor").Value;
                eventu = xml.Element("Event").Value;
                state = xml.Element("State").Value;
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionInstantStopAnimation";
            xml.Add(
                new XElement("Actor", actor),
                new XElement("Event", eventu),
                new XElement("State", state)
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
            TActor targetActor = (TActor)emulator.currentScene.findLayer(actor);
            if (targetActor != null) {
                targetActor.stopAnimation(eventu, state);
            }

            return base.step(emulator, time);
        }

        #endregion
    }
}
