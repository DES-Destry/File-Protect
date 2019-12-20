using System.IO;
using System.Runtime.Serialization.Json;

namespace FileProtect.Model
{
    class SettingsManipulator
    {
        public static Settings Read(string path)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                Settings settings = jsonSerializer.ReadObject(fs) as Settings;
                return settings;
            }
        }

        public static void Write(string path, Settings settings)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(Settings));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                jsonSerializer.WriteObject(fs, settings);
            }
        }
    }
}
