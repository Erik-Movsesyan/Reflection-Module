using System.Collections.Generic;
using ConfigurationManagement.Attributes;
using ConfigurationManagement.Providers;
using System.Linq;
using System.Reflection;
using System;
using ConfigurationManagement.Extensions;
using System.IO;
using System.Runtime.Loader;

namespace ConfigurationManagement.ConfigurationEntities
{
    public abstract class ConfigurationComponentBase
    {
        private const BindingFlags Binding_Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        private const string PluginsPath = "ConfigurationManagerPlugins";

        private readonly Type[] _applicablePropertyTypes = [typeof(int?),  typeof(string), typeof(float?), typeof(TimeSpan)];
        private readonly List<IConfigurationProvider> _configurationProviders = CreateConfigurationProviders(_loadedAssemblies).ToList();

        private static List<Assembly> _loadedAssemblies = [];

        private static readonly EnumerationOptions EnumerationOptions = new() { RecurseSubdirectories = true };

        static ConfigurationComponentBase()
        {
            LoadPlugins();
        }

        public void SaveSettings()
        {
            var properties = GetType().GetProperties(Binding_Flags);
            foreach (var property in properties) 
            {
                var customAttribute = property.GetCustomAttribute<ConfigurationItemAttribute>();
                if (customAttribute is null || !property.IsOfCompliantType(_applicablePropertyTypes))
                    continue;

                var propertyConfigProvider = GetConfigProvider(customAttribute);
                var settingKey = customAttribute.GetType().GetProperty("SettingName")?.GetValue(customAttribute)?.ToString();

                if (settingKey is not null && propertyConfigProvider is not null)
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
                var customAttribute = property.GetCustomAttribute<ConfigurationItemAttribute>();
                if (customAttribute is null || !property.IsOfCompliantType(_applicablePropertyTypes))
                    continue;

                var propertyConfigProvider = GetConfigProvider(customAttribute);
                var settingKey = customAttribute.GetType().GetProperty("SettingName")?.GetValue(customAttribute)?.ToString();

                if (settingKey is not null && propertyConfigProvider is not null)
                {
                    var settingValue = propertyConfigProvider.GetSetting(settingKey);
                    loadResults.Add(new KeyValuePair<string, object>(settingKey, settingValue));
                }
            }

            return loadResults;
        }

        private IConfigurationProvider GetConfigProvider(ConfigurationItemAttribute attribute)
        {
            var propertyConfigProviderType = attribute.ProviderType;
            var propertyConfigProvider = _configurationProviders.FirstOrDefault(configProvider => 
                    configProvider.GetType().Name.Equals(propertyConfigProviderType, StringComparison.OrdinalIgnoreCase));

            return propertyConfigProvider;
        }

        private static void LoadPlugins()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Directory.GetParent(baseDir).Parent.Parent.Parent.FullName;
            var pluginRelativePath = Path.Combine(projectDir, PluginsPath);

            var plugins = Directory.EnumerateFiles(pluginRelativePath, "*ConfigurationProvider.dll", EnumerationOptions);

            _loadedAssemblies = plugins.Select(LoadPlugin).ToList();
        }

        private static Assembly LoadPlugin(string pluginPath)
        {
            var loadContext = new AssemblyLoadContext(string.Empty);
            var dependencyResolver = new AssemblyDependencyResolver(pluginPath);

            loadContext.Resolving += (assemblyLoadContext, assemblyName) => 
            {
                var assemblyPath = dependencyResolver.ResolveAssemblyToPath(assemblyName);      

                return assemblyPath != null
                    ? assemblyLoadContext.LoadFromAssemblyPath(assemblyPath) 
                    : null;
            };

            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginPath)));
        }

        private static IEnumerable<IConfigurationProvider> CreateConfigurationProviders(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                int count = 0;
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IConfigurationProvider).IsAssignableFrom(type))
                    {
                        if (Activator.CreateInstance(type) is IConfigurationProvider result)
                        {
                            count++;
                            yield return result;
                        }
                    }
                }

                if (count == 0)
                {
                    var availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
                    throw new ApplicationException(
                        $"Can't find any type which implements ${nameof(IConfigurationProvider)} in {assembly} from {assembly.Location}.\n" +
                        $"Available types are: {availableTypes}");
                }
            }
        }
    }
}
