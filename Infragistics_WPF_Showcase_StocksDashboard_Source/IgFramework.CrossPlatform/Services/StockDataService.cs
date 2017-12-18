using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infragistics.Framework.Services 
{
      
    /// <summary> 
    /// Represents a service for downloading latest stock data from www.quandl.com
    /// </summary>
    public class StockDataService
    {
        #region Members
        public static readonly StockDataService Instance = new StockDataService();

        protected WebServiceCache Cache;

        private const string ApiAddress = "https://www.quandl.com/";
        private const string ApiVersion = "api/v3/";
        private const string ApiKey = "?api_key=pygU9YZDGTYVrfFcFZex";
        private const string ApiBase = ApiAddress + ApiVersion;
      
        private const string StockOutputJson = ".json";
        private const string StockOutputXml = ".xml";
        private const string StockOutputCsv = ".csv";
        private const string StockDataset = "datasets/WIKI/";
        private const string StockDateFormat = "yyyy-MM-dd";
           
        /// <summary> Gets or sets StartDate </summary>
        public static DateTime HistoryStartDate { get; set; }
        protected static DateTimeDictionary TradingDates = new DateTimeDictionary();

        #endregion
 
        public StockDataService()
        {
            //HistoryStartDate = new DateTime(2015, 1, 1);
            HistoryStartDate = new DateTime(2000, 1, 1);
            //HistoryStartDate = new DateTime(1993, 7, 13);
		
            Cache = WebServiceCache.Instance;
        }

        public StockPriceList GetStockHistoryCache(string symbol)
        {
            StockPriceList stockHistory = null;
          
            if (Cache.HasCacheData(symbol) &&
               !Cache.HasCacheExpired(symbol))
            {
                stockHistory = Cache.GetFromCache(symbol) as StockPriceList;
                Logs.Message(this, "Requesting stock history for " + symbol + " already cached");
            }

            return stockHistory;
        }
       
        protected static string GetStartDate(DateTime startDate)
        {
            const string format = "{0:" + StockDateFormat + "}";
            return "&start_date=" + string.Format(format, startDate);
        }

        #region Public Methods

        public string GetStockHistoryUrl(string stockSymbol, DateTime historyStart)
        {
            // https://www.quandl.com/api/v3/datasets/WIKI/TSLA.json?api_key=pygU9YZDGTYVrfFcFZex

            var stockUrl = ApiBase + StockDataset + stockSymbol;
            stockUrl += StockOutputCsv + ApiKey + GetStartDate(historyStart);

            return stockUrl;
        }

        public string GetStockHistoryUrl(string stockSymbol)
        {
            return GetStockHistoryUrl(stockSymbol, HistoryStartDate);
        }
         
        public event EventHandler<RequestStockHistoryCompletedEventArgs> RequestStockHistoryCompleted;
        protected virtual void OnRequestCompleted(RequestStockHistoryCompletedEventArgs eventArgs)
        {
            var eventHandler = this.RequestStockHistoryCompleted;
            if (eventHandler != null)
                eventHandler(this, eventArgs);
            Logs.Trace(this, "RequestStockHistoryCompleted");
        }
        protected virtual void OnRequestCompleted(StockPriceList data, object state)
        {
            var eventArgs = new RequestStockHistoryCompletedEventArgs(data, null, false, state);
            OnRequestCompleted(eventArgs);
        }
        protected virtual void OnRequestCompleted(StockPriceList data, object state, Exception error)
        {
            var eventArgs = new RequestStockHistoryCompletedEventArgs(data, error, false, state);
            OnRequestCompleted(eventArgs);
        }

        /// <summary> Request Stock History from default date 2000-1-1 </summary>
        public void RequestStockHistoryAsync(string symbol,
            Action<string, StockPriceList> callback)
        {
            RequestStockHistoryAsync(symbol, HistoryStartDate, callback);
        }
        /// <summary> Request Stock History from specified date </summary>
        public void RequestStockHistoryAsync(string symbol, DateTime startDate,
            Action<string, StockPriceList> callback)
        {
            if (string.IsNullOrEmpty(symbol)) return;

            // skip if this a redundant query
            var stocks = GetStockHistoryCache(symbol);
            if (stocks == null)
            { 
                var uri = GetStockHistoryUrl(symbol, startDate);
                var stockToken = new StockToken();
                stockToken.Symbol = symbol;
                stockToken.Callback = callback;
                
                Logs.Message(this, "Requesting stock history for " + symbol + "...");

                // var wc = new WebDownload();
                //wc.OpenReadCompleted += OnRequestStockHistoryCompleted;
                WebService.Request(uri, stockToken, OnRequestStockHistoryCompleted);
            }
            else
            {
                if (callback != null)
                {
                    SynchronizationContext.Current.Post(data =>
                        callback(symbol, (StockPriceList)data), stocks);
                }
            }
            //  //if (Cache.HasCacheData(symbol) &&
            //   !Cache.HasCacheExpired(symbol))
            //{
            //     var stockHistory = Cache.GetFromCache(symbol);

            //    //if (callback != null)
            //    //{
            //    //    SynchronizationContext.Current.Post(data =>
            //    //        callback((StockPriceList)data), stockHistory);
            //    //}
            //    return;
            //}
        }  
        #endregion

        #region OLD
        //public Task<StockPriceList> RequestStockHistoryAsync(string symbol)
        //{
        //    return RequestStockHistoryAsync(symbol, HistoryStartDate);
        //}
        ///// <summary> Request Stock History from specified date </summary>
        //private Task<StockPriceList> RequestStockHistoryAsync(
        //    string symbol, DateTime startDate)
        //{
        //    var tcs = new TaskCompletionSource<StockPriceList>();
        //    WebService.Iterate(RequestStockHistoryTask(symbol, startDate, tcs), tcs);
        //    return tcs.Task;
        //}


        //private IEnumerable<Task> RequestStockHistoryTask(
        //    string symbol, DateTime startDate,
        //    TaskCompletionSource<StockPriceList> tcs)
        //{

        //    var stockHistory = new StockPriceList();
        //    if (string.IsNullOrEmpty(symbol))
        //    {
        //        //OnRequestCompleted(stockHistory);
        //        yield return tcs.Task;
        //        tcs.TrySetResult(stockHistory);
        //        //return stockHistory;
        //    }

        //    var stockToken = new StockToken();
        //    stockToken.Symbol = symbol; 
        //    stockToken.StartDate = startDate;

        //    var process = "StockQuandlService -> Requesting stock history for " + stockToken.Symbol;

        //    // skip if this a redundant query
        //    if (Cache.HasCacheData(symbol) &&
        //       !Cache.HasCacheExpired(symbol))
        //    {
        //        Logs.Message(process + " skipped");
        //        stockHistory = (StockPriceList)Cache.GetFromCache(symbol);

        //        tcs.TrySetResult(stockHistory);
        //        //OnRequestCompleted(stockHistory);
                   
        //            //SynchronizationContext.Current.Post(data =>
        //            //    callback((StockPriceList)data), stockHistory);

        //        //return stockHistory;
        //    }
        //    var uriRequest = GetStockHistoryUrl(symbol, startDate);

        //    Logs.Message(process + "...");
             
        //    //WebService.Request(uri, stockToken, OnRequestStockHistoryCompleted);


        //    //tcs.TrySetResult(stockHistory);

        //    var task = Task.Factory.FromAsync<WebServiceResponse>
        //       (WebService.RequestAsync(uriRequest), null);
        //    yield return task;

        //    //stockHistory.Symbol = symbol;
        //    stockHistory.Add(new StockPriceItem());

        //    Cache.UpdateCache(stockToken.Symbol, new List<object> { stockHistory });
            
        //    tcs.TrySetResult(stockHistory);
            
        //    //var response = await WebService.RequestAsync(uriRequest);

        //    //using (var reader = new StreamReader(response.ResultStream, Encoding.UTF8))
        //    //{
        //    //    data = reader.ReadToEnd();
        //    //}
        //}

        #endregion
          
        public void OnRequestStockHistoryCompleted(WebServiceResponse response)
        {
            var stocks = ReadStockHistory(response);

            var stockToken = response.UserState as StockToken;
            if (stockToken != null && 
                stockToken.Callback != null)
            {
                SynchronizationContext.Current.Post(data =>
                    stockToken.Callback(stockToken.Symbol, (StockPriceList)data), stocks);
            }
        }
        public StockPriceList ReadStockHistory(WebServiceResponse response)
        {
            var symbol = "????";
            var stockToken = response.UserState as StockToken;
            if (stockToken != null)
                symbol = stockToken.Symbol;

            var process = "Reading stock history for " + symbol;
            
            var stockHistory = new StockPriceList();
            if (response.ErrorOccured)
            {
                Logs.Error(this, process + " failed: \n\t at " + response.StatusInfo);
                 
                var error = new Exception(response.ErrorStack);

                //OnRequestCompleted(stockHistory, symbol, error);

                return stockHistory;
            }
             
            Logs.Message(this, process + " status: " + response.StatusInfo);

            var stream = response.ResultStream;
            var csv = DataProvider.GetCsvFile(stream);
            var culture = CultureInfo.InvariantCulture;

            #region Parse csv data and create stock history
            //      0,    1,    2,   3,     4,      5,        6,           7, 
            //   Date, Open, High, Low, Close, Volume, Dividend, Split Ratio, Adj. Open,Adj. High,Adj. Low,Adj. Close,Adj. Volume
            // 2015-09-10,247.23,250.7231,245.33,248.381,2700138.0,0.0,1.0,247.23,250.7231,245.33,248.381,2700138.0
            // 2015-09-09,252.05,254.25,248.303,248.91,3363641.0,0.0,1.0,252.05,254.25,248.303,248.91,3363641.0

            for (var i = 1; i < csv.Count; i++)
            {
                var line = csv[i];
                var stock = new StockPriceItem();
                stock.Date = DateTime.ParseExact(line[0], StockDateFormat, culture);

                stock.Symbol = symbol;
                stock.Open = double.Parse(line[1]);
                stock.High = double.Parse(line[2]);
                stock.Low = double.Parse(line[3]);
                stock.Close = double.Parse(line[4]);
                stock.Volume = double.Parse(line[5]);

                stock.Dividend = double.Parse(line[6]);
                stock.Split = double.Parse(line[7]);

                stockHistory.Add(stock);

                var day = stock.Date.ToString();
                if (!TradingDates.ContainsKey(day))
                     TradingDates.Add(day, stock.Date);
            }
            //stockHistory.Sort((x, y) => +(Comparer<DateTime>.Default.Compare(x.Date, y.Date)));

            stockHistory.Sort((x, y) => Comparer.Ascending(x.Date, y.Date));

            Logs.Message(this, "Data parsed: " + stockHistory.Count);
        
            #endregion

            #region Align data
            if (stockHistory.Count > 0)
            {
                var first = stockHistory.First();
                while (first.Date > HistoryStartDate)
                {
                    var newStock = new StockPriceItem();
                    newStock.Date = first.Date.AddDays(-1);
                    newStock.Symbol = symbol;
                    if (TradingDates.ContainsKey(newStock.Date.ToString()))
                    {
                        stockHistory.Insert(0, newStock);
                    }

                    first = newStock;
                }
                var last = stockHistory.Last();
                var latestStock = new StockPriceItem(last);
                latestStock.Date = DateTime.Now;
                
                stockHistory.Add(latestStock);
            }
            Logs.Message(this, "Data aligned: " + stockHistory.Count);
           
            #endregion

            Cache.UpdateCache(symbol, new List<object> { stockHistory });

            //OnRequestCompleted(stockHistory, symbol);

            return stockHistory;
        }
    }
     
    public class RequestStockHistoryCompletedEventArgs : AsyncCompletedEventArgs
    {
        public RequestStockHistoryCompletedEventArgs(StockPriceList results,
            Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            Result = results;
        }
        public RequestStockHistoryCompletedEventArgs(StockPriceList results) :
            this(results, null, false, null)
        { }

        /// <summary> Gets or sets Result </summary>
        public StockPriceList Result { get; set; }
        
        //private ObservableCollection<StockPriceItem> _results;
         
        //public ObservableCollection<StockPriceItem> Result
        //{
        //    get
        //    {
        //        base.RaiseExceptionIfNecessary();
        //        return _results;
        //    }
        //}
    }

    public class StockToken
    {
        /// <summary> Gets or sets Symbol </summary>
        public string Symbol { get; set; }

        /// <summary> Gets or sets Callback </summary>
        public Action<string, StockPriceList> Callback { get; set; }

        /// <summary> Gets or sets Start Date </summary>
        public DateTime StartDate { get; set; }
    }

   

    

}
