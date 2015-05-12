using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.Configuration
{
    public static class ExternalConfiguration
    {
        public static T Load<T>(string sectionName, string filePath = null)
            where T : ConfigurationSection
        {
            T settings = null;
            if (filePath == null)
                settings = ConfigurationManager.GetSection(sectionName) as T;
            else
            {
                var configurationSource = ConfigurationManager.OpenMappedExeConfiguration(
                    new ExeConfigurationFileMap()
                    {
                        ExeConfigFilename = filePath
                    }, ConfigurationUserLevel.None);
                settings = configurationSource.GetSection(sectionName) as T;
            }
            return settings;
        }
    }
}
