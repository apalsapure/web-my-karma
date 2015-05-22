using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);

        Task<User> GetByNameAsync(string name);

        Task<User> SaveAsync(User user);

        Task<Profile> GetUserProfileAsync(string userId);

        Task<Profile> SaveUserProfileAsync(string userId, Profile profile);
    }
}
