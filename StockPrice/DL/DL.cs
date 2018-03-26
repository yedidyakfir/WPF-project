using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.IO;
using System.Xml.Serialization;
using System.Net;
using System.Data.Entity;
using System.Data.SqlClient;

namespace DL
{
    public class DAL : IDL
    {
        public List<Coin> DB;
        public List<CurrentCoinValue> CurrentCoins;
        //the number of the first key that works
        private int num = 0;
        //keys for the api (they have a limit of 1000 requests per month)
        private string[] keys = { "7017c5933ab8aaa1d0078693a5b1b8a9" , "e8c88f3997a6810671c6c70eb42dff3e" , "2c7dd5dc25ea2f920e1cad623c2f81a0" , "914f8d9624be9124eac51795568dd89f" , "e46b033087886daaa2fa2d8c7bb43abe" , "573ef63ec60efa508997c45f3315bb54", "209738903e50af28323b57b984be1495" };

        //returns the current value of a specific coin
        public CoinValue getCoinValue(string coin)
        {
            return new CoinValue(getCurrentCoins().First(d => d.CurrentCoinValueId == coin).value,
                getCurrentCoins().First(d => d.CurrentCoinValueId == coin).date);
        }

        //return the history of a specific coin
        public List<CoinValue> getCoinHistory(string coin)
        {
            try
            {
                if(DB.Count != 0 && DB.First().History.Count != 0) // if the DataBase is Up to date - we return the history in the DataBase
                    if ((DB.First().History.Max(d => d.date).Day == DateTime.Now.Day ||
                        DB.First().History.Max(d => d.date).Day == DateTime.Now.Day-1) &&
                        DB.First().History.Max(d => d.date).Year == DateTime.Now.Year &&
                        DB.First().History.Max(d => d.date).Month == DateTime.Now.Month)
                        return DB.First(d => d.CoinId == coin).History;
                
                //if the DataBase is empty we create add to the DataBase iformation from the last year
                //otherwise we just fill in the gap to make the DataBase up to date (in other words we will add info from the last date in the DataBase)
                DateTime t = (DB.Count == 0 || DB.First().History.Count == 0) ? (DateTime.Now.AddYears(-1)): DB.First().History.Max(d => d.date);
             
                string url;
                WebClient wc;
                List<CoinValue> l = new List<CoinValue>();

                //this while adds a value of all coins at date t each iteration until the current date
                while (!(t.Day == DateTime.Now.Day &&
                        t.Year == DateTime.Now.Year &&
                        t.Month == DateTime.Now.Month))
                {
                    t = t.AddDays(1);
                    url = "http://apilayer.net/api/historical?" +
                        "access_key=" + keys[num] +
                        "&date=" + t.Year.ToString() + "-" + (t.Month < 10 ? ("0" + t.Month.ToString()) : t.Month.ToString()) + "-" + (t.Day < 10 ? ("0" + t.Day.ToString()) : t.Day.ToString()) +
                        "&format=1";
                    wc = new WebClient();
                    string apiResponse;
                    try
                    { apiResponse = wc.DownloadString(url); }//getting the info from the internet
                    catch { return DB.First(d => d.CoinId == coin).History; }//incase there's a problam with the internet

                    int index = apiResponse.IndexOf("USD");
                    apiResponse = apiResponse.Substring(index + 3);
                    index = apiResponse.IndexOf("USD");
                    apiResponse = apiResponse.Substring(index + 3);

                    string name;
                    double value;

                    //this while breaks down the response and adds the info to the DataBase
                    while (index != -1)
                    {
                        name = apiResponse.Substring(0, 3);

                        //an exception when we reach USD because every value is in relation to USD
                        if (name == "USD")
                        {
                            apiResponse = apiResponse.Substring(3);
                            index = apiResponse.IndexOf("USD");
                            apiResponse = apiResponse.Substring(index + 3);
                            name = apiResponse.Substring(0, 3);
                        }

                        //an exception when we reach ZWL because it's the last one
                        if (name == "ZWL")
                            value = Double.Parse(apiResponse.Substring(5, 3));
                        else
                            value = Double.Parse(apiResponse.Substring(5, apiResponse.Substring(5).IndexOf(',')));

                        value = 1 / value;
                        if (!DB.Exists(c => c.CoinId == name))
                            getCurrentCoins();
                        DB.First(d => d.CoinId == name).History.Add(new CoinValue( value, t));
                        using (var db = new CoinContext())
                        {
                            db.CoinValues.Add(new CoinValueForDB(name,(db.CoinValues.Count()==0)?0:(db.CoinValues.Max(d => d.CoinValueForDBId)+0.01), value, t));
                            Save(db);
                        }
                        index = apiResponse.IndexOf("USD");
                        if (index != -1)
                            apiResponse = apiResponse.Substring(index + 3);
                    }
                }
                return DB.First(d => d.CoinId == coin).History;
            }
            catch (Exception)
            {
                try
                {
                    //incase we ran out of requests in the num key...
                    num++;
                    return getCoinHistory(coin);
                }
                catch (Exception)
                { throw new Exception("out of keys"); }           
            }
        }

