using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    public class PasswordResetViewModel
    {
        [Required]
        [EmailAddress()]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler aynı değil!")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}