using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DL
{
    public class DAL:IDL
    {
        private List<Coin> DB;
        public List<CoinValue> getCoinHistory(string coin)
        {
            foreach (Coin c in DB)
                if (c.name == coin)
                    return c.History;
            return null;
        }
        public void AddCoin(Coin c)
        {
            DB.Add(c);
        }
    }
}
