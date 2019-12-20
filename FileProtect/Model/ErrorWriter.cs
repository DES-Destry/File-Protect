using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileProtect.Model
{
    class ErrorWriter
    {
        public static void WriteError(Exception exception)
        {
            using(FileStream sw = new FileStream($@"{App.MainPath}\File Protect\error{DateTime.Now.ToString("MMddyyyyHHmmss")}.err", FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(sw, exception);
            }
        }
    }
}
