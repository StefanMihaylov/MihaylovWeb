using System;
using Microsoft.AspNetCore.Identity;

namespace Mihaylov.Users.Models.Responses
{
    public class RoleModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
