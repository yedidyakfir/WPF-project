using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Coin
    {
        public string name;
        public List<CoinValue> History;
        public Coin(string n, List<CoinValue> l)
        {
            name = n;
            History = new List<CoinValue>(l);
        }
        public Coin() { }
    };
}
