using System;
using System.Collections.Generic;

namespace Mihaylov.Users.Models.Responses
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
