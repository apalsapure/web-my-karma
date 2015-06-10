using KarmaRewards.Infrastructure.Container;
using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Appacitive
{
    public class ContainerSetup : IContainerSetup
    {
        public void SetupContainer(IDependencyContainer container)
        {
            container
                .Register<IUserRepository, UserRepository>()
                .Register<IIdentityProvider, IdentityProvider>()
                .Register<IUserAccessRepository, UserAccessRepository>()
                .Register<IKarmaPointRepository, KarmaPointRepository>()
                .Register<ICredentialTranslator, UsernamePasswordCredentialTranslator>("usernamepassword")
                .Register<ICredentialTranslator, TokenCredentialTranslator>("token")
                .Register<ICredentialTranslator, FacebookCredentialTranslator>("facebook")
                .Register<ICredentialTranslator, TwitterCredentialTranslator>("twitter")
                .Register<ICredentialTranslator, GooglePlusCredentialTranslator>("google+")
                ;
        }
    }
}
