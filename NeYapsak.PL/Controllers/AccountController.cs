using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NeYapsak.BLL.Identity;
using NeYapsak.BLL.Repository;
using NeYapsak.DAL.Context;
using NeYapsak.Entity.Identity;
using NeYapsak.PL.App_Start;
using NeYapsak.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApplicationUser = NeYapsak.Entity.Identity.ApplicationUser;

namespace NeYapsak.PL.Controllers
{
    public class AccountController : BaseController
    {
        Mailing mail = new Mailing();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult Register(RegisterViewModel model)
        {
            List<string> errors = new List<string>();
            if (model.Password.Count()<8)
            {
                ModelState.AddModelError("", "Şifre en az 8 karakter uzunluğunda olmalıdır!");
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json(errors, JsonRequestBehavior.AllowGet);
            }
            if (!ModelState.IsValid)
            {
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json(errors, JsonRequestBehavior.AllowGet);
            }
            var usermanager = IdentityTools.NewUserManager();
            var kullanici = usermanager.FindByEmail(model.Email);
            if (kullanici != null)
            {
                ModelState.AddModelError("", "Bu Email Sistemde Kayıtlı!");
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json(errors, JsonRequestBehavior.AllowGet);
            }
            ApplicationUser user = new ApplicationUser();
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.DogumTarihi = model.DogumTarihi;
            var result = usermanager.Create(user, model.Password);
            if (result.Succeeded)
            {
                usermanager.AddToRole(user.Id, "User");
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = "" }, protocol: Request.Url.Scheme);
                IdentityMessage msg = new IdentityMessage();
                msg.Destination = user.Email;
                msg.Body = "Nerdeyse tamamlandı! NeYapsak ağına katılmak için <a href=\"" + callbackUrl + "\">bu link</a>e tıkla.";
                msg.Subject = "NeYapsak Hesap Doğrulama Servisi";
                mail.SendMail(msg);
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
            return Json(errors, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DisplayEmail()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmEmail(string userId)
        {
            if (userId == null)
            {
                return View("Error");
            }
            var usermanager = IdentityTools.NewUserManager();
            ApplicationUser user = new ApplicationUser();
            NeYapsakContext ent = new NeYapsakContext();
            user = ent.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user != null)
            {
                user.EmailConfirmed = true;
                ent.SaveChanges();
            }
            else
            {
                return View("Error");
            }
            return View("ConfirmEmail");
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(PasswordResetRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var kullanici = await UserManager.FindByEmailAsync(model.Email);
            if (kullanici == null)
            {
                ModelState.AddModelError("", "Bu Email Sistemde Kayıtlı Değildir!");
                return View(model);
            }
            else
            {
                string code = await UserManager.GeneratePasswordResetTokenAsync(kullanici.Id);
                var callbackUrl = Url.Action("PasswordReset", "Account", new { email = kullanici.Email, userId = kullanici.Id, code = code }, protocol: Request.Url.Scheme);
                IdentityMessage msg = new IdentityMessage();
                msg.Destination = kullanici.Email;
                msg.Body = "Şifreni sıfırlama isteğini aldık, değerlendirdik ve uygun bulduk. İnsanlık hali, olur öyle. Sıfırlama işlemi için <a href=\"" + callbackUrl + "\">bu link</a>e tıkla.";
                msg.Subject = "NeYapsak Şifre Sıfırlama Servisi";
                mail.SendMail(msg);
                return View("DisplayPasswordReset");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordReset(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return View("Error");
            }
            var usermanager = IdentityTools.NewUserManager();
            var kullanici = usermanager.FindByEmail(email);
            if (kullanici == null)
            {
                return View("Error");
            }
            else
            {
                if (kullanici.Id != userId)
                {
                    return View("Error");
                }
                else
                {
                    if (!UserManager.VerifyUserTokenAsync(kullanici.Id, "ResetPassword", code).Result)
                    {
                        return View("Error");
                    }
                    else
                    {
                        PasswordResetViewModel model = new PasswordResetViewModel();
                        model.Email = email;
                        model.Code = code;
                        return View("PasswordReset", model);
                    }
                }

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PasswordReset(PasswordResetViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var kullanici = await UserManager.FindByEmailAsync(model.Email);
            var result = await UserManager.ResetPasswordAsync(kullanici.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("PasswordResetSuccess");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ChangePassword(PasswordChangeViewModel model)
        {
            var usermanager = IdentityTools.NewUserManager();
            var kullanici = usermanager.FindById(HttpContext.User.Identity.GetUserId());
            if (!ModelState.IsValid)
                return View(model);
            else if (!usermanager.CheckPassword(kullanici, model.OldPassword))
            {
                ModelState.AddModelError("", "Mevcut şifren bu değil!");
                return View(model);
            }
            var result = usermanager.ChangePassword(kullanici.Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                IdentityMessage msg = new IdentityMessage();
                msg.Subject = "Şifren değiştirildi!";
                msg.Destination = kullanici.Email;
                msg.Body = "Merhaba " + kullanici.Name + " az önce sitemiz üzerinden bir şifre yenileme işlemi gerçekleştirdin. Yeni şifreni sitemize bol bol giriş yaparak iyi günlerde kullanmanı dileriz.";
                mail.SendMail(msg);
                HttpContext.GetOwinContext().Authentication.SignOut();
                return View("PasswordResetSuccess");
            }
            else
            {
                return View("Error");
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //ViewBag.returnUrl = returnUrl;
            LoginViewModel model = new LoginViewModel() { returnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public JsonResult Login(LoginViewModel model)
        {
            List<string> errors = new List<string>();
            if (!ModelState.IsValid)
            {
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json(errors, JsonRequestBehavior.AllowGet);
            }
            var usermanager = IdentityTools.NewUserManager();
            var kullanici = usermanager.FindByName(model.Email);
            if (kullanici == null)
            {
                ModelState.AddModelError("", "Böyle bir kullanıcı kayıtlı değil!");
                errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                return Json(errors, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (!usermanager.CheckPassword(kullanici, model.Password))
                {
                    ModelState.AddModelError("", "Girilen şifre yanlış!");
                    errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                    return Json(errors, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (kullanici.EmailConfirmed)
                    {
                        var authManager = HttpContext.GetOwinContext().Authentication;
                        var identity = usermanager.CreateIdentity(kullanici, "ApplicationCookie");
                        var authProperty = new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe
                        };
                        authManager.SignIn(authProperty, identity);
                        return Json("True",JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ModelState.AddModelError("", "E-Posta doğrulamasını yapman gerekmektedir!");
                        errors = ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToList();
                        return Json(errors, JsonRequestBehavior.AllowGet);
                    }
                }
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