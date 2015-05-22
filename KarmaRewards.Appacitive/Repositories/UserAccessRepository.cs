using SDK = Appacitive.Sdk;
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
        #region IUserAccessRepository
        public async Task<Model.UserPermissions> GetLoggedInUserPermissionsAsync()
        {
            var user = SDK.AppContext.UserContext.LoggedInUser;
            var permissions = await this.GetPermissionsAsync(SDK.AppContext.UserContext.LoggedInUser.Id);
            if (permissions.Count == 0) throw new Exception("No permissions found for logged in user");
            return PermissionsParser.Parse(permissions[0], user);
        }

        public async Task<Model.UserPermissions> GetUserPermissionsAsync(string userId)
        {
            var permissions = await this.GetPermissionsAsync(userId);
            if (permissions.Count == 0) return null;
            return PermissionsParser.Parse(permissions[0]);
        }

        public async Task<UserPermissions> SaveUserPermissionsAsync(string userId, UserPermissions userPermissions)
        {
            var obj = userPermissions.ToAPOjbect();

            // create connection
            if (string.IsNullOrEmpty(obj.Id))
            {
                await SDK.APConnection
                        .New("user_permissions")
                        .FromExistingObject("user", userId)
                        .ToNewObject("permissions", obj)
                        .SaveAsync();
            }
            else
            {
                // update permissions
                await obj.SaveAsync();
            }

            obj.CopyEntity(userPermissions);

            return userPermissions;
        }
        #endregion

        private async Task<SDK.PagedList<SDK.APObject>> GetPermissionsAsync(string userId)
        {
            var user = new SDK.APUser(userId);
            return await user.GetConnectedObjectsAsync("user_permissions");
        }
    }
}
