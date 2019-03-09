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
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Repository<ApplicationUser> repoU = new Repository<ApplicationUser>(new NeYapsakContext());
            Repository<Ilan> repoIlan = new Repository<Ilan>(new NeYapsakContext());
            Repository<Ilgilenen> repoIlg = new Repository<Ilgilenen>(new NeYapsakContext());
            Repository<Katilan> repoKat = new Repository<Katilan>(new NeYapsakContext());

            UserViewModel usermodel = new UserViewModel();
            usermodel.Kullanici = repoU.Get(u => u.Id == "6e8edacb-d8f9-47b3-91f1-028643139e1d");
            usermodel.KullaniciIlanlari = repoIlan.GetAll().Where(i => i.KullaniciId == "6e8edacb-d8f9-47b3-91f1-028643139e1d").ToList();
            usermodel.IlgilendigiIlanlar= repoIlg.GetAll().Where(i=>i.KullaniciId== "6e8edacb-d8f9-47b3-91f1-028643139e1d").ToList();
            usermodel.KatildigiIlanlar = repoKat.GetAll().Where(k => k.KullaniciId == "6e8edacb-d8f9-47b3-91f1-028643139e1d").ToList();
            ViewBag.user =usermodel ;

            base.OnActionExecuting(filterContext);
        }
    }
}