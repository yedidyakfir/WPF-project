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
{//sqlcmd -S YEHUDA\SQLEXPRESS -E
    public class DAL : IDL
    {
        public List<Coin> DB;
        public List<CurrentCoinValue> CurrentCoins;
        private int num = 0;
        private string[] keys = { "7017c5933ab8aaa1d0078693a5b1b8a9" , "e8c88f3997a6810671c6c70eb42dff3e" , "2c7dd5dc25ea2f920e1cad623c2f81a0" , "914f8d9624be9124eac51795568dd89f" , "e46b033087886daaa2fa2d8c7bb43abe" , "573ef63ec60efa508997c45f3315bb54", "209738903e50af28323b57b984be1495" };

        public CoinValue getCoinValue(string coin)
        {
            return new CoinValue(getCurrentCoins().First(d => d.CurrentCoinValueId == coin).value,
                getCurrentCoins().First(d => d.CurrentCoinValueId == coin).date);
          /*  if (getCoinHistory(coin) != null &&
                getCoinHistory(coin).Last().date.Year == DateTime.Now.Year &&
                getCoinHistory(coin).Last().date.Month == DateTime.Now.Month)
                return getCoinHistory(coin).Last();
            try
            {
                string url = "http://apilayer.net/api/live?access_key=7017c5933ab8aaa1d0078693a5b1b8a9&currencies=" + coin + "&format=1";
                WebClient wc = new WebClient();
                string apiResponse = wc.DownloadString(url);
                int index = apiResponse.IndexOf("USD" + coin);
                apiResponse = apiResponse.Substring(index + 8, 8);
                double value = Double.Parse(apiResponse);
                value = 1 / value;

                using (var db = new CoinContext())
                {
                    List<CoinValue> l = getCoinHistory(coin);
                    l.Add(new CoinValue(value, DateTime.Now));
                    db.CoinValues.Add(new CoinValueForDB(coin, value, DateTime.Now));
                    Save(db);
                }
                return new CoinValue(value, DateTime.Now);
            }
            catch (Exception)//incase there's a problam with the internet
            {
                List<int> l = new List<int>();
                if (getCoinHistory(coin) != null)
                    return getCoinHistory(coin).Last();
                throw new Exception("There is no internet and the coin wasn't saved in the database");
            }*/
        }
        public List<CoinValue> getCoinHistory(string coin)
        {
            try
            {
                if(DB.Count != 0 && DB.First().History.Count != 0)
                    if ((DB.First().History.Max(d => d.date).Day == DateTime.Now.Day ||
                        DB.First().History.Max(d => d.date).Day == DateTime.Now.Day-1) &&
                        DB.First().History.Max(d => d.date).Year == DateTime.Now.Year &&
                        DB.First().History.Max(d => d.date).Month == DateTime.Now.Month)
                        return DB.First(d => d.CoinId == coin).History;
                
                DateTime t = (DB.Count == 0 || DB.First().History.Count == 0) ? (DateTime.Now.AddYears(-1)): DB.First().History.Max(d => d.date);
             
                string url;
                WebClient wc;
                List<CoinValue> l = new List<CoinValue>();

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
                    { apiResponse = wc.DownloadString(url); }
                    catch { return DB.First(d => d.CoinId == coin).History; }//incase there's a problam with the internet
                    /////////////////////////////////////////
                    int index = apiResponse.IndexOf("USD");
                    apiResponse = apiResponse.Substring(index + 3);
                    index = apiResponse.IndexOf("USD");
                    apiResponse = apiResponse.Substring(index + 3);

                    string name;
                    double value;
                    //CurrentCoins = new List<CurrentCoinValue>();

                    while (index != -1)
                    {
                        name = apiResponse.Substring(0, 3);
                        if (name == "USD")
                        {
                            apiResponse = apiResponse.Substring(3);
                            index = apiResponse.IndexOf("USD");
                            apiResponse = apiResponse.Substring(index + 3);
                            name = apiResponse.Substring(0, 3);
                        }
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
                    /////////////////////////////////////////
                }
                return DB.First(d => d.CoinId == coin).History;
            }
            catch (Exception)
            {
                try
                {
                    num++;
                    return getCoinHistory(coin);
                }
                catch (Exception)
                { throw new Exception("There is no internet and the coin wasn't saved in the database"); }           
            }
        }
        public List<CurrentCoinValue> getCurrentCoins()
        {
            if ((CurrentCoins.Count !=0) &&
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

                while (index != -1)
                {
                    name = apiResponse.Substring(0, 3);
                    if(name == "USD")
                    {
                        apiResponse = apiResponse.Substring(3);
                        index = apiResponse.IndexOf("USD");
                        apiResponse = apiResponse.Substring(index + 3);
                        name = apiResponse.Substring(0, 3);
                    }

                    if(name == "ZWL")
                        value = Double.Parse(apiResponse.Substring(5,3));
                    else
                        value = Double.Parse(apiResponse.Substring(5, apiResponse.Substring(5).IndexOf(',')));
                    value = 1 / value;
                    CurrentCoins.Add(new CurrentCoinValue(name, value, DateTime.Now));

                    if (!DB.Exists(c => c.CoinId == name))
                        DB.Add(new Coin(name, new List<CoinValue>()));
                    //DB.First(d => d.CoinId == name).History.Add(new CoinValue(value, DateTime.Now));
                   // using (var db = new CoinContext())
                     //   { db.CoinValues.Add(new CoinValueForDB(name, (db.CoinValues.Count() == 0)?0:(db.CoinValues.Max(d => d.CoinValueForDBId) + 0.01), value, DateTime.Now)); }

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
                    num++;
                    return getCurrentCoins();
                }
                catch { throw new Exception("out of keys");  }
            }
        }
        public void Save(CoinContext db)
        {
            db.SaveChanges();
            // WriteToXmlFile<DAL>("DataBase", this);
        }
        public void Load()
        {
            using (var db = new CoinContext())
            {
                //db.CoinValues.RemoveRange(db.CoinValues.Where(d => (d.name == "AUD") || (d.name == "CNY")));
                //db.CurrentCoins.RemoveRange(db.CurrentCoins.Where(d => (d.CurrentCoinValueId == "AUD") || (d.CurrentCoinValueId == "CNY")));
                //db.SaveChanges();nn
                //db.Dispose();
                /*DAL dal = ReadFromXmlFile<DAL>("DataBase");
                DB = dal.DB;
                CurrentCoins = dal.CurrentCoins;
                  foreach (var coin in dal.DB)
                  {
                    if(coin.CoinId != "EUR" && coin.CoinId != "ILS")
                     // db.Coins.Add(coin);
                     foreach (var item in coin.History)
                     {
                         db.CoinValues.Add(new CoinValueForDB(coin.CoinId,item.CoinValueId,item.date));
                     }
                  }
                  foreach (var currentCoin in dal.CurrentCoins)
                  {
                    if (currentCoin.CurrentCoinValueId != "EUR" && currentCoin.CurrentCoinValueId != "ILS")
                        db.CurrentCoins.Add(currentCoin);
                  }
                  db.SaveChanges();*/
              //  db.CoinValues.RemoveRange(db.CoinValues);
              //  db.CurrentCoins.RemoveRange(db.CurrentCoins);
                //  CurrentCoins = new List<CurrentCoinValue>();
                //  DB = new List<Coin>();
                //  getCurrentCoins();
                //getCoinHistory("ILS");
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
        /*
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        /// <summary>
        /// Reads an object instance from an XML file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }*/


    }
}