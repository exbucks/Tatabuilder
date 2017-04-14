using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder
{
    public partial class FrmImagesPicker : Form
    {
        public TLibraryManager libraryManager { get; set; }
        public bool multiSelect { get; set; }

        public List<string> selectedImages;

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int SetWindowTheme(IntPtr hWnd, string appName, string partList);

        public FrmImagesPicker()
        {
            InitializeComponent();

            multiSelect = true;
            selectedImages = new List<string>();

            SetWindowTheme(lvwImages.Handle, "explorer", null);
        }

        private void FrmImagesPicker_Load(object sender, EventArgs e)
        {
            lvwImages.MultiSelect = multiSelect;
            lvwImages.LargeImageList = libraryManager.largeImageListThumbnails();
            for (int i = 0; i < libraryManager.imageCount(); i++) {
                lvwImages.Items.Add(libraryManager.imageFileName(i), i);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvwImages.SelectedItems) {
                selectedImages.Add(item.Text);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
