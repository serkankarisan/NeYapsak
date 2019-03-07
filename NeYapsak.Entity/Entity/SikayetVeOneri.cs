using NeYapsak.Entity.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeYapsak.Entity.Entity
{
    [Table("SikayetVeOneriler")]
    public class SikayetVeOneri
    {

        #region 
        private int _id;
        private int _ilanId;
        private string _kullaniciId;
        private string _aciklama;
        private DateTime _tarih;
        private bool _silindi;
        #endregion

        [Key]
        public int Id { get => _id; set => _id = value; }
        public int? IlanId { get => _ilanId; set => _ilanId = (int)value; }//
        public string KullaniciId { get => _kullaniciId; set => _kullaniciId = value; }//

        [Required(ErrorMessage = "Lütfen Şikayet içeriği giriniz!")]
        [StringLength(300, ErrorMessage = "İçerik {0} karakterden uzun olmamalıdır!")]
        public string Aciklama
        {
            get
            {
                return _aciklama;
            }
            set
            {
                string BAciklama = "";
                if (!string.IsNullOrEmpty(value))
                {
                    string[] aciklamam = value.Split('.');
                    foreach (string item in aciklamam)
                    {
                        string a;
                        if (!string.IsNullOrEmpty(item))
                        {
                             a = item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower();

                        }
                        else
                        {
                            a = item;
                        }
                        if (BAciklama == "")
                        {
                            BAciklama += a;
                        }
                        else
                        {
                            BAciklama += "." + a;
                        }
                    }
                }
                else
                {
                    BAciklama = value;
                }
                _aciklama = BAciklama;
            }
        }

        [DataType(DataType.DateTime)]
        public DateTime Tarih { get => _tarih; set => _tarih = value; }
        public bool Silindi { get => _silindi; set => _silindi = value; }

        [ForeignKey("KullaniciId")]
        public virtual ApplicationUser User { get; set; }

    }
}
