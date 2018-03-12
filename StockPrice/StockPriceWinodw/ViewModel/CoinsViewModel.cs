using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using BL;
using StockPriceWinodw.Model;

namespace StockPriceWinodw.ViewModel
{
    internal class CoinsViewModel 
    {
        public List<CoinModel> Coins { get; set; }

        public CoinModel SelectedCoin { get; set; }

        public CoinsViewModel()
        {
            // transform this list to coinModel list
            List<CurrentCoinValue> BeCoins = FactoryBL.get().getCoinsValue();
            Coins = (from c in BeCoins
                     select new CoinModel(c.CurrentCoinValueId, c.date, c.value, FactoryBL.get().GetSlope(c.CurrentCoinValueId))).ToList();
            

            //taking the coins from the model list
            //Coins =
            //    new List<CoinModel>();
            //Coins.Add(new CoinModel("USD", DateTime.Now, 2,2));
            //Coins.Add(new CoinModel("ILS", DateTime.Now, 1,1));
            //Coins.Add(new CoinModel("c", DateTime.Now, 6,0.5));
        }
    }
}
