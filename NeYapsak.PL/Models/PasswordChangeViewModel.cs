using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    public class PasswordChangeViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name ="Eski Şifre")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre Tekrarı")]
        [Compare("NewPassword", ErrorMessage = "Yeni girilen şifreler aynı değil!")]
        public string ConfirmNewPassword { get; set; }
    }
}