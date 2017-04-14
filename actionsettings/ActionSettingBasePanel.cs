using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder.actionsettings
{
    public class ActionSettingBasePanel : UserControl
    {
        public TimeLineView timeLineView;
        public TAction action;

        public virtual void LoadData()
        {
        }

        public virtual void SaveData()
        {
            timeLineView.NotifyDataSourceChanged();
        }
    }
}
