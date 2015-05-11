using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace KarmaRewards.Web.Controllers
{
    public class BaseController : Controller
    {

        private List<Claim> _claims = new List<Claim>();
        private NameValueCollection _collection = new NameValueCollection();
        private Identity _user = null;
        private string _sessionId = null;

        public List<Claim> Claims { get { return _claims; } }
        public Identity CurrentIdentity { get { return _user; } }
        public string SessionId { get { return _sessionId; } }
        public bool IsInRole(string role)
        {
            return false;
            //if (this.CurrentIdentity == null || this.CurrentIdentity.Roles == null) return false;
            //return this.CurrentIdentity.Roles.Any(r => string.Equals(r, role, StringComparison.InvariantCultureIgnoreCase));
        }



        public BaseController()
            : base()
        {

            // user is not authenticated
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated) return;

            // user is authenticated, try to load the claims
            // and populate the context
            var identity = System.Web.HttpContext.Current.User.Identity;
            if (identity is System.Security.Claims.ClaimsIdentity == false) return;

            // get all claims
            this._claims = ((System.Security.Claims.ClaimsIdentity)(System.Web.HttpContext.Current.User.Identity)).Claims.ToList();

            // populate collection
            this._claims.ForEach(c => _collection[c.Type] = c.Value);

            // Important:
            // Following fields are populated when user is logged in
            // not doing any error handling
            //var roles = _collection["Role"].Split('|').ToList();

            this._user = Helper.BuildIdentity(_collection["UserName"], _collection["Email"], _collection["Type"], _collection["FirstName"], _collection["LastName"]);
            this._sessionId = _collection["SessionId"];
            // setting user in the view bag
            this.ViewBag.User = this._user;

            var routeData = System.Web.HttpContext.Current.Request.RequestContext.RouteData;
        }

    }
}
