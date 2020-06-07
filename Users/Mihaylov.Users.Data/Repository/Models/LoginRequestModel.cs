using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Users.Data.Repository.Models
{
    public class LoginRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
