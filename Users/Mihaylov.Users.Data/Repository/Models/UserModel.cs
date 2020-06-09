using System;
using System.Collections.Generic;

namespace Mihaylov.Users.Data.Repository.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
