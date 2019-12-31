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
                MainImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\images\\WhiteHomePage.png"));
            }
            catch (Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
