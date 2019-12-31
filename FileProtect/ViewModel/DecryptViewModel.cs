using FileProtect.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;

namespace FileProtect.ViewModel
{
    class DecryptViewModel : BaseViewModel
    {
        private Settings settings;

        private string xPath = "";
        public string XPath
        {
            get
            {
                return xPath;
            }
            set
            {
                xPath = value;
                OnPropertyChanged();
            }
        }


        private string yPath = "";
        public string YPath
        {
            get
            {
                return yPath;
            }
            set
            {
                yPath = value;
                OnPropertyChanged();
            }
        }


        private bool oldDataDel;
        public bool OldDataDel
        {
            get
            {
                return oldDataDel;
            }
            set
            {
                oldDataDel = value;
                OnPropertyChanged();
            }
        }


        private bool writeLogs;
        public bool WriteLogs
        {
            get
            {
                return writeLogs;
            }
            set
            {
                writeLogs = value;
                OnPropertyChanged();
            }
        }


        private bool operationWrite;
        public bool OperationWrite
        {
            get
            {
                return operationWrite;
            }
            set
            {
                operationWrite = value;
                OnPropertyChanged();
            }
        }


        private string kwfhdks;
        public string KWFHDKS
        {
            get
            {
                return kwfhdks;
            }
            set
            {
                kwfhdks = value;
                OnPropertyChanged();
            }
        }


        private int progress;
        public int Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
                OnPropertyChanged();
            }
        }


        private string textProgress = "Start decrypting now!";
        public string TextProgress
        {
            get
            {
                return textProgress;
            }
            set
            {
                textProgress = value;
                OnPropertyChanged();
            }
        }


        private RelayCommand copyCommand;
        public RelayCommand CopyCommand
        {
            get
            {
                return copyCommand ??
                    (copyCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (xPath != null)
                            {
                                string[] paths = xPath.Split('\\');
                                if (paths.Length > 1)
                                {
                                    string newPath = xPath.Replace($@"\{paths[paths.Length - 1]}", "");
                                    YPath = newPath;

                                    if (writeLogs)
                                        Logs.WriteLog("Path has been copied from main folder");
                                }
                                else
                                {
                                    YPath = "error";

                                    if (writeLogs)
                                        Logs.WriteLog("First path has been incorrect");

                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        private RelayCommand xPathChangeCommand;
        public RelayCommand XPathChangeCommand
        {
            get
            {
                return xPathChangeCommand ??
                    (xPathChangeCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog();
                            fileDialog.IsFolderPicker = false;

                            if (writeLogs)
                                Logs.WriteLog("Open file dialog for first decrypt path has been opened");

                            if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                XPath = fileDialog.FileName;

                                if (writeLogs)
                                    Logs.WriteLog("First decrypt path has been changed");
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        private RelayCommand yPathChangeCommand;
        public RelayCommand YPathChangeCommand
        {
            get
            {
                return yPathChangeCommand ??
                    (yPathChangeCommand = new RelayCommand(o =>
                    {
                        try
                        {
                            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog();
                            fileDialog.IsFolderPicker = true;

                            if (writeLogs)
                                Logs.WriteLog("Open file dialog for second decrypt path has been opened");
                            if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                YPath = fileDialog.FileName;

                                if (writeLogs)
                                    Logs.WriteLog("Second decrypt path has been changed");
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        private RelayCommand decryptCommand;
        public RelayCommand DecryptCommand
        {
            get
            {
                return decryptCommand;
            }
        }


        public DecryptViewModel()
        {
            try
            {
                if (SettingsManipulator.Read($@"{App.MainPath}\File protect\appsettings.json") is Settings json)
                {
                    settings = json;
                    Logs.WriteLog("Crypt settings has been readed!");
                }

                this.OldDataDel = settings.DelDataBefOperation;
                this.WriteLogs = settings.WriteLogs;
                this.OperationWrite = settings.SaveCache;
                Logs.WriteLog("Settings has been applied!");
            }
            catch (Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
