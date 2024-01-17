using System;
using System.Globalization;

namespace Mihaylov.Common.Host.BuildDate
{
    /// <summary>
    /// The release date assembly attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ReleaseDateAttribute : Attribute
    {
        /// <summary> 
        /// Constructor that takes in a DateTime string
        /// </summary>
        /// <param name="utcDateString"> A DateTime 'O' (round-trip date/time) format string </param>
        public ReleaseDateAttribute(string utcDateString)
        {
            ReleaseDate = DateTime.ParseExact(utcDateString, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        /// <summary>
        /// The date the assembly was built
        /// </summary>
        public DateTime ReleaseDate { get; }
    }
}
