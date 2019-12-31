using FileProtect.ViewModel;
using System.Windows.Controls;

namespace FileProtect.Pages
{
    public partial class Crypt : Page
    {
        public Crypt()
        {
            InitializeComponent();
            DataContext = new EncryptViewModel();
        }

        private void ChangedDATTAH(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is PasswordBox pass && DataContext is EncryptViewModel context)
            {
                context.KWFHDKS = pass.Password;
            }
        }
    }
}
