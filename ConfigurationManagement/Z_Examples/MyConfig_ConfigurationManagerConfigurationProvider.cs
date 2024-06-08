using ConfigurationManagement.Attributes;
using ConfigurationManagement.ConfigurationEntities;
using System;

namespace ConfigurationManagement.Z_Examples
{
    public class MyConfig_ConfigurationManagerConfigurationProvider : ConfigurationComponentBase
    {
        [ConfigurationItem("Setting1", "ConfigurationManagerConfigurationProvider")]
        public TimeSpan Setting1 { get; set; }

        [ConfigurationItem("Setting2", "ConfigurationManagerConfigurationProvider")]
        public int? Setting2 { get; set; }

        [ConfigurationItem("Setting3", "ConfigurationManagerConfigurationProvider")]
        public int? Setting3 { get; set; }

        [ConfigurationItem("Setting4", "ConfigurationManagerConfigurationProvider")]
        public int? Setting4 { get; set; }

        [ConfigurationItem("Setting5", null)]
        public int? Setting5 { get; set; }
    }
}
