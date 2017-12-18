using System.Collections.Generic;
using System.Linq;

namespace Infragistics.Framework
{
    /// <summary>
    /// Represents a list of <see cref="OlympicMedals"/> objects
    /// </summary>
    public class OlympicResults : List<OlympicMedals>
    {
        public OlympicResults()
        {
            //var gold = this.Sum(item => item.Gold);
        }
        /// <summary> Gets or sets total of Gold medals </summary>
        public double TotalGold { get { return this.Sum(item => item.Gold); } }
        /// <summary> Gets or sets total of Silver medals </summary>
        public double TotalSilver { get { return this.Sum(item => item.Silver); } }
        /// <summary> Gets or sets total of Bronze medals </summary>
        public double TotalBronze { get { return this.Sum(item => item.Bronze); } }
        /// <summary> Gets or sets total medals </summary>
        public double TotalMedals { get { return this.Sum(item => item.Total); } }

        /// <summary> Gets or sets total medals' ranking using formula: 
        /// <para>3pts for each Gold, 2pts for each Silver and 1 for each Bronze medal</para></summary>
        public double TotalRanking { get { return this.Sum(item => item.Ranking); } }

        /// <summary> Gets or sets total olympics </summary>
        public int TotalOlympics { get { return this.Count; } }

        public override string ToString()
        {
            return TotalRanking + " " + TotalMedals + " medals in " + TotalOlympics + " Olympics";
        }

        /// <summary> Gets or sets Country </summary>
        public string Country { get; set; }

    }

}