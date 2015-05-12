﻿using KarmaRewards.Infrastructure.Container;
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
                ;
        }
    }
}