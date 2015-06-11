using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KarmaRewards.Web.Controllers
{
    public class HomeController : ErrorController
    {
        private const string ROOT_TAB = "Dashboard";

        public ActionResult Index()
        {
            ViewBag.Primary = ROOT_TAB;
            return View();
        }      
    }
}
