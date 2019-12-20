using System.Windows;

namespace FileProtect.Messages
{
    public partial class InfoMessage : Window
    {
        public InfoMessage()
        {
            InitializeComponent();
        }

        public static void ShowInfo(string title, string message)
        {
            InfoMessage info = new InfoMessage();
            info.TitleText.Text = title;
            info.MessageText.Text = message;
            info.ShowDialog();
        }

        private void OK(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
