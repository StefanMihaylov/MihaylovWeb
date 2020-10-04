using System;

namespace Mihaylov.Users.Models.Requests
{
    public class AddRoleToUserRequest
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }
    }
}
