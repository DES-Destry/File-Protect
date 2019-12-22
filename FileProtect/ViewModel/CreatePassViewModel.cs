using FileProtect.Messages;
using FileProtect.Model;
using System;
using System.Media;
using System.Text;
using System.Windows;

namespace FileProtect.ViewModel
{
    class CreatePassViewModel : BaseViewModel
    {
        private Settings settings;

        private string newPassword = "";
        public string NewPassword
        {
            get
            {
                return newPassword;
            }
            set
            {
                OnPropertyChanged();
                newPassword = value;
            }
        }


        private string repPassword = "";
        public string RepPassword
        {
            get
            {
                return repPassword;
            }
            set
            {
                OnPropertyChanged();
                repPassword = value;
            }
        }


        private RelayCommand createCommand;
        public RelayCommand CreateCommand
        {
            get
            {
                try
                {
                    return createCommand ??
                        (createCommand = new RelayCommand(obj =>
                        {
                            if (newPassword.Length < 6)
                            {
                                SystemSounds.Exclamation.Play();

                                InfoMessage.ShowInfo("WARNING!", "Password must contain at least 6 characters!");
                                Logs.WriteLog("WARNING-when entering the password, the password contained less than 6 characters");

                                return;
                            }
                            else if (newPassword != repPassword)
                            {
                                SystemSounds.Hand.Play();

                                InfoMessage.ShowInfo("ERROR!", "These two fields should be the same in content!");
                                Logs.WriteLog("ERROR-when entering the password, the passwords in the two input fields differed from each other");

                                return;
                            }
                            else
                            {
                                var alkndfw = App.md5.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                                string asdffdsairw = Convert.ToBase64String(alkndfw);

                                settings = new Settings(asdffdsairw, "v1.0.0", true, false, true, true, true, true);
                                App.UpdateSettings(settings);
                                SettingsManipulator.Write($@"{App.MainPath}\File Protect\appsettings.json", settings);

                                Logs.WriteLog("Password has been created!");
                                Logs.WriteLog($"\"{App.MainPath}\\File Protect\\appsettings.json\" has been created!");
                            }
                        }));
                }
                catch (Exception ex)
                {
                    ErrorWriter.WriteError(ex);

                    return null;
                }
            }
        }


        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                try
                {
                    return cancelCommand ??
                        (cancelCommand = new RelayCommand(obj =>
                        {
                            Logs.WriteLog("The app has been turned off");
                            Application.Current.Shutdown();
                        }));
                }
                catch (Exception ex)
                {
                    ErrorWriter.WriteError(ex);

                    return null;
                }
            }
        }
    }
}
