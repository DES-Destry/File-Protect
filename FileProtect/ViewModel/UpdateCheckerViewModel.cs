using FileProtect.Messages;
using FileProtect.Model;
using FileProtect.Model.Parser;
using Ionic.Zip;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;

namespace FileProtect.ViewModel
{
    class UpdateCheckerViewModel : BaseViewModel
    {
        private ParserWorker<string> parser;
        private readonly string url = "http://w70198vo.beget.tech/cache/File%20protect%20v0.9.0.rar";
        private string size = default;
        private string sizeKB = default;


        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }


        private int progress = 1;
        private int Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
                OnPropertyChanged();
            }
        }


        private int maxProgress = 1;
        public int MaxProgress
        {
            get
            {
                return maxProgress;
            }
            set
            {
                maxProgress = value;
                OnPropertyChanged();
            }
        }


        private bool installEnabled = false;
        public bool InstallEnabled
        {
            get
            {
                return installEnabled;
            }
            set
            {
                installEnabled = value;
                OnPropertyChanged();
            }
        }


        private bool cancelEnabled = true;
        public bool CancelEnabled
        {
            get
            {
                return cancelEnabled;
            }
            set
            {
                cancelEnabled = value;
                OnPropertyChanged();
            }
        }


        private RelayCommand installCommand;
        public RelayCommand InstallCommand
        {
            get
            {
                return installCommand ??
                    (installCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            InstallEnabled = false;
                            CancelEnabled = false;

                            string path = Environment.CurrentDirectory;

                            MaxProgress = 100;
                            Message = $"1/3 part - downloading: 0%(0 KB/{sizeKB} KB)";

                            using (WebClient wc = new WebClient())
                            {
                                wc.DownloadProgressChanged += (s, e) =>
                                {
                                    Progress = e.ProgressPercentage;
                                    Message = $"1/3 part - downloading: {e.ProgressPercentage}% ({e.BytesReceived / 1024} KB/{sizeKB} KB)";
                                };

                                Logs.WriteLog("Download async has been started!");
                                wc.DownloadFileAsync(new Uri(url), $"{Environment.CurrentDirectory}\\UPDATE.zip");
                            }

                            Logs.WriteLog("Update zip file has been downloaded!");

                            Progress = 0;
                            string[] files = Directory.GetFiles(Environment.CurrentDirectory);
                            MaxProgress = files.Length;
                            Message = $"2/3 part - deleting old version ({Progress}/{MaxProgress})...";
                            Directory.CreateDirectory($@"{App.MainPath}\File Protect\old_version");

                            Logs.WriteLog($"\"{App.MainPath}\\File Protect\\old_version\" directory has been created!");

                            foreach (string file in files)
                            {
                                File.Move(file, $@"{App.MainPath}\File Protect\old_version\{Path.GetFileName(file)}");
                                Progress += 1;
                                Message = $"2/3 part - deleting old version ({Progress}/{MaxProgress})";
                            }
                            Logs.WriteLog("All old files has been deleted!");

                            Progress = 0;
                            using (ZipFile zip = ZipFile.Read($"{path}\\UPDATE.zip"))
                            {
                                Logs.WriteLog($"\"{path}\\UPDATE.zip\" has been readed!");

                                MaxProgress = zip.Entries.Count;
                                Message = $"3/3 part - installing new version ({Progress}/{MaxProgress})";

                                foreach (ZipEntry e in zip)
                                {
                                    e.Extract(Environment.CurrentDirectory, ExtractExistingFileAction.OverwriteSilently);
                                    Progress += 1;
                                    Message = $"3/3 part - installing new version ({Progress}/{MaxProgress})";
                                }
                            }

                            Logs.WriteLog($"\"{path}\\UPDATE.zip\" has been extracted!");

                            Logs.WriteLog($"App with new version has been started!");

                            Process.Start($"{path}\\FileProtect.exe");

                            Logs.WriteLog("The app has been turned off");

                            Application.Current.Shutdown();
                        }
                        catch(Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        public UpdateCheckerViewModel()
        {
            try
            {
                parser = new ParserWorker<string>(new AppParser(), new AppSettings());
                parser.OnNewData += Parser_OnNewData;
                parser.OnCompleted += Parser_OnCompleted;

                Message = "Checking updates...";
                MaxProgress = 2;
                Progress = 1;

                parser.Start();
            }
            catch(Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }

        private void Parser_OnCompleted(object obj)
        {
            try
            {
                InfoMessage.ShowInfo("COMPLETED", "Checking updates has been completed!");
                Logs.WriteLog("COMPLETED-Checking updates has been completed!");
                Progress = 0;
                parser.Abort();
            }
            catch(Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }

        private void Parser_OnNewData(object arg1, string arg2)
        {
            try
            {
                if (string.IsNullOrEmpty(arg2))
                {
                    InfoMessage.ShowInfo("ERROR", "Checking updates has been not correct!");
                    Logs.WriteLog("ERROR-Checking updates has been not correct!");
                    Progress = 0;
                    Message = "ERROR, Click at 'Cancel' button to continue...";
                }
                else
                {
                    if (arg2.Contains(App.Version.Replace("v", "")))
                    {
                        Progress = 0;
                        Message = "A new version of app not found. Your application has the current version. Click at 'Cancel' button to continue...";
                        Logs.WriteLog("A new version of app not found. Your application has the current version.");
                    }
                    else
                    {
                        Logs.WriteLog("Update file has been finded!");
                        Progress = 0;

                        //using (WebClient wc = new WebClient())
                        //{
                        //    wc.OpenRead(url);
                        //    size = wc.ResponseHeaders["Content-Lenght"];
                        //}
                        //Logs.WriteLog("Update file size recivied");
                        if (int.TryParse(size, out int res))
                        {
                            sizeKB = (res / 1024).ToString();
                            Message = $"A new version has been found. Install? Size:{sizeKB} KB in zip.";
                        }
                        else
                        {
                            Message = $"A new version has been found. Install? Size: unknown data format";
                        }

                        InstallEnabled = true;
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
