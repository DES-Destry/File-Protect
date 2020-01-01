using FileProtect.Messages;
using FileProtect.Model;
using Ionic.Zip;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace FileProtect.ViewModel
{
    class EncryptViewModel : BaseViewModel
    {
        private Settings settings;
        private Meta meta;

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


        private string extencion;
        public string Extencion
        {
            get
            {
                return extencion;
            }
            set
            {
                extencion = value;
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


        private string textProgress = "Start encrypting now!";
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
                            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
                            cofd.IsFolderPicker = true;

                            if (writeLogs)
                                Logs.WriteLog("Open file dialog for first encrypt path has been opened");

                            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                XPath = cofd.FileName;

                                if (writeLogs)
                                    Logs.WriteLog("First encrypt path has been changed");
                            }

                            if (File.Exists($@"{xPath}\.meta") && MetaManipulator.Read($@"{xPath}\.meta") is Meta meta)
                            {
                                this.meta = meta;
                                Extencion = meta.GetExtencion();
                                YPath = this.meta.FinalPath;

                                if (writeLogs == true)
                                {
                                    if (writeLogs)
                                        Logs.WriteLog("Folder old meta has been readed!");
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


        private RelayCommand yPathChangeCommand;
        public RelayCommand YPathChangeCommand
        {
            get
            {
                return yPathChangeCommand ??
                    (yPathChangeCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
                            cofd.IsFolderPicker = true;

                            if (writeLogs)
                                Logs.WriteLog("Open file dialog for second encrypt path has been opened");

                            if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
                            {
                                YPath = cofd.FileName;
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }


        private RelayCommand encryptCommand;
        public RelayCommand EncryptCommand
        {
            get
            {
                return encryptCommand ??
                    (encryptCommand = new RelayCommand(obj =>
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(kwfhdks))
                            {
                                var aastre = App.md5.ComputeHash(Encoding.UTF8.GetBytes(kwfhdks));
                                string ttart = Convert.ToBase64String(aastre);


                                if (settings.ASSKOP() != ttart)
                                {
                                    SystemSounds.Exclamation.Play();
                                    InfoMessage.ShowInfo("WARNING", "Entered password incorrect!");
                                    if (writeLogs)
                                        Logs.WriteLog("WARNING-Entered password incorrect!");
                                    return;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(xPath) && !string.IsNullOrEmpty(yPath) && !string.IsNullOrEmpty(extencion))
                                    {
                                        if (writeLogs)
                                            Logs.WriteLog("Encoding starting!");

                                        string fileName = new DirectoryInfo(xPath).Name;
                                        string path = $@"{yPath}\{fileName}";

                                        Directory.CreateDirectory(path);

                                        Progress = 1;
                                        TextProgress = "1/10 part - creating meta(very fast)";
                                        if (writeLogs)
                                            Logs.WriteLog("1/10 part - creating meta");

                                        if (!File.Exists($@"{xPath}\.meta"))
                                        {
                                            this.meta = new Meta($"{extencion}", yPath, App.Version);
                                            MetaManipulator.Write($@"{xPath}\.meta", this.meta);

                                            File.SetAttributes($@"{xPath}\.meta", FileAttributes.Hidden);

                                            if (writeLogs == true)
                                            {
                                                if (writeLogs)
                                                    Logs.WriteLog("New meta file has been writed");
                                            }
                                        }

                                        Progress = 2;
                                        TextProgress = "2/10 part - creating cache(slow)";
                                        if (writeLogs)
                                            Logs.WriteLog("2/10 part - creating cache");

                                        if (string.IsNullOrEmpty(extencion))
                                        {
                                            SystemSounds.Hand.Play();
                                            InfoMessage.ShowInfo("ERROR", "Extencion field must not be empty!");

                                            if (writeLogs)
                                                Logs.WriteLog("ERROR-The extencion field was empty when creating the metadata file containing the file extencion.");
                                            return;
                                        }

                                        EncryptAsync(path);
                                    }
                                    else
                                    {
                                        SystemSounds.Exclamation.Play();
                                        InfoMessage.ShowInfo("WARNING!", "Fill all fields!");

                                        if (writeLogs)
                                            Logs.WriteLog("WARNING-Encrypt page fields not filled!");
                                        return;
                                    }
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
                        catch (Exception ex)
                        {
                            ErrorWriter.WriteError(ex);
                        }
                    }));
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

        public EncryptViewModel()
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

        private async Task EncryptAsync(string path)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        ZipFile zip = new ZipFile();
                        zip.AddDirectory(path);
                        zip.Save($"{path}.zip");

                        File.SetAttributes($"{path}.zip", FileAttributes.Hidden);

                        if (writeLogs)
                            Logs.WriteLog("Cache of encrypt procces has been created");

                        Progress = 3;
                        TextProgress = "3/10 part - rate cache size(fast)";
                        if (writeLogs)
                            Logs.WriteLog("3/10 part - rate cache size");

                        FileInfo info = new FileInfo($"{ path }.zip");
                        long size = info.Length;

                        if (size > 2000000000)
                        {
                            if (writeLogs)
                                Logs.WriteLog("ERROR-Folder should not be more than 2 GB!");

                            Progress = 0;
                            TextProgress = "Encoding error... Try again with smaller file(not be more 2 GB).";
                            File.Delete($"{path}.zip");
                            return;
                        }

                        Progress = 4;
                        TextProgress = "4/10 part - reading...(fast)";
                        if (writeLogs)
                            Logs.WriteLog("4/10 part - reading...");

                        byte[] data = File.ReadAllBytes($"{path}.zip");

                        Progress = 5;
                        TextProgress = "5/10 part - encoding...(not fast)";
                        if (writeLogs)
                            Logs.WriteLog("5/10 part - encoding...");

                        string zipData = Encoding.Default.GetString(data);

                        Progress = 6;
                        TextProgress = "6/10 part - encoding...(not fast)";
                        if (writeLogs)
                            Logs.WriteLog("6/10 part - encoding...");

                        string newData = new string(zipData.ToCharArray().Reverse().ToArray());

                        Progress = 7;
                        TextProgress = "7/10 part - writing...(slow)";
                        if (writeLogs)
                            Logs.WriteLog("7/10 part - writing...");

                        File.Create($"{path}.{extencion}").Close();
                        File.WriteAllText($"{path}.{extencion}", newData);

                        Progress = 8;
                        TextProgress = "8/10 part - deleting cache(fast)";
                        if (writeLogs)
                            Logs.WriteLog("8/10 part - deleting cache");

                        File.Delete($"{path}.zip");

                        if (writeLogs)
                            Logs.WriteLog("Cache of encrypt procces has been deleted!");

                        Progress = 9;
                        TextProgress = "9/10 part - deleting old file if you want(fast)";
                        if (writeLogs)
                            Logs.WriteLog("9/10 part - deleting old file if you want");

                        if (oldDataDel)
                        {
                            Directory.Delete(xPath);
                            if (writeLogs)
                                Logs.WriteLog("Old directory has been deleted!");
                        }

                        zip.Dispose();

                        Progress = 10;
                        TextProgress = "10/10 part - encoding complete!";

                        if (writeLogs)
                            Logs.WriteLog("Encoding complete!");
                    }
                    catch(Exception ex)
                    {
                        ErrorWriter.WriteError(ex);
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorWriter.WriteError(ex);
            }
        }
    }
}
