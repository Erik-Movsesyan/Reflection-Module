namespace ConfigurationManagement.Providers
{
    public interface IConfigurationProvider
    {
        void SetSetting(string key, string value, bool overWriteOldFile = true);
        string GetSetting(string key);
    }
}
