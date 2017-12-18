using System;
using System.Collections.Generic;

namespace Infragistics.Framework 
{
    public class ValueScale : ObservableObject
    {
        protected const double DefaultMinimum = 0.0;
        protected const double DefaultMaximum = 100.0;
        protected const int DefaultLogarithmBase = 10;

        #region MinimumValue 
        private double _minimumValue = DefaultMinimum;
        /// <summary>
        /// Gets or sets MinimumValue
        /// </summary>
        public double MinimumValue
        {
            get { return _minimumValue;}
            set { if (_minimumValue == value) return; _minimumValue = value; OnPropertyChanged("MinimumValue"); }
        }

        #endregion

        #region MaximumValue 
        private double _maximumValue = DefaultMaximum;
        /// <summary>
        /// Gets or sets MaximumValue
        /// </summary>
        public double MaximumValue
        {
            get { return _maximumValue;}
            set { if (_maximumValue == value) return; _maximumValue = value; OnPropertyChanged("MaximumValue"); }
        }

        #endregion

        #region IsLogarithmic 
        private bool _isLogarithmic = false;
        /// <summary>
        /// Gets or sets IsLogarithmic
        /// </summary>
        public bool IsLogarithmic
        {
            get { return _isLogarithmic;}
            set { if (_isLogarithmic == value) return; _isLogarithmic = value; OnPropertyChanged("IsLogarithmic"); }
        }

        #endregion

        #region LogarithmBase 
        private int _logarithmBase = DefaultLogarithmBase;
        /// <summary>
        /// Gets or sets LogarithmBase
        /// </summary>
        public int LogarithmBase
        {
            get { return _logarithmBase;}
            set { if (_logarithmBase == value) return; _logarithmBase = value; OnPropertyChanged("LogarithmBase"); }
        }

        #endregion

        #region Methods
        public virtual double GetScaledValue(double value)
        {
            var min = double.IsNaN(this.MinimumValue) || double.IsInfinity(this.MinimumValue) ? DefaultMinimum : this.MinimumValue;
            var max = double.IsNaN(this.MaximumValue) || double.IsInfinity(this.MaximumValue) ? DefaultMaximum : this.MaximumValue;

            if (value < min) value = min;
            if (value > max) value = max;

            return GetLogarithmicScaledValue(min, max, value);
        }
        protected double GetLogarithmicScaledValue(double min, double max, double value)
        {
            if (this.IsLogarithmic && this.LogarithmBase > 0)
            {
                var newMin = System.Math.Log(min, this.LogarithmBase);
                if (double.IsNegativeInfinity(newMin)) newMin = min;
                var newMax = System.Math.Log(max, this.LogarithmBase);
                if (double.IsNegativeInfinity(newMax)) newMax = max;
                var newValue = System.Math.Log(value, this.LogarithmBase);
                if (double.IsNegativeInfinity(newValue)) newValue = min;

                return GetLinearScaledValue(newMin, newMax, newValue);
            }
            return GetLinearScaledValue(min, max, value);
        }
        protected double GetLinearScaledValue(double min, double max, double value)
        {
            //if the value is outside the range, return a null brush
            //if (value < min || value > max) return double.NaN;
            if (value < min) return min;
            if (value > max) return max;

            var scaledValue = (value - min) / (max - min);
            return scaledValue;
        }
        #endregion
    }

    public class SizeScale : ValueScale
    {
        protected const double DefaultMinimumSize = 10.0;
        protected const double DefaultMaximumSize = 50.0;

        public SizeScale()
        {

        }

        #region MinimumSize 

        private double _minimumSize = DefaultMinimumSize;
        /// <summary>
        /// Gets or sets MinimumSize
        /// </summary>
        public double MinimumSize
        {
            get { return _minimumSize;}
            set { if (_minimumSize == value) return; _minimumSize = value; OnPropertyChanged("MinimumSize"); }
        }

        #endregion

        #region MaximumSize 
        private double _maximumSize = DefaultMaximumSize;
        /// <summary>
        /// Gets or sets MaximumSize
        /// </summary>
        public double MaximumSize
        {
            get { return _maximumSize;}
            set { if (_maximumSize == value) return; _maximumSize = value; OnPropertyChanged("MaximumSize"); }
        } 
        #endregion


