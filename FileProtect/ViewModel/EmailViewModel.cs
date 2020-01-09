using DESTRY.Net.Emails;
using FileProtect.Model;
using System;
using System.Windows.Media;

namespace FileProtect.ViewModel
{
    class EmailViewModel : BaseViewModel
    {
        private readonly string head = "Help! Errors in \"File Protect\" application!";
        private SupportMessage message;

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
                        try
                        {
                            message = new SupportMessage(head, from, comment);
                            message.SendAsync();
                        }
                        catch (Exception ex)
                        {
                            StateColor = Brushes.Red;
                            State = "Sending ERROR";
                            ButtonEnabled = true;
                            ErrorWriter.WriteError(ex);
                        }
                    }));
            }
        }

        public EmailViewModel()
        {
            message = new SupportMessage();

            message.OnMailSendingStarted += Message_OnMailSendingStarted;
            message.OnMailSendingEnded += Message_OnMailSendingEnded;
        }

        private void Message_OnMailSendingStarted(object obj)
        {
            stateColor = Brushes.White;
            State = "Sending...";
            ButtonEnabled = false;
        }

        private void Message_OnMailSendingEnded(object obj)
        {
            StateColor = Brushes.LightGreen;
            State = "Email sended!";
            ButtonEnabled = true;
        }
    }
}
