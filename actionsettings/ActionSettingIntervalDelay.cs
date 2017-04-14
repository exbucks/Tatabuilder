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
    public partial class ActionSettingIntervalDelay : ActionSettingBasePanel
    {
        public ActionSettingIntervalDelay()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // load action data
            TActionIntervalDelay myAction = (TActionIntervalDelay)this.action;
            nudDuration.Value = (decimal)myAction.duration;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionIntervalDelay myAction = (TActionIntervalDelay)this.action;
                myAction.duration = (long)nudDuration.Value;

                base.SaveData();
            }
        }
    }
}
