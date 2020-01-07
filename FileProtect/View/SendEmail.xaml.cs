using System.Windows;

namespace FileProtect.View
{
    public partial class SendEmail : Window
    {
        public SendEmail() { InitializeComponent(); }
        private void Cancel_Click(object sender, RoutedEventArgs e) { this.Close(); }
    }
}
