using System;
using System.Collections.Generic;
using System.Text;

namespace Mihaylov.Users.Data.Repository.Models
{
    public class AddRoleToUserRequest
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }
    }
}
