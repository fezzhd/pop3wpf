using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pop3wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Thread MainThread;
        List<string> _mailServerList;
        public MainWindow()
        {
            InitializeComponent();
            _mailServerList = new List<string>
            {
                "pop.mail.ru",
                "pop.gmail.com"
            };
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            MailAccept mailAccept = new MailAccept(_mailServerList[MailServer.SelectedIndex], Mail.Text, Password.Password);
            MainThread = new Thread(mailAccept.LogIn);
            MainThread.Start();
        }
    }
}
