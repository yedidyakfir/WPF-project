﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class ObjectEx
    {
        public static T ConvertTo<T>(this object obj)
        {
            if (obj is T)
            {
                return (T)obj;
            }
            try
            {  
                return (T)Convert.ChangeType(obj, typeof(T), null);
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }

        public static object GetPropertyValue(this object obj, string propName)
        {
            return obj.GetPropertyValue<object>(propName);
        }
        /// <summary>
        /// Gets value for specified properly name of this object
        /// </summary>
        public static T GetPropertyValue<T>(this object obj, string propName)
        {
            //var info = src.GetType().GetTypeInfo();
            var type = obj.GetType();

            var property = type.GetProperty(propName, 
                BindingFlags.Public | 
                BindingFlags.Instance);

            if (property == null) return default(T);

            var value = property.GetValue(obj, null);
            if (value is T)
                return (T)value;
            
            return default(T);
            //return property.GetValue(src, null);
        }
        /// <summary>
        /// Gets a list of properly info for this object
        /// </summary>
        public static List<PropertyInfo> GetProperties(this object obj)
        {
            var properties = obj.GetType().
                GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties.ToList();
        }

        public static List<string> GetPropertiesNames(this object obj)
        {
            var properties = obj.GetProperties();
            return properties.Select(p => p.Name).ToList();
        }
        public static bool ContainsProperty(this object obj, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return false;
            var properties = obj.GetPropertiesNames();
            return properties.Contains(propertyName);
        }
        /// <summary>
        /// Gets a dictionary of properly names and their values for this object
        /// </summary>
        public static Dictionary<string, T> GetPropertiesValues<T>(this object obj)
        {
            var result = new Dictionary<string, T>();
            var properties = obj.GetProperties();
            foreach (var property in properties)
            {
                var value = obj.GetPropertyValue<T>(property.Name);
                
                if (!Equals(value, default(T)))
                     result.Add(property.Name, value);
            }

            return result;
        }

    }
}