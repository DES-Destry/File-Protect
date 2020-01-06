using DESTRY.IO.Errors;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;

namespace FileProtectUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string currPath = Environment.CurrentDirectory;
                TurnOff();

                if (Directory.Exists("UPDATE"))
                {
                    Delete(currPath);

                    string[] updateFiles = Directory.GetFiles($"{currPath}\\UPDATE");
                    foreach (string updateFile in updateFiles)
                    {
                        if (Path.GetFileNameWithoutExtension(updateFile) != "FileProtectUpdater")
                        {
                            File.Move(updateFile, $"{currPath}\\{Path.GetFileName(updateFile)}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Update file not installed... Try again.");
                    Console.WriteLine("Enter to continue...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Success! Enter to continue...");
                Console.ReadKey();
                Process.Start($"{currPath}\\FileProtect.exe");
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }

        private static void WriteError(Exception exception)
        {
            string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\DES Destry\File Protect\{DateTime.Now.ToString("MMddyyyyHHmmss")}-{exception.HResult}.err";
            ErrorManipulator.WriteError(exception, path);
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
                Console.WriteLine("Old app has been deleted");
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    Console.WriteLine("New attempt to remove old version... Don't touch anything...");
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
