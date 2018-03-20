using StockPriceWinodw.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using BE;
using LiveCharts;

namespace StockPriceWinodw.ViewModel
{
    internal class CoinHistoryViewModel
    {
        public List<CoinModel> history { get; private set; }
        private int yAxisSteps = 10;

        public CoinHistoryViewModel()
        {
            history = new List<CoinModel>();
            history.Add(new CoinModel("s", DateTime.Now, 1));
            history.Add(new CoinModel("s", DateTime.Now - TimeSpan.FromDays(1), 3));
            history.Add(new CoinModel("s", DateTime.Now - TimeSpan.FromDays(2), 2));
        }

        public CoinHistoryViewModel(string coin,string format)
        {
            try
            {
                List<CoinValue> BeCoins = FactoryBL.get().getCoinHistory(coin);

                history = (from c in BeCoins
                           select new CoinModel(coin, c.date, c.CoinValueId, 0)).ToList();

            }
            catch(Exception ex)
            {
                history = new List<CoinModel>();
            }
        }

        public float YStepValue
        {
            get
            {
                return (float)(MaxValue - MinValue) / yAxisSteps;
            }
        }

        public double MaxValue
        {
            get
            {
                if (history.Count == 0)
                    return 0;
                return (from c in history
                        select c.value).Max();
            }
        }

        public double MinValue
        {
            get
            {
                if (history.Count == 0)
                    return 0;
                return (from c in history
                        select c.value).Min();
            }
        }

        public double AverageValue
        {
            get
            {
                if (history.Count == 0)
                    return 0;
                return (from c in history
                        select c.value).Average();
            }
        }

        public string[] XValues
        {
            get
            {
                return history.Select(x => x.lastUpdate.ToString()).ToArray();
            }
        }

        public ChartValues<double> YValues
        {
            get
            {
                return new ChartValues<double>(from c in history
                                               select c.value);
            }
        }

        public override string ToString()
        {
            return history.FirstOrDefault().coin;
        }
    }
}
