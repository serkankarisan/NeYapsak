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
using ApplicationUser = NeYapsak.Entity.Identity.ApplicationUser;

namespace NeYapsak.PL.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Repository<ApplicationUser> repoU = new Repository<ApplicationUser>(new NeYapsakContext());
            Repository<Ilan> repoIlan = new Repository<Ilan>(new NeYapsakContext());
            Repository<Ilgilenen> repoIlg = new Repository<Ilgilenen>(new NeYapsakContext());
            Repository<Katilan> repoKat = new Repository<Katilan>(new NeYapsakContext());

            UserViewModel usermodel = new UserViewModel();
            usermodel.Kullanici = repoU.GetAll().Where(u => u.Id == HttpContext.User.Identity.GetUserId()).FirstOrDefault();
            usermodel.KullaniciIlanlari = repoIlan.GetAll().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId()).ToList();
            usermodel.IlgilendigiIlanSayisi = repoIlg.GetAll().Where(i => i.KullaniciId == HttpContext.User.Identity.GetUserId()).Count();
            usermodel.KatildigiIlanSayisi = repoKat.GetAll().Where(k => k.KullaniciId == HttpContext.User.Identity.GetUserId()).Count();
            ViewBag.user = usermodel;

            base.OnActionExecuting(filterContext);
        }
    }
}