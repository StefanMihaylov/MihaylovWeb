namespace Mihaylov.Users.Data.Database.Models
{
    public class UserProfile
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserProfile()
        {
        }

        public UserProfile(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}
