using ConfigurationManagement.Attributes;
using ConfigurationManagement.ConfigurationEntities;
using ConfigurationManagement.Providers;
using System;

namespace ConfigurationManagement.Z_Examples
{
    public class MyConfig_FileConfigurationProvider : ConfigurationComponentBase
    {
        [ConfigurationItem<FileConfigurationProvider>("Setting1")]
        public TimeSpan Setting1 { get; set; }

        [ConfigurationItem<FileConfigurationProvider>("Setting2")]
        public int? Setting2 { get; set; }

        [ConfigurationItem<FileConfigurationProvider>("Setting3")]
        public int? Setting3 { get; set; }

        [ConfigurationItem<FileConfigurationProvider>("Setting4")]
        public int? Setting4 { get; set; }

        [ConfigurationItem<FileConfigurationProvider>(null)]
        public int? Setting5 { get; set; }
    }
}
