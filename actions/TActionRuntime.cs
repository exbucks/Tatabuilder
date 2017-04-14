using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TataBuilder
{
    public class TActionRuntime : TAction
    {
        public delegate void RuntimeCodeDelegate(float percent);

        private RuntimeCodeDelegate runtimeCode;

        public TActionRuntime()
        {
            this.isInstant = false;
            this.duration = 0;
            this.runtimeCode = null;
        }

        public TActionRuntime(int duration, RuntimeCodeDelegate runtimeCode)
        {
            this.isInstant = false;
            this.duration = duration;
            this.runtimeCode = runtimeCode;
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionRuntime targetAction = (TActionRuntime)target;
            targetAction.isInstant = this.isInstant;
            targetAction.duration = this.duration;
            targetAction.runtimeCode = this.runtimeCode;
        }

        public override void reset(long time)
        {
            base.reset(time);
        }

        // execute action for every frame
        // if action is finished, return true;
        public override bool step(FrmEmulator emulator, long time)
        {
            float elapsed = time - run_startTime;
            if (elapsed > duration)
                elapsed = duration;

            float percent = duration > 0 ? elapsed / duration : 1;
            if (runtimeCode != null)
                runtimeCode(percent);

            return base.step(emulator, time);
        }
    }
}
