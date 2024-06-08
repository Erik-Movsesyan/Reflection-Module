using System;

namespace ConfigurationManagement.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigurationItemAttribute(string settingName, string providerType) : Attribute
    {
        public string SettingName { get; set; } = settingName;

        public string ProviderType { get; set; } = !string.IsNullOrEmpty(providerType)
                                                        ? providerType.ToLower().Trim()
                                                        : string.Empty;
    }
}
