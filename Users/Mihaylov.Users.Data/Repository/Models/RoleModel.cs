using System;
using Microsoft.AspNetCore.Identity;

namespace Mihaylov.Users.Data.Repository.Models
{
    public class RoleModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }


        public static explicit operator RoleModel(IdentityRole role)
        {
            if (role == null)
            {
                return null;
            }

            return new RoleModel()
            {
                Id = new Guid(role.Id),
                Name = role.Name
            };
        }
    }
}
