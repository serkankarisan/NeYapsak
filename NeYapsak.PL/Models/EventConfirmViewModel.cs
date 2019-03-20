using NeYapsak.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    public class EventConfirmViewModel
    {
        public Ilan Ilan { get; set; }
        public List<Ilan> OnayBekleyenIlanlar { get; set; }
        public List<Ilan> OnaylanmisIlanlar { get; set; }
    }
}