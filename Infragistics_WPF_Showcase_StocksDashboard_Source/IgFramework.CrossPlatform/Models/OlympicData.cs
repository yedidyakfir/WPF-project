using System.Collections.Generic;
using System.Linq;

namespace Infragistics.Framework
{
    /// <summary>
    /// Represents a provider with Olympics data   
    /// </summary>
    public static class OlympicData
    {
        static OlympicData()
        {
            var csv = DataProvider.GetCsvTable("world-olympics.csv");

            var countries = new Dictionary<string, Dictionary<int, OlympicMedals>>();
            var years = new List<int>();

            foreach (var row in csv.Rows)
            {
                if (row == csv.Rows[0])
                    continue; // skip csv header

                var year = int.Parse(row[0]);
                var gold = double.Parse(row[2]);
                var silver = double.Parse(row[3]);
                var bronze = double.Parse(row[4]);

                // add olympic results for countries that spitted
                var names = row[1].Split(';');
                foreach (var name in names)
                {
                    var item = new OlympicMedals
                        {
                            Gold = gold, Silver = silver, Bronze = bronze, 
                            Country = name, Year = year
                        };
                    
                    if (countries.ContainsKey(item.Country))
                    {
                        if (!countries[item.Country].ContainsKey(year))
                             countries[item.Country].Add(year, item);
                    }
                    else
                    {
                        var medals = new Dictionary<int, OlympicMedals>();
                        medals.Add(year, item);
                        countries.Add(item.Country, medals);
                    }
                }

                if (!years.Contains(year))
                     years.Add(year);
            }
            //years.Add(1916);
            //years.Add(1940);
            //years.Add(1944);

            years.Sort();

            ResultsByCountry = new Dictionary<string, OlympicResults>();
            MedalsByCountry = new Dictionary<string, OlympicMedals>();
            foreach (var pair in countries)
            {
                var country = pair.Key;
                var results = pair.Value;
                ResultsByCountry.Add(country, new OlympicResults { Country = country });

                var previous = new OlympicMedals {Year = years.First() - 4, Country = country};
                
                foreach (var year in years)
                {
                    // add Nan for a country that was not at the olympic 
                    if (!results.ContainsKey(year))
                         results.Add(year, new OlympicMedals { Year = year, Country = country });
                    else
                    {
                        results[year].GoldDelta = results[year].Gold - previous.Gold;
                        results[year].SilverDelta = results[year].Silver - previous.Silver;
                        results[year].BronzeDelta = results[year].Bronze - previous.Bronze;
                        results[year].TotalDelta = results[year].Total - previous.Total;

                        previous = results[year];
                    }
                    ResultsByCountry[country].Add(results[year]);
                }

                var item = new OlympicMedals();
                item.Year = years.Last();
                item.Country = country;
                item.Gold = ResultsByCountry[country].TotalGold;
                item.Silver = ResultsByCountry[country].TotalSilver;
                item.Bronze = ResultsByCountry[country].TotalBronze;
                
                MedalsByCountry.Add(country, item);
            }

        }

        /// <summary> Gets or sets results by country </summary>
        public static Dictionary<string, OlympicResults> ResultsByCountry { get; set; }
        /// <summary> Gets or sets results list sorted by medals ranking</summary>
        public static List<OlympicResults> Results { get { return GetResults(); } }

        internal static List<OlympicResults> GetResults()
        {
            var list = ResultsByCountry.Values.ToList();
            list = list.OrderByDescending(i => i.TotalRanking).ToList();
            return list;
        }

        /// <summary> Gets or sets medals by country </summary>
        public static Dictionary<string, OlympicMedals> MedalsByCountry { get; set; }
        /// <summary> Gets or sets medals list sorted by medals ranking</summary>
        public static List<OlympicMedals> Medals { get { return GetMedals(); } }

        internal static List<OlympicMedals> GetMedals()
        {
            var list = MedalsByCountry.Values.ToList();
            list = list.OrderByDescending(i => i.Ranking).ToList();
            return list;
        }

        ///// <summary> Gets or sets results   </summary>
        //public static List<OlympicMedals> ResultsToDate { get; set; }


    }

}