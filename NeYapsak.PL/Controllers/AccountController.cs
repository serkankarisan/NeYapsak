﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using NeYapsak.BLL.Identity;
using NeYapsak.DAL.Context;
using NeYapsak.Entity.Identity;
using NeYapsak.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NeYapsak.PL.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(LoginRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Home", model);
            var usermanager = IdentityTools.NewUserManager();
            var kullanici = usermanager.FindByEmail(model.RegisterView.Email);
            if (kullanici != null)
            {
                ModelState.AddModelError("", "Bu Email Sistemde Kayıtlı!");
                return RedirectToAction("Index", "Home", model);
            }
            ApplicationUser user = new ApplicationUser();
            user.Name = model.RegisterView.Name;
            user.Surname = model.RegisterView.Surname;
            user.Email = model.RegisterView.Email;
            user.UserName = model.RegisterView.Email;
            user.DogumTarihi =model.RegisterView.DogumTarihi;
            var result = usermanager.Create(user, model.RegisterView.Password);
            if (result.Succeeded)
            {
                usermanager.AddToRole(user.Id, "User");
                return RedirectToAction("Index", "Home");//sayfa hazırlanıyor.
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return RedirectToAction("Index", "Home", model);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //ViewBag.returnUrl = returnUrl;
            LoginViewModel model = new LoginViewModel() { returnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var usermanager = IdentityTools.NewUserManager();
            var kullanici = usermanager.FindByEmail(model.Email);
            if (kullanici == null)
            {
                ModelState.AddModelError("", "Böyle Bir Kullanıcı Kayıtlı Değil!");
                return View(model);
            }
            else
            {
                var authManager = HttpContext.GetOwinContext().Authentication;
                var identity = usermanager.CreateIdentity(kullanici, "ApplicationCookie");
                var authProperty = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                };
                authManager.SignIn(authProperty, identity);
                return Redirect(string.IsNullOrEmpty(model.returnUrl) ? "/" : model.returnUrl);
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult LogOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}