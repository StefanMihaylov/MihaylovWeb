using System.ComponentModel.DataAnnotations;
using Mihaylov.Users.Models.Enums;

namespace Mihaylov.Users.Models.Requests
{
    public class RegisterRequestModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public GenderType? Gender { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
