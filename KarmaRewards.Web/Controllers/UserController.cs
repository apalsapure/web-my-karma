using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace KarmaRewards.Web.Controllers
{
    public class UserController : ErrorController
    {
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
    }
}
