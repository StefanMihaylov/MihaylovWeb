using System;
using Microsoft.AspNetCore.Identity;
using Mihaylov.Common.Abstract.Databases.Interfaces;

namespace Mihaylov.Users.Data.Database.Models
{
    public class User : IdentityUser, IDeletableEntity
    {
        public User()
        {
        }

        public User(string userName, string email)
            : base(userName)
        {
            this.Email = email;
        }

        public UserProfile Profile { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
