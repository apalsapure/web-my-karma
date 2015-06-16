using KarmaRewards.Infrastructure;
using KarmaRewards.Model;
using KarmaRewards.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KarmaRewards.Web.Controllers
{
    [Authorize]
    public class UserController : ErrorController
    {
        [InjectionConstructor]
        public UserController(IUserService userService)
        {
            this.UserService = userService;
        }

        public IUserService UserService { get; set; }
        private const string ROOT_TAB = "Access Control";
        private const string ADD_USER = "Add User";
        private const string MANAGE_USER = "Manage User";

        #region Add User
        #region Add / Edit
        [AuthorizeAccess("user-add", "get")]
        public ActionResult Add()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = ADD_USER;
            ViewBag.ActiveTab = "basic";
            return View();
        }
        [AuthorizeAccess("user-add", "get")]
        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = MANAGE_USER;
            ViewBag.ActiveTab = "basic";

            var user = await this.UserService.Get(id);
            if (user == null) return this.Http404();
            user.UpdateDateString();
            return View("Add", user);
        }
        [HttpPost]
        [AuthorizeAccess("user-add", "post")]
        public async Task<ActionResult> Edit(Model.User user)
        {
            if (user != null)
            {
                try
                {
                    // check joining date
                    var joiningDate = DateTime.Now;
                    if (!Helper.TryParseDate(user.JoiningDateStr, out joiningDate))
                    {
                        ViewBag.ErrorMessage = "Joining Date is Incorrect.";
                        return View();
                    }
                    user.JoiningDate = joiningDate;

                    // check date of birth (optional)
                    if (!string.IsNullOrWhiteSpace(user.DateOfBirthStr))
                    {
                        // date control on client side initialize to this date
                        if (user.DateOfBirthStr == "01/01/2000")
                        {
                            user.DateOfBirth = null;
                            user.BirthInDays = 0;
                        }
                        else
                        {
                            var birthDate = DateTime.Now;
                            if (!Helper.TryParseDate(user.DateOfBirthStr, out birthDate))
                            {
                                ViewBag.ErrorMessage = "Birth Date is Incorrect.";
                                return View();
                            }
                            user.DateOfBirth = birthDate;
                            user.BirthInDays = birthDate.DayOfYear;
                        }
                    }

                    // set user name in lower case
                    user.Username = user.Username.ToLower();
                    user.Email = user.Email.ToLower();
                    user.ImageUrl = Helper.GetProfileImageForEmail(user.Email);

                    // set the password
                    if (string.IsNullOrEmpty(user.Id))
                        user.Password = Helper.EncryptPassword(user.Username);

                    var updatedUser = await this.UserService.Save(user);

                    return RedirectToAction("profile", "user", new { id = updatedUser.Id });
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            return this.Http500();
        }
        #endregion

        #region Profile
        [AuthorizeAccess("user-add", "get")]
        public new async Task<ActionResult> Profile(string id)
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = ADD_USER;
            ViewBag.ActiveTab = "profile";
            var user = await this.UserService.Get(id);
            if (user == null) return this.Http404();
            user.Profile = await this.UserService.GetProfile(user);
            user.UpdateDateString();
            return View("Profile", user);
        }
        [HttpPost]
        [AuthorizeAccess("user-add", "get")]
        public new async Task<ActionResult> Profile(string profileId, User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(profileId)) user.Profile.Id = profileId;

                await this.UserService.SaveProfile(user);

                return RedirectToAction("roles", "user", new { id = user.Id });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return this.Http500();
        }
        #endregion

        #region Roles & Permissions
        [AuthorizeAccess("user-add", "get")]
        public async Task<ActionResult> Roles(string id)
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = ADD_USER;
            ViewBag.ActiveTab = "roles";
            var user = await this.UserService.Get(id);
            if (user == null) return this.Http404();
            user.Permissions = await AccessControl.AccessControlRepository.GetUserPermissionsAsync(user.Id);
            user.UpdateDateString();
            return View("Roles", user);
        }
        [HttpPost]
        [AuthorizeAccess("user-add", "get")]
        public async Task<ActionResult> Roles(string permissionId, User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(permissionId)) user.Permissions.Id = permissionId;
                // set the role
                user.Permissions.Roles.Add(Request.Form["Roles"]);

                await AccessControl.AccessControlRepository.SaveUserPermissionsAsync(user.Id, user.Permissions);

                return RedirectToAction("access", "user", new { id = user.Id });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return this.Http500();
        }
        #endregion

        #region Access
        [AuthorizeAccess("user-add", "get")]
        public async Task<ActionResult> Access(string id)
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = ADD_USER;
            ViewBag.ActiveTab = "access";
            var user = await this.UserService.Get(id);
            if (user == null) return this.Http404();
            user.Permissions = await AccessControl.AccessControlRepository.GetUserPermissionsAsync(user.Id);
            return View("Access", user);
        }
        [HttpPost]
        [AuthorizeAccess("user-add", "get")]
        public async Task<ActionResult> Access(User user)
        {
            var isEnabled = user.IsEnabled;
            user = await this.UserService.Get(user.Id);
            user.IsEnabled = isEnabled;
            await this.UserService.Save(user);
            return RedirectToAction("manage", "user");
        }
        #endregion
        #endregion

        [AuthorizeAccess("user-manage", "get")]
        public ActionResult Manage()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = MANAGE_USER;
            return View();
        }
    }
}
