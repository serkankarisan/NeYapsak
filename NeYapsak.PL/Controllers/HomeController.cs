using NeYapsak.BLL.Repository;
using NeYapsak.DAL.Context;
using NeYapsak.Entity.Entity;
using NeYapsak.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NeYapsak.PL.Controllers
{
    public class HomeController : BaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());

            ViewBag.ilanlar = repoI.GetAll().Where(i => i.Silindi == false && i.KullaniciId != "6e8edacb-d8f9-47b3-91f1-028643139e1d" && i.Yayindami == true).OrderByDescending(i => i.OlusturmaTarihi).ToList();

            ViewBag.KullanicininIlanlari = repoI.GetAll().Where(i => i.Silindi == false && i.KullaniciId== "6e8edacb-d8f9-47b3-91f1-028643139e1d" && i.Yayindami == true).OrderByDescending(i => i.OlusturmaTarihi).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public ActionResult Main(Ilan model)
        {
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Ilan yeni = new Ilan();
            yeni.Baslik = model.Baslik;
            yeni.BaslangicTarihi = model.BaslangicTarihi;
            yeni.Il = "İstanbul";
            yeni.Ilce = model.Ilce;
            yeni.Kontenjan = model.Kontenjan;
            yeni.OlusturmaTarihi = DateTime.Now;
            yeni.Silindi = false;
            yeni.Yayindami = true;//Değişecek.
            yeni.Ozet = model.Ozet;
            yeni.KullaniciId = "6e8edacb-d8f9-47b3-91f1-028643139e1d";
            yeni.GoruntulenmeSayaci = 1;
            if (repoI.Add(yeni))
            {
                return RedirectToAction("Main","Home");
            }
            return View(model);
        }

        public ActionResult EventDetail()
        {
            return View();
        }

        public ActionResult User()
        {
            return View();
        }
    }
}