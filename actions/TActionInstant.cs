using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TataBuilder
{
    [Serializable]
    public abstract class TActionInstant : TAction
    {
        public TActionInstant()
        {
            isInstant = true;
            duration = 0;
        }
    }
}
