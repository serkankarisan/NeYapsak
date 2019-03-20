using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeYapsak.Entity.Entity
{
    public class Gorusmeler
    {
        public int Id { get; set; }
        public string GondericiId { get; set; }
        public string AliciId { get; set; }
        public string Mesaj { get; set; }
        public DateTime Tarih { get; set; } = DateTime.Now;
    }
}
