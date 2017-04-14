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
    public partial class ActionSettingInstantReadMode : ActionSettingBasePanel
    {
        public ActionSettingInstantReadMode()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // load action data
            TActionInstantReadMode myAction = (TActionInstantReadMode)this.action;
            if (myAction.type == TActionInstantReadMode.ActionType.READTOME)
                radReadToMe.Checked = true;
            else if (myAction.type == TActionInstantReadMode.ActionType.READBYMYSELF)
                radReadByMyself.Checked = true;
            else if (myAction.type == TActionInstantReadMode.ActionType.AUTOPLAY)
                radAutoplay.Checked = true;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionInstantReadMode myAction = (TActionInstantReadMode)this.action;
                if (radReadToMe.Checked)
                    myAction.type = TActionInstantReadMode.ActionType.READTOME;
                else if (radReadByMyself.Checked)
                    myAction.type = TActionInstantReadMode.ActionType.READBYMYSELF;
                else if (radAutoplay.Checked)
                    myAction.type = TActionInstantReadMode.ActionType.AUTOPLAY;

                base.SaveData();
            }
        }
    }
}
