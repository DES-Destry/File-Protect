using FileProtect.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FileProtect.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private Page mainPage;
        private Page cryptPage;
        private Page decryptPage;
        private Page settingsPage;

        private Page currentPage;
        public Page CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                OnPropertyChanged();
                currentPage = value;
            }
        }

        private double frameOpacity;
        public double FrameOpacity
        {
            get
            {
                return frameOpacity;
            }
            set
            {
                OnPropertyChanged();
                frameOpacity = value;
            }
        }

        public MainViewModel()
        {
            if(!Directory.Exists($"{App.MainPath}"))
            {
                Directory.CreateDirectory($"{App.MainPath}");
                CreateMainFolder();
            }
            else
            {
                if(!Directory.Exists($@"{App.MainPath}\File Protect"))
                {
                    CreateMainFolder();
                }
                else
                {
                    //TODO else logic
                }
            }

            mainPage = new Pages.Main();

            FrameOpacity = 1;
            currentPage = mainPage;
        }

        private void CreateMainFolder()
        {
            Directory.CreateDirectory($@"{App.MainPath}\File Protect");
            File.Create($@"{App.MainPath}\File Protect\0.log").Close();
            using (StreamWriter sw = new StreamWriter($@"{App.MainPath}\File Protect\0.log", true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- First application startup");
                sw.WriteLine($"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- \"{App.MainPath}\\File Protect\" has been created!");
                sw.WriteLine($"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- \"{App.MainPath}\\File Protect\\0.log\" has been created!");
            }

            View.CreatePass createPass = new View.CreatePass();
            createPass.ShowDialog();
        }
    }
}
