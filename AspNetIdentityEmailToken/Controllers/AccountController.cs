using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace IdentitySample.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager userManager
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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = userManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    // you could also set the provider in IdentityConfig.cs
                    var provider = new DpapiDataProtectionProvider("WebApp2015");
                    userManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create("UserToken"));

                    var code = userManager.GenerateEmailConfirmationToken(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    var callbackUrlFails = Url.Action("ConfirmEmailFails", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    // for the demo, don't send the email
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");

                    ViewBag.Link = callbackUrl;
                    ViewBag.LinkFails = callbackUrlFails;

                    return View("DisplayEmail");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            // you could also set the provider in IdentityConfig.cs
            var provider = new DpapiDataProtectionProvider("WebApp2015");
            userManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create("UserToken"));

            var result = userManager.ConfirmEmail(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        public ActionResult ConfirmEmailFails(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            // you could also set the provider in IdentityConfig.cs
            var provider = new DpapiDataProtectionProvider("WebApp2015");
            userManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create("X"));

            var result = userManager.ConfirmEmail(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}