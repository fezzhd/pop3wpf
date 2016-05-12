using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace pop3wpf
{
    class MailAccept
    {
        private string _serverName;
        private string _password;
        private string _user;
        public static SslStream MailSslStream;
        private byte[] _bufferBytes = new byte[8172];
        public MailAccept(string serverName, string user, string password)
        {
            _serverName = serverName;
            _user = user;
            _password = password;
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
                       int messageCount = messagesAction.GetMessageCount();
                        if (messageCount == -1 || messageCount < 0)
                        { 
                            return;
                        }
                        else
                        {
                            //todo: continue from here
                        }
                    }
                    else
                    {
                        MessageBox.Show(@"Неверный пароль");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(@"Неверный логин");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(@"Ой всё"); 
                return;
            }
        }


    }
}
