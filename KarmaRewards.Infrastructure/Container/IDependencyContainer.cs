using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarmaRewards.Infrastructure.Container
{
    public interface IDependencyContainer
    {
        bool IsRegistered(Type interfaceType, string name = null);

        IDependencyContainer Register(Type interfaceType, Type concreteType, string name = null);

        object Resolve(Type interfaceType, string name = null);

        IDependencyContainer RegisterInstance(Type type, object instance, string name = null);

        IEnumerable<object> ResolveAll(Type type);

        void RegisterAsSingleton(Type type, Type instanceType, string name = null);
    }
}