        //we return the current value of all coins (if there is no internet - we return the lastest in the DataBase
        public List<CurrentCoinValue> getCurrentCoins()
        {
            if ((CurrentCoins.Count !=0) && // if the DataBase is Up to date - we return the CurrentCoins 
                CurrentCoins.First().date.Day == DateTime.Now.Day &&
                CurrentCoins.First().date.Year == DateTime.Now.Year &&
                CurrentCoins.First().date.Month == DateTime.Now.Month)
                return CurrentCoins;
            try
            {
                string url = "http://apilayer.net/api/live?access_key=" + keys[num] + "&format=1";
                WebClient wc = new WebClient();
                string apiResponse;
                try
                { apiResponse = wc.DownloadString(url);}
                catch { return CurrentCoins; }//incase there's a problam with the internet

                int index = apiResponse.IndexOf("USD");
                apiResponse = apiResponse.Substring(index + 3);
                index = apiResponse.IndexOf("USD");
                apiResponse = apiResponse.Substring(index + 3);

                string name;
                double value;
                CurrentCoins = new List<CurrentCoinValue>();

                //this while breaks down the response and adds the current coins
                while (index != -1) 
                {
                    name = apiResponse.Substring(0, 3);

                    //an exception when we reach USD because every value is in relation to USD
                    if (name == "USD")
                    {
                        apiResponse = apiResponse.Substring(3);
                        index = apiResponse.IndexOf("USD");
                        apiResponse = apiResponse.Substring(index + 3);
                        name = apiResponse.Substring(0, 3);
                    }

                    //an exception when we reach ZWL because it's the last one
                    if (name == "ZWL")
                        value = Double.Parse(apiResponse.Substring(5,3));
                    else
                        value = Double.Parse(apiResponse.Substring(5, apiResponse.Substring(5).IndexOf(',')));
                    value = 1 / value;
                    CurrentCoins.Add(new CurrentCoinValue(name, value, DateTime.Now));

                    if (!DB.Exists(c => c.CoinId == name))
                        DB.Add(new Coin(name, new List<CoinValue>()));
                    index = apiResponse.IndexOf("USD");
                    if(index != -1)
                        apiResponse = apiResponse.Substring(index + 3);
                }
                using (var db = new CoinContext())
                {
                    db.CurrentCoins.RemoveRange(db.CurrentCoins);
                    db.CurrentCoins.AddRange(CurrentCoins);
                    Save(db);
                }
                return CurrentCoins;
            }
            catch (Exception e)
            {
                try
                {
                    //incase we ran out of requests in the num key...
                    num++;
                    return getCurrentCoins();
                }
                catch { throw new Exception("out of keys");  }
            }
        }

        //saves on sql server
        public void Save(CoinContext db)
        {
            db.SaveChanges();
        }

        //loads the data from the DataBase
        public void Load()
        {
            using (var db = new CoinContext())
            {
                Coin c;
                DB = new List<Coin>();
                foreach (var i in db.CurrentCoins)
                {
                    c = new Coin(i.CurrentCoinValueId, new List<CoinValue>());
                    foreach (var item in db.CoinValues.OrderBy(d => d.date).Where(d => d.name == i.CurrentCoinValueId))
                    {
                        c.History.Add(new CoinValue(item.value, item.date));

                    }
                    DB.Add(c);
                }
                CurrentCoins = db.CurrentCoins.ToList();

            }
        }
    }
}