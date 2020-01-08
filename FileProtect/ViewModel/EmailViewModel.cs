using FileProtect.Model;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FileProtect.ViewModel
{
    class EmailViewModel : BaseViewModel
    {
        private SmtpClient client = new SmtpClient("smtp.mail.ru", 587);
        private string to = "destry_callback0001@mail.ru";

        private string from;
        public string From
        {
            get
            {
                return from;
            }
            set
            {
                from = value;
                OnPropertyChanged();
            }
        }


        private string comment;
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
                OnPropertyChanged();
            }
        }


        private string state = "Send email please, help support app";
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                OnPropertyChanged();
            }
        }


        private Brush stateColor = Brushes.White;
        public Brush StateColor
        {
            get
            {
                return stateColor;
            }
            set
            {
                stateColor = value;
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


        private RelayCommand sendCommand;
        public RelayCommand SendCommand
        {
            get
            {
                return sendCommand ??
                    (sendCommand = new RelayCommand(obj =>
                    {
                        SendAsync();
                    }));
            }
        }

        private async void SendAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(from))
                    {
                        from = "Anonymous";
                    }

                    State = "Sending...";
                    ButtonEnabled = false;

                    client.Credentials = new NetworkCredential(to, ",PPUToai5tu2");
                    client.EnableSsl = true;
                    client.Timeout = 60000;

                    MailMessage message = new MailMessage();
                    message.To.Add(to);
                    message.From = new MailAddress(to);
                    message.Subject = "Help! Errors in \"File Protect\" application!";

                    if (!string.IsNullOrEmpty(comment))
                    {
                        message.Body = $"{from}: {comment}";
                    }
                    else
                    {
                        message.Body = $"Files without comment from: {from}";
                    }

                    string[] files = Directory.GetFiles($@"{App.MainPath}\File Protect");
                    foreach (string file in files)
                    {
                        Attachment attachment = new Attachment(file);
                        message.Attachments.Add(attachment);
                    }

                    client.Send(message);
                    client.Dispose();

                    StateColor = Brushes.LightGreen;
                    State = "Email sended!";
                    ButtonEnabled = true;
                }
                catch (Exception ex)
                {
                    StateColor = Brushes.Red;
                    State = "Sending ERROR";
                    ButtonEnabled = true;
                    ErrorWriter.WriteError(ex);
                    return;
                }
            });
        }
    }
}
