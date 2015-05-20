using KarmaRewards.Infrastructure;
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


        [AuthorizeAccess("user-add", "get")]
        public ActionResult Add()
        {
            ViewBag.Primary = "Access Control";
            ViewBag.Secondary = "Add User";
            return View();
        }

        [AuthorizeAccess("user-manage", "get")]
        public ActionResult Manage()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [AuthorizeAccess("user-add", "post")]
        public async Task<ActionResult> Add(Model.User user)
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
                        var birthDate = DateTime.Now;
                        if (!Helper.TryParseDate(user.DateOfBirthStr, out birthDate))
                        {
                            ViewBag.ErrorMessage = "Birth Date is Incorrect.";
                            return View();
                        }
                        user.DateOfBirth = birthDate;
                    }

                    // set user name in lower case
                    user.Username = user.Username.ToLower();

                    // set the password
                    user.Password = Helper.EncryptPassword(user.Username);

                    // set the role
                    user.Permissions.Roles.Add(Request.Form["Roles"]);

                    var updatedUser = await this.UserService.Create(user);

                    return RedirectToAction("add", "user");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }

            return View();
        }
    }
}
