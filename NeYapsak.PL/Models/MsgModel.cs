using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeYapsak.PL.Models
{
    /// <summary>
    /// Bu bir ViewModel değildir.
    /// </summary>
    public class MsgModel
    {
        public string AliciId { get; set; }
        public string GondericiId { get; set; }
        public string Mesaj { get; set; }
        public DateTime Tarih { get; set; }
    }
}