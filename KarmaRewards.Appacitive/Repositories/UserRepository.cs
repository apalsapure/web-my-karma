using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Appacitive
{
    public class UserRepository : IUserRepository
    {
        public Task<User> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<User> CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<User> CreateUserProfileAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
