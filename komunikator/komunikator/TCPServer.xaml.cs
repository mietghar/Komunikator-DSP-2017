using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            TcpListener serwer = null;
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
            //int i = 0;
            try
            {
                //AdresIPSerwera = IPAddress.Parse(localIP.ToString());
                //AdresIPSerwera = ((IPEndPoint)s.LocalEndPoint).Address.ToString())
                //serwer = new TcpListener(AdresIPSerwera, port);

                Thread nasluch = new Thread(delegate ()
                {
                    //MainWindow Klient = new MainWindow();
                    //Klient.Show();


        serwer = new TcpListener(IPAddress.Parse(listBox.Items.GetItemAt(0).ToString()), port);
                    serwer.Start();
                    // Buffer for reading data
                    Byte[] bytes = new Byte[256];
                    String data = null;
                    // Enter the listening loop.
                    MessageBox.Show("Czekam na połączenie.");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    klient = serwer.AcceptTcpClient();

                    MessageBox.Show("Połączono.");
                    //Console.WriteLine("Połączono!");
                    //while (true)
                    //{
                    //MessageBox.Show("Czekam na połączenie.");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    //klient = serwer.AcceptTcpClient();
                    //Console.WriteLine("Połączono!");

                    data = null;

                    // Get a stream object for reading and writing
                    /*NetworkStream stream = klient.GetStream();

                    int i = 0;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //listBox.Items.Add("Klient: " + data);
                        MessageBox.Show("Odebrano: ", data);

                        // Process the data sent by the client.
                        //data = data.ToUpper();

                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        //stream.Write(msg, 0, msg.Length);
                        //MessageBox.Show("Wysłano: {0}", data);
                    }

                    // Shutdown and end connection
                    //klient.Close();
                    //}*/
                    IPEndPoint IP = (IPEndPoint)klient.Client.RemoteEndPoint;
                    listBox.Items.Add(IP.ToString() + " Nawiązano połączenie");
                    MessageBox.Show(IP.ToString() + " Nawiązano połączenie");


                    System.Windows.Threading.Dispatcher.Run();
                });
                nasluch.SetApartmentState(ApartmentState.STA);
                //przeniesienie wątku do tła, dzięki temu przy zamykaniu aplikacji zostanie on automatycznie zamknięty
                nasluch.Priority = ThreadPriority.Highest;
                nasluch.IsBackground = true;
                nasluch.Start();


            }
            catch
            {
                listBox.Items.Add("Coś poszło nie tak.");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                serwer.Stop();
            }
            catch
            {
                MessageBox.Show("Coś poszło nie tak przy zamykaniu połączenia.");
            }
        }
    }
}
