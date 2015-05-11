using KarmaRewards.Appacitive;
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

            WebApiConfig.Register(GlobalConfiguration.Configuration);

            System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);


            // initialize appacitive
            string enableDebugging = ConfigurationManager.AppSettings["appacitive-enable-debugging"];
            if (string.IsNullOrWhiteSpace(enableDebugging)) enableDebugging = "false";
            Repository.Init(string.Equals(enableDebugging, "true", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}