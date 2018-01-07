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
        static void Main(string[] args)
        {
            IBL gogo = new BL();
            foreach (var cv in gogo.getCoinHistory("ILS"))
            {
                Console.WriteLine("Date: " + cv.date.ToString());
                Console.WriteLine(cv.value);               
            }
            Console.ReadKey();
        }
    }
}
