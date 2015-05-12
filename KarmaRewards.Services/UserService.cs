using KarmaRewards.Infrastructure;
using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Services
{
    public class UserService : IUserService
    {
        [InjectionConstructor]
        public UserService(IUserRepository userRepository, IIdentityProvider identityProvider)
        {
            this.Repository = userRepository;
            this.AuthProvider = identityProvider;
        }

        public IUserRepository Repository { get; set; }
        public IIdentityProvider AuthProvider { get; set; }

        public async Task<User> Get(string idOrUserName)
        {
            if (Strings.IsNumber(idOrUserName) == true)
                return await this.Repository.GetByIdAsync(idOrUserName);
            else
                return await this.Repository.GetByNameAsync(idOrUserName);
        }

        public async Task<User> Create(User user)
        {
            return await this.Repository.CreateAsync(user);
        }

        public async Task<User> CreateProfile(User user)
        {
            return await this.Repository.CreateUserProfileAsync(user);
        }
    }
}
