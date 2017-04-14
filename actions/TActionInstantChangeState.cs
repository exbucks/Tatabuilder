using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantChangeState : TActionInstant
    {
        public string actor { get; set; }
        public string state { get; set; }

        public TActionInstantChangeState()
        {
            name = "Change State";
            icon = Properties.Resources.icon_action_changestate;

            actor = "";
            state = "";
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantChangeState targetAction = (TActionInstantChangeState)target;
            targetAction.actor = this.actor;
            targetAction.state = this.state;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantChangeState")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                actor = xml.Element("Actor").Value;
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
            xml.Name = "ActionInstantChangeState";
            xml.Add(
                new XElement("Actor", actor),
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
            if (targetActor != null)
                targetActor.run_state = state;

            return base.step(emulator, time);
        }

        #endregion
    }
}
