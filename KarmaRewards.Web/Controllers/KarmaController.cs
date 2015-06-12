using KarmaRewards.Infrastructure;
using KarmaRewards.Model;
using KarmaRewards.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KarmaRewards.Web.Controllers
{
    [Authorize]
    public class KarmaController : ErrorController
    {
        private const string ROOT_TAB = "Karma Points";

        [InjectionConstructor]
        public KarmaController(IKarmaPointService karmaPointService)
        {
            this.KarmaPointService = karmaPointService;
        }
        private IKarmaPointService KarmaPointService;

        // get reward points page
        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_REWARD, "get")]
        [ActionName("reward-points")]
        public ActionResult RewardPoints()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "Reward Points";
            return View("RewardPoints");
        }

        // get reward points edit page
        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_REWARD, "get")]
        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "Reward Points";

            var point = await this.KarmaPointService.Get(id);
            if (point == null) return base.Http404();
            if (point.From != base.CurrentUser.Id) return base.Http403();
            return View(point);
        }

        // save / update reward points
        [HttpPost]
        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_REWARD, "post")]
        public async Task<ActionResult> RewardPoints(Model.KarmaPoints karmaPoints)
        {
            // if the reward points are moderated then user can not edit points
            if (string.IsNullOrEmpty(karmaPoints.ModeratedBy) == false)
            {
                var point = await this.KarmaPointService.Get(karmaPoints.Id);
                if (point == null) return base.Http404();
                point.Reason = karmaPoints.Reason;
                await this.KarmaPointService.Save(point);
            }
            else
            {
                // set who has rewarded points
                karmaPoints.From = this.CurrentUser.Id;
                await this.KarmaPointService.Save(karmaPoints);
            }
            return RedirectToAction("rewarded-points", "karma");
        }

       
        // leader board get page
        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_LEADER_BOARD, "get")]
        [ActionName("leader-board")]
        public ActionResult LeaderBoard()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "Leader Board";
            return View("LeaderBoard");
        }

        // get rewarded points page
        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_REWARDED_POINTS, "get")]
        [ActionName("rewarded-points")]
        public ActionResult RewardedPoints()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "Rewarded Points";
            return View("RewardedPoints");
        }

        // get my account page
        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_MY_ACCOUNT, "get")]
        [ActionName("my-account")]
        public ActionResult MyAccount()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "My Account";
            return View("MyAccount");
        }


        // get moderate points page
        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_MODERATE, "get")]
        public async Task<ActionResult> Moderate(string id)
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "Reward Points";

            var point = await this.KarmaPointService.Get(id);
            if (point == null) return base.Http404();
            return View(point);
        }

        // update moderation of rewarded points
        [HttpPost]
        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_MODERATE, "post")]
        public async Task<ActionResult> Moderate(KarmaPoints karmaPoints)
        {
            karmaPoints.ModeratedBy = this.CurrentUser.Id;
            await this.KarmaPointService.Save(karmaPoints);
            return RedirectToAction("rewarded-points", "karma");
        }
    }
}