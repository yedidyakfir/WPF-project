﻿using System;
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
            gogo.getCoinsValue();
            Console.ReadKey();
        }
    }
}
