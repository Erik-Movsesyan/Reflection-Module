using ConfigurationManagement.Attributes;
using ConfigurationManagement.ConfigurationEntities;
using ConfigurationManagement.Providers;
using System;

namespace ConfigurationManagement.Z_Examples
{
    internal class MyConfig_MixedConfigProviders : ConfigurationComponentBase
    {
        [ConfigurationItem<FileConfigurationProvider>("Setting1")]
        public TimeSpan Setting1 { get; set; }

        [ConfigurationItem<ConfigurationManagerConfigurationProvider>("Setting2")]
        public int? Setting2 { get; set; }

        [ConfigurationItem<FileConfigurationProvider>("Setting3")]
        public int? Setting3 { get; set; }

        [ConfigurationItem<ConfigurationManagerConfigurationProvider>("Setting4")]
        public int? Setting4 { get; set; }

        [ConfigurationItem<FileConfigurationProvider>(null)]
        public int? Setting5 { get; set; }
    }
}
