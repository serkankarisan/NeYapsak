using Microsoft.AspNet.Identity;
using NeYapsak.BLL.Identity;
using NeYapsak.BLL.Repository;
using NeYapsak.DAL.Context;
using NeYapsak.Entity.Entity;
using NeYapsak.Entity.Identity;
using NeYapsak.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NeYapsak.PL.Controllers
{
    [Authorize]
    public class MesajController : BaseController
    {
        NeYapsakContext ent = new NeYapsakContext();
        public ActionResult Index()
        {
            MsgBoxViewModel model = new MsgBoxViewModel();
            string Gonderici = HttpContext.User.Identity.GetUserId();
            model.UserId = Gonderici;
            var GecmisSohbetGiden = ent.Gorusmeler.Where(g => g.GondericiId == Gonderici ).Select(go => go.AliciId).Distinct().ToList();
            var usermanager = IdentityTools.NewUserManager();
            List<ApplicationUser> Kullanicilar = new List<ApplicationUser>();
            foreach (var id in GecmisSohbetGiden)
            {
                var kullanici = usermanager.FindById(id);
                Kullanicilar.Add(kullanici);
            }
            var GecmisSohbetGelen = ent.Gorusmeler.Where(g => g.AliciId == Gonderici).Select(go => go.GondericiId).Distinct().ToList();
            foreach (var id in GecmisSohbetGelen)
            {
                var kullanici = usermanager.FindById(id);
                if (Kullanicilar.Contains(kullanici))
                {
                    continue;
                }
                else
                {
                    Kullanicilar.Add(kullanici);
                }
            }
            model.Kullanicilar = Kullanicilar.Reverse<ApplicationUser>().ToList(); //güncel konuşmanın geçmiş listesinde başa gelmesi için.
            return View(model);
        }
 
        /// <summary>
        /// SohbetGecmisi bir partial viewdır.
        /// </summary>
        public ActionResult SohbetGecmisi()
        {
            MsgHistViewModel Gorusme = new MsgHistViewModel();
            Gorusme.Baslik = "Görüntülemek istediğin sohbet için Geçmiş Konuşmalar bölümünden seçim yap.";
            return PartialView(Gorusme);
        }

        [HttpPost]
        public ActionResult SohbetGecmisi(string AliciId, string GondericiId)
        {
            MsgHistViewModel Gorusme = new MsgHistViewModel();
            if (AliciId == null || GondericiId == null)
            {
                Gorusme.Baslik = "Görüntülenecek mesaj bulunamadı";
            }
            else
            {
                Gorusme.AliciId = AliciId;
                Gorusme.GondericiId = GondericiId;
            var usermanager = IdentityTools.NewUserManager();
            var Arkadas = usermanager.FindById(AliciId);
            Gorusme.Baslik = Arkadas.Name + " ile olan mesaj geçmişin";
                List<Gorusmeler> TumKonusmalari = ent.Gorusmeler.Where(m => (m.AliciId == AliciId || m.GondericiId == AliciId) && (m.AliciId == GondericiId || m.GondericiId == GondericiId)).OrderBy(me => me.Tarih).ToList();
                List<MsgModel> Mesajlar = new List<MsgModel>();
                foreach (var konusma in TumKonusmalari)
                {
                    MsgModel Mesaj = new MsgModel();
                    Mesaj.AliciId = konusma.AliciId;
                    Mesaj.GondericiId = konusma.GondericiId;
                    Mesaj.Tarih = konusma.Tarih;
                    Mesaj.Mesaj = konusma.Mesaj;
                    Mesajlar.Add(Mesaj);
                }
                Gorusme.Mesajlar = Mesajlar;
            }
            return PartialView(Gorusme);
        }
        [HttpPost]
        public ActionResult MesajGonder(string AliciId, string GondericiId,string Mesaj)
        {
            Gorusmeler Gorusme = new Gorusmeler();
            Gorusme.AliciId = AliciId;
            Gorusme.GondericiId = GondericiId;
            Gorusme.Mesaj = Mesaj;
            ent.Gorusmeler.Add(Gorusme);
            if (Convert.ToBoolean(ent.SaveChanges()))
            {
                MsgHistViewModel Guncellet = new MsgHistViewModel();
                Guncellet.AliciId = AliciId;
                Guncellet.GondericiId = GondericiId;
                return PartialView("SohbetGecmisi",Guncellet);
            }
            return PartialView("Error");
        }
        [HttpPost]
        public ActionResult IlanBildir(string IlanId, string GondericiId, string Mesaj)
        {
            var usermanager = IdentityTools.NewUserManager();
            var Gonderici = usermanager.FindById(GondericiId);
            int Ilanid = Convert.ToInt32(IlanId);
            Ilan Ilan = ent.Ilanlar.Where(i => i.Id == Ilanid).FirstOrDefault();
            IdentityMessage msg = new IdentityMessage();
            msg.Subject = Ilan.Baslik + " ilanı hakkında bildirim";
            msg.Destination = "m720694@outlook.com";
            var callbackUrl = Url.Action("OtherEventDetail", "Home", new { Id= IlanId }, protocol: Request.Url.Scheme);
            msg.Body = "İlanlarda yer alan <a href=\"" + callbackUrl + "\">" + Ilan.Baslik + "</a> için <strong><u>" + Mesaj + "</u></strong>şeklinde bir bildirim geldi.";
            Mailing Mail = new Mailing();
            Mail.SendMail(msg);
            if (true)
            {
                return RedirectToAction("Main","Home");
            }
            return PartialView("Error");
        }
    }
}