using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace KarmaRewards.Web.Controllers
{
    public class KarmaController : ErrorController
    {
        public ActionResult Index()
        {
            ViewBag.Primary = "Karma Points";
            ViewBag.Secondary = "Reward Points";
            return View();
        }

        public ActionResult List()
        {
            ViewBag.Primary = "Karma Points";
            ViewBag.Secondary = "Leader Board";
            return View();
        }
    }
}
