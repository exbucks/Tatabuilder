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
    public partial class ActionSettingInstantStopAnimation : ActionSettingBasePanel
    {
        public ActionSettingInstantStopAnimation()
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
            cmbAnimation.Items.Clear();

            // fill combo box
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            if (dlg != null && dlg.document != null) {
                List<TLayer> actors = dlg.document.currentScene().getAllChilds();
                for (int i = 0; i < actors.Count; i++) {
                    cmbActor.Items.Add(actors[i].name);
                }
            }

            // load action data
            TActionInstantStopAnimation myAction = (TActionInstantStopAnimation)this.action;
            cmbActor.Text = myAction.actor;
            if (myAction.eventu == "" || myAction.state == "")
                cmbAnimation.Text = "";
            else
                cmbAnimation.Text = myAction.eventu + " - " + myAction.state;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            TLayer layer = dlg.document.currentScene().findLayer(cmbActor.Text);

            if (manualChanged == false) {
                TActionInstantStopAnimation myAction = (TActionInstantStopAnimation)this.action;

                myAction.actor = cmbActor.Text;
                if (cmbAnimation.SelectedIndex != -1) {
                    myAction.eventu = layer.animations[cmbAnimation.SelectedIndex].eventu;
                    myAction.state = layer.animations[cmbAnimation.SelectedIndex].state;
                } else {
                    myAction.eventu = "";
                    myAction.state = "";
                }

                base.SaveData();
            }
        }

        private void cmbActor_SelectedIndexChanged(object sender, EventArgs e)
        {
            // clear event combo
            cmbAnimation.Items.Clear();
            cmbAnimation.Text = "";

            // fill combo box
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            if (dlg != null && dlg.document != null) {
                TLayer layer = dlg.document.currentScene().findLayer(cmbActor.Text);
                for (int i = 0; i < layer.animations.Count; i++) {
                    cmbAnimation.Items.Add(layer.animations[i].eventu + " - " + layer.animations[i].state);
                }
            }

            // save modified data
            SaveData(sender, e);
        }
    }
}
