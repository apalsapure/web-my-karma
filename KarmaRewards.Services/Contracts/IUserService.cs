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

        Task<User> Create(User user);

        Task<User> CreateProfile(User user);
    }
}
