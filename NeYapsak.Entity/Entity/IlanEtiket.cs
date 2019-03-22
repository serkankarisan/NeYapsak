using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeYapsak.Entity.Entity
{[Table("IlanEtiketler")]
    public class IlanEtiket
    {
        public int Id { get; set; }
        public int IlanId { get; set ; }
        public int EtiketId { get; set ; }
        public bool Silindi { get; set; }
        [ForeignKey("IlanId")]
        public virtual Ilan Ilan { get; set; }
        [ForeignKey("EtiketId")]
        public virtual Etiket Etiket { get; set; }
    }
}
