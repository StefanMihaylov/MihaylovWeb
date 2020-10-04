using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Mihaylov.Users.Models.Responses
{
    public class GenericResponse
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public GenericResponse(IdentityResult result)
            : this(result?.Succeeded ?? false, result?.Errors.Select(e => e.Description))
        {
        }

        public GenericResponse(Exception ex)
            : this(false, new List<string>() { ex.Message })
        {
        }

        public GenericResponse(bool succeeded, IEnumerable<string> errors)
        {
            this.Succeeded = succeeded;
            this.Errors = errors;
        }
    }
}