        #region Methods

        public override double GetScaledValue(double value)
        {
            return GetScaledSize(value);
        }

        private double GetScaledSize(double value)
        {
            var min = double.IsNaN(this.MinimumValue) || double.IsInfinity(this.MinimumValue) ? 0 : this.MinimumValue;
            var max = double.IsNaN(this.MaximumValue) || double.IsInfinity(this.MaximumValue) ? 10 : this.MaximumValue;

            var smallSize = double.IsNaN(this.MinimumSize) || double.IsInfinity(this.MinimumSize) ? 10 : this.MinimumSize;
            var largeSize = double.IsNaN(this.MaximumSize) || double.IsInfinity(this.MaximumSize) ? 50 : this.MaximumSize;

            if (value < min) value = min;
            if (value > max) value = max;

            if (this.IsLogarithmic && this.LogarithmBase > 0)
            {
                var logBase = this.LogarithmBase;
                //return GetLogarighmicSize(min, max, min, max, logBase, value);
                return GetLogarighmicSize(min, max, smallSize, largeSize, logBase, value);
            }

            return GetLinearSize(min, max, smallSize, largeSize, value);
        }
        /// <summary>
        /// Returns the a marker size for a given value based on a linear scale.
        /// </summary>
        internal static double GetLinearSize(double min, double max, double smallSize, double largeSize, double value)
        {
            //smaller than min size or invalid
            if (value <= min || double.IsNaN(value) || double.IsInfinity(value))
            {
                return smallSize;
            }

            if (value >= max)
            {
                return largeSize;
            }
            //double result = Clamp((value - min) / (max - min), min, max);
            //double result = Clamp((value - min) / (max - min), 0.0, 1.0);
            var result = smallSize + ((largeSize - smallSize) / (max - min)) * (value - min);
            return result;
        }
        public static double Clamp(double value, double minimum, double maximum)
        {
            return System.Math.Min(maximum, System.Math.Max(minimum, value));
        }
        /// <summary>
        /// Returns the marker size for a given value based on a logarithmic scale.
        /// </summary>
        internal static double GetLogarighmicSize(double min, double max, double smallSize, double largeSize, double logBase, double value)
        {
            var newValue = System.Math.Log(value, logBase);
            var newMin = System.Math.Log(min, logBase);
            if (double.IsNegativeInfinity(newMin)) newMin = min;
            var newMax = System.Math.Log(max, logBase);
            if (double.IsNegativeInfinity(newMax)) newMax = max;

            return GetLinearSize(newMin, newMax, smallSize, largeSize, newValue);
        }

        #endregion

    }
    
    public class RangeScale
    {
        #region Properties
        /// <summary> Gets or sets Absolute Minimum </summary>
        public double AbsoluteMinimum { get; private set; }

        /// <summary> Gets or sets Absolute Minimum </summary>
        public double AbsoluteMaximum { get; private set; }

        /// <summary> Gets or sets Absolute Minimum </summary>
        public double AbsoluteStep { get; set; }
        
        /// <summary> Gets or sets Absolute Range </summary>
        public double AbsoluteRange { get; private set; }
        /// <summary> Gets or sets Absolute Interval </summary>
        public double AbsoluteInterval { get; private set; }

        public double MajorInterval { get; private set; }
        public double MinorInterval { get; private set; }
        public double MajorCount { get; private set; }
        public double MinorCount { get; private set; }

        /// <summary> Gets or sets Range Minimum </summary>
        public double RangeMinimum { get; set; }
        /// <summary> Gets or sets Range Maximum </summary>
        public double RangeMaximum { get; set; }
        /// <summary> Gets or sets Range Midpoint </summary>
        public double RangeMidpoint { get; private set; }
        
        protected double RangeSize { get; private set; }
        protected double RangeDelta { get; private set; }

        protected double RangeLog { get; set; }
        protected double RangePow { get; set; }

        protected const double Precision = 1.01;
        public List<double> MajorValues { get; private set; }
        public List<double> MinorValues { get; private set; }

        #endregion
        public RangeScale(double min, double max, double step = 2.0)
        {
            AbsoluteStep = step;
            RangeMinimum = min;
            RangeMaximum = max;
            Compute();
        }
        public RangeScale() : this(0, 100)
        {

        }
        
