using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public interface IDL
    {
        List<CoinValue> getCoinHistory(string coin);
        void AddCoin(Coin c);
        void Save();
        void Load();
    }
}
