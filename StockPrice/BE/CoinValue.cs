using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class CoinValue
    {
        public double CoinValueId { get; set; }
        public DateTime date { get; set; }
        public CoinValue(double v, DateTime d)
        {
            CoinValueId = v;
            date = d;
        }
        public CoinValue() { }
    }
    public class CurrentCoinValue
    {
        public double value { get; set; }
        public DateTime date { get; set; }
        public string CurrentCoinValueId { get; set; }
        public CurrentCoinValue(string n, double v, DateTime d)
        {
            CurrentCoinValueId = n;
            value = v;
            date = d;
        }
        public CurrentCoinValue() { }
    }
    public class CoinValueForDB
    {
        public double CoinValueForDBId { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public CoinValueForDB(string n, double v, DateTime d)
        {
            name = n;
            CoinValueForDBId = v;
            date = d;
        }
        public CoinValueForDB() { }
    }

}
