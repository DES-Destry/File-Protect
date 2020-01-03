using FileProtect.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace FileProtect.View
{
    public partial class CreatePass : Window
    {
        public CreatePass()
        {
            InitializeComponent();
            DataContext = new CreatePassViewModel();
        }

        private void NewPasswordChanged(object sender, RoutedEventArgs e)
        {
            if(DataContext is CreatePassViewModel viewModel && sender is PasswordBox passwordBox)
            {
                viewModel.NewPassword = passwordBox.Password;
            }
        }

        private void RepeatPassworChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreatePassViewModel viewModel && sender is PasswordBox passwordBox)
            {
                viewModel.RepPassword = passwordBox.Password;
            }
        }

        private void OK(object sender, RoutedEventArgs e)
        {
            if (NewPassword.Password == RepeatPassword.Password && NewPassword.Password.Length >= 6)
            {
                Close();
            }
        }
    }
}
