using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    class Program
    {
        class g
        {
            public static List<CoinValue> l = new List<CoinValue>();
            public List<CoinValue> get() { return l; }
        };
        static void Main(string[] args)
        {
            g d = new g();
            List<CoinValue> l = d.get();
            l.Add(new CoinValue(23, DateTime.Now));
            Console.WriteLine(g.l.First().value);
            Console.WriteLine(g.l.First().date);
            Console.ReadKey();
        }
    }
}
