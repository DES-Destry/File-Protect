using FileProtect.Messages;
using FileProtect.Model;
using System;
using System.IO;
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
                                using (StreamWriter sw = new StreamWriter($@"{App.MainPath}\File Protect\0.log", true))
                                {
                                    sw.WriteLine($"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- WARNING-when entering the password, the password contained less than 6 characters");
                                }
                                return;
                            }
                            else if (newPassword != repPassword)
                            {
                                SystemSounds.Hand.Play();
                                InfoMessage.ShowInfo("ERROR!", "These two fields should be the same in content!");
                                using (StreamWriter sw = new StreamWriter($@"{App.MainPath}\File Protect\0.log", true))
                                {
                                    sw.WriteLine($"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- ERROR-when entering the password, the passwords in the two input fields differed from each other");
                                }
                                return;
                            }
                            else
                            {
                                var alkndfw = App.md5.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                                string asdffdsairw = Convert.ToBase64String(alkndfw);

                                settings = new Settings(asdffdsairw, "v1.0.0", true, false, true, true, true);
                                SettingsManipulator.Write($@"{App.MainPath}\File Protect\appsettings.json", settings);

                                using (StreamWriter sw = new StreamWriter($@"{App.MainPath}\File Protect\0.log", true))
                                {
                                    sw.WriteLine($"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- Password has been created!");
                                    sw.WriteLine($"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- \"{App.MainPath}\\File Protect\\appsettings.json\" has been created!");
                                }
                            }
                        }));
                }
                catch (Exception ex)
                {
                    using (StreamWriter sw = new StreamWriter($@"{App.MainPath}\File Protect\0.log", true))
                    {
                        sw.WriteLine($"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- ERROR-{ex.Message}");
                    }
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
                            Application.Current.Shutdown();
                        }));
                }
                catch (Exception ex)
                {
                    using (StreamWriter sw = new StreamWriter($@"{App.MainPath}\File Protect\0.log", true))
                    {
                        sw.WriteLine($"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- ERROR-{ex.Message}");
                    }
                    ErrorWriter.WriteError(ex);
                    return null;
                }
            }
        }
    }
}
