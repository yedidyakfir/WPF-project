using StockPriceWinodw.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using BE;

namespace StockPriceWinodw.ViewModel
{
    internal class CoinHistoryViewModel
    {
        public List<CoinModel> history { get; private set; }

        public CoinHistoryViewModel()
        {
            history = new List<CoinModel>();
        }

        public double MaxValue
        {
            get
            {
                return (from c in history
                        select c.value).Max();
            }
        }

        public double MinValue
        {
            get
            {
                return (from c in history
                        select c.value).Min();
            }
        }

        public CoinHistoryViewModel(string coin,string format)
        {
            List<CoinValue> BeCoins = FactoryBL.get().getCoinHistory(coin);

            history = (from c in BeCoins
                       select new CoinModel(coin, c.date, c.CoinValueId, 0)).ToList();
        }

        public override string ToString()
        {
            return history.FirstOrDefault().coin;
        }
    }
}
