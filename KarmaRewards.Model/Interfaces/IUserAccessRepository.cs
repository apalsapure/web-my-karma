using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public interface IUserAccessRepository
    {
        Task<UserPermissions> GetLoggedInUserPermissionsAsync();

        Task<UserPermissions> GetUserPermissionsAsync(string userId);

        Task<UserPermissions> SaveUserPermissionsAsync(string userId, UserPermissions userPermissions);
    }
}
