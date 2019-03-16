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
    [Table("Ilanlar")]
    public class Ilan
    {

        #region 
        private int _id;
        private string _il;
        private string _ilce;
        private string konum;
        private string _baslik;
        private string _ozet;
        private int _kontenjan;
        private string _kullaniciId;
        private DateTime _olusturmaTarihi;
        private DateTime _baslangicTarihi;
        private int _goruntulenmeSayaci;
        private bool _yayindami;
        private bool _silindi;
        #endregion

        [Key]
        public int Id { get => _id; set => _id = value; }

        [Required(ErrorMessage = "Lütfen il ismi giriniz!")]
        [StringLength(30, ErrorMessage = "İçerik {0} karakterden uzun olmamalıdır!")]
        public string Il { get => _il; set => _il = value; }

        [Required(ErrorMessage = "Lütfen ilçe ismi giriniz!")]
        [StringLength(30, ErrorMessage = "İçerik {0} karakterden uzun olmamalıdır!")]
        public string Ilce { get => _ilce; set => _ilce = value; }


        public string Konum { get => konum; set => konum = value; }

        [Required(ErrorMessage = "Lütfen başlık içeriği giriniz!")]
        [StringLength(50, ErrorMessage = "İçerik {0} karakterden uzun olmamalıdır!")]
        public string Baslik
        {
            get
            {
                return _baslik;
            }
            set
            {
                string Bbaslik = "";
                if (!string.IsNullOrEmpty(value))
                {
                    string[] basligim = value.Split(' ');
                    foreach (string item in basligim)
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
                        if (Bbaslik == "")
                        {
                            Bbaslik += b;
                        }
                        else
                        {
                            Bbaslik += " " + b;
                        }
                    }
                }
                else
                {
                    Bbaslik = value;
                }
                _baslik = Bbaslik;
            }
        }

        [Required(ErrorMessage = "Lütfen ozet içeriği giriniz!")]
        [StringLength(300, ErrorMessage = "İçerik {0} karakterden uzun olmamalıdır!")]
        public string Ozet
        {
            get
            {
                return _ozet;
            }
            set
            {
                string BOzet = "";
                if (!string.IsNullOrEmpty(value))
                {
                    string[] ozetim = value.Split('.');
                    foreach (string item in ozetim)
                    {
                        string o;
                        if (!string.IsNullOrEmpty(item))
                        {
                            o = item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower();
                        }
                        else
                        {
                            o = item;
                        }
                        if (BOzet=="")
                        {
                            BOzet += o;
                        }
                        else
                        {
                            BOzet += "." + o;
                        }
                    }
                }
                else
                {
                    BOzet = value;
                }
                _ozet = BOzet;
            }
        }

        [Required(ErrorMessage = "Lütfen etiket içeriği giriniz!")]
        public int Kontenjan { get => _kontenjan; set => _kontenjan = value; }
        public string KullaniciId { get => _kullaniciId; set => _kullaniciId = value; }//

        [DataType(DataType.DateTime)]
        public DateTime OlusturmaTarihi { get => _olusturmaTarihi; set => _olusturmaTarihi = value; }

        [DataType(DataType.DateTime)]
        public DateTime BaslangicTarihi { get => _baslangicTarihi; set => _baslangicTarihi = value; }

        public int GoruntulenmeSayaci { get => _goruntulenmeSayaci; set => _goruntulenmeSayaci = value; }
        public bool Yayindami { get => _yayindami; set => _yayindami = value; }
        public bool Silindi { get => _silindi; set => _silindi = value; }

        public virtual List<Etiket> Etiketler { get; set; }
        public virtual List<Katilan> Katilanlar { get; set; }
        public virtual List<IlanHareket> IlanHareketler { get; set; }

        [ForeignKey("KullaniciId")]
        public virtual ApplicationUser User { get; set; }

    }
}
