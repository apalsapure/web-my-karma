using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDK = Appacitive.Sdk;

namespace KarmaRewards.Appacitive
{
    public class UserRepository : IUserRepository
    {
        public Task<User> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<User> SaveAsync(User user)
        {
            // Create user
            var userObj = user.ToAPUser();

            await userObj.SaveAsync();

            userObj.CopyEntity(user);

            return user;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var aUser = await SDK.APUsers.GetByIdAsync(id);
            return aUser.ToUser();
        }

        public async Task<Profile> GetUserProfileAsync(string userId)
        {
            var user = new SDK.APUser(userId);
            var profiles = await user.GetConnectedObjectsAsync("user_profile");
            if (profiles.Count == 0) return null;
            var profile = new Profile();
            profiles[0].CopyProfile(profile);
            return profile;
        }

        public async Task<Profile> SaveUserProfileAsync(string userId, Profile profile)
        {
            var obj = profile.ToAPObject();

            // create connection
            if (string.IsNullOrEmpty(profile.Id))
            {
                await SDK.APConnection
                        .New("user_profile")
                        .FromExistingObject("user", userId)
                        .ToNewObject("profile", obj)
                        .SaveAsync();
            }
            else
            {
                // update permissions
                await obj.SaveAsync();
            }

            // translate back
            obj.CopyProfile(profile);

            return profile;
        }
    }
}
