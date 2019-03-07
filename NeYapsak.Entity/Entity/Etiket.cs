using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeYapsak.Entity.Entity
{
    [Table("Etiketler")]
    public class Etiket
    {

        #region 
        private int _id;
        private string _etiketAdi;
        private string _aciklama;
        private bool _silindi;
        #endregion

        #region 

        [Key]
        public int Id { get => _id; set => _id = value; }

        [Required(ErrorMessage = "Lütfen etiket içeriği giriniz!")]
        [StringLength(30, ErrorMessage = "İçerik {0} karakterden uzun olmamalıdır!")]
        public string EtiketAdi {
            get
            {
                return _etiketAdi;
            }
            set
            {
                string BEtiketadi = "";
                if (!string.IsNullOrEmpty(value))
                {
                    string[] etiketim = value.Split(' ');
                    foreach (string item in etiketim)
                    {
                        string e;
                        if (!string.IsNullOrEmpty(item))
                        {
                            e = item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower();
                        }
                        else
                        {
                            e = item;
                        }
                        if (BEtiketadi == "")
                        {
                            BEtiketadi += e;
                        }
                        else
                        {
                            BEtiketadi += " " + e;
                        }
                    }
                }
                else
                {
                    BEtiketadi = value;
                }
                _etiketAdi = BEtiketadi;
            }
        }

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
                    if (value.Contains('.'))
                    {
                        string[] Aciklamam = value.Split('.');
                        foreach (string item in Aciklamam)
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
                        BAciklama = value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
                    }
                }
                else
                {
                    BAciklama = value;
                }
                _aciklama = BAciklama;
            }
        }

        public bool Silindi { get => _silindi; set => _silindi = value; }
        #endregion

        public virtual List<Ilan> Ilanlar { get; set; }

    }
}
