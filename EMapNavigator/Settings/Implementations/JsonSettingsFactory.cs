using System;
using System.IO;
using System.Net;
using EMapNavigator.Settings.Interfaces;
using Geographics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EMapNavigator.Settings.Implementations
{
    public class JsonSettingsFactory<TSettings> : ISettingsFactory<TSettings>, IDisposable
        where TSettings : new()
    {
        private readonly JsonSerializerSettings _jsonSerializationSettings;
        private readonly Lazy<TSettings> _settings;
        private readonly string _settingsFileName;

        public JsonSettingsFactory(string FileName)
        {
            _jsonSerializationSettings = new JsonSerializerSettings();
            _jsonSerializationSettings.Converters.Add(new IPAddressConverter());
            _jsonSerializationSettings.Converters.Add(new DegreeConverter());
            _jsonSerializationSettings.Formatting = Formatting.Indented;

            _settingsFileName =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                             "Saut", "MapViewer", FileName);

            _settings = new Lazy<TSettings>(LoadSettings);
        }

        public void Dispose()
        {
            if (_settings.IsValueCreated)
                SaveSettings(_settings.Value);
        }

        public TSettings Produce() { return _settings.Value; }

        private TSettings LoadSettings()
        {
            try
            {
                string jsonString = File.ReadAllText(_settingsFileName);
                return JsonConvert.DeserializeObject<TSettings>(jsonString, _jsonSerializationSettings);
            }
            catch (Exception)
            {
                return new TSettings();
            }
        }

        private void SaveSettings(TSettings SettingsObject)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_settingsFileName));
                string json = JsonConvert.SerializeObject(SettingsObject, _jsonSerializationSettings);
                File.WriteAllText(_settingsFileName, json);
            }
            catch (Exception) { }
        }

        private class DegreeConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) { return (objectType == typeof (Degree)); }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var degree = (Degree)value;
                writer.WriteValue(degree.Value);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JToken token = JToken.Load(reader);
                return new Degree(token.Value<double>());
            }
        }

        // ReSharper disable once InconsistentNaming
        private class IPAddressConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) { return (objectType == typeof (IPAddress)); }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var ip = (IPAddress)value;
                writer.WriteValue(ip.ToString());
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JToken token = JToken.Load(reader);
                return IPAddress.Parse(token.Value<string>());
            }
        }
    }
}
