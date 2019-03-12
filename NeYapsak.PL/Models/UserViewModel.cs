using NeYapsak.Entity.Entity;
using NeYapsak.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    public class UserViewModel
    {
        public ApplicationUser Kullanici { get; set; }
        public List<Ilan> KullaniciIlanlari { get; set; }
        public HttpPostedFileBase PictureUpload { get; set; }
        public int IlgilendigiIlanSayisi { get; set; }
        public int KatildigiIlanSayisi { get; set; }
    }
}