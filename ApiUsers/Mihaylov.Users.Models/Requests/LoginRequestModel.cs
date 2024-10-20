﻿using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Users.Models.Requests
{
    public class LoginRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public int? ClaimTypes { get; set; }

        public bool LockoutOnFailure { get; set; }
    }
}
