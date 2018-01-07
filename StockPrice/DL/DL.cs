using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.IO;
using System.Xml.Serialization;

namespace DL
{
    public class DAL:IDL
    {
        public List<Coin> DB;
        public List<CoinValue> getCoinHistory(string coin)
        {
            foreach (Coin c in DB)
                if (c.name == coin)
                    return c.History;
            return null;
        }
        public void AddCoin(Coin c)
        {
            DB.Add(c);
        }
        public void Save()
        {
            WriteToXmlFile<DAL>("DataBase", this);
        }
        public void Load()
        {
            DAL dal = ReadFromXmlFile<DAL>("DataBase");
            if (dal.DB == null)
                DB = new List<Coin>();   
            else
                DB = dal.DB;
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
