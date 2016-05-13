using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pop3wpf
{
    class MailList
    {
        public string _time { get; set; }
        public string _fromWho { get; set; }


        public MailList(string time, string fromWho)
        {
            _time = time;
            _fromWho = fromWho;
        }

    }
}
