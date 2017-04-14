using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder
{
    public partial class FrmNewProject : Form
    {
        public FrmNewProject()
        {
            InitializeComponent();
        }

        private void FrmNewProject_Load(object sender, EventArgs e)
        {
            txtLocation.Text = Program.getWorkspaceLocation();
            txtName.Text = "Untitled";
            txtIdentifier.Text = "com.examplecompany.examplebook";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtLocation.Text)) {
                MessageBox.Show("Location is not valid.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                txtLocation.Focus();
                return;
            }

            if (txtName.Text.Trim() == "") {
                MessageBox.Show("Please input the book name.\nThis will be the name of book folder.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                txtName.Focus();
                return;
            }

            if (txtIdentifier.Text.Trim() == "") {
                MessageBox.Show("Please input the book identifier.\nThis should be matched with the book identifier at the author site.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                txtIdentifier.Focus();
                return;
            }

            string path = txtLocation.Text + "\\" + txtName.Text;
            if (File.Exists(path) || Directory.Exists(path)) {
                MessageBox.Show("There is a already file or folder with the same name as the name you specified.\nPlease specify a different name.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                txtName.Focus();
                return;
            }

            Directory.CreateDirectory(path);
            Program.setWorkspaceLocation(txtLocation.Text);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            dlgLocation.SelectedPath = txtLocation.Text;
            if (dlgLocation.ShowDialog() == DialogResult.OK) {
                txtLocation.Text = dlgLocation.SelectedPath;
            }
        }
    }
}
