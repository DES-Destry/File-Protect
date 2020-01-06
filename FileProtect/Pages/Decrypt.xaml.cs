using FileProtect.ViewModel;
using System.Windows.Controls;

namespace FileProtect.Pages
{
    public partial class Decrypt : Page
    {
        public Decrypt()
        {
            InitializeComponent();
        }

        private void ChangedGGDAFR(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is PasswordBox pass && DataContext is DecryptViewModel context)
            {
                context.KWFHDKS = pass.Password;
            }
        }
    }
}
