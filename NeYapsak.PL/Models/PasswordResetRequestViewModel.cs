﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    public class PasswordResetRequestViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}