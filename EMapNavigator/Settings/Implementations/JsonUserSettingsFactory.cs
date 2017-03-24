using System;
using System.Diagnostics;
using System.IO;
using EMapNavigator.Settings.Interfaces;
using Geographics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EMapNavigator.Settings.Implementations
{
    public class JsonUserSettingsFactory : ISettingsFactory<UserSettings>, IDisposable
    {
        private static readonly string _settingsFileName =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                         "Saut", "MapViewer", "Settings.json");

        private static readonly JsonSerializerSettings _jsonSerializationSettings;

        private readonly Lazy<UserSettings> _settings;

        static JsonUserSettingsFactory()
        {
            _jsonSerializationSettings = new JsonSerializerSettings();
            _jsonSerializationSettings.Converters.Add(new DegreeConverter());
            _jsonSerializationSettings.Formatting = Formatting.Indented;
        }

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
                var settings = JsonConvert.DeserializeObject<UserSettings>(jsonString, _jsonSerializationSettings);
                return settings;
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
                var json = JsonConvert.SerializeObject(SettingsObject, _jsonSerializationSettings);
                File.WriteAllText(_settingsFileName, json);
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message, e.ToString());
            }
        }

        private class DegreeConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) { return objectType == typeof(Degree); }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var degree = (Degree)value;
                writer.WriteValue(degree.Value);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var token = JToken.Load(reader);
                return new Degree(token.Value<double>());
            }
        }
    }
}
