using KarmaRewards.Infrastructure.Configuration;
using KarmaRewards.Infrastructure.Container;
using KarmaRewards.Infrastructure.Hosting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.Unity
{
    public class ContainerFactory : IContainerFactory
    {
        public IDependencyContainer CreateContainer(ContainerSettings settings)
        {
            var container = new UnityContainer();
            container.AddNewExtension<CustomContainerExtension>();
            // Configure container with module level configuration.
            var depContainer = new DependencyContainer(container);
            depContainer.Configure(settings);
            // Create a child container and configure with unity config file overrides.
            var child = container.CreateChildContainer();
            var unitySettings = ExternalConfiguration.Load<UnityConfigurationSection>("unity", Paths.MapFile("unity.config"));
            if (unitySettings != null)
                child.LoadConfiguration(unitySettings);
            // Create child which will be used in the actual dependency container.
            return new DependencyContainer(child.CreateChildContainer());
        }
    }
}
