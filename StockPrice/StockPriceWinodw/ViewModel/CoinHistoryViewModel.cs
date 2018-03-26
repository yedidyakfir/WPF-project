using StockPriceWinodw.Model;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using BE;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace StockPriceWinodw.ViewModel
{
    //this class is responsible for the view logic and connect between the data and the view
    internal class CoinHistoryViewModel
    {
        private List<CoinModel> history;
        private int yAxisSteps = 5;

        public CoinHistoryViewModel()
        {
            history = new List<CoinModel>();
            history.Add(new CoinModel("Demo", DateTime.Now, 1)); //well this is dumb but necessery otherwise the application will callapse
            history.Add(new CoinModel("Demo", DateTime.Now - TimeSpan.FromDays(1), 3));
            history.Add(new CoinModel("Demo", DateTime.Now - TimeSpan.FromDays(2), 2));

            YFormatter = valueTostring;// value => value.ToString("C");
        }

        //this function tells the graph how to represent numbers
        private string valueTostring(double val)
        {
            if (val < 0.01)
                return val.ToString("E");
            else
                return val.ToString("C");
        }

        public CoinHistoryViewModel(string coin, string format)
        {
            try
            {
                List<CoinValue> BeCoins = FactoryBL.get().getCoinHistory(coin,format);

                history = (from c in BeCoins
                           select new CoinModel(coin, c.date, c.CoinValueId, 0)).ToList();

                if(history.Count == 1) //precaution for break mode ,if the graph has only single value (y = 4 for example) the code will enter break mode 
                {
                    CoinModel tempC = history.First();
                    history.Add(new CoinModel(tempC.coin, tempC.lastUpdate - TimeSpan.FromDays(1), tempC.value + 0.0000001));
                    history.Add(new CoinModel(tempC.coin, tempC.lastUpdate - TimeSpan.FromDays(2), tempC.value));
                }

                if(FactoryBL.get().GetSlope(coin) == 0) //another reprecaution for break mode ,so the graph won't have only 1 value and enter a break mode
                {
                    CoinModel tempC = history.First();
                    history.Remove(tempC);
                    history.Add(new CoinModel(tempC.coin, tempC.lastUpdate - TimeSpan.FromDays(1), tempC.value + 0.0000001));
                }
            }
            catch (Exception ex)
            {
                history = new List<CoinModel>();
                history.Add(new CoinModel("NOT FOUND", DateTime.Now, 1));
            }

            YFormatter = valueTostring;
        }

        public string Coin
        {
            get
            {
                if (history.Count == 0)
                    return "";
                return history.First().coin;
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

        public Func<double, string> YFormatter { get; set; }

        public override string ToString()
        {
            return history.FirstOrDefault().coin;
        }
    }
}
