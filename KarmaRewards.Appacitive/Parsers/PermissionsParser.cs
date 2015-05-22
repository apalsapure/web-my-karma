using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDK = Appacitive.Sdk;

namespace KarmaRewards.Appacitive
{
    internal static class PermissionsParser
    {
        public static UserPermissions Parse(SDK.APObject permissions, SDK.APUser user)
        {
            var userPermissions = Parse(permissions);

            // populate to the user
            user.SetList("roles", userPermissions.Roles);
            user.SetList("allowed_claims", userPermissions.Permissions.Allowed);
            user.SetList("denied_claims", userPermissions.Permissions.Denied);

            return userPermissions;
        }

        public static UserPermissions Parse(SDK.APObject permissions)
        {
            var userPermissions = new UserPermissions();

            // parse
            userPermissions.Roles = permissions.GetList<string>("roles").ToList();
            userPermissions.Permissions.Allowed = permissions.GetList<string>("allowed_claims").ToList();
            userPermissions.Permissions.Denied = permissions.GetList<string>("denied_claims").ToList();
            permissions.CopyEntity(userPermissions);

            return userPermissions;
        }

        public static SDK.APObject ToAPOjbect(this UserPermissions userPermissions)
        {
            SDK.APObject obj = null;
            if (string.IsNullOrWhiteSpace(userPermissions.Id) == true)
                obj = new SDK.APObject("permissions");
            else
                obj = new SDK.APObject("permissions", userPermissions.Id);

            obj.SetList<string>("roles", userPermissions.Roles);
            obj.SetList<string>("allowed_claims", userPermissions.Permissions.Allowed);
            obj.SetList<string>("denied_claims", userPermissions.Permissions.Denied);

            return obj;
        }
    }
}
