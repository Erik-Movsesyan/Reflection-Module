using System;
using ConfigurationManagement.Z_Examples;

namespace ConfigurationManagement
{
    internal class Program
    {
        static void Main()
        {
            #region loading examples

            var loadingExampleWithAllPropertiesUsingFileConfigProvider = new MyConfig_FileConfigurationProvider();

            var collection = loadingExampleWithAllPropertiesUsingFileConfigProvider.LoadSettings();
            foreach (var item in collection)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }

            Console.WriteLine("\n");

            var loadingExampleWithAllPropertiesUsingConfigurationManagerConfigProvider = new MyConfig_ConfigurationManagerConfigurationProvider();

            var collection2 = loadingExampleWithAllPropertiesUsingConfigurationManagerConfigProvider.LoadSettings();
            foreach (var item in collection2)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }

            Console.WriteLine("\n");

            var loadingExampleWithPropertiesUsingMixedConfigProviders = new MyConfig_MixedConfigProviders();

            var collection3 = loadingExampleWithPropertiesUsingMixedConfigProviders.LoadSettings();
            foreach (var item in collection3)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }

            #endregion

            #region saving examples

            var savingExampleWithAllPropertiesUsingFileConfigProvider = new MyConfig_FileConfigurationProvider()
            {
                Setting1 = new TimeSpan(9, 21, 49),
                Setting2 = 9,
                Setting3 = 5,
                Setting4 = 6,
                Setting5 = 10, //will not load since the ""SettingName" parameter is set to null
            };
            savingExampleWithAllPropertiesUsingFileConfigProvider.SaveSettings();

            var savingExampleWithAllPropertiesUsingConfigurationManagerConfigProvider = new MyConfig_ConfigurationManagerConfigurationProvider()
            {
                Setting1 = new TimeSpan(9, 21, 49),
                Setting2 = 9,
                Setting3 = 5,
                Setting4 = 6,
                Setting5 = 10, //will not load since the ""SettingName" parameter is set to null
            };
            savingExampleWithAllPropertiesUsingConfigurationManagerConfigProvider.SaveSettings();

            // after saving the below config in the bin the FileConfig.json will contain Setting1 and Setting3 since in the 
            // MyConfig_MixedConfigProviders the respective properties are set up to use the FileConfigurationProvider
            // and the Setting2 and Setting4 will be in the bin in the ConfigurationManagement.dll.config 
            // You can comment all the below lines to see how they work separately
            var savingExampleWithPropertiesUsingMixedConfigProviders = new MyConfig_MixedConfigProviders()
            {
                Setting1 = new TimeSpan(9, 21, 49),
                Setting2 = 9,
                Setting3 = 5,
                Setting4 = 6,
                Setting5 = 10, //will not load since the ""SettingName" parameter is set to null
            };
            savingExampleWithPropertiesUsingMixedConfigProviders.SaveSettings();

            #endregion
        }
    }
}
