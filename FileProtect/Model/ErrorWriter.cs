using DESTRY.IO.Errors;
using System;

namespace FileProtect.Model
{
    class ErrorWriter
    {
        public static void WriteError(Exception exception)
        {
            if (App.Settings == null || App.Settings.WriteErrorFiles)
            {
                ErrorManipulator.WriteError(exception, $@"{App.MainPath}\File Protect\{DateTime.Now.ToString("MMddyyyyHHmmss")}-{exception.HResult}.err");

                Logs.WriteLog($"CRITICALERROR-{exception.Message}");
                Logs.WriteLog($"\"{App.MainPath}\\File Protect\\{DateTime.Now.ToString("MMddyyyyHHmmss")}-{exception.HResult}.err\" has been created");
            }
            else
            {
                return;
            }
        }
    }
}
