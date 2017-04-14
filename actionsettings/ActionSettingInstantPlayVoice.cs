using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TataBuilder.actionsettings
{
    public partial class ActionSettingInstantPlayVoice : ActionSettingBasePanel
    {
        public ActionSettingInstantPlayVoice()
        {
            InitializeComponent();
        }

        private bool manualChanged = false;

        public override void LoadData()
        {
            // set manualChanged flag
            manualChanged = true;

            // clear combo box
            cmbSound.Items.Clear();

            // fill combo box
            FrmAnimationTimeline dlg = this.findAncestorControl(typeof(FrmAnimationTimeline)) as FrmAnimationTimeline;
            if (dlg != null && dlg.document != null) {
                TLibraryManager libraryManager = dlg.document.libraryManager;
                for (int i = 0; i < libraryManager.soundCount(); i++) {
                    cmbSound.Items.Add(libraryManager.soundFileName(i));
                }
            }

            // load action data
            TActionInstantPlayVoice myAction = (TActionInstantPlayVoice)this.action;
            cmbSound.Text = myAction.sound;
            chkLoop.Checked = myAction.loop;

            // clear mnualChanged flag
            manualChanged = false;
        }

        private void SaveData(object sender, EventArgs e)
        {
            if (manualChanged == false) {
                TActionInstantPlayVoice myAction = (TActionInstantPlayVoice)this.action;
                myAction.sound = cmbSound.Text;
                myAction.loop = chkLoop.Checked;

                base.SaveData();
            }
        }
    }
}
