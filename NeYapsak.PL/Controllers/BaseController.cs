using NeYapsak.BLL.Repository;
using NeYapsak.DAL.Context;
using NeYapsak.Entity.Entity;
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
            //Repository<Ilan> repoI = new Repository<Ilan>(new NeYapsakContext());
            //ViewBag.ilanlar = repoI.GetAll().Where(i => i.Silindi == false && i.Yayindami == true).OrderByDescending(i => i.OlusturmaTarihi).ToList();

            base.OnActionExecuting(filterContext);
        }
    }
}