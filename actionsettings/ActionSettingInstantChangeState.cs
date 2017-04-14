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
    public partial class ActionSettingInstantChangeState : ActionSettingBasePanel
    {
        public ActionSettingInstantChangeState()
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
            cmbState.Items.Clear();

            // fill combo box
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            if (dlg != null && dlg.document != null) {
                List<TLayer> actors = dlg.document.currentScene().getAllChilds();
                for (int i = 0; i < actors.Count; i++) {
                    cmbActor.Items.Add(actors[i].name);
                }
            }

            // load action data
            TActionInstantChangeState myAction = (TActionInstantChangeState)this.action;
            cmbActor.Text = myAction.actor;
            cmbState.Text = myAction.state;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionInstantChangeState myAction = (TActionInstantChangeState)this.action;

                myAction.actor = cmbActor.Text;
                myAction.state = cmbState.Text;

                base.SaveData();
            }
        }

        private void cmbActor_SelectedIndexChanged(object sender, EventArgs e)
        {
            // clear event combo
            cmbState.Items.Clear();
            cmbState.Text = "";

            // fill combo box
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            if (dlg != null && dlg.document != null) {
                TLayer layer = dlg.document.currentScene().findLayer(cmbActor.Text);
                string[] states = layer.getStates();
                for (int i = 0; i < states.Length; i++)
                    cmbState.Items.Add(states[i]);
            }

            // save modified data
            SaveData(sender, e);
        }
    }
}
