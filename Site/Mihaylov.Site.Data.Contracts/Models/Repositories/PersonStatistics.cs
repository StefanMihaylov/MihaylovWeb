using System;
using System.Collections.Generic;
using System.Linq;

namespace Mihaylov.Site.Data.Models
{
    public class PersonStatistics
    {
        public Dictionary<string, int> CountDictionary { get; set; }

        public decimal Average { get; set; }

        public decimal Min { get; set; }

        public decimal Max { get; set; }

        public int TotalCount { get; set; }

        public int Disabled { get; set; }

        public int Count
        {
            get
            {
                return CountDictionary.Values.Sum();
            }
        }

        public Dictionary<string, string> CountDictionaryResult
        {
            get
            {
                var result = new Dictionary<string, string>
                {
                    { "Average", $"{this.Average:0.00} ({this.Min:0.0} - {this.Max:0.0})" }
                };

                foreach (var countPair in this.CountDictionary)
                {
                    result.Add(countPair.Key, $"{countPair.Value} ({(decimal)countPair.Value / this.Count:P2})");
                }

                result.Add("Count", $"{this.Count} ({this.TotalCount})");
                result.Add("Disabled", $"{this.Disabled} ({((decimal)this.Disabled / this.Count):P2})");

                return result;
            }
        }
    }
}