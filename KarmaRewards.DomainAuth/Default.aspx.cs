using KarmaRewards.Infrastructure;
using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KarmaRewards.DomainAuth
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            timer.Tick += new EventHandler<EventArgs>(timer_Tick);
            if (IsPostBack == false)
            {
                timer.Interval = 3000;
                timer.Enabled = true;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            string returnUrl = Request.QueryString["Return"];
            timer.Enabled = false;
            if (Request.IsAuthenticated)
            {
                RemotePost post = new RemotePost();
                post.Add("data", GetUserInfo());
                post.Url = HttpUtility.UrlDecode(returnUrl);
                post.Method = "post";
                post.FormName = "domainauth";
                post.Post();
            }
        }

        protected string GetUserInfo()
        {
            string domainUserName = HttpContext.Current.User.Identity.Name;
            string displayName = string.Empty;
            string emailAddress = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;

            #region Get User Details from Active Directory
            string userName = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf('\\') + 1);
            #endregion

            Identity identity = new Identity(userName.ToLower(), emailAddress, AuthenticationProvider.Windows, firstName, lastName);
            return EncryptionProvider.Encrypt(identity.ToString());
        }

        private string GetProperty(SearchResult sResultSet, string key)
        {
            if (sResultSet.Properties.Contains(key) && sResultSet.Properties[key] != null && sResultSet.Properties[key].Count > 0) return sResultSet.Properties[key][0].ToString();
            else return string.Empty;
        }
    }

    public class RemotePost
    {
        private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();
        public string Url = "";
        public string Method = "post";
        public string FormName = "form1";
        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }
        public void Post()
        {
            System.Web.HttpContext.Current.Response.Clear();

            System.Web.HttpContext.Current.Response.Write("<html><head>");

            System.Web.HttpContext.Current.Response.Write(string.Format("</head><body style=\"background-color: #416DA5;\" onload=\"document.{0}.submit(); \">", FormName));
            System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
            }
            System.Web.HttpContext.Current.Response.Write("</form>");
            System.Web.HttpContext.Current.Response.Write("</body></html>");

            System.Web.HttpContext.Current.Response.End();
        }
    }
}