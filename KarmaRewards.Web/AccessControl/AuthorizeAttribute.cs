using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KarmaRewards.Web.AccessControl
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes"), AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            // When user is authenticated and try to access a resource on which they don't have access
            // show user 403 view
            // else allow MVC to handle the error
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                var result = new ViewResult();
                result.ViewName = "Http403";
                filterContext.Result = result;
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}