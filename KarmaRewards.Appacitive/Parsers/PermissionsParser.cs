using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Appacitive
{
    internal class PermissionsParser
    {
        internal static Model.UserPermissions Parse(global::Appacitive.Sdk.APObject permissions, global::Appacitive.Sdk.APUser user)
        {
            var userPermissions = new Model.UserPermissions();
            
            // parse
            userPermissions.Roles = permissions.GetList<string>("roles").ToList();
            userPermissions.Permissions.Allowed = permissions.GetList<string>("allowed_claims").ToList();
            userPermissions.Permissions.Denied = permissions.GetList<string>("denied_claims").ToList();

            // populate to the user
            user.SetList("roles", userPermissions.Roles);
            user.SetList("allowed_claims", userPermissions.Permissions.Allowed);
            user.SetList("denied_claims", userPermissions.Permissions.Denied);

            return userPermissions;
        }
    }
}
