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
        private const string ROOT_TAB = "Karma Points";

        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_REWARD, "get")]
        public ActionResult Reward()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "Reward Points";
            return View();
        }

        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_LEADER_BOARD, "get")]
        [System.Web.Mvc.ActionName("leader-board")]
        public ActionResult LeaderBoard()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "Leader Board";
            return View("LeaderBoard");
        }

        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_MODERATE, "get")]
        [System.Web.Mvc.ActionName("rewarded-points")]
        public ActionResult RewardedPoints()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "Rewarded Points";
            return View("RewardedPoints");
        }

        //[AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_MODERATE, "get")]
        [System.Web.Mvc.ActionName("my-account")]
        public ActionResult MyAccount()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "My Account";
            return View("MyAccount");
        }

    }
}
