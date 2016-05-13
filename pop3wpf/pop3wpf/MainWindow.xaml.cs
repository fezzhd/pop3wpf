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
        private List<string> _mailServerList;
        private MailAccept _mailAccept;

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
            EnterButton.IsEnabled = false;
            _mailAccept = new MailAccept(_mailServerList[MailServer.SelectedIndex], Mail.Text, Password.Password, MainForm, MailListBox);
            if (_mailAccept.IsConected)
            {
                _mailAccept.LogOut();
            }
            MainThread = new Thread(_mailAccept.LogIn);
            MainThread.Start();
        }

        private void MailListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MailListBox.SelectedIndex != -1)
            {
                DeleteButton.IsEnabled = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Messages deleteMessages = new Messages();
            deleteMessages.DeleteMessage(MailListBox.SelectedIndex, _mailAccept.MessageCount);  
            _mailAccept.MailList.RemoveAt(MailListBox.SelectedIndex);                
            MailListBox.Items.Refresh();
            _mailAccept.MessageCount = deleteMessages.GetMessageCount();
        }

        private void MainForm_Closed(object sender, EventArgs e)
        {
            if ((_mailAccept != null) && (_mailAccept.IsConected))
            {
                _mailAccept.LogOut();
            }
        }
    }
}
