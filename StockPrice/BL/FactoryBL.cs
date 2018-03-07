using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class FactoryBL
    {
        private static IBL bl;
        public static IBL get()
        {
            if (bl == null)
                bl = new BL();
            return bl;
        }
    }
}
