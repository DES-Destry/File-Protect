using FileProtect.Model;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FileProtect.Pages
{
    public partial class Main : Page
    {
        public Main()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
