namespace Infragistics.Framework
{
    /// <summary>
    /// Represents data indicator with info about numeric data column
    /// </summary>
    public class DataIndicator : DataColumn
    {
        public DataIndicator()
        {
            Format = "0:0";
            IsLogarithmic = true;
            Min = double.MaxValue;
            Max = double.MinValue;
            Sum = 0;
            Average = 0;
        }

        public new string ToString()
        {
            var format = "{" + Format + "}";
            var range = string.Format(format, Min) + " " +
                        string.Format(format, Max);
            return Label + " {" + range + "}";
        }
        /// <summary> Gets or sets value format </summary>
        public string Format { get; set; }

        /// <summary> Gets or sets IsLogarithmic </summary>
        public bool IsLogarithmic { get; set; }

        /// <summary> Gets or sets Minimum </summary>
        public double Min { get; set; }

        /// <summary> Gets or sets Maximum </summary>
        public double Max { get; set; }

        /// <summary> Gets or sets Sum </summary>
        public long Sum { get; set; }

        /// <summary> Gets or sets Sum </summary>
        public double Range { get { return System.Math.Abs(Max - Min); } }

        /// <summary> Gets or sets Average </summary>
        public double Average { get; set; }

    }

    /// <summary>
    /// Represents data column with info  
    /// </summary>
    public class DataColumn
    {
        /// <summary> Gets or sets Name </summary>
        public string Label { get; set; }
        /// <summary> Gets or sets Key </summary>
        public string Key { get; set; }
        /// <summary> Gets or sets Description </summary>
        public string Description { get; set; }
        /// <summary> Gets or sets Symbol </summary>
        public string Symbol { get; set; }

        /// <summary> Gets or sets Tooltip </summary>
        public string Tooltip { get; set; }

        /// <summary> Gets or sets ImagePath </summary>
        public string ImagePath { get; set; }

        ///// <summary> Gets or sets Label </summary>
        //public string Label { get; set; }

        public new string ToString()
        {
            return Label + " {" + this.Description + "}";
        }
    }
}