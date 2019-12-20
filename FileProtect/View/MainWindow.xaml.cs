﻿using FileProtect.Model;
using System;
using System.IO;
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
                using (StreamWriter sw = new StreamWriter($@"{App.MainPath}\File Protect\0.log", true))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")} -- ERROR-{ex.Message}");
                }
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
