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
using System.Windows.Shapes;

namespace komunikator
{
    /// <summary>
    /// Interaction logic for TCPServer.xaml
    /// </summary>
    public partial class TCPServer : Window
    {
        //pola prywatne
        private TcpListener serwer;
        private TcpClient klient;

        public TCPServer()
        {
            InitializeComponent();
            IPAddress[] LocalIPS = GetLocalIPs();
            foreach (IPAddress ip in LocalIPS)
            {
                listBox.Items.Add(ip.ToString());
            }
            tbAdresSerwera.Text = LocalIPS[0].ToString();
        }

        private IPAddress[] GetLocalIPs()
        {
            List<IPAddress> list = new List<IPAddress>();
            IPAddress[] AdresIPSerwera = null;
            IPHostEntry host = null;
            try
            {
                host = Dns.GetHostEntry(Dns.GetHostName());
            }
            catch
            {
                return null;
            }
            foreach (IPAddress IP in host.AddressList)
            {
                if (IP.AddressFamily == AddressFamily.InterNetwork)
                {
                    list.Add(IPAddress.Parse(IP.ToString()));
                }
            }
            AdresIPSerwera = list.ToArray();
            return AdresIPSerwera;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {


            int port = 20001;
            int i = 0;
            try
            {
                //AdresIPSerwera = IPAddress.Parse(localIP.ToString());
                //AdresIPSerwera = ((IPEndPoint)s.LocalEndPoint).Address.ToString())
                //serwer = new TcpListener(AdresIPSerwera, port);
                serwer = new TcpListener(IPAddress.Parse("192.168.0.104"), port);
                serwer.Start();
                klient = serwer.AcceptTcpClient();
                IPEndPoint IP = (IPEndPoint)klient.Client.RemoteEndPoint;
                listBox.Items.Add(IP.ToString() + " Nawiązano połączenie");
            }
            catch
            {
                listBox.Items.Add("Coś poszło nie tak.");
            }
        }
    }
}
