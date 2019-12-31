using System.IO;
using System.Runtime.Serialization.Json;

namespace FileProtect.Model
{
    class MetaManipulator
    {
        public static Meta Read(string path)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(Meta));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                Meta settings = jsonSerializer.ReadObject(fs) as Meta;
                return settings;
            }
        }

        public static void Write(string path, Meta dataObject)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(Meta));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                jsonSerializer.WriteObject(fs, dataObject);
            }
        }
    }
}
