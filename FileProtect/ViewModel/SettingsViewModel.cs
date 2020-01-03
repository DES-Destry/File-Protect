using FileProtect.Messages;
using FileProtect.Model;
using FileProtect.View;
using System;
using System.IO;
using System.Windows;

namespace FileProtect.ViewModel
{
    class SettingsViewModel : BaseViewModel
    {
        private Settings currentSettings;
        private readonly Settings standartSettings = new Settings(App.Settings.ASSKOP(), App.Version, true, false, true, true, true, true);
        private readonly string settingsPath = $@"{App.MainPath}\File Protect\appsettings.json";


        private bool delOldData;
        public bool DelOldData
        {
            get
            {
                return delOldData;
            }
            set
            {
                delOldData = value;
                SavedChanges = false;
                OnPropertyChanged();
            }
        }


        private bool saveOperations;
        public bool SaveOperations
        {
            get
            {
                return saveOperations;
            }
            set
            {
                saveOperations = value;
                SavedChanges = false;
                OnPropertyChanged();
            }
        }


        private bool writeErrors;
        public bool WriteErrors
        {
            get
            {
                return writeErrors;
            }
            set
            {
                writeErrors = value;
                SavedChanges = false;
                OnPropertyChanged();
            }
        }


        private bool warningMessages;
        public bool WarningMessages
        {
            get
            {
                return warningMessages;
            }
            set
            {
                warningMessages = value;
                SavedChanges = false;
                OnPropertyChanged();
            }
        }


        private bool checkUpdates;
        public bool CheckUpdates
        {
            get
            {
                return checkUpdates;
            }
            set
            {
                checkUpdates = value;
                SavedChanges = false;
                OnPropertyChanged();
            }
        }


        private bool writeLogs;
        public bool WriteLogs
        {
            get
            {
                return writeLogs;
            }
            set
            {
                writeLogs = value;
                SavedChanges = false;
                OnPropertyChanged();
            }
        }


