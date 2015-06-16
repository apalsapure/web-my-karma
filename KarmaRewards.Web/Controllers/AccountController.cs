using KarmaRewards.Infrastructure;
using KarmaRewards.Model;
using KarmaRewards.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
            try
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
                //identity = new Identity("apalsapure", "", AuthenticationProvider.Windows, "amar");
                if (identity != null) { await FormAuth(identity); }

                return RedirectToLocal("~/");
                #endregion
            }
            catch (Exception ex)
            {
                // TODO: handle error
            }
            return RedirectToAction("login", "account");
        }

        private async Task FormAuth(Identity identity)
        {
            // Authenticate User
            var response = await this.IdentityService.AuthenticateAsync(new Credentials()
            {
                Type = "usernamepassword",
                Tokens = new Dictionary<string, string>() { 
                { "username", identity.Username}, 
                { "password", Helper.EncryptPassword(identity.Username)} }
            });

            // check if user is enabled
            // if not redirect to login page
            if (!response.User.IsEnabled)
            {
                // TODO: Error Space
                throw new Exception("User is enabled");
            }

            PopulateClaims(response.User);

            await AccessManager.SetFeatureList(response.User);

        }

        private void PopulateClaims(User user)
        {
            // Save Identity in cookie
            var claimIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Id) }, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);

            claimIdentity.AddClaim(new Claim("Id", user.Id));
            claimIdentity.AddClaim(new Claim("UserName", user.Username));
            claimIdentity.AddClaim(new Claim("FirstName", user.FirstName));
            claimIdentity.AddClaim(new Claim("LastName", user.LastName));
            claimIdentity.AddClaim(new Claim("Email", user.Email));
            claimIdentity.AddClaim(new Claim("Provider", "Windows"));
            claimIdentity.AddClaim(new Claim("Joined", user.JoiningDate.ToString("MM/dd/yyyy")));
            claimIdentity.AddClaim(new Claim("Designation", user.Designation));
            claimIdentity.AddClaim(new Claim("Image", user.ImageUrl));

            // Authenticate user to Owin
            Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, claimIdentity);
        }

        public async Task<ActionResult> Logout(string returnUrl)
        {
            Authentication.SignOut();

            try
            {
                await IdentityService.LogoutAsync();
            }
            catch
            {
                // suppress the error
            }

            return RedirectToAction("login", new { returnUrl = returnUrl });
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

        public new async Task<ActionResult> Profile(string state)
        {
            return string.Equals(state, "advance", StringComparison.OrdinalIgnoreCase) ? await AdvanceProfile() : await BasicProfile();
        }

        private async Task<ActionResult> BasicProfile()
        {
            ViewBag.ActiveTab = "basic";
            var user = await this.UserService.Get(this.CurrentUser.Id);
            if (user.DateOfBirth != null && user.DateOfBirth.HasValue)
                user.DateOfBirthStr = Helper.ToDateString(user.DateOfBirth.Value);
            return View(ViewBag.ActiveTab, user);
        }

        private async Task<ActionResult> AdvanceProfile()
        {
            ViewBag.ActiveTab = "advance";
            var user = await this.UserService.Get(this.CurrentUser.Id);
            var profile = await this.UserService.GetProfile(user);
            user.Profile = profile;
            return View(ViewBag.ActiveTab, profile);
        }

        public async Task<ActionResult> UpdateBasicProfile(User user)
        {
            var dbUser = await this.UserService.Get(this.CurrentUser.Id);
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;

            var birthDate = DateTime.Now;
            if (!Helper.TryParseDate(user.DateOfBirthStr, out birthDate))
            {
                ViewBag.ErrorMessage = "Birth Date is Incorrect.";
                return View(user);
            }
            dbUser.DateOfBirth = birthDate;
            await this.UserService.Save(dbUser);

            this.PopulateClaims(dbUser);

            return RedirectToAction("profile/basic", "account");
        }

        public async Task<ActionResult> UpdateAdvanceProfile(Profile profile)
        {
            var dbUser = await this.UserService.Get(this.CurrentUser.Id);
            var dbProfile = await this.UserService.GetProfile(dbUser);
            if (dbProfile != null) profile.Id = dbProfile.Id;
            dbUser.Profile = profile;
            await this.UserService.SaveProfile(dbUser);

            return RedirectToAction("profile/advance", "account");
        }
    }
}
