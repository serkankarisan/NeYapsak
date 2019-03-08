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
    public class HomeController : Controller
    {
        Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
        public ActionResult Index()
        {
            NeYapsakContext ent = new NeYapsakContext();
            List<Ilan> a = ent.Ilanlar.ToList();
            return View();
        }

        public ActionResult Main()
        {
            //ViewBag.Sehirler = new List<Sehirler>() {
            //    new Sehirler{ Plaka=1,SehirAdi="Adana"},new Sehirler{ Plaka=34,SehirAdi="İstanbul"},new Sehirler{ Plaka=6,SehirAdi="Ankara"},new Sehirler{ Plaka=35,SehirAdi="İzmir"},new Sehirler{ Plaka=61,SehirAdi="Trabzon"}
            //};

            ViewBag.ilanlar = repoI.GetAll().Where(i => i.Silindi == false && i.Yayindami == true).OrderByDescending(i => i.OlusturmaTarihi).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public ActionResult Main(Ilan model)
        {
            Ilan yeni = new Ilan();
            yeni.Baslik = model.Baslik;
            yeni.BaslangicTarihi = model.BaslangicTarihi;
            yeni.Il = model.Il;
            yeni.Ilce = model.Ilce;
            yeni.Kontenjan = model.Kontenjan;
            yeni.OlusturmaTarihi = DateTime.Now;
            yeni.Silindi = false;
            yeni.Yayindami = false;
            yeni.Ozet = model.Ozet;
            yeni.KullaniciId = "6e8edacb-d8f9-47b3-91f1-028643139e1d";
            yeni.GoruntulenmeSayaci = 1;
            repoI.Add(yeni);
            return View();
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