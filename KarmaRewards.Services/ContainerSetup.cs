using KarmaRewards.Infrastructure.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Services
{
    public class ContainerSetup : IContainerSetup
    {
        public void SetupContainer(IDependencyContainer container)
        {
            container
                .Register<IUserService, UserService>()
                .Register<IKarmaPointService, KarmaPointService>()
                .Register<IIdentityService, IdentityService>()
                ;
        }
    }
}
