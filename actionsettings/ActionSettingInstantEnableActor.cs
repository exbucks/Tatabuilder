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
    public partial class ActionSettingInstantEnableActor : ActionSettingBasePanel
    {
        public ActionSettingInstantEnableActor()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // clear combo boxes
            cmbActor.Items.Clear();

            // fill combo box
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            if (dlg != null && dlg.document != null) {
                List<TLayer> actors = dlg.document.currentScene().getAllChilds();
                for (int i = 0; i < actors.Count; i++) {
                    cmbActor.Items.Add(actors[i].name);
                }
            }

            // load action data
            TActionInstantEnableActor myAction = (TActionInstantEnableActor)this.action;
            cmbActor.SelectedIndex = cmbActor.FindString(myAction.actor);
            chkEnabled.Checked = myAction.enabled;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionInstantEnableActor myAction = (TActionInstantEnableActor)this.action;
                myAction.actor = cmbActor.Text;
                myAction.enabled = chkEnabled.Checked;

                base.SaveData();
            }
        }
    }
}
