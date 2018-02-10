using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceWinodw.Model
{
    internal class CoinModel
    {
        public string coin { get; private set; }
        public DateTime lastUpdate { get; private set; }
        public double value { get; private set; }
        public double trend { get; private set; }

        public CoinModel(string c,DateTime d,double v,double t = 0)
        {
            coin = c;
            lastUpdate = d;
            value = v;
            trend = t;
        }
    }
}
