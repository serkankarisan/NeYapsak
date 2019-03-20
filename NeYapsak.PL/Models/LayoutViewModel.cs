using NeYapsak.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    public class LayoutViewModel
    {
        public ApplicationUser Kullanici { get; set; }
        public int KullaniciIlanSayisi{ get; set; }
        public int IlgilendigiIlanSayisi { get; set; }
        public int KatildigiIlanSayisi { get; set; }
        public int OnayimiBekleyenIlanSayisi { get; set; }
        public int OnayladigimIlanSayisi { get; set; }
    }
}