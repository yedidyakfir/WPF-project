using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class CoinValue
    {
        public double value;
        public DateTime date;
        public CoinValue(double v, DateTime d)
        {
            value = v;
            date = d;
        }
    }
}
