using Microsoft.AspNet.Identity;
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
    public class HomeController : BaseController
    {

        public ActionResult Index(string ReturnUrl)
        {
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                return Redirect("/Home/Index#Giris");
            }
            return View();
        }
        [Authorize]
        public ActionResult Main()
        {
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());

            ViewBag.ilanlar = repoI.GetAll().Where(i => i.Silindi == false && i.KullaniciId != HttpContext.User.Identity.GetUserId() && i.Yayindami == true).OrderByDescending(i => i.OlusturmaTarihi).ToList();

            ViewBag.KullanicininIlanlari = repoI.GetAll().Where(i => i.Silindi == false && i.KullaniciId == HttpContext.User.Identity.GetUserId() && i.Yayindami == true).OrderByDescending(i => i.OlusturmaTarihi).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Main(Ilan model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Ilan yeni = new Ilan();
            yeni.Baslik = model.Baslik;

            if (!ModelState.IsValid)
                return View(model);

            if (model.BaslangicTarihi < DateTime.Now)
            {
                ModelState.AddModelError("", "Başlangıç Tarihi İleri Bir Tarih Olmalı!");
                return View("MyEventDetail", model);
            }
            if (model.Kontenjan <= 0)
            {
                ModelState.AddModelError("", "Kontenjan 0'dan Büyük Olmalı!");
                return View("MyEventDetail", model);
            }
            yeni.BaslangicTarihi = model.BaslangicTarihi;
            yeni.Il = "İstanbul";
            yeni.Ilce = model.Ilce;
            yeni.Kontenjan = model.Kontenjan;
            yeni.OlusturmaTarihi = DateTime.Now;
            yeni.Silindi = false;
            yeni.Yayindami = true;//Değişecek.
            yeni.Ozet = model.Ozet;
            yeni.KullaniciId = HttpContext.User.Identity.GetUserId();
            yeni.GoruntulenmeSayaci = 1;
            if (repoI.Add(yeni))
            {
                return RedirectToAction("Main", "Home");
            }
            return View(model);
        }
        [Authorize]
        public ActionResult MyEventDetail(int Id)
        {
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Ilan etk = repoI.Get(i => i.Id == Id);
            return View(etk);
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MyEventDetail(Ilan model)//değişebilir.
        {
            return View();
        }
        [Authorize]
        public ActionResult MyProfile()
        {
            return View();
        }
        [Authorize]
        public ActionResult OtherProfile(string Id)
        {
            Repository<ApplicationUser> repoU = new Repository<ApplicationUser>(new NeYapsakContext());
            Repository<Ilgilenen> repoIlg = new Repository<Ilgilenen>(new NeYapsakContext());
            Repository<Katilan> repoKat = new Repository<Katilan>(new NeYapsakContext());
            Repository<Ilan> repoIlan = new Repository<Ilan>(new NeYapsakContext());
            UserViewModel usermodel = new UserViewModel();
            usermodel.Kullanici = repoU.GetAll().Where(u => u.Id == Id).FirstOrDefault();
            usermodel.KullaniciIlanlari = repoIlan.GetAll().Where(i => i.KullaniciId == Id).ToList();
            usermodel.IlgilendigiIlanSayisi = repoIlg.GetAll().Where(i => i.KullaniciId == Id).Count();
            usermodel.KatildigiIlanSayisi = repoKat.GetAll().Where(k => k.KullaniciId == Id).Count();
            return View(usermodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult MyProfile(UserViewModel model)
        {
            Repository<ApplicationUser> repoU = new Repository<ApplicationUser>(new NeYapsakContext());
            if (model.PictureUpload != null)
            {
                //string name = Path.GetFileNameWithoutExtension(model.PictureUpload.FileName);
                //string ext = Path.GetExtension(model.PictureUpload.FileName);
                ////Resim upload edilecek.
                //name = name.Replace(" ", "");
                string filename = model.PictureUpload.FileName;
                string imagePath = Server.MapPath("/images/" + filename);
                model.PictureUpload.SaveAs(imagePath);
                ApplicationUser degisen = repoU.GetAll().Where(u => u.Id == HttpContext.User.Identity.GetUserId()).FirstOrDefault();
                degisen.ProfilAvatarYolu = "/images/" + filename;
                if (repoU.Update(degisen))
                    return RedirectToAction("MyProfile");
                return View(model);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult HKDuzenle(UserViewModel model)
        {
            Repository<ApplicationUser> repoU = new Repository<ApplicationUser>(new NeYapsakContext());
            if (model.Kullanici.Bio != null && model.Kullanici.Bio.Length >= 10)
            {
                ApplicationUser degisen = repoU.GetAll().Where(u => u.Id == HttpContext.User.Identity.GetUserId()).FirstOrDefault();
                degisen.Bio = model.Kullanici.Bio;
                if (repoU.Update(degisen))
                    return RedirectToAction("MyProfile");
                return View("MyProfile", model);
            }
            return View("MyProfile");
        }
        [Authorize]
        public ActionResult OtherEventDetail(int Id)
        {
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Ilan etk = repoI.Get(i => i.Id == Id);
            return View(etk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EtkDuzenle(Ilan model)
        {
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Ilan degisen = repoI.GetAll().Where(i => i.Id == model.Id).FirstOrDefault();
            if (!ModelState.IsValid)
                return View(degisen);

            if (model.BaslangicTarihi < DateTime.Now)
            {
                ModelState.AddModelError("", "Başlangıç Tarihi İleri Bir Tarih Olmalı!");
                return View("MyEventDetail", degisen);
            }
            if (model.Kontenjan <= 0)
            {
                ModelState.AddModelError("", "Kontenjan 0'dan Büyük Olmalı!");
                return View("MyEventDetail", degisen);
            }
            degisen.BaslangicTarihi = model.BaslangicTarihi;
            degisen.Baslik = model.Baslik;
            degisen.GoruntulenmeSayaci = model.GoruntulenmeSayaci;
            degisen.Il = model.Il;
            degisen.Ilce = model.Ilce;
            degisen.Kontenjan = model.Kontenjan;
            degisen.Konum = model.Konum;
            degisen.Ozet = model.Ozet;
            if (repoI.Update(degisen))
            {
                return Redirect("/Home/MyEventDetail/" + degisen.Id);

            }
            return View("MyEventDetail", degisen);
        }
        [Authorize]
        public ActionResult EtkSil(int Id)
        {
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Ilan etk = repoI.Get(i => i.Id == Id);
            etk.Silindi = true;
            if (repoI.Delete(Id))
            {
                return Redirect("/Home/Main");

            }
            return View("MyEventDetail", etk);
        }
    }
}