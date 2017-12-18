using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization; 
using System.Linq;

namespace Infragistics.Framework
{
    /// <summary>
    /// Represents a provider with Stock Market data  
    /// </summary>
    public static class StockMarketData
    {
        internal static Random Rand = new Random();
        static StockMarketData()
        {
            Stocks = new List<StockMarketItem>();
            var csv = DataProvider.GetCsvTable("stock-market.csv");

            //var year = DateTime.Now.Year - 1;
            var date = DateTime.Now; // new DateTime(year, 1, 1);
            date = date.AddDays(-csv.Rows.Count);

            for (var i = 0; i <  csv.Rows.Count; i++)
            {
                var row = csv.Rows[i];
                if (i == 0)
                    continue; // skip csv header
                 
                var prec = Rand.NextDouble();
                date = date.AddDays(1);
                //var date = new DateTime(year, month, day);
                var open = double.Parse(row[1]) + prec;
                var high = double.Parse(row[2]) + prec;
                var low = double.Parse(row[3]) + prec;
                var close = double.Parse(row[4]) + prec;
                var volume = long.Parse(row[5]);

                var item = new StockMarketItem(open, low, high, close, volume, date);
                item.Index = i; 
                Stocks.Add(item);

            }

            Stocks = Stocks.OrderBy(i => i.DateTime.Ticks).ToList();
              
        }

        /// <summary> Gets or sets PropertyName </summary>
        public static List<StockMarketItem> Stocks { get; set; }

    }
    public class StockMarketItemList : List<StockMarketItem>
    {
    }

    [DataContract]
    public class StockMarketItem //: ObservableObject
    {
        public StockMarketItem()
        { }
        public StockMarketItem(StockMarketItem stockData)
        {
            this.Open = stockData.Open;
            this.High = stockData.High;
            this.Low = stockData.Low;
            this.Close = stockData.Close;
            this.Volume = stockData.Volume;
            this.DateTime = stockData.DateTime;

            Update();
        }
        public StockMarketItem(double open, double low, double high, double close,
            long volume, DateTime date)
        {
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
            this.DateTime = date;

            Update();
        }
        public new string ToString()
        {
            var result = string.Empty;
            result += " Open: " + Open + " Close: " + Close + " Volume: " + Volume + " Date: " + DateTime;
            return result;
        }
        public StockMarketItem Copy()
        {
            return new StockMarketItem(this);
        }

        #region Data Members
        [DataMember(Name = "Volume")]
        public double Volume { get; private set; }

        [DataMember(Name = "Open")]
        public double Open { get; private set; }
        [DataMember(Name = "Low")]
        public double Low { get; private set; }
        [DataMember(Name = "High")]
        public double High { get; private set; }
        [DataMember(Name = "Close")]
        public double Close { get; private set; } 
        [DataMember(Name = "Date")]
        public string DateString { get; private set; } 
        #endregion

        public DateTime DateTime { get; private set; }
        public int Index { get; set; }
   
        // TODO-MT implement % scale of OHLC values like Google finance chart does
        //public double ClosePercent { get { return ChangePercent; } }
        //public double OpenPercent { get { return (OpenChange / Open) * 100; } }
        //public double HighPercent { get { return (HighChange / Open) * 100; } }
        //public double LowPercent { get { return (LowChange / Open) * 100; } }
        //public double OpenPercent { get; set; }
        //public double HighChange { get { return (High - Open); } }
        //public double LowChange { get { return (Low - Open); } }
        //public double OpenChange { get { return (Open - Close); } }
        public double Change { get; private set; }
        public double ChangePercent { get; private set; }
        public double Range { get; private set; }

        public void Update()
        {
            this.Change = (Close - Open);
            this.ChangePercent = (Change / Open) * 100;
            this.Range = (High - Low);

            this.DateString = DateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
         
    }
    
}