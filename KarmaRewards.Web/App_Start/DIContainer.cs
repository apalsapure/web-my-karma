using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarmaRewards.Web
{
    public static class DIContainer
    {
        public static void Register()
        {
            System.Web.Mvc.DependencyResolver.SetResolver(new KarmaRewards.Infrastructure.WebApi.DependencyResolver());
        }
    }
}