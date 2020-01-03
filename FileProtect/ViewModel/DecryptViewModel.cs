using FileProtect.Messages;
using FileProtect.Model;
using Ionic.Zip;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

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


        private bool buttonEnabled = true;
        public bool ButtonEnabled
        {
            get
            {
                return buttonEnabled;
            }
            set
            {
                buttonEnabled = value;
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
                return decryptCommand ??
                    (decryptCommand = new RelayCommand(o =>
                    {
                        if (WarningMessage.ShowWarning("Are you sure?", "This file will be decrypted with your preferences") == WarningResultType.Continue)
                        {
                            if (!string.IsNullOrEmpty(xPath) && !string.IsNullOrEmpty(yPath))
                            {
                                if (!string.IsNullOrEmpty(kwfhdks))
                                {
                                    var aastre = App.md5.ComputeHash(Encoding.UTF8.GetBytes(kwfhdks));
                                    string ttart = Convert.ToBase64String(aastre);


                                    if (App.Settings.ASSKOP() != ttart)
                                    {
                                        SystemSounds.Exclamation.Play();
                                        InfoMessage.ShowInfo("WARNING", "Entered password incorrect!");
                                        if (writeLogs)
                                            Logs.WriteLog("WARNING-Entered password incorrect!");
                                        return;
                                    }
                                    else
                                    {
                                        DecryptAsync();
                                    }
                                }
                                else
                                {
                                    SystemSounds.Exclamation.Play();
                                    InfoMessage.ShowInfo("WARNING", "Entered password incorrect!");
                                    if (writeLogs)
                                        Logs.WriteLog("WARNING-Entered password incorrect!");
                                    return;
                                }
                            }
                            else
                            {
                                SystemSounds.Exclamation.Play();
                                InfoMessage.ShowInfo("WARNING", "All fields must be filled!");
                                if (writeLogs)
                                    Logs.WriteLog("WARNING-All fields must be filled!");
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }));
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

        private async Task DecryptAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    if (writeLogs)
                        Logs.WriteLog("Decrypting has been started!");

                    ButtonEnabled = false;

                    string output = $@"{yPath}\{Path.GetFileNameWithoutExtension(xPath)}";

                    Progress = 1;
                    TextProgress = "1/7 part - data reading...(slow)";
                    if (writeLogs)
                        Logs.WriteLog("1/7 part - data reading...");

                    string data = File.ReadAllText(xPath);

                    Progress = 2;
                    TextProgress = "2/7 part - decrypting...(slow)";
                    if (writeLogs)
                        Logs.WriteLog("2/7 part - decrypting...");

                    string newData = new string(data.ToCharArray().Reverse().ToArray());
                    byte[] bytes = Encoding.Default.GetBytes(newData);

                    Progress = 3;
                    TextProgress = "3/7 part - cache creating...(slow)";
                    if (writeLogs)
                        Logs.WriteLog("3/7 part - cache creating...");

                    File.WriteAllBytes($"{output}.zip", bytes);
                    File.SetAttributes($"{output}.zip", FileAttributes.Hidden);

                    try
                    {
                        using (ZipFile zip = ZipFile.Read($"{output}.zip"))
                        {
                            Directory.CreateDirectory(output);

                            Progress = 4;
                            TextProgress = "4/7 part - decrypting...(not fast)";
                            if (writeLogs)
                                Logs.WriteLog("4/7 part - decrypting...");

                            foreach (ZipEntry e in zip)
                            {
                                e.Extract(output, ExtractExistingFileAction.OverwriteSilently);
                            }
                        }

                        Progress = 5;
                        TextProgress = "5/7 part - cache deleting...(very fast)";
                        if (writeLogs)
                            Logs.WriteLog("5/7 part - cache deleting...");

                        File.Delete($"{output}.zip");
                    }
                    catch (Exception ex)
                    {
                        SystemSounds.Hand.Play();

                        Progress = 0;
                        TextProgress = "ERROR-Decrypt uncknown file format!";

                        ButtonEnabled = true;

                        if (writeLogs)
                            Logs.WriteLog("ERROR-Decrypt uncknown file format!");

                        ErrorWriter.WriteError(ex);

                        File.Delete($"{output}.zip");
                        return;
                    }

                    Progress = 6;
                    TextProgress = "6/7 part - deleting old files if you need...(very fast)";
                    if (writeLogs)
                        Logs.WriteLog("6/7 part - deleting old files if you need...");
                    if (oldDataDel)
                    {
                        File.Delete(xPath);
                        if (writeLogs)
                            Logs.WriteLog("Old directory has been deleted!");
                    }

                    Progress = 7;
                    TextProgress = "7/7 part - decrypting complete!";
                    if (writeLogs)
                        Logs.WriteLog("7/7 part - decrypting complete!");

                    if (operationWrite)
                    {
                        string guid = Guid.NewGuid().ToString();
                        List<Operation> list;
                        if (File.Exists($@"{App.MainPath}\File Protect\appcache.json"))
                        {
                            list = OperationManipulator.Read($@"{App.MainPath}\File Protect\appcache.json");
                            Logs.WriteLog($"\"{App.MainPath}\\File Protect\\appcache.json\" has been readed!");
                        }
                        else
                        {
                            list = new List<Operation>();
                        }
                        Operation operation = new Operation(guid, OperationType.Decrypt, DateTime.Now, xPath, output);
                        list.Add(operation);

                        OperationManipulator.Write($@"{App.MainPath}\File Protect\appcache.json", list);
                        Logs.WriteLog($"\"{App.MainPath}\\File Protect\\appcache.json\" has been writed!");
                    }

                    ButtonEnabled = true;

                    if (writeLogs)
                        Logs.WriteLog("Decoding complete!");
                }
                catch (FileNotFoundException)
                {
                    ButtonEnabled = true;

                    Progress = 0;
                    TextProgress = "This file don't exist!";
                }
                catch (Exception ex)
                {
                    ButtonEnabled = true;

                    Progress = 0;
                    TextProgress = "Unknown error. Send files to email in settings page to help fix bugs! thx";

                    ErrorWriter.WriteError(ex);
                }
            });
        }
    }
}
