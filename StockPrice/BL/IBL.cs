using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface IBL
    {
        //returns the relation (coin1/coin2)*amount
        double Relation(string inputCurrency, string outputCurrency, double value);

        //returns the CoinValue of 'coin', in the date closest to 'date'
        List<CurrentCoinValue> getCoinsValue();

        //returns a list of CoinValue of the coin
        List<CoinValue> getCoinHistory(string coin);  

        void addCurrentCoinValue(CurrentCoinValue c);

    }
}
