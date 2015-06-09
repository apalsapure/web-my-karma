using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
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
        private User _user = null;
        private string _sessionId = null;

        public List<Claim> Claims { get { return _claims; } }
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

            this._user = new Model.User()
            {
                Id = _collection["Id"],
                Username = _collection["UserName"],
                Email = _collection["Email"],
                Provider = _collection["Provider"],
                FirstName = _collection["FirstName"],
                LastName = _collection["LastName"],
                Designation = _collection["Designation"],
                ImageUrl = _collection["Image"],
                JoiningDate = DateTime.ParseExact(_collection["Joined"], "MM/dd/yyyy", new CultureInfo("en-US"))
            };
            this._sessionId = _collection["SessionId"];

            // setting user in the view bag
            this.ViewBag.User = this._user;

            var routeData = System.Web.HttpContext.Current.Request.RequestContext.RouteData;


            ViewBag.Features = AccessManager.GetFeatureList();
        }

    }
}
