
namespace Infragistics.Framework
{
    /// <summary>
    /// Represents medals for a country at a given Olympic
    /// </summary>
    public class OlympicMedals  : ObservableObject
    {
        // using doubles in order to use Nan for not values

        public override string ToString()
        {
            return Year + ", " + Ranking + ", " + Total + " (" + Gold + ", " + Silver + ", " + Bronze + ") " + Country;
        }

        /// <summary> Gets or sets Country </summary>
        public string Country { get; set; }

        private int _year;
        /// <summary> Gets or sets Year </summary>
        public int Year
        {
            get { return _year;}
            set { if (_year == value) return; _year = value; OnPropertyChanged("Year"); }
        }

        private double _gold;
        /// <summary> Gets or sets Gold </summary>
        public double Gold
        {
            get { return _gold;}
            set { if (_gold == value) return; _gold = value; OnPropertyChanged("Gold"); }
        }

        private double _silver;
        /// <summary> Gets or sets Silver </summary>
        public double Silver
        {
            get { return _silver; }
            set { if (_silver == value) return; _silver = value; OnPropertyChanged("Silver"); }
        }
        private double _bronze;
        /// <summary> Gets or sets Bronze </summary>
        public double Bronze
        {
            get { return _bronze;}
            set { if (_bronze == value) return; _bronze = value; OnPropertyChanged("Bronze"); }
        }


        /// <summary> Gets or sets GoldDelta </summary>
        public double GoldDelta { get; set; }
        /// <summary> Gets or sets SilverDelta </summary>
        public double SilverDelta { get; set; }
        /// <summary> Gets or sets BronzeDelta </summary>
        public double BronzeDelta { get; set; }
        /// <summary> Gets or sets TotalDelta </summary>
        public double TotalDelta { get; set; }

      
        //public double GoldRatio { get { return Gold / Total * 100; } }
        //public double SilverRatio { get { return Silver / Total * 100; } }
        //public double BronzeRatio { get { return Bronze / Total * 100; } }
  
        /// <summary> Gets or sets PropertyName </summary>
        public double Ranking { get { return (Gold * 3) + (Silver * 2) + (Bronze); } }
  
        /// <summary> Gets or sets PropertyName </summary>
        public double Total { get { return Gold + Silver + Bronze; } }
    }
}