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
    public partial class ActionSettingInstantDispatchEvent : ActionSettingBasePanel
    {
        public ActionSettingInstantDispatchEvent()
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
            cmbEvent.Items.Clear();

            // fill combo box
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            if (dlg != null && dlg.document != null) {
                List<TLayer> actors = dlg.document.currentScene().getAllChilds();
                for (int i = 0; i < actors.Count; i++) {
                    cmbActor.Items.Add(actors[i].name);
                }
            }

            // load action data
            TActionInstantDispatchEvent myAction = (TActionInstantDispatchEvent)this.action;
            cmbActor.Text = myAction.actor;
            cmbEvent.Text = myAction.eventu;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionInstantDispatchEvent myAction = (TActionInstantDispatchEvent)this.action;

                myAction.actor = cmbActor.Text;
                myAction.eventu = cmbEvent.Text;

                base.SaveData();
            }
        }

        private void cmbActor_SelectedIndexChanged(object sender, EventArgs e)
        {
            // clear event combo
            cmbEvent.Items.Clear();
            cmbEvent.Text = "";

            // fill combo box
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            if (dlg != null && dlg.document != null) {
                TLayer layer = dlg.document.currentScene().findLayer(cmbActor.Text);
                string[] events = layer.getEvents();
                for (int i = 0; i < events.Length; i++)
                    cmbEvent.Items.Add(events[i]);
            }

            // save modified data
            SaveData(sender, e);
        }
    }
}
