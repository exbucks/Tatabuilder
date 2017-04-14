using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantEnableActor : TActionInstant
    {
        public string actor { get; set; }
        public bool enabled { get; set; }

        public TActionInstantEnableActor()
        {
            name = "Ena./Dis. Actor";
            icon = Properties.Resources.icon_action_enableactor;

            actor = "";
            enabled = false;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantEnableActor targetAction = (TActionInstantEnableActor)target;
            targetAction.actor = this.actor;
            targetAction.enabled = this.enabled;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantEnableActor")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                actor = xml.Element("Actor").Value;
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
            xml.Name = "ActionInstantEnableActor";
            xml.Add(
                new XElement("Actor", actor),
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
            TActor targetActor = (TActor)emulator.currentScene.findLayer(actor);
            if (targetActor != null) {
                targetActor.run_enabled = this.enabled;
            }

            return base.step(emulator, time);
        }

        #endregion
    }
}
