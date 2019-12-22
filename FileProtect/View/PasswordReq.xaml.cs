using FileProtect.Messages;
using FileProtect.Model;
using System;
using System.Media;
using System.Text;
using System.Windows;

namespace FileProtect.View
{
    public partial class PasswordReq : Window
    {
        private Settings settings;
        private string exehs;
        public PasswordReq()
        {
            try
            {
                InitializeComponent();

                settings = SettingsManipulator.Read($@"{App.MainPath}\File Protect\appsettings.json");
                App.UpdateSettings(settings);

                Logs.WriteLog($"\"{App.MainPath}\\File Protect\\appsettings.json\" has been readed");
                Logs.WriteLog("Global application's settings has been updated");
            }
            catch(Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }

        private void Continue(object sender, RoutedEventArgs e)
        {
            try
            {
                string arrarar = settings.ASSKOP();

                if (Password.Password != null)
                {
                    exehs = Convert.ToBase64String(App.md5.ComputeHash(Encoding.UTF8.GetBytes(Password.Password)));
                }

                if (exehs == arrarar)
                {
                    Logs.WriteLog("Password was entered correctly!");
                    this.Close();
                }
                else
                {
                    SystemSounds.Hand.Play();

                    InfoMessage.ShowInfo("ERROR!", "Password was entered incorrectly!");
                    Logs.WriteLog("ERROR-Password was entered incorrectly!");

                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            try
            {
                Logs.WriteLog("The app has been turned off");
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
