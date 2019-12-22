using System;
using System.IO;

namespace FileProtect.Model
{
    class Logs
    {
        public static void WriteLog(string message)
        {
            try
            {
                if (App.Settings == null || App.Settings.WriteLogs == true)
                {
                    using (StreamWriter sw = new StreamWriter($@"{App.MainPath}\File Protect\.log", true))
                    {
                        sw.WriteLine(" ");
                        sw.WriteLine($@"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- {message}");
                    }
                }
                else
                {
                    return;
                }
            }
            catch(Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
