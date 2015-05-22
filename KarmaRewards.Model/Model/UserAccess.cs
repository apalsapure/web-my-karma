using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class UserAccess
    {
        public static async Task<IEnumerable<Feature>> GetFeaturesForLoggedInUserAsync()
        {
            var userSession = await AccessControl.IdentityProvider.GetUserSessionAsync();
            if (userSession == null)
                return Enumerable.Empty<Feature>();
            if (userSession.User == null)
                return Enumerable.Empty<Feature>();

            // Get override rules.
            var permissions = await AccessControl.AccessControlRepository.GetLoggedInUserPermissionsAsync();
            List<Feature> results = new List<Feature>();
            foreach (var feature in AccessControl.Configuration.Features)
            {
                var hasAccess = HasAccessAsync(userSession.User, feature, permissions);
                if (hasAccess == true)
                    results.Add(feature);
            }
            return results;
        }

        private static bool HasAccessAsync(User user, Feature feature, UserPermissions permissions)
        {
            var hasAccess = true;
            var roles = AccessControl.Configuration.Roles.ToList().Where(x => permissions.Roles.Contains(x.Name)).ToList();
            for (int i = 0; i < roles.Count; i++)
            {
                var role = roles[i];
                if (!role.Permission.HasAccess(feature)) hasAccess = false;
            }

            // permission override
            if (hasAccess)
            {
                hasAccess = !permissions.Permissions.HasDeniedAccess(feature);
            }

            return hasAccess;
        }
    }
}
