using FileProtect.Model;
using System;
using System.Security.Cryptography;
using System.Windows;

namespace FileProtect
{
    public partial class App : Application
    {
        public static readonly string MainPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\DES Destry";
        public static readonly MD5 md5 = MD5.Create();
        public static Settings Settings { get; set; }

        public static void UpdateSettings(Settings settings)
        {
            try
            {
                Settings = settings;
            }
            catch(Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
