using System.Configuration;
using System.IO;

namespace ConfigurationManagement.Providers
{
    public class ConfigurationManagerConfigurationProvider : IConfigurationProvider
    {
        private const string ConfigFieNameInBin = "ConfigurationManagement.dll.config";
        private bool _oldFileDeletedFlag;

        public void SetSetting(string key, string value, bool overWriteOldFile = true) 
        {
            if(overWriteOldFile)
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
            var appSettings = ConfigurationManager.AppSettings;
            var configValue = appSettings[key];

            return configValue;
        }

        private void DeleteOldConfigFile()
        {
            if (!File.Exists(ConfigFieNameInBin) || _oldFileDeletedFlag)
                return;

            File.Delete(ConfigFieNameInBin);
            _oldFileDeletedFlag = true;
        }
    }
}
