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
    [Table("IlanHareketler")]
    public class IlanHareket
    {

        #region 
        private int _id;
        private int _ilanId;
        private string _kullaniciId;
        private int _katilanId;
        private int _ilgilenenId;
        private DateTime _tarih;
        private bool _silindi;
        #endregion

        [Key]
        public int Id { get => _id; set => _id = value; }
        public int IlanId { get => _ilanId; set => _ilanId = value; }//
        public string KullaniciId { get => _kullaniciId; set => _kullaniciId = value; }//
        public int? KatilanId { get => _katilanId; set => _katilanId = (int)value; }//
        public int? IlgilenenId { get => _ilgilenenId; set => _ilgilenenId =(int)value; }//

        [DataType(DataType.DateTime)]
        public DateTime Tarih { get => _tarih; set => _tarih = value; }
        public bool Silindi { get => _silindi; set => _silindi = value; }


        [ForeignKey("IlanId")]
        public virtual Ilan Ilan { get; set; }

        [ForeignKey("KullaniciId")]
        public virtual ApplicationUser User { get; set; }

        //[ForeignKey("IlgilenenId")]
        //public virtual Ilgilenen Ilgilenen { get; set; }

        //[ForeignKey("KatilanId")]
        //public virtual Katilan Katilan { get; set; }

    }
}
