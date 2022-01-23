using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Lib
{
    public class Countdown
    {
        private float max;
        private float curr;

        public Countdown(float max)
        {
            this.max = max;
            this.curr = 0;
        }

        public void Start()
        {
            this.curr = this.max;
        }

        public void Proceed(float amount)
        {
            this.curr = Math.Max(this.curr - amount, 0);
        }

        public bool IsRunning()
        {
            return this.curr > 0;
        }
    }
}
