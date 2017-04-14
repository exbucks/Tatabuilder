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
    public partial class ActionSettingIntervalMove : ActionSettingBasePanel
    {
        public ActionSettingIntervalMove()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // load action data
            TActionIntervalMove myAction = (TActionIntervalMove)this.action;
            cmbType.SelectedIndex = (int)myAction.type;
            nudDuration.Value = (decimal)myAction.duration;
            nudPositionX.Value = (decimal)myAction.position.X;
            nudPositionY.Value = (decimal)myAction.position.Y;
            cmbEasingType.SelectedIndex = (int)myAction.easingType;
            cmbEasingMode.SelectedIndex = (int)myAction.easingMode;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionIntervalMove myAction = (TActionIntervalMove)this.action;
                myAction.type = (TActionIntervalMove.ActionType)cmbType.SelectedIndex;
                myAction.duration = (long)nudDuration.Value;
                myAction.position = new Point((int)nudPositionX.Value, (int)nudPositionY.Value);
                myAction.easingType = (TEasingFunction.EasingType)cmbEasingType.SelectedIndex;
                myAction.easingMode = (TEasingFunction.EasingMode)cmbEasingMode.SelectedIndex;

                base.SaveData();
            }
        }
    }
}
