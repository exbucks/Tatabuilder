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
    public partial class ActionSettingInstantEnableBGM : ActionSettingBasePanel
    {
        public ActionSettingInstantEnableBGM()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // load action data
            TActionInstantEnableBGM myAction = (TActionInstantEnableBGM)this.action;
            chkEnabled.Checked = myAction.enabled;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionInstantEnableBGM myAction = (TActionInstantEnableBGM)this.action;
                myAction.enabled = chkEnabled.Checked;

                base.SaveData();
            }
        }
    }
}
