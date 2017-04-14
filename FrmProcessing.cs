using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder
{
    public partial class FrmProcessing : Form
    {
        string message;

        public FrmProcessing(string message)
        {
            InitializeComponent();
            this.message = message;
        }

        private void FrmProcessing_Load(object sender, EventArgs e)
        {
            lblMessage.Text = message;
        }
    }
}
