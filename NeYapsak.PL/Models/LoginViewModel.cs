﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    public class LoginViewModel
    {
            [Required]
            [Display(Name = "E-Posta")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Şifre")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Beni Hatırla")]
            public bool RememberMe { get; set; }

            public string returnUrl { get; set; }
    }
}