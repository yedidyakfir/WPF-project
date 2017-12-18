using System;
using System.Collections.Generic;

namespace Infragistics.Framework
{
    public class FinancialData : List<FinancialDataItem>
    {
        internal static Random Random = new Random();
        
        public static FinancialData GetCompanyFinances()
        {
            var labels = new [] { "IT", "Marketing", "Management", "Sales", "Development", "Support" };

            var data = new FinancialData();
            for (var i = 0; i < labels.Length; i++)
            {
                var item = new FinancialDataItem();
                item.Spending = (Random.NextDouble() * (100 - 20)) + 20;
                item.Budget = (Random.NextDouble() * (100 - 40)) + 40;
                item.Label = labels[i];
                data.Add(item);
            }
            return data;
        } 
    }
    public class FinancialDataItem
    {
        /// <summary> Gets or sets Spending </summary>
        public double Spending { get; set; }

        /// <summary> Gets or sets Budget </summary>
        public double Budget { get; set; }

        /// <summary> Gets or sets Balance </summary>
        public double Balance { get { return Budget - Spending;  } }

        /// <summary> Gets or sets Label </summary>
        public string Label { get; set; }

    }
}