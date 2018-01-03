using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    class BL
    {
        public CoinValue getCoinValue(string coin)
        {
            string url = "http://apilayer.net/api/live?access_key=0c54679e2255988cd03b9ed59129983a&currencies=" + coin + "&source=USD&format=1";
            WebClient wc = new WebClient();
            string apiResponse = wc.DownloadString(url);
            Console.WriteLine(apiResponse);
            return new CoinValue(0,DateTime.Now);
        }


    }
}
/*string request = String.Format("http://www.xe.com/ucc/convert.cgi?Amount={0}&From={1}&To={2}", value, inputCurrency, outputCurrency);
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
                return Double.Parse(apiResponse);*/