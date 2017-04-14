using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantGoScene : TActionInstant
    {
        public enum ActionType { PREVIOUS, NEXT, COVER, SPECIFIC };

        public ActionType type { get; set; }
        public string scene { get; set; }

        public TActionInstantGoScene()
        {
            name = "Go to Scene";
            icon = Properties.Resources.icon_action_gotoscene;

            type = ActionType.SPECIFIC;
            scene = "";
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantGoScene targetAction = (TActionInstantGoScene)target;
            targetAction.type = this.type;
            targetAction.scene = this.scene;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantGoScene")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                type = (ActionType)int.Parse(xml.Element("Type").Value);
                scene = xml.Element("Scene").Value;
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionInstantGoScene";
            xml.Add(
                new XElement("Type", (int)type),
                new XElement("Scene", scene)
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
            switch (type) {
                case ActionType.PREVIOUS:
                    emulator.gotoPrevScene();
                    break;
                case ActionType.NEXT:
                    emulator.gotoNextScene();
                    break;
                case ActionType.COVER:
                    emulator.gotoCoverScene();
                    break;
                case ActionType.SPECIFIC:
                    emulator.gotoSpecificScene(scene);
                    break;
            }

            return base.step(emulator, time);
        }

        #endregion
    }
}
