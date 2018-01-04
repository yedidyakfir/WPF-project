﻿using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DL;

namespace BL
{
    class BL
    {
        private IDL Dal = new DAL();

        public CoinValue getCoinValue(string coin)
        {
            try
            {
                string url = "http://apilayer.net/api/live?access_key=0c54679e2255988cd03b9ed59129983a&currencies=" + coin + "&source=USD&format=1";
                WebClient wc = new WebClient();
                string apiResponse = wc.DownloadString(url);
                int index = apiResponse.IndexOf("USD" + coin);
                apiResponse = apiResponse.Substring(index + 8, 8);
                double value = Double.Parse(apiResponse);
                value = 1 / value;
                List<CoinValue> l = getCoinHistory(coin);
                l.Add(new CoinValue(value, DateTime.Now));
                return new CoinValue(value, DateTime.Now);
            }
            catch (Exception)//incase there's a problam with the internet
            {
                List<int> l = new List<int>();
                if (Dal.getCoinHistory(coin) != null)
                    return Dal.getCoinHistory(coin).Last();
                throw new Exception("There is no internet and the coin wasn't saved in the database");
            }
        }
        public double relation(string coin1, string coin2, int amount)
        {
            return (getCoinValue(coin1).value / getCoinValue(coin2).value) * amount;
        }
        public List<CoinValue> getCoinHistory(string coin)
        {
            try
            {           
                if (Dal.getCoinHistory(coin) != null)
                    return Dal.getCoinHistory(coin);

                DateTime t = DateTime.Now;
                t.AddYears(-3);
                string url;
                WebClient wc;
                List<CoinValue> l = new List<CoinValue>();
                                
                for (int i = 0; i < 36; i++,t.AddMonths(1))
                {
                    url = "http://apilayer.net/api/historical?" +
                        "access_key=0c54679e2255988cd03b9ed59129983a" +
                        "&date=" + t.Year.ToString() + "-" + (t.Month<10?("0"+t.Month.ToString()):t.Month.ToString()) + "-" + (t.Day < 10 ? ("0" + t.Day.ToString()) : t.Day.ToString()) +
                        "&source=USD" +
                        "&currencies=" + coin +
                        "&format=1";
                    wc = new WebClient();
                    string apiResponse = wc.DownloadString(url);
                    int index = apiResponse.IndexOf("USD" + coin);
                    apiResponse = apiResponse.Substring(index + 8, 8);
                    double value = Double.Parse(apiResponse);
                    value = 1 / value;
                    l.Add(new CoinValue(value, t));
                }
                Dal.AddCoin(new Coin(coin, l));
                return l;
            }
            catch (Exception)
            {
                throw new Exception("There is no internet and the coin wasn't saved in the database");
            }
        }
    }
}
/*string request = String.Format("http://www.xe.com/ucc/convert.cgi?Amount={0}&From={1}&To={2}", value, inputCurrency, outputCurrency);
                Console.WriteLine(request);
                Console.Read();
                WebClient wc = new WebClient();
                string apiResponse = wc.DownloadString(request);    // This is a blocking operation.
                int index = apiResponse.IndexOf(@"class='uccResultUnit' data-amount=");
                apiResponse = apiResponse.Substring(index + 34);
                index = apiResponse.IndexOf("=");
                apiResponse = apiResponse.Substring(index + 2);
                apiResponse = apiResponse.Substring(0, 4);
                wc.Dispose();
                return Double.Parse(apiResponse); g
                    u
                    n
                    c
                    h*/