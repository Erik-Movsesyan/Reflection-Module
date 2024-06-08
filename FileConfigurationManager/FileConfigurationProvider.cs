using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ConfigurationManagement.Providers
{
    public class FileConfigurationProvider : IConfigurationProvider
    {
        private const string ConfigFilePath = "FileConfig.json";
        private bool _oldFileDeletedFlag;

        public void SetSetting(string key, string value, bool overWriteOldFile = true)
        {
            if (overWriteOldFile)
                DeleteOldConfigFile();

            var setting = File.Exists(ConfigFilePath)
                ? JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(ConfigFilePath))
                : [];

            if (!setting.TryAdd(key, value))
            {
                setting[key] = value;
            }

            using var fileStream = File.OpenWrite(ConfigFilePath);
            using var fileWriter = new StreamWriter(fileStream);
            fileWriter.WriteLine(JsonConvert.SerializeObject(setting));
        }

        public string GetSetting(string key)
        {
            if (!File.Exists(ConfigFilePath))
                return null;

            var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(ConfigFilePath));
            return settings.TryGetValue(key, out var value) ? value : null;
        }

        private void DeleteOldConfigFile()
        {
            if (!File.Exists(ConfigFilePath) || _oldFileDeletedFlag)
                return;

            File.Delete(ConfigFilePath);
            _oldFileDeletedFlag = true;
        }
    }
}