using FileProtect.Model;
using System;
using System.Windows;

namespace FileProtect
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                DataContext = new ViewModel.MainViewModel();
            }
            catch (Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
