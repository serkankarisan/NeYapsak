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
    [Table("Katilanlar")]
    public class Katilan
    {

        #region 
        private int _id;
        private int _ilanId;
        private string _kullaniciId;
        private DateTime _tarih;
        private bool _silindi;
        #endregion

        [Key]
        public int Id { get => _id; set => _id = value; }
        public int? IlanId { get => _ilanId; set => _ilanId = (int)value; }
        public string KullaniciId { get => _kullaniciId; set => _kullaniciId = value; }

        [DataType(DataType.DateTime)]
        public DateTime Tarih { get => _tarih; set => _tarih = value; }
        public bool Silindi { get => _silindi; set => _silindi = value; }


        [ForeignKey("IlanId")]
        public virtual Ilan Ilan { get; set; }

        [ForeignKey("KullaniciId")]
        public virtual ApplicationUser User { get; set; }

    }
}
