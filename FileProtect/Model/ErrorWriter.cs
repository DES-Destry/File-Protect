using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileProtect.Model
{
    class ErrorWriter
    {
        public static void WriteError(Exception exception)
        {
            if (App.Settings == null || App.Settings.WriteErrorFiles == true)
            {
                using (FileStream sw = new FileStream($@"{App.MainPath}\File Protect\{DateTime.Now.ToString("MMddyyyyHHmmss")}-{exception.HResult}.err", FileMode.Create))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(sw, exception);

                    Logs.WriteLog($"CRITICALERROR-{exception.Message}");
                    Logs.WriteLog($"\"{App.MainPath}\\File Protect\\{DateTime.Now.ToString("MMddyyyyHHmmss")}-{exception.HResult}.err\" has been created");
                }
            }
            else
            {
                return;
            }
        }
    }
}
