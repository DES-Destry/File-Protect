using DESTRY.IO.Errors;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;

namespace FileProtectUninstall
{
    class Program
    {
        private static string currentPath = Environment.CurrentDirectory;
        private static string cahcePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\DES Destry\\File Protect";
        static void Main(string[] args)
        {
            TurnOff();
            Delete(currentPath);
            if (Directory.Exists(cahcePath))
            {
                Delete(cahcePath);
            }
            Console.WriteLine("File protect application has been fully uninstalled! Uninstall file please delete manually");
            Console.WriteLine("Enter to continue...");
            Console.ReadKey();
        }

        private static void TurnOff()
        {
            try
            {
                Process[] processes = Process.GetProcesses();
                foreach (var process in processes)
                {
                    if (process.ProcessName == "FileProtect")
                    {
                        process.Kill();
                        Console.WriteLine("FileProtect process killed...");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is Win32Exception)
                {
                    Console.WriteLine("FileProtect process not killed. Don't touch anything...");
                    TurnOff();
                }
                else
                {
                    WriteError(ex);
                }
            }
        }

        private static void WriteError(Exception exception)
        {
            string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\DES Destry\File Protect\{DateTime.Now.ToString("MMddyyyyHHmmss")}-{exception.HResult}.err";
            ErrorManipulator.WriteError(exception, path);
        }

        private static void Delete(string path)
        {
            try
            {
                string[] files = Directory.GetFiles(path);
                Directory.SetAccessControl(path, new DirectorySecurity(path, AccessControlSections.None));

                foreach (string file in files)
                {
                    if (Path.GetFileNameWithoutExtension(file) != "FileProtectUpdater")
                    {
                        File.Delete(file);
                    }
                }
                if (path != currentPath)
                {
                    Directory.Delete(path);
                }
                Console.WriteLine($"{path} has been deleted");
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    Console.WriteLine("New attempt to delete files... Don't touch anything...");
                    Delete(path);
                }
                else
                {
                    WriteError(ex);
                }
            }
        }
    }
}
