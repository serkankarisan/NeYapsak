﻿using Microsoft.AspNet.Identity;
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
using ApplicationUser = NeYapsak.Entity.Identity.ApplicationUser;

namespace NeYapsak.PL.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Repository<ApplicationUser> repoU = new Repository<ApplicationUser>(new NeYapsakContext());
            Repository<Ilan> repoIlan = new Repository<Ilan>(new NeYapsakContext());
            Repository<Katilan> repoKat = new Repository<Katilan>(new NeYapsakContext());
            Repository<Etiket> repoE = new Repository<Etiket>(new NeYapsakContext());
            Repository<IlanEtiket> repoIE = new Repository<IlanEtiket>(new NeYapsakContext());
            ViewBag.Etiketler = repoE.GetAll().Where(e => e.Silindi == false).ToList();
            ViewBag.IlanEtiketler = repoIE.GetAll().Where(e => e.Silindi == false).ToList();

            LayoutViewModel LayoutModel = new LayoutViewModel();

            LayoutModel.Kullanici = repoU.GetAll().Where(u => u.Id == HttpContext.User.Identity.GetUserId()).FirstOrDefault();

            LayoutModel.KullaniciIlanSayisi = repoIlan.GetAll().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId()).Count();

            LayoutModel.IlgilendigiIlanSayisi = repoKat.GetAll().Where(k => k.KullaniciId == HttpContext.User.Identity.GetUserId() && k.Onay == false && k.Silindi == false).Select(k => k.Ilan).Distinct().Count();

            LayoutModel.KatildigiIlanSayisi = repoKat.GetAll().Where(k => k.KullaniciId == HttpContext.User.Identity.GetUserId() && k.Onay == true && k.Silindi == false).Select(k => k.Ilan).Distinct().Count();

            LayoutModel.OnayimiBekleyenIlanSayisi = repoKat.GetAll().Where(k => k.Onay == false && k.Silindi == false).Select(k => k.Ilan).Distinct().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId() && i.Silindi == false).Count();

            LayoutModel.OnayladigimIlanSayisi = repoKat.GetAll().Where(k => k.Onay == true && k.Silindi == false).Select(k => k.Ilan).Distinct().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId() && i.Silindi == false).Count();

            ViewBag.user = LayoutModel;
            ViewBag.UserID = HttpContext.User.Identity.GetUserId();           
            base.OnActionExecuting(filterContext);
        }
    }
}