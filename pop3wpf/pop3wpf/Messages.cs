using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pop3wpf
{
    class Messages
    {
        byte[] bufferBytes = new byte[8127];


        public int GetMessageCount()
        {
            string data;
            MailAccept.MailSslStream.Write(Encoding.ASCII.GetBytes("STAT\r\n"));
            int bytesRead = MailAccept.MailSslStream.Read(bufferBytes, 0, bufferBytes.Length);
            data = Encoding.UTF8.GetString(bufferBytes, 0, bytesRead);
            if (data.Contains("+OK"))
            {
                data = data.Remove(0, 4);
                string[] parsedArgiments = data.Split(' ');
                int result; 
                Int32.TryParse(parsedArgiments[0], out result);
                return result;
            }
            else
            {
                return -1;
            }
        }
    }
}
