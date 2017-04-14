using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder
{
    public partial class FrmSignIn : Form
    {
        public FrmSignIn() {
            InitializeComponent();
        }

        private void FrmSignIn_Load(object sender, EventArgs e)
        {
            // Create a new collection if it was not serialized before.
            if (Properties.Settings.Default.RecentAuthors == null) {
                Properties.Settings.Default.RecentAuthors = new StringCollection();
            }

            // create the recently used document list
            foreach (string author in Properties.Settings.Default.RecentAuthors)
                cmbUserName.Items.Add(author);
        }

        private void btnOK_Click(object sender, EventArgs e) {

            this.DialogResult = DialogResult.None;

            if (String.IsNullOrEmpty(cmbUserName.Text.Trim())) {
                MessageBox.Show("Email cannot be empty.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbUserName.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtPassword.Text)) {
                MessageBox.Show("Password cannot be empty.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }

            try {
                var values = new NameValueCollection {
                    { "username", cmbUserName.Text },
                    { "password", txtPassword.Text }
                };

                byte[] response = Program.webClient.UploadValues(Program.URL_LOGIN, "POST", values);
                string responseString = Encoding.ASCII.GetString(response);

                try {
                    dynamic result = JObject.Parse(responseString);
                    if (String.Compare(result.status.Value, "Success", true) == 0) {
                        // success
                        updateRecentAuthor(cmbUserName.Text);

                        // save signed variables
                        Program.signed = true;
                        Program.author = result.user.Value;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    } else {
                        MessageBox.Show("Could not sign in. Please check your credential.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Could not sign in. Please check your credential.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Could not connect the server. Please check your internet connection.", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
            }
        }

        private void updateRecentAuthor(string user)
        {
            int i = Properties.Settings.Default.RecentAuthors.IndexOf(user);
            if (i >= 0) {
                Properties.Settings.Default.RecentAuthors.RemoveAt(i);
            }

            Properties.Settings.Default.RecentAuthors.Insert(0, user);
            Properties.Settings.Default.Save();
        }
    }
}
