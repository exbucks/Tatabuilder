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
    public partial class ActionSettingInstantZIndex : ActionSettingBasePanel
    {
        public ActionSettingInstantZIndex()
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
            TActionInstantZIndex myAction = (TActionInstantZIndex)this.action;
            cmbActor.Text = myAction.actor;
            nudZIndex.Value = (decimal)myAction.zIndex;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionInstantZIndex myAction = (TActionInstantZIndex)this.action;
                myAction.actor = cmbActor.Text;
                myAction.zIndex = (int)nudZIndex.Value;

                base.SaveData();
            }
        }
    }
}
