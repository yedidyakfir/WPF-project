using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace System.Collections.Generic
{
    public static class EnumerableEx
    {
        public static int Count(this IEnumerable items)
        {
            return Enumerable.Count(items.Cast<object>());
        }
        public static object First(this IEnumerable items)
        {
            return items.Cast<object>().FirstOrDefault();
        }
        public static object Last(this IEnumerable items)
        {
            return items.Cast<object>().LastOrDefault();
        }

        public static object At(this IEnumerable items, int index)
        {
            var list = items as IList<object> ?? items.Cast<object>().ToList();
            return list.ToList()[index];
        }


        /// <summary>
        /// Converts IEnumerable to ObservableCollection
        /// </summary>
        public static ObservableCollection<TSource> ToCollection<TSource>(this IEnumerable<TSource> source)
        {
            var ret = new ObservableCollection<TSource>();
            foreach (var item in source)
            {
                ret.Add(item);
            }
            return ret;
        }
       
        /// <summary>
        /// Sorts IEnumerable by specified property name and optional sort direction
        /// </summary>
        /// <remarks>sort direction is descending by default</remarks>
        public static IEnumerable SortByProperty(this IEnumerable items, 
            string propertyName, SortDirection sortDirection = SortDirection.Descending)
        {
            var list = items as IList<object> ?? items.Cast<object>().ToList();
            return list.SortByProperty(propertyName, sortDirection);
        }
        public static IEnumerable SortByProperty(this IEnumerable items,
            string propertyName1, string propertyName2, SortDirection sortDirection = SortDirection.Descending)
        {
            var list = items as IList<object> ?? items.Cast<object>().ToList();
            return list.SortByProperty(propertyName1, propertyName2, sortDirection);
        }
        /// <summary>
        /// Sorts IEnumerable by specified sort definition
        /// </summary>
        public static IEnumerable SortBy(this IEnumerable items, SortDefinition sort)
        {
            return items.SortByProperty(sort.PropertyName, sort.Direction);
        }

        /// <summary>
        /// Sorts IEnumerable by specified property name and optional sort direction
        /// </summary>
        /// <remarks>sort direction is descending by default</remarks>
        public static IOrderedEnumerable<T> SortByProperty<T>(this IEnumerable<T> items, 
            string propertyName, SortDirection sortDirection = SortDirection.Descending)
        {
            if (string.IsNullOrEmpty(propertyName)) 
                throw new ArgumentException("Cannot sort by null or empty property name");

            var enumerable = items as IList<T> ?? items.ToList();
            var item = enumerable.First();
            var type = item.GetType();  
         
            var propInfo = type.GetProperty(propertyName);
            if (propInfo == null)
            {
                throw new ArgumentException("Sorting failed because '" + propertyName + "' property " +
                                            "does not exist in an object of " + type + " type");
            }
            return enumerable.SortByExpression(x => propInfo.GetValue(x, null), sortDirection);
        }

        public static IOrderedEnumerable<T> SortByProperty<T>(this IEnumerable<T> items,
            string propertyName1, string propertyName2, SortDirection sortDirection = SortDirection.Descending)
        {
            if (string.IsNullOrEmpty(propertyName1))
                throw new ArgumentException("Cannot sort by null or empty property name");
           
            if (string.IsNullOrEmpty(propertyName2))
                throw new ArgumentException("Cannot sort by null or empty property name");

            var enumerable = items as IList<T> ?? items.ToList();
            var item = enumerable.First();
            var type = item.GetType();

            var propInfo1 = type.GetProperty(propertyName1);
            if (propInfo1 == null)
            {
                throw new ArgumentException("Sorting failed because '" + propertyName1 + "' property " +
                                            "does not exist in an object of " + type + " type");
            }
            var propInfo2 = type.GetProperty(propertyName2);
            if (propInfo2 == null)
            {
                throw new ArgumentException("Sorting failed because '" + propertyName2 + "' property " +
                                            "does not exist in an object of " + type + " type");
            }
            return enumerable.SortByExpression(x => propInfo1.GetValue(x, null),
                                               x => propInfo2.GetValue(x, null), sortDirection);
        }
        internal static IOrderedEnumerable<T> SortByExpression<T, TKey>
        (
            this IEnumerable<T> items,
            Func<T, TKey> keyExpression,
            SortDirection sortDirection = SortDirection.Ascending
        )
        {
            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    return items.OrderBy(keyExpression);
                case SortDirection.Descending:
                    return items.OrderByDescending(keyExpression);
            }
            throw new ArgumentException("Unknown SortDirection: " + sortDirection);
        }
        internal static IOrderedEnumerable<T> SortByExpression<T, TKey>
        (
            this IEnumerable<T> items,
            Func<T, TKey> keyExpression1,
            Func<T, TKey> keyExpression2,
            SortDirection sortDirection = SortDirection.Ascending
        )
        {
            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    return items.OrderBy(keyExpression1).ThenBy(keyExpression2);
                case SortDirection.Descending:
                    return items.OrderByDescending(keyExpression1).ThenBy(keyExpression2);
            }
            throw new ArgumentException("Unknown SortDirection: " + sortDirection);
        }
    }

    public class SortDefinition
    {
        public string PropertyName { get; set; }
        public SortDirection Direction { get; set; }

        public SortDefinition()
        {
            Direction = SortDirection.Descending;
        }
        public SortDefinition(string propertyName)
        {
            Direction = SortDirection.Descending;
            PropertyName = propertyName;
        }
        public SortDefinition(SortDirection direction, string propertyName)
        {
            Direction = direction;
            PropertyName = propertyName;
        }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }

}