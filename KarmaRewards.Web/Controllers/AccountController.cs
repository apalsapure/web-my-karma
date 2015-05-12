using DotNetOpenAuth.AspNet;
using KarmaRewards.Infrastructure;
using KarmaRewards.Model;
using KarmaRewards.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace KarmaRewards.Web.Controllers
{
    [Authorize]
    public class AccountController : ErrorController
    {

        [InjectionConstructor]
        public AccountController(IIdentityService identityService, IUserService userService)
        {
            this.IdentityService = identityService;
            this.UserService = userService;
        }

        public IIdentityService IdentityService { get; set; }

        public IUserService UserService { get; set; }


        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [ActionName("Domain-Auth")]
        public async Task<ActionResult> DomainAuth()
        {
            #region Domain Authentication
            HttpRequest req = System.Web.HttpContext.Current.Request;
            Identity identity = null;
            // Extract the 'data' parameter from the request, if any.
            string data = req["data"];
            // If "data" token found, decrypt it and create the User's Identity
            if (data != null)
            {
                string token = EncryptionProvider.Decrypt<string>(data);
                string[] userInfo = token.Split('|');
                identity = Helper.BuildIdentity(userInfo[0], userInfo[1], userInfo[2],
                                                            userInfo[3], userInfo[4]);
            }
            #endregion

            #region Redirect User Back
            if (identity != null) { await FormAuth(identity); }
            #endregion

            return RedirectToLocal("~/");
        }

        private async Task FormAuth(Identity identity)
        {
            // Authenticate User
            var response = await this.IdentityService.AuthenticateAsync(new Credentials()
            {
                Type = "usernamepassword",
                Tokens = new Dictionary<string, string>() { 
                { "username", identity.Username}, 
                { "password", identity.Password } }
            });
            
            // Save Identity in cookie
            var claimIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, response.User.Id) }, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);

            claimIdentity.AddClaim(new Claim("UserName", response.User.Username));
            claimIdentity.AddClaim(new Claim("FirstName", response.User.FirstName));
            claimIdentity.AddClaim(new Claim("LastName", response.User.LastName));
            claimIdentity.AddClaim(new Claim("Email", response.User.Email));
            claimIdentity.AddClaim(new Claim("Type", response.User.Type));

            // Authenticate user to Owin
            Authentication.SignIn(new AuthenticationProperties() { IsPersistent = true }, claimIdentity);

        }


        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
    }
}
