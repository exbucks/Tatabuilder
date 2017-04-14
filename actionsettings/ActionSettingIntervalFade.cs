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
    public partial class ActionSettingIntervalFade : ActionSettingBasePanel
    {
        public ActionSettingIntervalFade()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // load action data
            TActionIntervalFade myAction = (TActionIntervalFade)this.action;
            cmbType.SelectedIndex = (int)myAction.type;
            nudDuration.Value = (decimal)myAction.duration;
            nudStartAlpha.Value = (decimal)myAction.startAlpha;
            nudEndAlpha.Value = (decimal)myAction.endAlpha;
            cmbEasingType.SelectedIndex = (int)myAction.easingType;
            cmbEasingMode.SelectedIndex = (int)myAction.easingMode;

            cmbType_SelectedIndexChanged(null, null);

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionIntervalFade myAction = (TActionIntervalFade)this.action;
                myAction.type = (TActionIntervalFade.ActionType)cmbType.SelectedIndex;
                myAction.duration = (long)nudDuration.Value;
                myAction.startAlpha = (float)nudStartAlpha.Value;
                myAction.endAlpha = (float)nudEndAlpha.Value;
                myAction.easingType = (TEasingFunction.EasingType)cmbEasingType.SelectedIndex;
                myAction.easingMode = (TEasingFunction.EasingMode)cmbEasingMode.SelectedIndex;

                base.SaveData();
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveData(sender, e);

            TActionIntervalFade myAction = (TActionIntervalFade)this.action;
            switch (myAction.type) {
                case TActionIntervalFade.ActionType.TO:
                    pnlStartAlphaRow.Visible = false;
                    pnlEndAlphaRow.Visible = true;
                    break;
                case TActionIntervalFade.ActionType.FROMTO:
                    pnlStartAlphaRow.Visible = true;
                    pnlEndAlphaRow.Visible = true;
                    break;
                case TActionIntervalFade.ActionType.IN:
                    pnlStartAlphaRow.Visible = false;
                    pnlEndAlphaRow.Visible = false;
                    break;
                case TActionIntervalFade.ActionType.OUT:
                    pnlStartAlphaRow.Visible = false;
                    pnlEndAlphaRow.Visible = false;
                    break;
            }
        }
    }
}
