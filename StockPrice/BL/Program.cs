using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BE;
using BL;

namespace BL
{
    class Program
    {
        static void Main(string[] args)
        { 
            IBL gogo = new Bl();
            gogo.getCoinHistory("CAD");
            gogo.getCoinHistory("EUR");
            gogo.getCoinHistory("CHF");
            gogo.getCoinHistory("GBP");
            gogo.getCoinHistory("ILS");
            gogo.getCoinHistory("JPY");
            gogo.getCoinHistory("SGD");
            gogo.getCoinHistory("ZAR");

            //Console.WriteLine("The slope of Israel new Shekel is - {0}", gogo.GetSlope("ILS"));

            foreach (var item in gogo.getCoinsValue())
            {
                Console.WriteLine(item.CurrentCoinValueId);
                foreach (var item2 in gogo.getCoinHistory(item.CurrentCoinValueId))
                {
                    Console.WriteLine(item2.CoinValueId);
                    Console.WriteLine(item2.date);
                }
            }
            foreach (var item in gogo.getCoinsValue())
            {
                Console.WriteLine(item.CurrentCoinValueId);
            }



            Console.ReadKey();
        }
    }
}
