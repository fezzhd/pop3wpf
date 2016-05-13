using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

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
                int.TryParse(parsedArgiments[0], out result);
                return result;
            }
            else
            {
                return -1;
            }
        }

        public List<MailList> GetMessageList(int index)
        {
            List<MailList> currentMailList = new List<MailList>();
            for (int tempIndex = index; tempIndex > 0; tempIndex--)
            {
                currentMailList.Add(GetMessageInfo(tempIndex));
            }
            return currentMailList;
        }


        private MailList GetMessageInfo(int index)
        {
            string result = "";
            MailAccept.MailSslStream.Write(Encoding.ASCII.GetBytes("TOP " + index + " 0\r\n"));
            int bytesRead = -1;
            while ((bytesRead != 0) && (!result.Contains("\r\n.\r\n")))
            {
                bytesRead = MailAccept.MailSslStream.Read(bufferBytes, 0, bufferBytes.Length);
                result += Encoding.ASCII.GetString(bufferBytes, 0, bytesRead);
            }
            result = result.Replace('\r', ' ');
            string[] resultArray = result.Split('\n');
            return new MailList(resultArray[8], resultArray[2].Replace("Return-path: ", ""));
        }

        public void PrintList(List<MailList> list, ListBox listBox)
        {
            listBox.ItemsSource = list;
        }
    }
}
