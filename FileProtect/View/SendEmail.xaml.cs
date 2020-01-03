using FileProtect.ViewModel;
using System.Windows;

namespace FileProtect.View
{
    public partial class SendEmail : Window
    {
        public SendEmail()
        {
            InitializeComponent();
            DataContext = new EmailViewModel();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
