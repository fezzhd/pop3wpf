using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace pop3wpf
{
    class MailAccept
    {
        private string _serverName;
        private string _password;
        private string _user;
        public static SslStream MailSslStream;
        private byte[] _bufferBytes = new byte[8172];
        public List<MailList> MailList { get; set; }
        private MainWindow _window;
        private ListBox _box;
        public int MessageCount { get; set; }
        public bool IsConected { get; set; }

        public MailAccept(string serverName, string user, string password, MainWindow window, ListBox box)
        {
            _serverName = serverName;
            _user = user;
            _password = password;
            _window = window;
            _box = box;
        }


        public void LogIn()
        {
            try
            {
                TcpClient mailClient = new TcpClient(_serverName, 995);
                MailSslStream = new SslStream(mailClient.GetStream());
                MailSslStream.AuthenticateAsClient(_serverName);
                int readedBytes = MailSslStream.Read(_bufferBytes, 0, _bufferBytes.Length);
                if (Encoding.ASCII.GetString(_bufferBytes, 0, readedBytes).Contains("+OK"))
                {
                    IsConected = true;
                    CheckLogIn(); 
                }
                else
                {
                    MessageBox.Show(@"Ошибка сервера");
                    return;
                }
            }
            catch (AuthenticationException)
            {
                MessageBox.Show(@"Ошибка аутентификации сервера");
            }
        }


        private void CheckLogIn()
        {
            try
            {
                MailSslStream.Write(Encoding.ASCII.GetBytes("USER " + _user + "\r\n"));
                int bytesRead =  MailSslStream.Read(_bufferBytes, 0, _bufferBytes.Length);
                string data = Encoding.UTF8.GetString(_bufferBytes, 0, bytesRead);
                if (data.Contains("+OK"))
                {
                    MailSslStream.Flush();
                    MailSslStream.Write(Encoding.ASCII.GetBytes("PASS " + _password + "\r\n"));
                    bytesRead = MailSslStream.Read(_bufferBytes, 0, _bufferBytes.Length);
                    data = Encoding.UTF8.GetString(_bufferBytes, 0, bytesRead);
                    if (data.Contains("+OK"))
                    {
                       Messages messagesAction = new Messages();
                       MessageCount = messagesAction.GetMessageCount();
                        if (MessageCount < 0)
                        { 
                            return;
                        }
                        else
                        {
                            MailList = new List<MailList>();
                            //todo: continue from here
                            MailList = messagesAction.GetMessageList(MessageCount);
                            _window.Dispatcher.Invoke(new ThreadStart(delegate
                            {
                                messagesAction.PrintList(MailList, _box);
                                _window.EnterButton.IsEnabled = true;
                            }));
                 
                        }
                    }
                    else
                    {
                        MessageBox.Show(@"Неверный пароль");
                    }
                }
                else
                {
                    MessageBox.Show(@"Неверный логин");
                }
            }
            catch (Exception)
            {
                MessageBox.Show(@"Ой всё"); 
            }
            MainWindow.MainThread.Abort();
        }


        public void LogOut()
        {
            MailSslStream.Write(Encoding.ASCII.GetBytes("QUIT\r\n"));
            int bytesRead = MailSslStream.Read(_bufferBytes, 0, _bufferBytes.Length);
            string result = Encoding.ASCII.GetString(_bufferBytes, 0, bytesRead);
            if (!result.Contains("+OK"))
            {
                MessageBox.Show(result);
            }
        }


    }
}
