using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public interface IIdentityProvider
    {
        Task<UserSession> AuthenticateAsync(Credentials credentials);

        Task<bool> ValidateSessionAsync(string sessionToken);

        Task InvalidateSessionAsync(string sessionToken);

        Task<UserSession> GetUserSessionAsync();

        Task LogoutAsync();

        Task<User> GetUserById(string id);
    }
}
