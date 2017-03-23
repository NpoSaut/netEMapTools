using System;
using System.Diagnostics;
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
                var jsonString = File.ReadAllText(_settingsFileName);
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
                var settingsDirectory = Path.GetDirectoryName(_settingsFileName);
                if (settingsDirectory != null)
                    Directory.CreateDirectory(settingsDirectory);
                var json = JsonConvert.SerializeObject(SettingsObject, Formatting.Indented);
                File.WriteAllText(_settingsFileName, json);
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message, e.ToString());
            }
        }
    }
}
