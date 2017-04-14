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
    public partial class ActionSettingIntervalRotate : ActionSettingBasePanel
    {
        public ActionSettingIntervalRotate()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // load action data
            TActionIntervalRotate myAction = (TActionIntervalRotate)this.action;
            cmbType.SelectedIndex = (int)myAction.type;
            nudDuration.Value = (decimal)myAction.duration;
            nudAngle.Value = (decimal)myAction.angle;
            cmbEasingType.SelectedIndex = (int)myAction.easingType;
            cmbEasingMode.SelectedIndex = (int)myAction.easingMode;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionIntervalRotate myAction = (TActionIntervalRotate)this.action;
                myAction.type = (TActionIntervalRotate.ActionType)cmbType.SelectedIndex;
                myAction.duration = (long)nudDuration.Value;
                myAction.angle = (int)nudAngle.Value;
                myAction.easingType = (TEasingFunction.EasingType)cmbEasingType.SelectedIndex;
                myAction.easingMode = (TEasingFunction.EasingMode)cmbEasingMode.SelectedIndex;

                base.SaveData();
            }
        }
    }
}
