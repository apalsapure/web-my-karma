using KarmaRewards.Infrastructure.Container;
using KarmaRewards.Infrastructure.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KarmaRewards.Web
{
    public class ContainerSetup : IContainerSetup
    {
        public void SetupContainer(IDependencyContainer container)
        {
            container
                .Register<IActionInvoker, InterceptingControllerActionInvoker>()
                ;
        }
    }
}