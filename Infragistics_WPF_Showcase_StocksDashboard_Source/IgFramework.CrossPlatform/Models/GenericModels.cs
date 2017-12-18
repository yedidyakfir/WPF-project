using System;
using System.Collections.Generic;

namespace Infragistics.Framework
{
    public class DateTimeDictionary : Dictionary<string, DateTime> 
    {
         
    }
    public class DateTimeList : List<DateTime>
    {

    }

    public static class Comparer 
    {
        public static int Ascending(DateTime a, DateTime b)
        {
            return +Comparer<DateTime>.Default.Compare(a.Date, b.Date);
        }

        public static int Descending(DateTime a, DateTime b)
        {
            return -Comparer<DateTime>.Default.Compare(a.Date, b.Date);
        }

        public static int Ascending(double a, double b)
        {
            return +Comparer<double>.Default.Compare(a, b);
        }

        public static int Descending(double a, double b)
        {
            return -Comparer<double>.Default.Compare(a, b);
        }

    }

}