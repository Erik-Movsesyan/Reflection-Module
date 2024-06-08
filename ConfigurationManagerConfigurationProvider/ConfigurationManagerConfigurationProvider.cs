using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ConfigurationManagement.Providers
{
    public class ConfigurationManagerConfigurationProvider : IConfigurationProvider
    {
        private const string ConfigFileNameInBinForSetting = "ConfigurationManagement.dll.config";
        private const string ConfigFileNameInBinForGetting = "MyConfigs.config";
        private readonly Dictionary<string, string> _appSettings = LoadConfigFileForReading();
        private bool _oldFileDeletedBeforeSettingFlag;

        public void SetSetting(string key, string value, bool overWriteOldFile = true)
        {
            if (overWriteOldFile)
                DeleteOldConfigFile();

            var configurationFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (configurationFile.AppSettings.Settings[key] != null)
            {
                configurationFile.AppSettings.Settings[key].Value = value;
            }
            else
            {
                configurationFile.AppSettings.Settings.Add(key, value);
            }

            configurationFile.Save(ConfigurationSaveMode.Full);
        }
        public string GetSetting(string key)
        {
            _appSettings.TryGetValue(key, out var value);
            return value;
        }

        private void DeleteOldConfigFile()
        {
            if (!File.Exists(ConfigFileNameInBinForSetting) || _oldFileDeletedBeforeSettingFlag)
                return;

            File.Delete(ConfigFileNameInBinForSetting);
            _oldFileDeletedBeforeSettingFlag = true;
        }

        private static Dictionary<string,string> LoadConfigFileForReading()
        {
            var config = XDocument.Load(ConfigFileNameInBinForGetting);
            var appSettings = config.Descendants("appSettings").First().Elements();
            return appSettings.ToDictionary(x => x.Attribute("key").Value, x => x.Attribute("value").Value);
        }
    }
}