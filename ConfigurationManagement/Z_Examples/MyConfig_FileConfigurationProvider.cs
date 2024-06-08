using ConfigurationManagement.Attributes;
using ConfigurationManagement.ConfigurationEntities;
using System;

namespace ConfigurationManagement.Z_Examples
{
    public class MyConfig_FileConfigurationProvider : ConfigurationComponentBase
    {
        [ConfigurationItem("Setting1", "FileConfigurationProvider")]
        public TimeSpan Setting1 { get; set; }

        [ConfigurationItem("Setting2", "FileConfigurationProvider")]
        public int? Setting2 { get; set; }

        [ConfigurationItem("Setting3", "FileConfigurationProvider")]
        public int? Setting3 { get; set; }

        [ConfigurationItem("Setting4", "FileConfigurationProvider")]
        public int? Setting4 { get; set; }

        [ConfigurationItem(null, "FileConfigurationProvider")]
        public int? Setting5 { get; set; }
    }
}
