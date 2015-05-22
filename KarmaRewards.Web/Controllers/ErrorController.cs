using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KarmaRewards.Web.Controllers
{
    public class ErrorController : BaseController
    {
        protected override void HandleUnknownAction(string actionName)
        {
            if (this.GetType() != typeof(ErrorController))
            {
                var errorRoute = new RouteData();
                errorRoute.Values.Add("controller", "Error");
                errorRoute.Values.Add("action", "Http404");
                errorRoute.Values.Add("url", HttpContext.Request.Url.OriginalString);

                View("Http404").ExecuteResult(this.ControllerContext);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            //TODO: Need to handle session expired
            if (filterContext.Exception != null
                && !string.IsNullOrWhiteSpace(filterContext.Exception.Message)
                && filterContext.Exception.Message.StartsWith("Invalid user token provided"))
            {
                Response.Redirect("/account/logout");
            }

            base.OnException(filterContext);
        }

        public ActionResult Main()
        {
            return View("Error");
        }

        public ActionResult Http404()
        {
            return View("Http404");
        }

        public ActionResult Http403()
        {
            return View("Http403");
        }

        public ActionResult Http500()
        {
            return View("Http500");
        }

        public ActionResult AccessDenied()
        {
            return Http403();
        }
    }
}
