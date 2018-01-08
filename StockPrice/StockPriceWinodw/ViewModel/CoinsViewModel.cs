using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using StockPriceWinodw.Model;

namespace StockPriceWinodw.ViewModel
{
    internal class CoinsViewModel 
    {
        public List<CoinModel> Coins { get; set; }

        public CoinModel SelectedCoin { get; set; }

        public CoinsViewModel()
        {
            //taking the coins from the model list
            Coins = new List<CoinModel>();
            Coins.Add(new CoinModel("a", DateTime.Now, 2));
            Coins.Add(new CoinModel("b", DateTime.Now, 1));
            Coins.Add(new CoinModel("c", DateTime.Now, 6));
        }
    }
}
