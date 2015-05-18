using KarmaRewards.Appacitive;
using KarmaRewards.Services;
using KarmaRewards.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KarmaRewards.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DIContainer.Register();

            System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // initialize appacitive
            string enableDebugging = ConfigurationManager.AppSettings["appacitive-enable-debugging"];
            if (string.IsNullOrWhiteSpace(enableDebugging)) enableDebugging = "false";
            Repository.Init(string.Equals(enableDebugging, "true", StringComparison.InvariantCultureIgnoreCase));

            AccessRules.Setup();
        }

        // reference http://stackoverflow.com/questions/21999409/web-api-2-session
        protected void Application_PostAuthorizeRequest()
        {
            // This enables session state for Web APIS
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }

        // reference http://forums.asp.net/t/1773026.aspx?Enabling+Session
        protected void Application_AcquireRequestState()
        {
            // if request is authenticated, check if api session is valid or not
            // skip it for logout, bundles and resources
            if (Request.IsAuthenticated
                && string.Equals(Request.Url.AbsolutePath, "/account/logout", StringComparison.InvariantCultureIgnoreCase) == false
                && Request.Url.AbsolutePath.StartsWith("/bundles/") == false
                && Request.Url.AbsolutePath.StartsWith("/resources") == false)
            {
                IIdentityService identityService = KarmaRewards.Infrastructure.ObjectFactory.Resolve<IIdentityService>();
                var user = identityService.GetUserSessionAsync().Result;
                if (user == null)
                {
                    Response.Redirect("/account/logout");
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Do whatever you want to do with the error

            //Show the custom error page...
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";

            if ((Context.Server.GetLastError() is HttpException) && ((Context.Server.GetLastError() as HttpException).GetHttpCode() != 404))
            {
                routeData.Values["action"] = "Main";
            }
            else
            {
                // Handle 404 error and response code
                Response.StatusCode = 404;
                routeData.Values["action"] = "Http404";
            }
            Response.TrySkipIisCustomErrors = true; // If you are using IIS7, have this line
            IController errorsController = new ErrorController();
            HttpContextWrapper wrapper = new HttpContextWrapper(Context);
            var rc = new System.Web.Routing.RequestContext(wrapper, routeData);
            errorsController.Execute(rc);
        }
    }
}