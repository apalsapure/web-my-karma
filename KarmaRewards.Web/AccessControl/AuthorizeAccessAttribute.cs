using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KarmaRewards.Web
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class AuthorizeAccessAttribute : AuthorizeAttribute
    {
        private string[] _target;

        public AuthorizeAccessAttribute(string[] target, string action)
        {
            this._target = target;
        }

        public AuthorizeAccessAttribute(string target, string action) : this(new string[] { target }, action) { }

        // This will check if the user has claim on the given claim type
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var features = AccessManager.GetFeatureList();

            // if there is not rule, that means user doesn't have access
            if (features.Count(x => this._target.Contains(x)) > 0)
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}