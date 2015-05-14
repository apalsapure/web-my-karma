using KarmaRewards.Infrastructure.Container;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.Unity
{
    /// <summary>
    /// DI Container implementation over unity container.
    /// </summary>
    public sealed class DependencyContainer : IDependencyContainer, IDisposable
    {
        private IUnityContainer _container = new UnityContainer();

        public DependencyContainer(IUnityContainer innerContainer)
        {
            _container = innerContainer;
        }

        public bool IsRegistered(Type interfaceType)
        {
            try
            {
                return _container.IsRegistered(interfaceType);
            }
            catch (Exception ex)
            {
                throw new ObjectResolutionException(ex.Message, ex);
            }
        }

        public bool IsRegistered(Type interfaceType, string name = null)
        {
            try
            {
                return _container.IsRegistered(interfaceType, name);
            }
            catch (Exception ex)
            {
                throw new ObjectResolutionException(ex.Message, ex);
            }
        }

        public IDependencyContainer Register(Type interfaceType, Type concreteType, string name = null)
        {
            try
            {
                _container.RegisterType(interfaceType, concreteType, name);
                return this;
            }
            catch (Exception ex)
            {
                throw new ObjectResolutionException(ex.Message, ex);
            }
        }

        public object Resolve(Type interfaceType, string name = null)
        {
            try
            {
                return _container.Resolve(interfaceType, name);
            }
            catch (Exception ex)
            {
                throw new ObjectResolutionException(ex.Message, ex);
            }
        }

        public IDependencyContainer RegisterInstance(Type type, object instance, string name)
        {
            try
            {
                _container.RegisterInstance(type, name, instance);
                return this;
            }
            catch (Exception ex)
            {
                throw new ObjectResolutionException(ex.Message, ex);
            }
        }


        public IEnumerable<object> ResolveAll(Type type)
        {
            try
            {
                return _container.ResolveAll(type);
            }
            catch (Exception ex)
            {
                throw new ObjectResolutionException(ex.Message, ex);
            }
        }


        public void RegisterAsSingleton(Type type, Type instanceType, string name = null)
        {
            try
            {
                if (name != null)
                    _container.RegisterType(type, instanceType, name, new ContainerControlledLifetimeManager());
                else
                    _container.RegisterType(type, instanceType, new ContainerControlledLifetimeManager());
            }
            catch (Exception ex)
            {
                throw new ObjectResolutionException(ex.Message, ex);
            }
        }

        private bool _isDisposed = false;
        public void Dispose()
        {
            if (_isDisposed == true) return;
            try
            {
                if (_container != null)
                    _container.Dispose();
                _container = null;
            }
            finally
            {
                _isDisposed = true;
            }
        }
    }
}
