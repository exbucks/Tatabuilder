using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TataBuilder
{
    [Serializable]
    public abstract class TActionInterval : TAction
    {
        public TActionInterval()
        {
            isInstant = false;
            duration = 1000;
        }
    }
}
