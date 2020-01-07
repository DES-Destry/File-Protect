using System.Windows;

namespace FileProtect.View
{
    public partial class UpdateChecker : Window
    {
        public UpdateChecker() { InitializeComponent(); }
        private void Cancel(object sender, RoutedEventArgs e) { this.Close(); }
    }
}