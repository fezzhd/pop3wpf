using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pop3wpf
{
    class MailList
    {
        private string _time;
        private string _topic;
        private string _fromWho;


        public MailList(string time, string topic, string fromWho)
        {
            _time = time;
            _topic = topic;
            _fromWho = fromWho;
        }

    }
}
