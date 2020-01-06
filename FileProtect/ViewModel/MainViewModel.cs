using FileProtect.Model;
using FileProtect.View;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileProtect.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Page mainPage;
        private readonly Page cryptPage;
        private readonly Page decryptPage;
        private readonly Page settingsPage;


        private Page currentPage;
        public Page CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                OnPropertyChanged();
                currentPage = value;
            }
        }


        private double frameOpacity;
        public double FrameOpacity
        {
            get
            {
                return frameOpacity;
            }
            set
            {
                OnPropertyChanged();
                frameOpacity = value;
            }
        }


        private RelayCommand cryptOpenCommand;
        public RelayCommand CryptOpenCommand
        {
            get
            {
                return cryptOpenCommand ??
                    (cryptOpenCommand = new RelayCommand(obj =>
                    {
                        ChangePage(cryptPage);
                    }));
            }
        }


        private RelayCommand decryptOpenCommand;
        public RelayCommand DecryptOpenCommand
        {
            get
            {
                return decryptOpenCommand ??
                    (decryptOpenCommand = new RelayCommand(obj =>
                    {
                        ChangePage(decryptPage);
                    }));
            }
        }


        private RelayCommand settingsOpenCommand;
        public RelayCommand SettingsOpenCommand
        {
            get
            {
                return settingsOpenCommand ??
                    (settingsOpenCommand = new RelayCommand(obj =>
                    {
                        ChangePage(settingsPage);
                    }));
            }
        }


        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ??
                    (exitCommand = new RelayCommand(obj =>
                    {
                        Logs.WriteLog("The app has been turned off");
                        Application.Current.Shutdown();
                    }));
            }
        }


        public MainViewModel()
        {
            try
            {
                if (!Directory.Exists($"{App.MainPath}"))
                {
                    Directory.CreateDirectory($"{App.MainPath}");
                    CreateMainFolder();
                }
                else
                {
                    if (!Directory.Exists($@"{App.MainPath}\File Protect"))
                    {
                        CreateMainFolder();
                    }
                    else
                    {
                        if (!File.Exists($@"{App.MainPath}\File Protect\.log"))
                        {
                            File.Create($@"{App.MainPath}\File Protect\.log").Close();
                            Logs.WriteLog($"\"{App.MainPath}\\File Protect\\.log\" has been created!");
                        }
                        if (!File.Exists($@"{App.MainPath}\File Protect\appsettings.json"))
                        {
                            Logs.WriteLog("Application starts working!");

                            View.CreatePass createPass = new View.CreatePass();

                            Logs.WriteLog("Creating password window has been opened!");
                            createPass.ShowDialog();
                        }
                        else
                        {
                            Logs.WriteLog("Application starts working!");

                            View.PasswordReq passwordReq = new View.PasswordReq();

                            Logs.WriteLog("Password request window has been opened!");
                            passwordReq.ShowDialog();
                        }
                    }
                }

                mainPage = new Pages.Main();
                cryptPage = new Pages.Crypt();
                decryptPage = new Pages.Decrypt();
                settingsPage = new Pages.Settings();

                Logs.WriteLog("Pages has been initialized");

                FrameOpacity = 1;
                currentPage = mainPage;

                if (Directory.Exists($"{Environment.CurrentDirectory}\\UPDATE"))
                {
                    string[] files = Directory.GetDirectories($"{Environment.CurrentDirectory}\\UPDATE");
                    foreach (string file in files)
                    {
                        File.Move(file, $"{Environment.CurrentDirectory}\\{Path.GetFileName(file)}");
                    }

                    Directory.Delete($"{Environment.CurrentDirectory}\\UPDATE");
                }

                if (App.Settings == null || App.Settings.CheckUpdates)
                {
                    UpdateChecker uc = new UpdateChecker();
                    uc.ShowDialog();
                }

                Logs.WriteLog("Main page has been selected");
            }
            catch (Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }

        private void CreateMainFolder()
        {
            Directory.CreateDirectory($@"{App.MainPath}\File Protect");
            File.Create($@"{App.MainPath}\File Protect\.log").Close();

            Logs.WriteLog($"First application startup");
            Logs.WriteLog($"\"{App.MainPath}\\File Protect\" has been created!");
            Logs.WriteLog($"\"{App.MainPath}\\File Protect\\.log\" has been created!");

            View.CreatePass createPass = new View.CreatePass();

            Logs.WriteLog("Creating password window has been opened!");
            createPass.ShowDialog();
        }

        private async void ChangePage(Page page)
        {
            await Task.Factory.StartNew(() =>
            {
                for (double opacity = 1.0; opacity > 0.0; opacity -= 0.1)
                {
                    FrameOpacity = opacity;
                    Thread.Sleep(50);
                }
                Logs.WriteLog("Page off animation has been worked");

                CurrentPage = page;
                Logs.WriteLog("Page has been changed");

                for (double opacity = 0.0; opacity < 1.1; opacity += 0.1)
                {
                    FrameOpacity = opacity;
                    Thread.Sleep(50);
                }
                Logs.WriteLog("Page on animation has been worked");
            });
        }
    }
}
