using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class BL
    {
        private double GetConvertedCurrencyValue(string inputCurrency, string outputCurrency, double value)
        {
            string request = String.Format("http://www.xe.com/ucc/convert.cgi?Amount={0}&From={1}&To={2}", value, inputCurrency, outputCurrency);
            Console.WriteLine(request);
            Console.Read();
            WebClient wc = new WebClient();
            string apiResponse = wc.DownloadString(request);    // This is a blocking operation.
            int index = apiResponse.IndexOf(@"class='uccResultUnit' data-amount=");
            apiResponse = apiResponse.Substring(index + 34);
            index = apiResponse.IndexOf("=");
            apiResponse = apiResponse.Substring(index + 2);
            apiResponse = apiResponse.Substring(0, 4);
            wc.Dispose();
            return Double.Parse(apiResponse);
        }
    }
}
