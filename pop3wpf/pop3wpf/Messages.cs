using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace pop3wpf
{
    class Messages
    {
        private byte[] _bufferBytes = new byte[8127];


        public int GetMessageCount()
        {
            int result;
            string data;
            MailAccept.MailSslStream.Write(Encoding.ASCII.GetBytes("STAT\r\n"));
            int bytesRead = MailAccept.MailSslStream.Read(_bufferBytes, 0, _bufferBytes.Length);
            data = Encoding.UTF8.GetString(_bufferBytes, 0, bytesRead);
            if (data.Contains("+OK"))
            {
                data = data.Remove(0, 4);
                string[] parsedArgiments = data.Split(' ');
                Int32.TryParse(parsedArgiments[0], out result);
            }
            else
            {
                result = -1;
            } 
            return result;
        }

        public string[] GetMessageList()
        {
            string result;
            MailAccept.MailSslStream.Write(Encoding.ASCII.GetBytes("LIST\r\n"));
            int bytesRead = MailAccept.MailSslStream.Read(_bufferBytes, 0, _bufferBytes.Length);
            result = Encoding.ASCII.GetString(_bufferBytes, 0, bytesRead);
            if ((bytesRead != 0) && (!result.Contains("\r\n.\r\n")))
            {
                bytesRead = MailAccept.MailSslStream.Read(_bufferBytes, 0, _bufferBytes.Length);
                result += Encoding.ASCII.GetString(_bufferBytes, 0, bytesRead);
            }
            result = result.Replace('\r',' ');
            string[] resultArray = result.Split('\n');
            return resultArray;
        }
    }
}
