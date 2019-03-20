using NeYapsak.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    public class MsgBoxViewModel
    {
        public List<ApplicationUser> Kullanicilar { get; set; }
        public string UserId { get; set; }

    }
}