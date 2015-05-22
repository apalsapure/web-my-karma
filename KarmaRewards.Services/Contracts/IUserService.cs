using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Services
{
    public interface IUserService
    {
        Task<User> Get(string idOrUserName);

        Task<User> Save(User user);

        Task<Profile> GetProfile(User user);

        Task<Profile> SaveProfile(User user);
    }
}
