using ConfigurationManagement.Attributes;
using ConfigurationManagement.ConfigurationEntities;
using ConfigurationManagement.Providers;
using System;

namespace ConfigurationManagement.Z_Examples
{
    public class MyConfig_ConfigurationManagerConfigurationProvider : ConfigurationComponentBase
    {
        [ConfigurationItem<ConfigurationManagerConfigurationProvider>("Setting1")]
        public TimeSpan Setting1 { get; set; }

        [ConfigurationItem<ConfigurationManagerConfigurationProvider>("Setting2")]
        public int? Setting2 { get; set; }

        [ConfigurationItem<ConfigurationManagerConfigurationProvider>("setting3")]
        public int? Setting3 { get; set; }

        [ConfigurationItem<ConfigurationManagerConfigurationProvider>("Setting4")]
        public int? Setting4 { get; set; }

        [ConfigurationItem<ConfigurationManagerConfigurationProvider>(null)]
        public int? Setting5 { get; set; }
    }
}
