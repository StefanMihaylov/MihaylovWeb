using System;

namespace Mihaylov.Api.Site.Contracts.Models
{
    public class QuizAnswer
    {
        public const int DetailsMaxLength = 2000;

        public long Id { get; set; }

        public long PersonId { get; set; }

        public DateTime AskDate { get; set; }

        public int QuestionId { get; set; }

        public string Question { get; set; }

        public decimal? Value { get; set; }

        public int? UnitId { get; set; }

        public string Unit { get; set; }

        public decimal? ConvertedValue { get; set; }

        public string ConvertedUnit { get; set; }

        public int? HalfTypeId { get; set; }

        public string HalfType { get; set; }

        public string Details { get; set; }


        public string ValueDisplay
        {
            get
            {
                if (!Value.HasValue && !ConvertedValue.HasValue)
                {
                    return string.Empty;
                }

                if (!ConvertedValue.HasValue)
                {
                    return $"{Value.Value:0.##}";
                }
                else if (Value.Value == ConvertedValue.Value)
                {
                    return $"{Value.Value:0.##}";
                }
                else
                {
                    return $"{Value.Value} ({ConvertedValue.Value:0.##} {ConvertedUnit})";
                }
            }
        }
    }
}
