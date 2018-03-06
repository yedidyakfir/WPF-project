using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BE;

namespace DL
{
    public class CoinContext : DbContext
    {
        //public DbSet<Coin> Coins { get; set; }
        public DbSet<CurrentCoinValue> CurrentCoins { get; set; }
        public DbSet<CoinValueForDB> CoinValues { get; set; }
        public CoinContext() : base() { }
    }
}
