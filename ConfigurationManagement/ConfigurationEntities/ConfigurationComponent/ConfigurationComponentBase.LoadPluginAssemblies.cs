using ConfigurationManagement.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ConfigurationManagement.ConfigurationEntities
{
    public abstract partial class ConfigurationComponentBase
    {
        private static List<Assembly> _loadedAssemblies = [];
        private readonly List<IConfigurationProvider> _configurationProviders = CreateConfigurationProviders(_loadedAssemblies).ToList();

        static ConfigurationComponentBase()
        {
            LoadPlugins();
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
