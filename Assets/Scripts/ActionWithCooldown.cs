using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Lib;

namespace Assets.Scripts
{
    public class ActionWithCooldown
    {
        /// <summary>
        /// Keeps the trigger active for some time.
        /// </summary>
        Countdown triggerKept;

        /// <summary>
        /// Prevents action for some time.
        /// </summary>
        Countdown inProgress;

        Func<bool> Attempt;

        public ActionWithCooldown(float triggerLeeway, float duration, Func<bool> attempt)
        {
            this.triggerKept = new Countdown(triggerLeeway);
            this.inProgress = new Countdown(duration);
            this.Attempt = attempt;
        }

        public void Proceed(float deltaTime)
        {
            if (triggerKept.IsRunning() && !inProgress.IsRunning())
            {
                if (Attempt())
                {
                    inProgress.Start();
                }
            }

            triggerKept.Proceed(deltaTime);
            inProgress.Proceed(deltaTime);
        }

        public void Trigger()
        {
            triggerKept.Start();
        }
    }
}
