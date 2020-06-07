using System.Collections.Generic;

namespace Mihaylov.Users.Data.Repository.Models
{
    public class RegisterResponseModel
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public RegisterResponseModel(bool succeeded, IEnumerable<string> errors)
        {
            this.Succeeded = succeeded;
            this.Errors = errors;
        }
    }
}