        private bool savedChanges;
        public bool SavedChanges
        {
            get
            {
                return savedChanges;
            }
            set
            {
                savedChanges = value;
                OnPropertyChanged();
            }
        }


        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                    (cancelCommand = new RelayCommand(obj =>
                    {
                        SetChecks(currentSettings);
                        Logs.WriteLog("New settings has been canceled!");

                        SavedChanges = true;
                    }));
            }
        }


        private RelayCommand standartCommand;
        public RelayCommand StandartCommand
        {
            get
            {
                return standartCommand ??
                    (standartCommand = new RelayCommand(obj =>
                    {
                        SetChecks(standartSettings);
                        Logs.WriteLog("Standart settings has been chosen");

                        SavedChanges = false;
                    }));
            }
        }


        private RelayCommand applyCommand;
        public RelayCommand ApplyCommand
        {
            get
            {
                return applyCommand ??
                    (applyCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (WarningMessage.ShowWarning("Are you sure?", "These settings will be applied irrevocably. Later you can restore only the default settings") == WarningResultType.Continue)
                            {
                                Settings settings = new Settings(currentSettings.ASSKOP(), currentSettings.Version, checkUpdates, delOldData, writeLogs, saveOperations, warningMessages, writeErrors);
                                SettingsManipulator.Write(settingsPath, settings);
                                currentSettings = settings;
                                Logs.WriteLog($"New settings \"{settingsPath}\" has been writed");

                                App.UpdateSettings(settings);
                                Logs.WriteLog("Global settings has been updated!");

                                SavedChanges = true;
                            }
                            else
                            {
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        private RelayCommand resetSettingsCommand;
        public RelayCommand ResetSettingsCommand
        {
            get
            {
                return resetSettingsCommand ??
                    (resetSettingsCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (WarningMessage.ShowWarning("Are you sure?", "All settings will be deleted, the password will be reset, the application will close.") == WarningResultType.Continue)
                            {
                                File.Delete($@"{App.MainPath}\File Protect\appsettings.json");
                                Logs.WriteLog("The app has been turned off");
                                Application.Current.Shutdown();
                            }
                            else
                            {
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        private RelayCommand clearLogsCommand;
        public RelayCommand ClearLogsCommand
        {
            get
            {
                return clearLogsCommand ??
                    (clearLogsCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            FileInfo info = new FileInfo($@"{App.MainPath}\File Protect\.log");
                            long size = info.Length;

                            if (WarningMessage.ShowWarning("Are you sure?", $"If you want fully clear log file, please turn off \"Write logs\" settings. You will clear {size / 1024} KB") == WarningResultType.Continue)
                            {
                                using (StreamWriter sw = new StreamWriter($@"{App.MainPath}\File Protect\.log", false))
                                {
                                    sw.Write("");
                                }

                                Logs.WriteLog("Logs has been cleared");
                            }
                            else
                            {
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        private RelayCommand deleteErrorCommand;
        public RelayCommand DeleteErrorCommand
        {
            get
            {
                return deleteErrorCommand ??
                    (deleteErrorCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (WarningMessage.ShowWarning("Are you sure?", "This files can help to support this app. Send email in settings page before deleting error files!") == WarningResultType.Continue)
                            {
                                string[] files = Directory.GetFiles($@"{App.MainPath}\File Protect", "*.err");

                                foreach (string file in files)
                                {
                                    File.Delete(file);
                                }

                                Logs.WriteLog("All error files has been deleted!");
                            }
                            else
                            {
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        private RelayCommand changePasswordCommand;
        public RelayCommand ChangePasswordCommand
        {
            get
            {
                return changePasswordCommand ??
                    (changePasswordCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (WarningMessage.ShowWarning("Are you sure?", "Your password will be changed") == WarningResultType.Continue)
                            {
                                CreatePass cp = new CreatePass();
                                Logs.WriteLog("Password creation window has been opened");
                                cp.ShowDialog();
                            }
                            else
                            {
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        private RelayCommand deleteAllCommand;
        public RelayCommand DeleteAllCommand
        {
            get
            {
                return deleteAllCommand ??
                    (deleteAllCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (WarningMessage.ShowWarning("Are you sure?", "All app data will be deleted. Everything will be reset to the state of the newly installed app.") == WarningResultType.Continue)
                            {
                                string[] files = Directory.GetFiles($@"{App.MainPath}\File Protect");
                                foreach (string file in files)
                                {
                                    File.Delete(file);
                                }

                                Directory.Delete($@"{App.MainPath}\File Protect");
                                Application.Current.Shutdown();
                            }
                            else
                            {
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        private RelayCommand deleteStoryCommand;
        public RelayCommand DeleteStoryCommand
        {
            get
            {
                return deleteStoryCommand ??
                    (deleteStoryCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (WarningMessage.ShowWarning("Are you sure?", "Your operation story will be deleted!") == WarningResultType.Continue)
                            {
                                if (File.Exists($@"{App.MainPath}\File Protect\appcache.json"))
                                {
                                    File.Delete($@"{App.MainPath}\File Protect\appcache.json");
                                }

                                Logs.WriteLog("Operation story has been deleted!");
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
                    }));
            }
        }


        private RelayCommand sendCommand;
        public RelayCommand SendCommand
        {
            get
            {
                return sendCommand ??
                    (sendCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            SendEmail email = new SendEmail();
                            email.ShowDialog();
                        }
                        catch(Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        public SettingsViewModel()
        {
            currentSettings = App.Settings;
            Logs.WriteLog($"\"{settingsPath}\" has been readed!");

            SetChecks(currentSettings);
        }

        private void SetChecks(Settings settings)
        {
            DelOldData = settings.DelDataBefOperation;
            SaveOperations = settings.SaveCache;
            WriteErrors = settings.WriteErrorFiles;
            WarningMessages = settings.WarningMessageShow;
            CheckUpdates = settings.CheckUpdates;
            WriteLogs = settings.WriteLogs;
        }
    }
}
