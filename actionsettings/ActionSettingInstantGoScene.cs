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
    public partial class ActionSettingInstantGoScene : ActionSettingBasePanel
    {
        public ActionSettingInstantGoScene()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // clear combo boxes
            cmbScene.Items.Clear();

            // fill combo box
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            if (dlg != null && dlg.document != null) {
                for (int i = 0; i < dlg.document.sceneManager.sceneCount(); i++) {
                    cmbScene.Items.Add(dlg.document.sceneManager.scene(i).name);
                }
            }

            // load action data
            TActionInstantGoScene myAction = (TActionInstantGoScene)this.action;
            if (myAction.type == TActionInstantGoScene.ActionType.PREVIOUS) {
                radPreviousScene.Checked = true;
                cmbScene.Enabled = false;
            } else if (myAction.type == TActionInstantGoScene.ActionType.NEXT) {
                radNextScene.Checked = true;
                cmbScene.Enabled = false;
            } else if (myAction.type == TActionInstantGoScene.ActionType.COVER) {
                radCoverScene.Checked = true;
                cmbScene.Enabled = false;
            } else if (myAction.type == TActionInstantGoScene.ActionType.SPECIFIC) {
                radSpecificScene.Checked = true;
                cmbScene.Enabled = true;
            }

            cmbScene.Text = myAction.scene;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionInstantGoScene myAction = (TActionInstantGoScene)this.action;

                if (radPreviousScene.Checked) {
                    myAction.type = TActionInstantGoScene.ActionType.PREVIOUS;
                    cmbScene.Enabled = false;
                } else if (radNextScene.Checked) {
                    myAction.type = TActionInstantGoScene.ActionType.NEXT;
                    cmbScene.Enabled = false;
                } else if (radCoverScene.Checked) {
                    myAction.type = TActionInstantGoScene.ActionType.COVER;
                    cmbScene.Enabled = false;
                } else if (radSpecificScene.Checked) {
                    myAction.type = TActionInstantGoScene.ActionType.SPECIFIC;
                    cmbScene.Enabled = true;
                }

                myAction.scene = cmbScene.Text;

                base.SaveData();
            }
        }
    }
}
