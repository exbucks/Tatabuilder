using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantDispatchEvent : TActionInstant
    {
        public string actor { get; set; }
        public string eventu { get; set; }
        public bool recursive { get; set; }

        public TActionInstantDispatchEvent()
        {
            name = "Dispatch Event";
            icon = Properties.Resources.icon_action_dispatch;

            actor = "";
            eventu = "";
            recursive = true;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantDispatchEvent targetAction = (TActionInstantDispatchEvent)target;
            targetAction.actor = this.actor;
            targetAction.eventu = this.eventu;
            targetAction.recursive = this.recursive;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantDispatchEvent")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                actor = xml.Element("Actor").Value;
                eventu = xml.Element("Event").Value;
                recursive = bool.Parse(xml.Element("Recursive").Value);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionInstantDispatchEvent";
            xml.Add(
                new XElement("Actor", actor),
                new XElement("Event", eventu),
                new XElement("Recursive", recursive)
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
            if (targetActor != null)
                targetActor.fireEvent(eventu, recursive);

            return base.step(emulator, time);
        }

        #endregion
    }
}
