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
        public CoinValue getCoinValue(string coin)
        {
            if (getCoinHistory(coin) != null &&
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
            }
        }
        public List<CoinValue> getCoinHistory(string coin)
        {
            try
            {
                foreach (Coin c in DB)
                    if (c.CoinId == coin)
                        return c.History;

                DateTime t = DateTime.Now;
                t = t.AddYears(-3);
                string url;
                WebClient wc;
                List<CoinValue> l = new List<CoinValue>();

                for (int i = 0; i < 36; i++)
                {
                    url = "http://apilayer.net/api/historical?" +
                        "access_key=7017c5933ab8aaa1d0078693a5b1b8a9" +
                        "&date=" + t.Year.ToString() + "-" + (t.Month < 10 ? ("0" + t.Month.ToString()) : t.Month.ToString()) + "-" + (t.Day < 10 ? ("0" + t.Day.ToString()) : t.Day.ToString()) +
                        "&currencies=" + coin +
                        "&format=1";
                    wc = new WebClient();
                    string apiResponse = wc.DownloadString(url);
                    int index = apiResponse.IndexOf("USD" + coin);
                    apiResponse = apiResponse.Substring(index + 8, 8);
                    double value = Double.Parse(apiResponse);
                    value = 1 / value;
                    l.Add(new CoinValue(value, t));
                    t = t.AddMonths(1);
                }
                AddCoin(new Coin(coin, l));
                return l;
            }
            catch (Exception e)
            {
                throw new Exception("There is no internet and the coin wasn't saved in the database");
            }
        }
        public void addCurrentCoinValue(CurrentCoinValue c)
        {
            using (var db = new CoinContext())
            {
                CurrentCoins.Add(c);
                db.CurrentCoins.Add(c);
                Save(db);
            }
        }
        public List<CurrentCoinValue> getCurrentCoins()
        { return CurrentCoins; }
        public void AddCoin(Coin c)
        {
            using (var db = new CoinContext())
            {
                try
                {
                    DB.Add(c);
                    CurrentCoins.Add(new CurrentCoinValue(c.CoinId, getCoinValue(c.CoinId).CoinValueId, DateTime.Now));
                    foreach (var item in c.History)
                    {
                        db.CoinValues.Add(new CoinValueForDB(c.CoinId, item.CoinValueId, item.date));
                    }
                    db.CurrentCoins.Add(new CurrentCoinValue(c.CoinId, getCoinValue(c.CoinId).CoinValueId, DateTime.Now));
                    Save(db);
                }
                catch (Exception e)
                {
                    throw new Exception("I think the Coin is already in the database");
                }
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
                Coin c;
                DB = new List<Coin>();
                foreach (var i in db.CurrentCoins)
                {
                    c = new Coin(i.CurrentCoinValueId, new List<CoinValue>());
                    foreach (var item in db.CoinValues.OrderBy(d => d.date).Where(d => d.name == i.CurrentCoinValueId))
                    {
                        c.History.Add(new CoinValue(item.CoinValueForDBId, item.date));

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