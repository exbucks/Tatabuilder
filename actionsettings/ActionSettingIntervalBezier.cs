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
    public partial class ActionSettingIntervalBezier : ActionSettingBasePanel
    {
        public ActionSettingIntervalBezier()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // load action data
            TActionIntervalBezier myAction = (TActionIntervalBezier)this.action;
            cmbType.SelectedIndex = (int)myAction.type;
            nudDuration.Value = (decimal)myAction.duration;
            nudControlPoint1X.Value = (decimal)myAction.point1.X;
            nudControlPoint1Y.Value = (decimal)myAction.point1.Y;
            nudControlPoint2X.Value = (decimal)myAction.point2.X;
            nudControlPoint2Y.Value = (decimal)myAction.point2.Y;
            nudControlPoint3X.Value = (decimal)myAction.point3.X;
            nudControlPoint3Y.Value = (decimal)myAction.point3.Y;
            cmbEasingType.SelectedIndex = (int)myAction.easingType;
            cmbEasingMode.SelectedIndex = (int)myAction.easingMode;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionIntervalBezier myAction = (TActionIntervalBezier)this.action;
                myAction.type = (TActionIntervalBezier.ActionType)cmbType.SelectedIndex;
                myAction.duration = (long)nudDuration.Value;
                myAction.point1 = new Point((int)nudControlPoint1X.Value, (int)nudControlPoint1Y.Value);
                myAction.point2 = new Point((int)nudControlPoint2X.Value, (int)nudControlPoint2Y.Value);
                myAction.point3 = new Point((int)nudControlPoint3X.Value, (int)nudControlPoint3Y.Value);
                myAction.easingType = (TEasingFunction.EasingType)cmbEasingType.SelectedIndex;
                myAction.easingMode = (TEasingFunction.EasingMode)cmbEasingMode.SelectedIndex;

                base.SaveData();
            }
        }
    }
}
