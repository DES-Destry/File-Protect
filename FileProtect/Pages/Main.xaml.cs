using FileProtect.Model;
using System;
using System.IO;
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
            catch(Exception ex)
            {
                using (StreamWriter sw = new StreamWriter($@"{App.MainPath}\File Protect\0.log", true))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- ERROR-{ex.Message}");
                }
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
