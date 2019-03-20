using NeYapsak.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    public class MainViewModel
    {
        public List<Ilan> KullanicininIlanlari { get; set; }
        public List<Ilan> DigerIlanlar { get; set; }
        public Ilan Ilan { get; set; }
    }
}