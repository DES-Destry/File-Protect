using FileProtect.ViewModel;
using System.Windows;

namespace FileProtect.View
{
    public partial class UpdateChecker : Window
    {
        public UpdateChecker()
        {
            InitializeComponent();
            DataContext = new UpdateCheckerViewModel();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
