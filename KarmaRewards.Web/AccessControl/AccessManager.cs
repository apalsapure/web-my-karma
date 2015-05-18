using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KarmaRewards.Web
{
    public class AccessManager
    {
        private const string SESSION_VARIABLE = "Features";

        internal static List<string> GetFeatureList()
        {
            var features = System.Web.HttpContext.Current.Session[SESSION_VARIABLE] as List<string>;
            return features == null ? new List<string>() : features;
        }

        internal async static Task SetFeatureList(User user)
        {
            var features = (await UserAccess.GetFeaturesForLoggedInUserAsync()).Select(x => x.Name).ToList();

            System.Web.HttpContext.Current.Session.Add(SESSION_VARIABLE, features);
        }

        internal static bool HasAccessOnFeature(string feature)
        {
            var features = GetFeatureList();
            if (features != null) return features.Contains(feature.ToLower());
            return false;
        }
    }
}