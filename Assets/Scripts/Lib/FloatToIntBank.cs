using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Lib
{
    public class FloatToIntBank
    {
        private float one_unit;
        private float total;

        public FloatToIntBank(float one_unit)
        {
            this.one_unit = one_unit;
            this.total = 0;
        }

        public void Deposit(float amount)
        {
            this.total += amount;
        }

        public int CashOut()
        {
            int res = (int)Math.Floor(total / one_unit);
            total %= one_unit;
            return res;
        }
    }
}
