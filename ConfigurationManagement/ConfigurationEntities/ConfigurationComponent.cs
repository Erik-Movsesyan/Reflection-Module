﻿using System.Collections.Generic;
using ConfigurationManagement.Attributes;
using ConfigurationManagement.Providers;
using System.Linq;
using System.Reflection;
using System;
using ConfigurationManagement.Extensions;

namespace ConfigurationManagement.ConfigurationEntities
{
    public abstract class ConfigurationComponentBase
    {
        private const BindingFlags Binding_Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private readonly Type[] _applicablePropertyTypes = [typeof(int?),  typeof(string), typeof(float?), typeof(TimeSpan)];

        private readonly IConfigurationProvider[] _configurationProviders =
        [
            new FileConfigurationProvider(),
            new ConfigurationManagerConfigurationProvider()
        ];
        
        public void SaveSettings()
        {
            var properties = GetType().GetProperties(Binding_Flags);
            foreach (var property in properties) 
            {
                var customAttribute = property.GetCustomAttribute(typeof(ConfigurationItemAttribute<>));
                if (customAttribute is null || !property.IsOfCompliantType(_applicablePropertyTypes))
                    continue;

                var propertyConfigProvider = GetConfigProvider(customAttribute);
                var settingKey = customAttribute.GetType().GetProperty("SettingName")?.GetValue(customAttribute)?.ToString();

                if (settingKey is not null)
                {
                    var valueToSet = property.GetValue(this)?.ToString();
                    propertyConfigProvider!.SetSetting(settingKey, valueToSet);
                }
            }
        }

        public IEnumerable<KeyValuePair<string, object>> LoadSettings()
        {
            var properties = GetType().GetProperties(Binding_Flags);
            var loadResults = new List<KeyValuePair<string, object>> ();

            foreach (var property in properties)
            {
                var customAttribute = property.GetCustomAttribute(typeof(ConfigurationItemAttribute<>));
                if (customAttribute is null || !property.IsOfCompliantType(_applicablePropertyTypes))
                    continue;

                var propertyConfigProvider = GetConfigProvider(customAttribute);
                var settingKey = customAttribute.GetType().GetProperty("SettingName")?.GetValue(customAttribute)?.ToString();

                if (settingKey is not null)
                {
                    var settingValue = propertyConfigProvider.GetSetting(settingKey);
                    loadResults.Add(new KeyValuePair<string, object>(settingKey, settingValue));
                }
            }

            return loadResults;
        }

        private IConfigurationProvider GetConfigProvider(Attribute attribute)
        {
            var propertyConfigProviderType = attribute.GetType().GetGenericArguments().First();
            var propertyConfigProvider = _configurationProviders.FirstOrDefault(configProvider => configProvider.GetType() == propertyConfigProviderType);

            return propertyConfigProvider;
        }
    }
}
