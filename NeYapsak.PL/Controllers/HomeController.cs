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

            ViewBag.KullanicininIlanlari = repoI.GetAll().Where(i => i.Silindi == false && i.KullaniciId== HttpContext.User.Identity.GetUserId() && i.Yayindami == true).OrderByDescending(i => i.OlusturmaTarihi).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
            yeni.KullaniciId = HttpContext.User.Identity.GetUserId();
            yeni.GoruntulenmeSayaci = 1;
            if (repoI.Add(yeni))
            {
                return RedirectToAction("Main","Home");
            }
            return View(model);
        }
        [Authorize]
        public ActionResult EventDetail()
        {
            return View();
        }
        [Authorize]
        public ActionResult User()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult User(UserViewModel model)
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
                    return RedirectToAction("User");
                return View(model);
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult HKDuzenle(UserViewModel model)
        {
            Repository<ApplicationUser> repoU = new Repository<ApplicationUser>(new NeYapsakContext());
            if (model.Kullanici.Bio != null &&model.Kullanici.Bio.Length>=10)
            {
                ApplicationUser degisen = repoU.GetAll().Where(u => u.Id == HttpContext.User.Identity.GetUserId()).FirstOrDefault();
                degisen.Bio = model.Kullanici.Bio;
                if (repoU.Update(degisen))
                    return RedirectToAction("User");
                return View("User",model);
            }
            return View("User");
        }
    }
}