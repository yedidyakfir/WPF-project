using StockPriceWinodw.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace StockPriceWinodw.ViewModel
{
    internal class CoinHistoryViewModel
    {
        public List<CoinModel> history { get; private set; }

        public CoinHistoryViewModel()
        {
            history = new List<CoinModel>();
            history.Add(new CoinModel("a", DateTime.Now - TimeSpan.FromDays(1), 3));
            history.Add(new CoinModel("a", DateTime.Now + TimeSpan.FromDays(1), 1));
            history.Add(new CoinModel("a", DateTime.Now + TimeSpan.FromDays(2), 6));
            history.Add(new CoinModel("a", DateTime.Now + TimeSpan.FromDays(3), 4));
        }

        public CoinHistoryViewModel(string coin,string format)
        {
            //FactoryBL.get().getCoinHistory(coin);
            history = new List<CoinModel>();
            history.Add(new CoinModel("a", DateTime.Now - TimeSpan.FromDays(1), 3));
            history.Add(new CoinModel("a", DateTime.Now + TimeSpan.FromDays(1), 1));
            history.Add(new CoinModel("a", DateTime.Now + TimeSpan.FromDays(2), 6));
            history.Add(new CoinModel("a", DateTime.Now + TimeSpan.FromDays(3), 4));
        }

        public override string ToString()
        {
            return history.FirstOrDefault().coin;
        }
    }
}
