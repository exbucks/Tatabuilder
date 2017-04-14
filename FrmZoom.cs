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
    public partial class FrmZoom : Form
    {
        public float zoom { get; private set; }

        public FrmZoom(float zoom)
        {
            InitializeComponent();

            this.zoom = zoom;

            nudZoomPercent.Value = (decimal)(zoom * 100);
            if (zoom == 0.33)
                radZoom33.Checked = true;
            else if (zoom == 0.5)
                radZoom50.Checked = true;
            else if (zoom == 0.66)
                radZoom66.Checked = true;
            else if (zoom == 1)
                radZoom100.Checked = true;
            else if (zoom == 2)
                radZoom200.Checked = true;
            else if (zoom == 4)
                radZoom400.Checked = true;
            else
                radZoomSpecific.Checked = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (radZoom33.Checked)
                this.zoom = 0.33f;
            else if (radZoom50.Checked)
                this.zoom = 0.5f;
            else if (radZoom66.Checked)
                this.zoom = 0.66f;
            else if (radZoom100.Checked)
                this.zoom = 1f;
            else if (radZoom200.Checked)
                this.zoom = 2f;
            else if (radZoom400.Checked)
                this.zoom = 4f;
            else if (radZoomSpecific.Checked)
                this.zoom = (float)nudZoomPercent.Value / 100;
        }
    }
}
