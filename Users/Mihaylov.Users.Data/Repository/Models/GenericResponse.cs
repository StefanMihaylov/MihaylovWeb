using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Mihaylov.Users.Data.Repository.Models
{
    public class GenericResponse
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public GenericResponse(IdentityResult result)
        {
            this.Succeeded = result?.Succeeded ?? false;
            this.Errors = result?.Errors.Select(e => e.Description);
        }
    }
}
