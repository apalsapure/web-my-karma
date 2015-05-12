using KarmaRewards.Infrastructure;
using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Services
{
    public class IdentityService : IIdentityService
    {
        [InjectionConstructor]
        public IdentityService(IIdentityProvider identityRepository)
        {
            this.AuthProvider = identityRepository;
        }

        public IIdentityProvider AuthProvider { get; set; }



        public async Task<UserSession> AuthenticateAsync(Credentials credentials)
        {
            return await this.AuthProvider.AuthenticateAsync(credentials);
        }

        public async Task<bool> ValidateSessionAsync(string sessionToken)
        {
            return await this.AuthProvider.ValidateSessionAsync(sessionToken);
        }

        public async Task InvalidateSessionAsync(string sessionToken)
        {
            await this.AuthProvider.InvalidateSessionAsync(sessionToken);
        }

        public async Task<UserSession> GetUserSessionAsync()
        {
            return await this.AuthProvider.GetUserSessionAsync();
        }

        public async Task LogoutAsync()
        {
            await this.AuthProvider.LogoutAsync();
        }

        public Task<User> GetUserById(string id)
        {
            return AuthProvider.GetUserById(id);
        }
    }
}
