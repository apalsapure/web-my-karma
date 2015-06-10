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

        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_REWARD, "get")]
        [ActionName("reward-points")]
        public ActionResult RewardPoints()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "Reward Points";
            return View("RewardPoints");
        }

        [HttpPost]
        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_REWARD, "post")]
        public async Task<ActionResult> RewardPoints(Model.KarmaPoints karmaPoints)
        {
            karmaPoints.From = this.CurrentUser.Id;
            await this.KarmaPointService.Save(karmaPoints);
            return RedirectToAction("rewarded-points", "karma");
        }

        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_LEADER_BOARD, "get")]
        [ActionName("leader-board")]
        public ActionResult LeaderBoard()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "Leader Board";
            return View("LeaderBoard");
        }

        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_MODERATE, "get")]
        [ActionName("rewarded-points")]
        public ActionResult RewardedPoints()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "Rewarded Points";
            return View("RewardedPoints");
        }

        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_MY_ACCOUNT, "get")]
        [ActionName("my-account")]
        public ActionResult MyAccount()
        {
            ViewBag.Primary = ROOT_TAB;
            ViewBag.Secondary = "My Account";
            return View("MyAccount");
        }

        [AuthorizeAccess(KarmaRewards.Web.Claims.KARMA_MODERATE, "get")]
        public async Task<ActionResult> Moderate(string id)
        {
            try
            {
                var point = await this.KarmaPointService.Get(id);
                if (point == null) return base.Http404();
                return View(point);
            }
            catch { }
            return base.Http500();
        }
    }
}
