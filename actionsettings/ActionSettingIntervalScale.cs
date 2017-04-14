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
    public partial class ActionSettingIntervalScale : ActionSettingBasePanel
    {
        public ActionSettingIntervalScale()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // load action data
            TActionIntervalScale myAction = (TActionIntervalScale)this.action;
            cmbType.SelectedIndex = (int)myAction.type;
            nudDuration.Value = (decimal)myAction.duration;
            nudScaleX.Value = (decimal)myAction.scale.Width;
            nudScaleY.Value = (decimal)myAction.scale.Height;
            cmbEasingType.SelectedIndex = (int)myAction.easingType;
            cmbEasingMode.SelectedIndex = (int)myAction.easingMode;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionIntervalScale myAction = (TActionIntervalScale)this.action;
                myAction.type = (TActionIntervalScale.ActionType)cmbType.SelectedIndex;
                myAction.duration = (long)nudDuration.Value;
                myAction.scale = new SizeF((float)nudScaleX.Value, (float)nudScaleY.Value);
                myAction.easingType = (TEasingFunction.EasingType)cmbEasingType.SelectedIndex;
                myAction.easingMode = (TEasingFunction.EasingMode)cmbEasingMode.SelectedIndex;

                base.SaveData();
            }
        }
    }
}
