using FileProtect.ViewModel;
using System.Windows.Controls;

namespace FileProtect.Pages
{
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }
    }
}
