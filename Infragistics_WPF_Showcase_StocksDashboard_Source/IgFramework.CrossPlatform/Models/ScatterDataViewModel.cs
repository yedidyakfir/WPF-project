using System; 
using System.Collections.Generic; 
using System.ComponentModel;
using System.Linq; 

namespace Infragistics.Framework
{

    public class ScatterDataViewModel : DataViewModel
    {
        public ScatterDataViewModel()
        {
            DataLabelFormatX = "0:0";
            DataLabelFormatY = "0:0";
            DataLabelFormatR = "0:0";

            this.DataPoints = new List<ScatterDataItem>();

            //this.PropertyChanged += OnModelPropertyChanged;
        }

        public override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "DataSource")
            {
                InitializeDataPoints();
            }
            else if (propertyName == "DataMemberX" ||
                     propertyName == "DataMemberY" ||
                     propertyName == "DataMemberR")
            {
                UpdateDataPoints();

                if (propertyName == "DataMemberX")
                {
                    if (IsValidIndicator(DataMemberX))
                        this.DataLabelFormatX = Indicators[DataMemberX].Format;
                }
                if (propertyName == "DataMemberY")
                {
                    if (IsValidIndicator(DataMemberY))
                        this.DataLabelFormatY = Indicators[DataMemberY].Format;
                }
                if (propertyName == "DataMemberR")
                {
                    if (IsValidIndicator(DataMemberR))
                        this.DataLabelFormatR = Indicators[DataMemberR].Format;
                }
            }
        }

        // Initialize
        private void InitializeDataPoints()
        {
            this.DataPoints.Clear();

            foreach (var dataObject in this.DataView)
            {
                var item = new ScatterDataItem();

                if (IsValidIndicator(this.DataMemberX))
                    item.X = dataObject.GetPropertyValue<double>(this.DataMemberX);

                if (IsValidIndicator(this.DataMemberY))
                    item.Y = dataObject.GetPropertyValue<double>(this.DataMemberY);

                //TODO scale value here
                if (IsValidIndicator(this.DataMemberR))
                {
                    var indicator = Indicators[this.DataMemberR];
                    DataScaleR.MinimumValue = indicator.Min;
                    DataScaleR.MaximumValue = indicator.Max;

                    var actual = dataObject.GetPropertyValue<double>(this.DataMemberR);
                    //var scaled = DataScaleR.GetScaledValue(actual);
                    //TODO scale value here
                    item.R = actual; //scaled;
                    //item.R = dataObject.GetPropertyValue<double>(this.DataMemberR);
                }

                DataPoints.Add(item);
            }
        }

        public void UpdateDataPoints()
        {
            if (DataView == null) return;

            for (var i = 0; i < DataView.Count(); i++)
            {
                if (IsValidIndicator(this.DataMemberX))
                {
                    DataPoints[i].X = DataView.At(i).GetPropertyValue<double>(this.DataMemberX);
                }
                else
                {
                    DataPoints[i].X = double.NaN;
                }

                if (IsValidIndicator(this.DataMemberY))
                {
                    DataPoints[i].Y = DataView.At(i).GetPropertyValue<double>(this.DataMemberY);
                }
                else
                {
                    DataPoints[i].Y = double.NaN;
                }

                if (IsValidIndicator(this.DataMemberR))
                {
                    var actual = DataView.At(i).GetPropertyValue<double>(this.DataMemberR);
                    //var scaled = DataScaleR.GetScaledValue(actual);
                    //DataPoints[i].R = scaled;
                    DataPoints[i].R = actual;
                }
                else
                {
                    DataPoints[i].R = double.NaN;
                }
            }
        }


        /// <summary>
        /// Gets or sets DataView
        /// </summary>
        public List<ScatterDataItem> DataPoints { get; set; }

        private SizeScale _dataScaleR = new SizeScale();
        /// <summary>
        /// Gets or sets DataScaleR
        /// </summary>
        public SizeScale DataScaleR
        {
            get { return _dataScaleR; }
            set { if (_dataScaleR == value) return; _dataScaleR = value; OnPropertyChanged("DataScaleR"); }
        }

        private DataIndicator _dataIndicatorX;
        /// <summary> Gets or sets DataIndicatorX </summary>
        public DataIndicator DataIndicatorX
        {
            get { return _dataIndicatorX;}
            set { if (_dataIndicatorX == value) return; _dataIndicatorX = value; OnPropertyChanged("DataIndicatorX"); }
        }




        private string _dataMemberX;
        /// <summary>
        /// Gets or sets DataMemberX
        /// </summary>
        public string DataMemberX
        {
            get { return _dataMemberX; }
            set { if (_dataMemberX == value) return; _dataMemberX = value; OnPropertyChanged("DataMemberX"); }
        }

        private string _dataMemberY;
        /// <summary>
        /// Gets or sets DataMemberY
        /// </summary>
        public string DataMemberY
        {
            get { return _dataMemberY; }
            set { if (_dataMemberY == value) return; _dataMemberY = value; OnPropertyChanged("DataMemberY"); }
        }

        private string _dataMemberR;
        /// <summary>
        /// Gets or sets DataMemberR
        /// </summary>
        public string DataMemberR
        {
            get { return _dataMemberR; }
            set { if (_dataMemberR == value) return; _dataMemberR = value; OnPropertyChanged("DataMemberR"); }
        }

        private string _dataLabelFormatX;
        /// <summary>
        /// Gets or sets DataLabelFormatX
        /// </summary>
        public string DataLabelFormatX
        {
            get { return _dataLabelFormatX; }
            set { if (_dataLabelFormatX == value) return; _dataLabelFormatX = value; OnPropertyChanged("DataLabelFormatX"); }
        }

        private string _dataLabelFormatY;
        /// <summary>
        /// Gets or sets DataLabelFormatY
        /// </summary>
        public string DataLabelFormatY
        {
            get { return _dataLabelFormatY; }
            set { if (_dataLabelFormatY == value) return; _dataLabelFormatY = value; OnPropertyChanged("DataLabelFormatY"); }
        }

        private string _dataLabelFormatR;
        /// <summary>
        /// Gets or sets DataLabelFormatR
        /// </summary>
        public string DataLabelFormatR
        {
            get { return _dataLabelFormatR; }
            set { if (_dataLabelFormatR == value) return; _dataLabelFormatR = value; OnPropertyChanged("DataLabelFormatR"); }
        }

    }

}