using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.Container
{
    public static class ContainerExtensions
    {
        public static IDependencyContainer Register<TInterface, TConcrete>(this IDependencyContainer container)
        {
            return container.Register(typeof(TInterface), typeof(TConcrete));
        }

        public static IDependencyContainer Register<TInterface, TConcrete>(this IDependencyContainer container, string name)
        {
            return container.Register(typeof(TInterface), typeof(TConcrete), name);
        }

        public static bool IsRegistered<TInterface>(this IDependencyContainer container)
        {
            return container.IsRegistered(typeof(TInterface));
        }

        public static bool IsRegistered<TInterface>(this IDependencyContainer container, string name)
        {
            return container.IsRegistered(typeof(TInterface), name);
        }

        public static T Resolve<T>(this IDependencyContainer container)
            where T : class
        {
            return container.Resolve(typeof(T)) as T;
        }

        public static T Resolve<T>(this IDependencyContainer container, string name)
            where T : class
        {
            return container.Resolve(typeof(T), name) as T;
        }

        public static IDependencyContainer RegisterAsSingleton<TInterface, TObject>(this IDependencyContainer container)
            where TObject : TInterface
        {
            container.RegisterAsSingleton(typeof(TInterface), typeof(TObject));
            return container;
        }

        public static IDependencyContainer RegisterAsSingleton<TInterface, TObject>(this IDependencyContainer container, string name)
            where TObject : TInterface
        {
            container.RegisterAsSingleton(typeof(TInterface), typeof(TObject), name);
            return container;
        }

        public static IDependencyContainer RegisterInstance<TInterface, TObject>(this IDependencyContainer container, TObject instance)
            where TObject : TInterface
        {
            container.RegisterInstance(typeof(TInterface), instance);
            return container;
        }

        public static IDependencyContainer RegisterInstance<TInterface, TObject>(this IDependencyContainer container, TObject instance, string name)
            where TObject : TInterface
        {
            container.RegisterInstance(typeof(TInterface), instance, name);
            return container;
        }

        public static IEnumerable<T> ResolveAll<T>(this IDependencyContainer container)
        {
            return container.ResolveAll(typeof(T)).Cast<T>();
        }

        public static void Configure(this IDependencyContainer container, ContainerSettings containerSettings = null)
        {
            containerSettings = containerSettings ?? ContainerSettings.Load();
            if (containerSettings == null) return;
            containerSettings
                .Modules.Cast<ModuleSettings>().ToList()
                .ForEach(m => m.Configure(container));
        }

        public static IDependencyContainer CreateContainer(this IContainerFactory factory)
        {
            return factory.CreateContainer(ContainerSettings.Load());
        }
    }
}
