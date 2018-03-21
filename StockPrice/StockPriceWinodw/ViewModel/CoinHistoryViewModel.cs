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
    internal class CoinHistoryViewModel
    {
        private List<CoinModel> history;
        private int yAxisSteps = 5;
        //private ZoomingOptions _zoomingMode = ZoomingOptions.X;
        //public event PropertyChangedEventHandler PropertyChanged;

        //public ZoomingOptions ZoomingMode
        //{
        //    get { return _zoomingMode; }
        //    set
        //    {
        //        _zoomingMode = value;
        //        OnPropertyChanged();
        //    }
        //}

        //protected virtual void OnPropertyChanged(string propertyName = null)
        //{
        //    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public CoinHistoryViewModel()
        {
            history = new List<CoinModel>();
            history.Add(new CoinModel("Demo", DateTime.Now, 1)); //well this is dumb but necessery otherwise the application will callapse
            history.Add(new CoinModel("Demo", DateTime.Now - TimeSpan.FromDays(1), 3));
            history.Add(new CoinModel("Demo", DateTime.Now - TimeSpan.FromDays(2), 2));

            YFormatter = valueTostring;// value => value.ToString("C");
        }

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

                if(history.Count == 1) //precaution for break mode 
                {
                    CoinModel tempC = history.First();
                    history.Add(new CoinModel(tempC.coin, tempC.lastUpdate - TimeSpan.FromDays(1), tempC.value + 0.0000001));
                    history.Add(new CoinModel(tempC.coin, tempC.lastUpdate - TimeSpan.FromDays(2), tempC.value));
                }

                if(FactoryBL.get().GetSlope(coin) == 0)
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
