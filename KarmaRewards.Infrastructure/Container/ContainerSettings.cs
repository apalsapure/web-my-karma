using KarmaRewards.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.Container
{
    [Serializable]
    public class ContainerSettings : ConfigurationSection
    {
        [ConfigurationProperty("factory", DefaultValue = null, IsRequired = true)]
        public string FactoryType
        {
            get
            {
                return (String)this["factory"];
            }
            set
            {
                this["factory"] = value;
            }
        }

        [ConfigurationProperty("modules")]
        [ConfigurationCollection(typeof(ModuleSettingsCollection), AddItemName = "add")]
        public ModuleSettingsCollection Modules
        {
            get
            {
                return this["modules"] as ModuleSettingsCollection;
            }
            set
            {
                this["modules"] = value;
            }
        }

        public IContainerFactory CreateFactory()
        {
            return Activator.CreateInstance(Type.GetType(this.FactoryType)) as IContainerFactory;
        }

        public static ContainerSettings Load(string configFile = null)
        {
            return ExternalConfiguration.Load<ContainerSettings>("container.settings", configFile);
        }

    }

    [Serializable]
    public class ModuleSettingsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleSettings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleSettings)element).Name;
        }
    }

    [Serializable]
    public class ModuleSettings : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = null, IsRequired = true)]
        public string Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("type", DefaultValue = null, IsRequired = true)]
        public string Type
        {
            get
            {
                return (String)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }

        public void Configure(IDependencyContainer container)
        {
            var setup = Activator.CreateInstance(System.Type.GetType(this.Type)) as IContainerSetup;
            if (setup != null)
                setup.SetupContainer(container);
        }
    }
}