        private void ValidateRange()
        {
            //Check if the max and min are the same
            if (RangeMinimum == RangeMaximum)
            {
                RangeMaximum *= Precision;
                RangeMinimum /= Precision;
            }
            //Check if dMax is bigger than dMin - swap them if not
            if (RangeMaximum < RangeMinimum)
            {
                var temp = RangeMinimum;
                RangeMinimum = RangeMaximum;
                RangeMaximum = temp;
            }

            //Make dMax a little bigger and dMin a little smaller (by 1% of their difference)
            RangeDelta = (RangeMaximum - RangeMinimum);
            RangeSize = RangeDelta / 2;
            RangeMidpoint = (RangeMaximum + RangeMinimum) / 2;

            RangeMaximum = RangeMidpoint + Precision * RangeSize;
            RangeMinimum = RangeMidpoint - Precision * RangeSize;

            //What if they are both 0?
            if (RangeMaximum == 0 && RangeMinimum == 0)
            {
                RangeMaximum = 1;
            }
        }
        
        private void ComputeIntervals()
        {
            // Find the scaling factor
            RangeLog = Math.Log(RangeDelta) / Math.Log(10);
            RangePow = Math.Pow(10, RangeLog - Math.Floor(RangeLog));
            if (RangePow > 0 && RangePow <= 2.5)
            {
                MajorInterval = 0.2;
                MinorInterval = 0.05;
            }
            else if (RangePow > 2.5 && RangePow < 5)
            {
                MajorInterval = 0.5;
                MinorInterval = 0.1;
            }
            else if (RangePow > 5 && RangePow < 7.5)
            {
                MajorInterval = 1;
                MinorInterval = 0.2;
            }
            else
            {
                MajorInterval = 2;
                MinorInterval = 0.5;
            }
            MajorInterval = Math.Pow(10, Math.Floor(RangeLog)) * MajorInterval;
            MinorInterval = Math.Pow(10, Math.Floor(RangeLog)) * MinorInterval;
            MajorCount = (int)Math.Ceiling(RangeDelta / MajorInterval);
            MinorCount = (int)Math.Ceiling(RangeDelta / MinorInterval);
        }

        private void ComputeRange()
        {
            var logRange = Math.Log10(RangeSize);       // log10
            var logRangeInt = (int)logRange;            // integer part
            var logRangeMod = logRange - logRangeInt;   // fractional part
            if (logRangeMod < 0) // adjust if negative
            {                       
                logRangeMod += 1;
                logRangeInt -= 1;
            } 
            /* increase log range rest to the boundaries you like */
            if (logRangeMod < Math.Log10(2)) 
                logRangeMod = Math.Log10(2);
            else if (logRangeMod < Math.Log10(2.5)) 
                     logRangeMod = Math.Log10(2.5);
            else if (logRangeMod < Math.Log10(5)) 
                     logRangeMod = Math.Log10(5);
            else 
                logRangeMod = /* log10(10) */ 1;

            //AbsoluteMinimum = Math.Pow(10, logRangeInt + logRangeMod);

            var floor = (int)Math.Floor(RangeMinimum / MajorInterval);
            AbsoluteMinimum = floor * MajorInterval;
            var ceiling = (int)Math.Ceiling(RangeMaximum / MajorInterval);
            AbsoluteMaximum = ceiling * MajorInterval;

            AbsoluteRange = AbsoluteMaximum - AbsoluteMinimum;
            AbsoluteInterval = AbsoluteRange / AbsoluteStep;
        }

        public void Compute()
        {
            ValidateRange();

            ComputeIntervals();
            ComputeRange();

            ComputeMajorValues();
            ComputeMinorValues();
        }

        private void ComputeMajorValues()
        {
            var values = new List<double>();
            for (var i = 0; i < MajorCount; i++)
            {
                values.Add(AbsoluteMinimum + MajorInterval * i);
            }
            MajorValues = values;
        }
        private void ComputeMinorValues()
        {
            var values = new List<double>();
            for (var i = 0; i < MinorCount; i++)
            {
                values.Add(AbsoluteMinimum + MinorInterval * i);
            }
            MinorValues = values;
        }
        
    }

}