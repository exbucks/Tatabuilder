using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder
{
    public class FocuslessButton : Button
    {
        /// <summary>
        /// Avoids the inner solid rectangle shown on focus
        /// </summary> 
        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }
    }
}
