using System;

namespace Mihaylov.Api.Site.Client
{
    public static class Extensions
    {
        public static int GetAge(this DateTime dateOfBirth)
        {
            var dateNow = DateTime.UtcNow;

            int age = dateNow.Year - dateOfBirth.Year;
            if (dateOfBirth > dateNow.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        public static DateTime GetBirthDate(this int age)
        {
           return DateTime.UtcNow.Date.AddYears(-age).AddMonths(-6);
        }
    }
}
