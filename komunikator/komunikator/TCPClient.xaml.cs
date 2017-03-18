using komunikator.Models;
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

        public TcpClient klient = new TcpClient();

        IPAddress adresIPHost;
        IPAddress adresIPClient;
        IClient client;

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

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                if (klient.Connected)
                {
                    //klient.Client.Disconnect(true);
                    klient.Close();
                }
                else
                {
                    klient.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Błąd, próba zamknięcia połączenia z serwerem nieudana. " +ex.ToString());
            }
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            int port = 0;
            port = (int)Properties.Settings.Default["ClientPortSetting"];
            string host = "";
            host = (string)Properties.Settings.Default["ServerIPSetting"];
            try { 
            client = new Client();
            
                client.Connect(port, IPAddress.Parse(host));
                client.Disconnect();
            }
            catch(TimeoutException toex)
            {
                MessageBox.Show(toex.Message.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            /*            
            try
            {
                if (klient.Connected!=true) { 
                klient.Connect(host, port);
                EventsListBox.Items.Add("Nawiązano połącznie z serwerem.");
                }
            }
            catch (Exception ex)
            {
                EventsListBox.Items.Add("Błąd" + ex.ToString());
                //MessageBox.Show("Błąd." +ex.ToString());
            }
            */
            /*
            try
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(messageTextBox.Text);
                NetworkStream stream = klient.GetStream();
                stream.Write(data, 0, data.Length);
                EventsListBox.Items.Add("Ja: " + messageTextBox.Text);
                messageTextBox.Text = "";
                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                //data = new Byte[256];
                // String to store the response ASCII representation.
                //String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                //Int32 bytes = stream.Read(data, 0, data.Length);
                //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                //EventsListBox.Items.Add(responseData);
                //MessageBox.Show("Received: {0}", responseData);
                stream.Close();
            }
            catch
            {
                MessageBox.Show("Coś poszło nie tak przy streamowaniu.");
            }*/
        }
    }
}
