using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DL;

namespace BL
{
    class Bl : IBL
    {
        private IDL Dal = new DAL();

        public Bl()
        {
            Dal.Load();
        }

        public CoinValue getCoinValue(string coin)
        {
            return Dal.getCoinValue(coin);
        }

        //returns the relation of values between two coins
        public double Relation(string coin1, string coin2, double amount)
        {
            return (getCoinValue(coin1).CoinValueId / getCoinValue(coin2).CoinValueId) * amount;
        }

        public List<CoinValue> getCoinHistory(string coin,string format = "day")
        {
            List<CoinValue> l = new List<CoinValue>();
            List<CoinValue> temp = Dal.getCoinHistory(coin).OrderBy(d => d.date).ToList();
            if (format == "day")
                return temp;
            else if(format == "month")
                for (int i = 0; i < Dal.getCoinHistory(coin).Count; i = i+30)
                    l.Add(temp.ToArray()[i]);
            else
                for (int i = 0; i < Dal.getCoinHistory(coin).Count; i = i + 182)//every half a year
                    l.Add(temp.ToArray()[i]);
            return l;             
        }

        public List<CurrentCoinValue> getCoinsValue()
        {
            return Dal.getCurrentCoins();
        }
        

        //returns the slope of a coin - calculated recusivly as such:
        //Slope(Now) = ( (valueNow - vlaueYesterday)/(dateNow - dateYesterday) )*0.1 + Slop(Yesterday)*0.9
        public double GetSlope(string coin)
        {
            return getSlope(coin, new List<CoinValue>(Dal.getCoinHistory(coin)));
        }

        private double getSlope(string coin, List<CoinValue> history)
        {
            if (history.Count <= 1)
                return 0;
            CoinValue Current = history.Last();
            history.Remove(history.Last());
            CoinValue Previous = history.Last();
            return 0.1 * ((Current.CoinValueId - Previous.CoinValueId) / (Current.date - Previous.date).TotalDays) + 0.9 * getSlope(coin, history);
        }
    }
}
