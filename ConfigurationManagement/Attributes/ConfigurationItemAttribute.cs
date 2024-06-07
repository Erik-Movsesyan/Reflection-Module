using ConfigurationManagement.Providers;
using System;

namespace ConfigurationManagement.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigurationItemAttribute<T>(string settingName) : Attribute where T : IConfigurationProvider
    {
        public string SettingName { get; set; } = settingName;
        public Type ProviderType { get; set; } = typeof(T);
    }
}
