using KarmaRewards.Infrastructure.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    public class ContainerSetup : IContainerSetup
    {
        public void SetupContainer(IDependencyContainer container)
        {
            container
                // Resource manager instance should be single.
                .RegisterInstance<IUserAccessConfiguration, ResourceBasedUserAccessConfiguration>(ResourceBasedUserAccessConfiguration.Instance)
                ;
        }
    }
}
