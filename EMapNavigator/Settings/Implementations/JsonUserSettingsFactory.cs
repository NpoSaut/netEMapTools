using System;
using System.IO;
using EMapNavigator.Settings.Interfaces;
using Newtonsoft.Json;

namespace EMapNavigator.Settings.Implementations
{
    public class JsonUserSettingsFactory : ISettingsFactory<UserSettings>, IDisposable
    {
        private static readonly string _settingsFileName =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                         "Saut", "MapViewer", "Settings.json");

        private readonly Lazy<UserSettings> _settings;
        public JsonUserSettingsFactory() { _settings = new Lazy<UserSettings>(LoadSettings); }

        public void Dispose()
        {
            if (_settings.IsValueCreated)
                SaveSettings(_settings.Value);
        }

        public UserSettings Produce() { return _settings.Value; }

        private static UserSettings LoadSettings()
        {
            try
            {
                string jsonString = File.ReadAllText(_settingsFileName);
                return JsonConvert.DeserializeObject<UserSettings>(jsonString);
            }
            catch (Exception)
            {
                return new UserSettings();
            }
        }

        private static void SaveSettings(UserSettings SettingsObject)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_settingsFileName));
                File.WriteAllText(_settingsFileName,
                                  JsonConvert.SerializeObject(SettingsObject, Formatting.Indented));
            }
            catch (Exception) { }
        }
    }
}
