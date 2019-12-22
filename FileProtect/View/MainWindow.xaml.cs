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
                DataContext = new ViewModel.MainViewModel();
                InitializeComponent();
            }
            catch (Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
