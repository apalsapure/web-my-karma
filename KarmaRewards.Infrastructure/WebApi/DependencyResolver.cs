using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using IDependencyResolverForMvc = System.Web.Mvc.IDependencyResolver;

namespace KarmaRewards.Infrastructure.WebApi
{
    public sealed class DependencyResolver : IDependencyResolver, IDependencyResolverForMvc
    {
        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return ObjectFactory.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return ObjectFactory.ResolveAll(serviceType).Cast<object>();
            }
            catch
            {
                return Enumerable.Empty<object>();
            }
        }

        public void Dispose()
        {
        }
    }
}
