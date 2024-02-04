using System;

namespace Mihaylov.Users.Models.Requests
{
    public class UpdateRoleRequest
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
    }
}
