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
    public partial class ActionSettingInstantMakeAvatar : ActionSettingBasePanel
    {
        public ActionSettingInstantMakeAvatar()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // load action data
            // nothing

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                // nothing

                base.SaveData();
            }
        }
    }
}
