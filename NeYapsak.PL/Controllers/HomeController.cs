using NeYapsak.DAL.Context;
using NeYapsak.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NeYapsak.PL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            NeYapsakContext ent = new NeYapsakContext();
            List<Ilan> a = ent.Ilanlar.ToList();
            return View();
        }

        public ActionResult Main()
        {
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