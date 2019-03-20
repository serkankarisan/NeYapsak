using NeYapsak.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    public class MsgHistViewModel
    {
        public string AliciId { get; set; }
        public string GondericiId { get; set; }
        public string Baslik { get; set; }
        public List<Gorusmeler> Gorusmeler { get; set; }
        public List<MsgModel> Mesajlar { get; set; }
    }
}