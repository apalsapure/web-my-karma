using KarmaRewards.Infrastructure.Container;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure
{
    /// <summary>
    /// Static factory for building objects via the inbuilt DI infrastructure.
    /// </summary>
    public static class ObjectFactory
    {
        public static object Resolve(Type type, string name = null)
        {
            return GetContainer().Resolve(type, name);
        }

        public static T Resolve<T>(string name = null)
            where T : class
        {
            return GetContainer().Resolve<T>(name);
        }

        public static IEnumerable ResolveAll(Type type)
        {
            return GetContainer().ResolveAll(type);
        }

        public static IEnumerable<T> ResolveAll<T>()
        {
            return GetContainer().ResolveAll<T>();
        }

        public static bool IsRegistered(Type type, string name = null)
        {
            return GetContainer().IsRegistered(type, name);
        }

        public static bool IsRegistered<T>(string name = null)
        {
            if (name == null)
                return GetContainer().IsRegistered<T>();
            else
                return GetContainer().IsRegistered<T>(name);
        }


        public static void SetConfiguration(string configurationFile)
        {
            lock (_locker)
            {
                _configurationFile = configurationFile;
                _rootContainer = null;
            }
        }

        private static string _configurationFile = null;

        private static IDependencyContainer _rootContainer = null;
        private static readonly object _locker = new object();
        private static IDependencyContainer GetContainer()
        {
            if (_rootContainer == null)
            {
                lock (_locker)
                {
                    if (_rootContainer == null)
                    {
                        var settings = ContainerSettings.Load(_configurationFile);
                        IContainerFactory factory = settings.CreateFactory();
                        _rootContainer = factory.CreateContainer(settings);
                    }
                }
            }
            return _rootContainer;
        }
    }
}
