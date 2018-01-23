using StockPriceWinodw.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceWinodw.ViewModel
{
    internal class CoinHistoryViewModel
    {
        public List<CoinModel> history { get; private set; }
        
        public CoinHistoryViewModel()
        {
            history = new List<CoinModel>();
            history.Add(new CoinModel("a", DateTime.Now, 3));
            history.Add(new CoinModel("a", DateTime.Now + TimeSpan.FromDays(1), 1));
            history.Add(new CoinModel("a", DateTime.Now + TimeSpan.FromDays(2), 6));
            history.Add(new CoinModel("a", DateTime.Now + TimeSpan.FromDays(3), 4));
        }
    }
}
