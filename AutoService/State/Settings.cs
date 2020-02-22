using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace AutoService.State
{
    [DataContract]
    public class Settings
    {
        public const string RegistryPath = @"DialogPage\AutoService";

        [DataMember]
        public ServiceEntry[] Services { get; set; } = ServiceEntries();

        [DataMember]
        public bool ShowStartPage { get; set; }

        private static readonly string ProgramDataFolder;
        private static readonly string SettingsFile;

        public static event EventHandler SettingsUpdated;

        private static void OnSettingsUpdated(object sender, EventArgs ea) => SettingsUpdated?.Invoke(sender, ea);

        static Settings()
        {
            ProgramDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutoService");
            SettingsFile = Path.Combine(ProgramDataFolder, "autoService.json");
        }

        public static Settings Load()
        {
            if (Runtime.RunningUnitTests) return new Settings();
            Directory.CreateDirectory(ProgramDataFolder);
            if (!File.Exists(SettingsFile)) new Settings().Save();
            using (var stream = new FileStream(SettingsFile, FileMode.Open))
            {
                var deserialize = new DataContractJsonSerializer(typeof(Settings));
                var settings = (Settings)deserialize.ReadObject(stream);
                return settings;
            }
        }

        public void Save()
        {
            if (Runtime.RunningUnitTests) return;
            Directory.CreateDirectory(ProgramDataFolder);
            using (var stream = new FileStream(SettingsFile, FileMode.Create))
            {
                var serializer = new DataContractJsonSerializer(typeof(Settings));
                serializer.WriteObject(stream, this);
            }
            OnSettingsUpdated(this, EventArgs.Empty);
        }

        private static ServiceEntry[] ServiceEntries()
        {
            return new ServiceEntry[0];
        }
    }
}
