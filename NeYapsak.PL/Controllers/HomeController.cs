﻿using Microsoft.AspNet.Identity;
using NeYapsak.BLL.Repository;
using NeYapsak.DAL.Context;
using NeYapsak.Entity.Entity;
using NeYapsak.Entity.Identity;
using NeYapsak.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NeYapsak.PL.Controllers
{
    public class HomeController : BaseController
    {
        Mailing mail = new Mailing();
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
            MainViewModel MainModel = new MainViewModel();
            MainModel.KullaniciId = HttpContext.User.Identity.GetUserId();
            MainModel.DigerIlanlar = repoI.GetAll().Where(i => i.Silindi == false && i.KullaniciId != MainModel.KullaniciId && i.Yayindami == true).OrderByDescending(i => i.OlusturmaTarihi).ToList();

            MainModel.KullanicininIlanlari = repoI.GetAll().Where(i => i.Silindi == false && i.KullaniciId == MainModel.KullaniciId && i.Yayindami == true).OrderByDescending(i => i.OlusturmaTarihi).ToList();

            return View(MainModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Main(MainViewModel model, List<int> EtiketIDleri)
        {
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            List<string> errors = new List<string>();
            if (!ModelState.IsValid)
            {
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            Ilan yeni = new Ilan();
            yeni.Baslik = model.Ilan.Baslik;

            if (model.Ilan.BaslangicTarihi < DateTime.Now)
            {
                ModelState.AddModelError("", "Başlangıç Tarihi İleri Bir Tarih Olmalı!");
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json(errors, JsonRequestBehavior.AllowGet);
            }
            if (model.Ilan.Kontenjan <= 0)
            {
                ModelState.AddModelError("", "Kontenjan 0'dan Büyük Olmalı!");
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json(errors, JsonRequestBehavior.AllowGet);
            }
            yeni.BaslangicTarihi = model.Ilan.BaslangicTarihi;
            yeni.Il = "İstanbul";
            yeni.Ilce = model.Ilan.Ilce;
            yeni.Kontenjan = model.Ilan.Kontenjan;
            yeni.OlusturmaTarihi = DateTime.Now;
            yeni.Silindi = false;
            yeni.Yayindami = true;//Değişecek.
            yeni.Ozet = model.Ilan.Ozet;
            yeni.Konum = "Girilmedi";
            yeni.KullaniciId = HttpContext.User.Identity.GetUserId();
            yeni.GoruntulenmeSayaci = 0;
            if (repoI.Add(yeni))
            {
                if (EtiketIDleri != null)
                {
                    Repository<IlanEtiket> repoİE = new Repository<IlanEtiket>(new NeYapsakContext());
                    List<IlanEtiket> ieESkiler = repoİE.GetAll().Where(e => e.IlanId == yeni.Id).ToList();
                    IlanEtiket ie = new IlanEtiket();

                    if (ieESkiler.Count() > 0)
                    {
                        foreach (var ieeski in ieESkiler)
                        {
                            ieeski.Silindi = true;
                            if (!repoİE.Update(ieeski))
                            {
                                ModelState.AddModelError("", ieeski.Etiket.EtiketAdi + " Etiketi Eklenemedi!");
                            }
                        }

                        foreach (var id in EtiketIDleri)
                        {
                            bool var = false;
                            foreach (var item in ieESkiler)
                            {
                                if (id == item.EtiketId)
                                {
                                    var = true;
                                }
                            }
                            if (!var)
                            {
                                IlanEtiket y = new IlanEtiket();
                                y.EtiketId = id;
                                y.IlanId = yeni.Id;
                                if (!repoİE.Add(y))
                                {
                                    ModelState.AddModelError("", y.Etiket.EtiketAdi + " Etiketi Eklenemedi!");
                                }
                            }
                            else
                            {
                                IlanEtiket y = new IlanEtiket();
                                y = repoİE.Get(e => e.EtiketId == id);
                                y.Silindi = false;
                                if (!repoİE.Update(y))
                                {
                                    ModelState.AddModelError("", y.Etiket.EtiketAdi + " Etiketi Eklenemedi!");
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in EtiketIDleri)
                        {
                            IlanEtiket y = new IlanEtiket();
                            y.EtiketId = item;
                            y.IlanId = yeni.Id;
                            if (!repoİE.Add(y))
                            {
                                ModelState.AddModelError("", y.Etiket.EtiketAdi + " Etiketi Eklenemedi!");
                            }
                        }
                    }
                }

                return Json("True", JsonRequestBehavior.AllowGet);
            }
            errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
            return Json(errors, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        public JsonResult KatilmaIstegiGonder(int EtkID)
        {
            string result = "false";
            Repository<Katilan> repoK = new Repository<Katilan>(new NeYapsakContext());
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Repository<ApplicationUser> repoA = new Repository<ApplicationUser>(new NeYapsakContext());
            ApplicationUser User = repoA.GetAll().Where(u => u.Id == HttpContext.User.Identity.GetUserId()).FirstOrDefault();
            Katilan kat = new Katilan();

            foreach (Katilan katilan in repoK.GetAll(k => k.IlanId == EtkID).ToList())
            {
                if (katilan.KullaniciId == HttpContext.User.Identity.GetUserId())
                {
                    if (katilan.Silindi == true)
                    {
                        katilan.Silindi = false;
                        katilan.Tarih = DateTime.Now;
                        if (repoK.Update(katilan))
                        {
                            //IdentityMessage msg = new IdentityMessage();
                            //msg.Subject = "Etkinliğine Katılmak İsteyenler Var.";
                            //msg.Destination = katilan.Ilan.User.Email;
                            //msg.Body = "Merhaba " + katilan.Ilan.User.Name + ", " + katilan.User.Name + " " + katilan.User.Surname + " " + katilan.Ilan.BaslangicTarihi + " tarihinde yapacağın " + katilan.Ilan.Baslik + " etkinliğine katılmak istiyor. Onaylamak veya Reddetmek için profil sayfana gidebilirsin.";
                            //mail.SendMail(msg);
                            result = "true";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Daha Önce Katılma İsteği Gönderdiniz!");
                        result = "Daha Önce Katılma İsteği Gönderdiniz!";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            Ilan i = repoI.GetAll().Where(k => k.Id == EtkID).FirstOrDefault();
            kat.IlanId = EtkID;
            kat.KullaniciId = HttpContext.User.Identity.GetUserId();
            kat.Tarih = DateTime.Now;
            kat.Silindi = false;
            kat.Onay = false;
            if (repoK.Add(kat))
            {
                //IdentityMessage msg = new IdentityMessage();
                //msg.Subject = "Etkinliğine Katılmak İsteyenler Var.";
                //msg.Destination = i.User.Email;
                //msg.Body = "Merhaba " + i.User.Name + ", " + User.Name + " " + User.Surname + " " + i.BaslangicTarihi + " tarihinde yapacağın " + i.Baslik + " etkinliğine katılmak istiyor. Onaylamak veya Reddetmek için profil sayfana gidebilirsin.";
                //mail.SendMail(msg);
                result = "true";
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult MyEventDetail(int? Id)
        {
            if (Id != null)
            {
                Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
                Ilan etk = repoI.Get(i => i.Id == Id);
                if (etk.KullaniciId == HttpContext.User.Identity.GetUserId())
                {
                    return View(etk);

                }

                return Redirect("/Home/OtherEventDetail/" + Id);
            }
            return Redirect("/Home/Main");
        }

        [Authorize]
        public JsonResult EtkSil(int EtkID)
        {
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Repository<Katilan> repoK = new Repository<Katilan>(new NeYapsakContext());
            Ilan etk = repoI.Get(i => i.Id == EtkID);
            if (etk.KullaniciId == HttpContext.User.Identity.GetUserId())
            {
                etk.Silindi = true;//update ile aynı.
                if (repoI.Delete(EtkID))
                {
                    foreach (Katilan k in repoK.GetAll().Where(k => k.IlanId == etk.Id).ToList())
                    {
                        k.Onay = false;
                        k.Silindi = true;
                        if (!repoK.Update(k))
                        {
                            return Json("Katılanlar silinemedi.", JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json("True", JsonRequestBehavior.AllowGet);

                }
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            return Json("Bu Etkinlik Başkasının", JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public ActionResult MyEventDetail(Ilan model)//değişebilir.
        //{
        //    return View();
        //}

        //Burada MyEventDetail postu kullanılmıyor MyEventDetail syafasından EtkinlikDuzenle Action'ına post oluyor.

        [HttpPost]
        [Authorize]
        public ActionResult EtkinlikDuzenle(Ilan model, List<int> EtiketIDleri)
        {
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Repository<IlanEtiket> repoİE = new Repository<IlanEtiket>(new NeYapsakContext());
            Ilan degisen = repoI.GetAll().Where(i => i.Id == model.Id).FirstOrDefault();
            List<string> errors = new List<string>();
            if (!ModelState.IsValid)
            {
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            if (model.BaslangicTarihi < DateTime.Now)
            {
                ModelState.AddModelError("", "Başlangıç Tarihi İleri Bir Tarih Olmalı!");
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json(errors, JsonRequestBehavior.AllowGet);
            }
            if (model.Kontenjan <= 0)
            {
                ModelState.AddModelError("", "Kontenjan 0'dan Büyük Olmalı!");
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json(errors, JsonRequestBehavior.AllowGet);
            }
            if (EtiketIDleri != null)
            {
                List<IlanEtiket> ieESkiler = repoİE.GetAll().Where(e => e.IlanId == degisen.Id).ToList();
                IlanEtiket ie = new IlanEtiket();

                if (ieESkiler.Count() > 0)
                {
                    foreach (var ieeski in ieESkiler)
                    {
                        ieeski.Silindi = true;
                        if (!repoİE.Update(ieeski))
                        {
                            ModelState.AddModelError("", ieeski.Etiket.EtiketAdi + " Etiketi Eklenemedi!");
                        }
                    }

                    foreach (var id in EtiketIDleri)
                    {
                        bool var = false;
                        foreach (var item in ieESkiler)
                        {
                            if (id == item.EtiketId)
                            {
                                var = true;
                            }
                        }
                        if (!var)
                        {
                            IlanEtiket yeni = new IlanEtiket();
                            yeni.EtiketId = id;
                            yeni.IlanId = degisen.Id;
                            if (!repoİE.Add(yeni))
                            {
                                ModelState.AddModelError("", yeni.Etiket.EtiketAdi + " Etiketi Eklenemedi!");
                            }
                        }
                        else
                        {
                            IlanEtiket yeni = new IlanEtiket();
                            yeni = repoİE.Get(e => e.EtiketId == id);
                            yeni.Silindi = false;
                            if (!repoİE.Update(yeni))
                            {
                                ModelState.AddModelError("", yeni.Etiket.EtiketAdi + " Etiketi Eklenemedi!");
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in EtiketIDleri)
                    {
                        IlanEtiket yeni = new IlanEtiket();
                        yeni.EtiketId = item;
                        yeni.IlanId = degisen.Id;
                        if (!repoİE.Add(yeni))
                        {
                            ModelState.AddModelError("", yeni.Etiket.EtiketAdi + " Etiketi Eklenemedi!");
                        }
                    }
                }
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
                return Json("True", JsonRequestBehavior.AllowGet);

            }
            errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
            return Json(errors, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            Repository<ApplicationUser> repoU = new Repository<ApplicationUser>(new NeYapsakContext());
            Repository<Ilan> repoIlan = new Repository<Ilan>(new NeYapsakContext());
            Repository<Katilan> repoKat = new Repository<Katilan>(new NeYapsakContext());
            UserViewModel usermodel = new UserViewModel();

            usermodel.Kullanici = repoU.GetAll().Where(u => u.Id == HttpContext.User.Identity.GetUserId()).FirstOrDefault();

            usermodel.KullaniciIlanlari = repoIlan.GetAll().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId()).ToList();

            usermodel.IlgilendigiIlanlar = repoKat.GetAll().Where(k => k.KullaniciId == HttpContext.User.Identity.GetUserId() && k.Onay == false && k.Silindi == false).Select(k => k.Ilan).Distinct().ToList();

            usermodel.KatildigiIlanlar = repoKat.GetAll().Where(k => k.KullaniciId == HttpContext.User.Identity.GetUserId() && k.Onay == true && k.Silindi == false).Select(k => k.Ilan).Distinct().ToList();

            usermodel.OnayimiBekleyenIlanlar = repoKat.GetAll().Where(k => k.Onay == false && k.Silindi == false).Select(k => k.Ilan).Distinct().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId() && i.Silindi == false).ToList();

            usermodel.OnayladigimIlanlar = repoKat.GetAll().Where(k => k.Onay == true && k.Silindi == false).Select(k => k.Ilan).Distinct().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId() && i.Silindi == false).ToList();

            return View(usermodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult MyProfile(UserViewModel model)
        {
            Repository<ApplicationUser> repoU = new Repository<ApplicationUser>(new NeYapsakContext());
            Repository<Ilan> repoIlan = new Repository<Ilan>(new NeYapsakContext());
            Repository<Katilan> repoKat = new Repository<Katilan>(new NeYapsakContext());

            model.Kullanici = repoU.GetAll().Where(u => u.Id == HttpContext.User.Identity.GetUserId()).FirstOrDefault();

            model.KullaniciIlanlari = repoIlan.GetAll().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId()).ToList();

            model.IlgilendigiIlanlar = repoKat.GetAll().Where(k => k.KullaniciId == HttpContext.User.Identity.GetUserId() && k.Onay == false && k.Silindi == false).Select(k => k.Ilan).Distinct().ToList();

            model.KatildigiIlanlar = repoKat.GetAll().Where(k => k.KullaniciId == HttpContext.User.Identity.GetUserId() && k.Onay == true && k.Silindi == false).Select(k => k.Ilan).Distinct().ToList();

            model.OnayimiBekleyenIlanlar = repoKat.GetAll().Where(k => k.Onay == false && k.Silindi == false).Select(k => k.Ilan).Distinct().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId() && i.Silindi == false).ToList();

            model.OnayladigimIlanlar = repoKat.GetAll().Where(k => k.Onay == true && k.Silindi == false).Select(k => k.Ilan).Distinct().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId() && i.Silindi == false).ToList();

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
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult HakkimdaDuzenle(UserViewModel model)
        {
            Repository<ApplicationUser> repoU = new Repository<ApplicationUser>(new NeYapsakContext());
            Repository<Ilan> repoIlan = new Repository<Ilan>(new NeYapsakContext());
            Repository<Katilan> repoKat = new Repository<Katilan>(new NeYapsakContext());

            model.KullaniciIlanlari = repoIlan.GetAll().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId()).ToList();

            model.IlgilendigiIlanlar = repoKat.GetAll().Where(k => k.KullaniciId == HttpContext.User.Identity.GetUserId() && k.Onay == false && k.Silindi == false).Select(k => k.Ilan).Distinct().ToList();

            model.KatildigiIlanlar = repoKat.GetAll().Where(k => k.KullaniciId == HttpContext.User.Identity.GetUserId() && k.Onay == true && k.Silindi == false).Select(k => k.Ilan).Distinct().ToList();

            model.OnayimiBekleyenIlanlar = repoKat.GetAll().Where(k => k.Onay == false && k.Silindi == false).Select(k => k.Ilan).Distinct().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId() && i.Silindi == false).ToList();

            model.OnayladigimIlanlar = repoKat.GetAll().Where(k => k.Onay == true && k.Silindi == false).Select(k => k.Ilan).Distinct().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId() && i.Silindi == false).ToList();
            if (model.Kullanici.Bio != null && model.Kullanici.Bio.Length >= 10)
            {
                ApplicationUser degisen = repoU.GetAll().Where(u => u.Id == HttpContext.User.Identity.GetUserId()).FirstOrDefault();
                degisen.Bio = model.Kullanici.Bio;
                model.Kullanici = degisen;
                if (repoU.Update(degisen))
                    return RedirectToAction("MyProfile");
                return View("MyProfile", model);
            }
            return View("MyProfile", model);
        }


        [HttpPost]
        [Authorize]
        public JsonResult KatilmaktanVazgec(int EtkIDIptal)
        {
            string result = "false";
            Repository<Katilan> repoK = new Repository<Katilan>(new NeYapsakContext());
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Katilan katilan = repoK.GetAll().Where(k => k.KullaniciId == HttpContext.User.Identity.GetUserId() && k.IlanId == EtkIDIptal).FirstOrDefault();
            katilan.Silindi = true;
            katilan.Onay = false;
            Ilan i = repoI.GetAll().Where(k => k.Id == katilan.IlanId).FirstOrDefault();
            i.Kontenjan += 1;
            if (!repoI.Update(i))
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (repoK.Delete(katilan.Id))
            {
                result = "true";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        public JsonResult KatilmaIstegiIptal(int IstekIptalEtkID)
        {
            string result = "false";
            Repository<Katilan> repoK = new Repository<Katilan>(new NeYapsakContext());
            Katilan katilan = repoK.GetAll().Where(k => k.KullaniciId == HttpContext.User.Identity.GetUserId() && k.IlanId == IstekIptalEtkID).FirstOrDefault();
            katilan.Silindi = true;
            katilan.Onay = false;
            if (repoK.Delete(katilan.Id))
            {
                result = "true";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult OtherProfile(string Id)
        {
            Repository<ApplicationUser> repoU = new Repository<ApplicationUser>(new NeYapsakContext());
            Repository<Katilan> repoKat = new Repository<Katilan>(new NeYapsakContext());
            Repository<Ilan> repoIlan = new Repository<Ilan>(new NeYapsakContext());
            UserViewModel usermodel = new UserViewModel();
            usermodel.Kullanici = repoU.GetAll().Where(u => u.Id == Id).FirstOrDefault();
            usermodel.KullaniciIlanlari = repoIlan.GetAll().Where(i => i.KullaniciId == Id).ToList();
            usermodel.IlgilendigiIlanlar = repoKat.GetAll().Where(k => k.KullaniciId == Id && k.Onay == false && k.Silindi == false).Select(k => k.Ilan).Distinct().ToList();
            usermodel.KatildigiIlanlar = repoKat.GetAll().Where(k => k.KullaniciId == Id && k.Onay == true && k.Silindi == false).Select(k => k.Ilan).Distinct().ToList();
            usermodel.OnayimiBekleyenIlanlar = repoKat.GetAll().Where(k => k.Onay == false && k.Silindi == false).Select(k => k.Ilan).Distinct().Where(i => i.KullaniciId == Id && i.Silindi == false).ToList();
            usermodel.OnayladigimIlanlar = repoKat.GetAll().Where(k => k.Onay == true && k.Silindi == false).Select(k => k.Ilan).Where(i => i.KullaniciId == Id && i.Silindi == false).ToList();
            if (Id == HttpContext.User.Identity.GetUserId())
            {
                return Redirect("/Home/MyProfile/" + Id);
            }
            return View(usermodel);
        }


        [Authorize]
        public JsonResult GSayacArttir(int EtkID)
        {
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Ilan etk = repoI.Get(i => i.Id == EtkID);
            etk.GoruntulenmeSayaci += 1;
            if (!repoI.Update(etk))
            {
                return Json("Sayaç Arttırılamadı.", JsonRequestBehavior.AllowGet);
            }
            return Json("True", JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult OtherEventDetail(int? Id)
        {
            if (Id != null)
            {
                Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
                Ilan etk = repoI.Get(i => i.Id == Id);
                if (etk.User.Id == HttpContext.User.Identity.GetUserId())
                {
                    return Redirect("/Home/MyEventDetail/" + Id);
                }
                ViewBag.ktldurumOnay = Convert.ToBoolean(etk.Katilanlar.Where(k => k.Silindi == false && (k.Onay == true && k.KullaniciId == HttpContext.User.Identity.GetUserId())).Count());

                ViewBag.ktldurumistek = Convert.ToBoolean(etk.Katilanlar.Where(k => k.Silindi == false && (k.Onay == false && k.KullaniciId == HttpContext.User.Identity.GetUserId())).Count());

                return View(etk);
            }
            return Redirect("/Home/Main");
        }


        [HttpPost]
        [Authorize]
        public JsonResult EtkinlikOnayla(int KatilanIDOnayla, string OnayDurumu)
        {
            string result = "false";
            Repository<Katilan> repoK = new Repository<Katilan>(new NeYapsakContext());
            Katilan katilan = repoK.GetAll().Where(k => k.Id == KatilanIDOnayla).FirstOrDefault();
            if (OnayDurumu == "Onayla")
            {
                katilan.Onay = true;
                Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
                Ilan i = repoI.GetAll().Where(k => k.Id == katilan.IlanId).FirstOrDefault();
                i.Kontenjan -= 1;
                if (!repoI.Update(i))
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else if (OnayDurumu == "Reddet")
            {
                katilan.Silindi = true;
                katilan.Onay = false;
            }
            if (repoK.Update(katilan))
            {
                result = "true";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult EtkinlikOnayiIptal(int KatilanIDOnayIptal)
        {
            string result = "false";
            Repository<Katilan> repoK = new Repository<Katilan>(new NeYapsakContext());
            Katilan katilan = repoK.GetAll().Where(k => k.Id == KatilanIDOnayIptal).FirstOrDefault();
            Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            Ilan i = repoI.GetAll().Where(k => k.Id == katilan.IlanId).FirstOrDefault();

            katilan.Onay = false;
            katilan.Silindi = true;
            if (!repoK.Update(katilan))
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            i.Kontenjan += 1;
            if (!repoI.Update(i))
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result = "true";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult EventConfirmPage(int Id)
        {
            Repository<Ilan> repoIlan = new Repository<Ilan>(new NeYapsakContext());
            Repository<Katilan> repoKat = new Repository<Katilan>(new NeYapsakContext());
            EventConfirmViewModel EventModel = new EventConfirmViewModel();

            EventModel.Ilan = repoIlan.GetAll().Where(i => i.Id == Id).FirstOrDefault();

            EventModel.OnayBekleyenIlanlar = repoKat.GetAll().Where(k => k.Onay == false && k.Silindi == false).Select(k => k.Ilan).Distinct().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId() && i.Silindi == false).ToList();

            EventModel.OnaylanmisIlanlar = repoKat.GetAll().Where(k => k.Onay == true && k.Silindi == false).Select(k => k.Ilan).Distinct().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId() && i.Silindi == false).ToList();


            if (EventModel != null)
            {
                return View(EventModel);
            }
            return Redirect("/Home/MyProfile#collapseTwo");
        }
    }
}