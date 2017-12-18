using System;
using System.Collections.Generic;
using System.Linq;

namespace Infragistics.Framework
{ 

    public class ScatterData : List<ScatterDataItem>
    {
        internal static Random Random = new Random();

        public static List<ScatterDataItem> GetBodyMassIndex(
            double heightMin, double heightMax, double massRange, double bmi)
        {
            var data = new List<ScatterDataItem>();
            for (var i = 0; i < 100; i++)
            {
                var height = (Random.NextDouble() * (heightMax - heightMin)) + heightMin;
                //r.nextInt(High-Low) + Low;
                var mass = bmi * height * height;
                var massMin = mass - massRange;
                var massMax = mass + massRange;
                mass = (Random.NextDouble() * (massMax - massMin)) + massMin;

                var newItem = new ScatterDataItem();
                newItem.X = mass;
                newItem.Y = height;
                data.Add(newItem);
            }
            data = data.SortByProperty("Y").ToList();

            return data;
        }
         
        /// <summary>
        /// Gets wind scatter data where x is wind direction and y wind is speed
        /// </summary>  
        public static List<ScatterDataItem> GetWindData(double offset = 1.0)
        {
            if (offset == 0) offset = 1.0;

            var data = new List<ScatterDataItem>();
            data.Add(new ScatterDataItem(0, 10 * offset));
            data.Add(new ScatterDataItem(45, 15 * offset));

            data.Add(new ScatterDataItem(90, 10 * offset));
            data.Add(new ScatterDataItem(135, 5 * offset));

            data.Add(new ScatterDataItem(180, 10 * offset));
            data.Add(new ScatterDataItem(225, 25 * offset));

            data.Add(new ScatterDataItem(270, 10 * offset));
            data.Add(new ScatterDataItem(315, 30 * offset));
            data.Add(new ScatterDataItem(360, 10 * offset));

            //data.sortByY();
            return data;
        }
    }
    public class ScatterDataItem : ObservableObject
    {
        public ScatterDataItem()
            : this(double.NaN, double.NaN, double.NaN, "")
        { }
        public ScatterDataItem(double x, double y)
            : this(x, y, double.NaN, "")
        { }
        public ScatterDataItem(double x, double y, double r)
            : this(x, y, r, "")
        { }
        public ScatterDataItem(double x, double y, double r, string label)
        {
            X = x;
            Y = y;
            R = r;
            Label = label;
        }

        /// <summary> Gets or sets Data </summary>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets Identifier
        /// </summary>
        public string Id { get; set; }

        private double _x;
        /// <summary>
        /// Gets or sets X
        /// </summary>
        public double X
        {
            get { return _x; }
            set { if (_x == value) return; _x = value; OnPropertyChanged("X"); }
        }

        private double _y;
        /// <summary>
        /// Gets or sets Y
        /// </summary>
        public double Y
        {
            get { return _y; }
            set { if (_y == value) return; _y = value; OnPropertyChanged("Y"); }
        }

        private double _r;
        /// <summary>
        /// Gets or sets R
        /// </summary>
        public double R
        {
            get { return _r; }
            set { if (_r == value) return; _r = value; OnPropertyChanged("R"); }
        }
        
        /// <summary> Gets or sets X </summary>
        public string Label { get; set; }

        public override string ToString()
        {
            return this.Id + " " + this.X + " " + this.Y + " " + this.R;
        }
    }
}