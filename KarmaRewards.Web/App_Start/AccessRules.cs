using KarmaRewards.Infrastructure;
using KarmaRewards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarmaRewards.Web
{
    public static class AccessRules
    {
        public static void Setup()
        {
            // Initialize the access control.
            AccessControl.Initialize(
                ObjectFactory.Resolve<IUserAccessRepository>(),
                ObjectFactory.Resolve<IUserAccessConfiguration>(),
                ObjectFactory.Resolve<IIdentityProvider>());
        }
    }
}