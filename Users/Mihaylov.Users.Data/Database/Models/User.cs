using Microsoft.AspNetCore.Identity;

namespace Mihaylov.Users.Data.Database.Models
{
    public class User : IdentityUser
    {
        public User()
        {
        }

        public User(string userName, string email)
            : base(userName)
        {
            this.Email = email;
        }
    }
}
