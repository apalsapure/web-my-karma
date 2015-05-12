using SDK = Appacitive.Sdk;
using Appacitive.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using KarmaRewards.Model;
using KarmaRewards.Infrastructure;

namespace KarmaRewards.Appacitive
{
    public class IdentityProvider : IIdentityProvider
    {
        [InjectionConstructor]
        public IdentityProvider(IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }

        public IUserRepository UserRepository { get; set; }

        public async Task<Model.UserSession> AuthenticateAsync(Model.Credentials credentials)
        {
            var cred = credentials.ToAPCredential();
            var session = await AppContext.LoginAsync(cred);
            var response = new Model.UserSession()
            {
                Token = session.UserToken
            };
            // get user profile;
            response.User = await this.UserRepository.GetByIdAsync(session.LoggedInUser.Id);
            return response;
        }

        public async Task<bool> ValidateSessionAsync(string sessionToken)
        {
            return await SDK.UserSession.IsValidAsync(sessionToken);
        }

        public async Task InvalidateSessionAsync(string sessionToken)
        {
            await SDK.UserSession.InvalidateAsync(sessionToken);
        }

        public Task<Model.UserSession> GetUserSessionAsync()
        {
            var token = AppContext.UserContext.SessionToken;
            var user = AppContext.UserContext.LoggedInUser;
            return user == null ?
                Task.FromResult<Model.UserSession>(null) :
                Task.FromResult<Model.UserSession>(new Model.UserSession { Token = token, User = user.ToUser() });

        }

        public async Task LogoutAsync()
        {
            await AppContext.LogoutAsync();
        }

        public Task<User> GetUserById(string id)
        {
            return this.UserRepository.GetByIdAsync(id);
        }
    }
}
