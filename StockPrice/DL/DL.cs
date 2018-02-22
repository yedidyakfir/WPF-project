using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.IO;
using System.Xml.Serialization;
using System.Net;

namespace DL
{
    public class DAL:IDL
    {
        public List<Coin> DB;
        public List<CurrentCoinValue> CurrentCoins;
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
                try
                {
                    List<CoinValue> l = getCoinHistory(coin);
                    l.Add(new CoinValue(value, DateTime.Now));
                }
                catch (Exception) { }
                Save();
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
                    if (c.name == coin)
                        return c.History;  
                
                DateTime t = DateTime.Now;
                t = t.AddYears(-3);
                string url;
                WebClient wc;
                List<CoinValue> l = new List<CoinValue>();

                for (int i = 0; i < 36; i++)
                {
                    url = "http://apilayer.net/api/historical?" +
                        "access_key=0c54679e2255988cd03b9ed59129983a" +
                        "&date=" + t.Year.ToString() + "-" + (t.Month < 10 ? ("0" + t.Month.ToString()) : t.Month.ToString()) + "-" + (t.Day < 10 ? ("0" + t.Day.ToString()) : t.Day.ToString()) +
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
                    t = t.AddMonths(1);
                }
                AddCoin(new Coin(coin, l));
                Save();
                return l;
            }
            catch (Exception e)
            {
                throw new Exception("There is no internet and the coin wasn't saved in the database");
            }
        }
        public void addCurrentCoinValue(CurrentCoinValue c)
        { CurrentCoins.Add(c); Save(); }
        public List<CurrentCoinValue> getCurrentCoins()
        { return CurrentCoins; }
        public void AddCoin(Coin c)
        {
            DB.Add(c);
            CurrentCoins.Add(new CurrentCoinValue(c.name, getCoinValue(c.name).value, DateTime.Now));
            Save();
        }
        public void Save()
        {
            WriteToXmlFile<DAL>("DataBase", this);
        }
        public void Load()
        {
            DAL dal = ReadFromXmlFile<DAL>("DataBase");
            if (dal.DB == null)
            {
                DB = new List<Coin>();
                CurrentCoins = new List<CurrentCoinValue>();
            }
            else
            {
                DB = dal.DB;
                CurrentCoins = dal.CurrentCoins;
            }
        }
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
        }
    }
}
