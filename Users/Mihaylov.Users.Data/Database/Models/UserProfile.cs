using System.ComponentModel.DataAnnotations;
using Mihaylov.Users.Models.Enums;

namespace Mihaylov.Users.Data.Database.Models
{
    public class UserProfile
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public GenderType? Gender { get; set; }

        public virtual Gender GenderModel { get; set; }

        public UserProfile()
        {
        }

        public UserProfile(string firstName, string lastName, GenderType? gender)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
        }
    }
}
