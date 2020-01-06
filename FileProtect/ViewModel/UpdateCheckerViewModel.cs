using FileProtect.Messages;
using FileProtect.Model;
using FileProtect.Model.Parser;
using Ionic.Zip;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace FileProtect.ViewModel
{
    class UpdateCheckerViewModel : BaseViewModel
    {
        private ParserWorker<string> parser;
        private readonly string url = "https://github.com/DES-Destry/DES-Destry.github.io/raw/master/cache/FIle%20Protect%20v0.9.5.zip";
        private string size = default;
        private string sizeKB = default;
        private bool completed = false;
        private bool installed = false;


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
        public int Progress
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


        private int maxProgress = 2;
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

                            Downloading(path);

                            Installing(path);

                            Updating(path);
                        }
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                            Progress = 0;
                            Message = "ERROR, Click at 'Cancel' button to continue...";
                            CancelEnabled = true;
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
            catch (Exception ex)
            {
                ErrorWriter.WriteError(ex);
                Progress = 0;
                Message = "ERROR, Click at 'Cancel' button to continue...";
            }
        }

        private void Parser_OnCompleted(object obj)
        {
            try
            {
                Logs.WriteLog("COMPLETED-Checking updates has been completed!");
                Progress = 0;
                parser.Abort();
            }
            catch (Exception ex)
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
                        Message = "Your application has the current version. Click at 'Cancel' button to continue...";
                        Logs.WriteLog("A new version of app not found. Your application has the current version.");
                    }
                    else
                    {
                        Logs.WriteLog("Update file has been finded!");
                        Progress = 0;

                        using (WebClient wc = new WebClient())
                        {
                            wc.OpenRead(url);
                            size = wc.ResponseHeaders["Content-Lenght"];
                        }
                        Logs.WriteLog("Update file size recivied");
                        if (int.TryParse(size, out int res))
                        {
                            sizeKB = (res / 1024).ToString();
                            Message = $"A new version has been found. Install? Size:{sizeKB} KB in zip.";
                        }
                        else
                        {
                            Message = $"A new version has been found. Install? Size: ???";
                            sizeKB = "???";
                        }

                        InstallEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorWriter.WriteError(ex);
                Progress = 0;
                Message = "ERROR, Click at 'Cancel' button to continue...";
            }
        }

        private void OnPercentageChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
            Message = $"1/3 part - downloading: {e.ProgressPercentage}% ({e.BytesReceived / 1024} KB/{sizeKB} KB)";
            if (e.ProgressPercentage == 100)
            {
                completed = true;
            }
        }

        private void Downloading(string path)
        {
            Progress = 0;
            MaxProgress = 100;
            Message = $"1/3 part - downloading: 0%(0 KB/{sizeKB} KB)";

            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += OnPercentageChanged;
                Logs.WriteLog("Download async has been started!");
                Task.Run(async () => { wc.DownloadFileAsync(new Uri(url), $"{path}\\UPDATE.zip"); }).ConfigureAwait(false);
            }

            Logs.WriteLog("Update zip file has been downloaded!");
        }

        private async void Installing(string path)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (File.Exists($"{path}\\UPDATE.zip") && completed)
                    {
                        using (ZipFile zip = ZipFile.Read($"{path}\\UPDATE.zip"))
                        {
                            Progress = 0;
                            Logs.WriteLog($"\"{path}\\UPDATE.zip\" has been readed!");

                            Directory.CreateDirectory($"{path}\\UPDATE");

                            MaxProgress = zip.Entries.Count;
                            Message = $"3/3 part - installing new version ({Progress}/{MaxProgress})";

                            foreach (ZipEntry e in zip)
                            {
                                e.Extract($"{path}\\UPDATE", ExtractExistingFileAction.OverwriteSilently);
                                Progress += 1;
                                Message = $"3/3 part - installing new version ({Progress}/{MaxProgress})";
                            }
                        }

                        File.Delete($"{path}\\UPDATE.zip");
                        Logs.WriteLog($"\"{path}\\UPDATE.zip\" has been extracted!");
                        installed = true;
                        break;
                    }
                }
            });
        }

        private async void Updating(string path)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (installed)
                    {
                        Logs.WriteLog($"FileProtectUpdater has been started!!");

                        Process.Start($"{path}\\FileProtectUpdater.exe");

                        Logs.WriteLog("The app has been turned off");

                        Application.Current.Shutdown();
                        break;
                    }
                }
            });
        }
    }
}
