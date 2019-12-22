using FileProtect.Model;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FileProtect.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private Page mainPage;
        private Page cryptPage;
        private Page decryptPage;
        private Page settingsPage;

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

                    }));
            }
        }


        private RelayCommand decryptOpenCommand;
        public RelayCommand DeryptOpenCommand
        {
            get
            {
                return decryptOpenCommand ??
                    (decryptOpenCommand = new RelayCommand(obj =>
                    {

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

                Logs.WriteLog("Main page has been selected");
            }
            catch(Exception ex)
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
    }
}
