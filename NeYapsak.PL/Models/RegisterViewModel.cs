using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Ad")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Soyad")]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "E-Posta")]
        [EmailAddress()]
        public string Email { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime DogumTarihi { get; set; }

        [Required]
        [Display(Name = "Şifre")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler aynı değil!")]
        public string ConfirmPassword { get; set; }
    }
}