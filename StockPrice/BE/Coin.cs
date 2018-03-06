using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BE
{
    public class Coin
    {
        public string CoinId { get; set; }
        public List<CoinValue> History { get; set; }
        public Coin(string n, List<CoinValue> l)
        {
            CoinId = n;
            History = new List<CoinValue>(l);
        }
        public Coin() { }
    };
}
