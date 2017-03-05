using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

namespace komunikator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetMyIPAddress();
        }

        IPAddress adresIPHost;
        IPAddress adresIPClient;

        private void SetMyIPAddress()
        {
            //pobranie mojej nazwy komputera - hosta
            try
            {
                IPHostEntry MyComputer = Dns.GetHostEntry(Dns.GetHostName());
                //odczytanie mojego ip na podstawie nazwy hosta
                foreach (var ip in MyComputer.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                         MyIPAddress.Text=ip.ToString();
                    }
                }
            }
            catch
            {
            }
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            string host = myPartnerAddressTextBox.Text;
            int port = 20001;
            try
            {
                TcpClient klient = new TcpClient(host, port);
                EventsListBox.Items.Add("Nawiązano połącznie z serwerem.");
                klient.Close();
            }
            catch(Exception ex)
            {
                EventsListBox.Items.Add("Błąd" + ex.ToString());
                //MessageBox.Show("Błąd." +ex.ToString());
            }
        }
    }
}
