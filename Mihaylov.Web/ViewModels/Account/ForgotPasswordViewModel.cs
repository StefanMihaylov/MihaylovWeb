﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}