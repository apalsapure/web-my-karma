using Appacitive.Sdk;
using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Appacitive
{
    public class UserAccessRepository : IUserAccessRepository
    {
        public async Task<Model.UserPermissions> GetUsersPermissionsAsync()
        {
            var user = AppContext.UserContext.LoggedInUser;
            var permissions = await user.GetConnectedObjectsAsync("user_permissions");
            if (permissions.Count == 0) throw new Exception("No permissions found for logged in user");
            return PermissionsParser.Parse(permissions[0], user);
        }
    }
}
