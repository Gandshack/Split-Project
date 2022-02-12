using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Lib
{
    class PeriodicEvent
    {
        private float timeElapsed;

        private float periodLength;
        private float duration;

        // how much of the period is spent on
        private float portionOn;

        public PeriodicEvent(float periodLength, float duration, float portionOn)
        {
            this.periodLength = periodLength;
            this.duration = duration;
            this.portionOn = portionOn;
            this.timeElapsed = -1;
        }

        public void Start()
        {
            timeElapsed = 0;
        }

        public bool Tick(float deltaTime)
        {
            if (timeElapsed < 0)
            {
                return false;
            }
            timeElapsed += deltaTime;
            if (timeElapsed > duration)
            {
                timeElapsed = -1;
                return false;
            }
            float f = (timeElapsed / periodLength) % 1;
            return f < portionOn;
        }
    }
}
