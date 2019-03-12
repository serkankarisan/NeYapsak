using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NeYapsak.Entity.Identity
{
    public class ApplicationUser : IdentityUser
    {
        #region 
        private string _name;
        private string _surname;
        private DateTime _dogumTarihi;
        private string _profilAvatarYolu;
        private string _bio;
        #endregion

        [Required(ErrorMessage = "Lütfen Ad içeriği giriniz!")]
        [StringLength(50, ErrorMessage = "İçerik {0} karakterden uzun olmamalıdır!")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                string BName = "";
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Contains(' '))
                    {
                        string[] Names = value.Split(' ');
                        foreach (string item in Names)
                        {
                            string n;
                            if (!string.IsNullOrEmpty(item))
                            {
                                n = item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower();

                            }
                            else
                            {
                                n = item;
                            }
                            if (BName == "")
                            {
                                BName += n;
                            }
                            else
                            {
                                BName += " " + n;
                            }
                        }
                    }
                    else
                    {
                        BName = value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
                    }
                }
                else
                {
                    BName = value;
                }
                _name = BName;
            }
        }

        [Required(ErrorMessage = "Lütfen Soyad içeriği giriniz!")]
        [StringLength(50, ErrorMessage = "İçerik {0} karakterden uzun olmamalıdır!")]
        public string Surname
        {
            get
            {
                return _surname;
            }
            set
            {
                string BSurName = "";
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Contains(' '))
                    {
                        string[] SurNames = value.Split(' ');
                        foreach (string item in SurNames)
                        {
                            string sn;
                            if (!string.IsNullOrEmpty(item))
                            {
                                sn = item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower();

                            }
                            else
                            {
                                sn = item;
                            }
                            if (BSurName == "")
                            {
                                BSurName += sn;
                            }
                            else
                            {
                                BSurName += " " + sn;
                            }
                        }
                    }
                    else
                    {
                        BSurName = value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
                    }
                }
                else
                {
                    BSurName = value;
                }
                _surname = BSurName;
            }
        }

        [Required(ErrorMessage = "Lütfen Doğum Tarihi içeriği giriniz!")]
        public DateTime DogumTarihi { get => _dogumTarihi; set => _dogumTarihi = value; }
        public string ProfilAvatarYolu { get => _profilAvatarYolu; set => _profilAvatarYolu = value; }

        [StringLength(300, ErrorMessage = "Bio {0} karakterden uzun olmamalıdır!")]
        public string Bio {
            get
            {
                return _bio;
            }
            set
            {
                string BBio = "";
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Contains('.'))
                    {
                        string[] Biom = value.Split('.');
                        foreach (string item in Biom)
                        {
                            string b;
                            if (!string.IsNullOrEmpty(item))
                            {
                                b = item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower();
                            }
                            else
                            {
                                b = item;
                            }
                            if (BBio == "")
                            {
                                BBio += b;
                            }
                            else
                            {
                                BBio += "." + b;
                            }
                        }
                    }
                    else
                    {
                        BBio = value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
                    }
                }
                else
                {
                    BBio = value;
                }
                _bio = BBio;
            }

        }

        public ApplicationUser()
        {
            ProfilAvatarYolu = "/Images/Profil/profilavatar.png";
            Bio = "Hakkımda Bilgisi Girilmemiş.";
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
