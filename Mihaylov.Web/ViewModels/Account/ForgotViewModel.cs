﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}