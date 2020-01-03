using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace FileProtect.Model
{
    class OperationManipulator
    {
        public static List<Operation> Read(string path)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(List<Operation>));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                var settings = jsonSerializer.ReadObject(fs) as List<Operation>;
                return settings;
            }
        }

        public static void Write(string path, List<Operation> dataObject)
        {
            if (App.Settings == null || App.Settings.SaveCache)
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(List<Operation>));
                if (File.Exists(path)) File.Delete(path);

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    jsonSerializer.WriteObject(fs, dataObject);
                }
            }
        }
    }
}
