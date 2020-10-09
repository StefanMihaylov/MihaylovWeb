using System;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class DateOfBirth
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateOfBirth(DateOfBirthType type)
        {
            Id = (int)type;
            Name = type.ToString();
        }

        public static IEnumerable<DateOfBirth> GetValues()
        {
            var values = Enum.GetValues(typeof(DateOfBirthType))
                             .Cast<DateOfBirthType>()
                             .Select(a => new DateOfBirth(a));

            return values;
        }
    }
}
