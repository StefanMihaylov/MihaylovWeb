using System;
using System.Collections.Generic;

namespace Mihaylov.Users.Models.Responses
{
    public class GenericResponse
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }

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
