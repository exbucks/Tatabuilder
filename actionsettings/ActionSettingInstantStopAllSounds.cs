using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder.actionsettings
{
    public partial class ActionSettingInstantStopAllSounds : ActionSettingBasePanel
    {
        public ActionSettingInstantStopAllSounds()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // load action data
            TActionInstantStopAllSounds myAction = (TActionInstantStopAllSounds)this.action;
            chkBGM.Checked      = myAction.bgm;
            chkEffect.Checked   = myAction.effect;
            chkVoice.Checked    = myAction.voice;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionInstantStopAllSounds myAction = (TActionInstantStopAllSounds)this.action;
                myAction.bgm    = chkBGM.Checked;
                myAction.effect = chkEffect.Checked;
                myAction.voice  = chkVoice.Checked;

                base.SaveData();
            }
        }
    }
}
