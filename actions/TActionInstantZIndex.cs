using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantZIndex : TActionInstant
    {
        public string actor { get; set; }
        public int zIndex { get; set; }

        public TActionInstantZIndex()
        {
            name = "Z-Index Change";
            icon = Properties.Resources.icon_action_zindex;

            actor = "";
            zIndex = 0;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionInstantZIndex targetAction = (TActionInstantZIndex)target;
            targetAction.actor = this.actor;
            targetAction.zIndex = this.zIndex;
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantZIndex")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                actor = xml.Element("Actor").Value;
                zIndex = int.Parse(xml.Element("ZIndex").Value);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionInstantZIndex";
            xml.Add(
                new XElement("Actor", actor),
                new XElement("ZIndex", zIndex)
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
                targetActor.zIndex = zIndex;

            return base.step(emulator, time);
        }

        #endregion
    }
}
