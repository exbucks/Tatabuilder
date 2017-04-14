using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TActionInstantClearAvatar : TActionInstant
    {
        public TActionInstantClearAvatar()
        {
            name = "Clear Avatar";
            icon = Properties.Resources.icon_action_clearavatar;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionInstantClearAvatar")
                return false;

            if (!base.parseXml(xml))
                return false;

            return true;
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionInstantClearAvatar";

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
            emulator.clearAvatar();

            return base.step(emulator, time);
        }

        #endregion
    }
}
