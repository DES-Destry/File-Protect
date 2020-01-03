using FileProtect.Model;
using System.Media;
using System.Threading.Tasks;
using System.Windows;

namespace FileProtect.Messages
{
    public partial class WarningMessage : Window
    {
        private static WarningResultType result = WarningResultType.None;
        public WarningMessage()
        {
            InitializeComponent();
        }

        private void Continue(object sender, RoutedEventArgs e)
        {
            SetResult(WarningResultType.Continue);
            Logs.WriteLog($"Warning message returned:{WarningResultType.Continue.ToString()}");
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            SetResult(WarningResultType.Cancel);
            Logs.WriteLog($"Warning message returned:{WarningResultType.Cancel.ToString()}");
            Close();
        }

        public static WarningResultType ShowWarning(string title, string message)
        {
            if (App.Settings.WarningMessageShow)
            {
                SystemSounds.Exclamation.Play();
                WarningMessage warningMessage = new WarningMessage();
                warningMessage.TitleText.Text = title;
                warningMessage.MessageText.Text = message;
                warningMessage.ShowDialog();
                Logs.WriteLog("Warning message has been opened!");

                WaitAsync();
                return result;
            }
            else
            {
                return WarningResultType.Continue;
            }
        }

        private void SetResult(WarningResultType type)
        {
            result = type;
        }

        private async static void WaitAsync()
        {
            await Task.Run(() => Wait());
        }

        private static void Wait()
        {
            while (result == WarningResultType.None)
            {
                continue;
            }
        }
    }
}
