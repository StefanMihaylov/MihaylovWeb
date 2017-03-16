using System;
using System.Collections.Generic;
using System.Linq;

namespace Mihaylov.Data.Models.Repositories
{
    public class PersonStatistics
    {
        public Dictionary<string, int> CountDictionary { get; set; }

        public decimal Average { get; set; }

        public decimal Min { get; set; }

        public decimal Max { get; set; }

        public int TotalCount { get; set; }

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
                var result = new Dictionary<string, string>();

                result.Add("Average", $"{this.Average:0.00} ({this.Min:0.0} - {this.Max:0.0})");

                foreach (var countPair in this.CountDictionary)
                {
                    int count = countPair.Value;
                    decimal percent = (decimal)count / this.Count;

                    string countAsText = $"{count} ({percent:P2})";
                    result.Add(countPair.Key, countAsText);
                }

                result.Add("Count", $"{this.Count} ({this.TotalCount})");

                return result;
            }
        }
    }
}