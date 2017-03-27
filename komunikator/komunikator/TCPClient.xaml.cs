using komunikator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
            ClientListener();
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
            SendMessage();
        }

        delegate void SetTextCallBack(string tekst);
        private void SetText(string tekst)
        {
            try { 
            if (!TalkTextBox.CheckAccess())
            {
                SetTextCallBack f = new SetTextCallBack(SetText);
                Dispatcher.Invoke(f, new object[] { tekst });
            }
            else
            {
                this.TalkTextBox.Text = this.TalkTextBox.Text+"\n"+tekst;
            }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void ClientListener()
        {
            Thread clientListener = new Thread(delegate()
            {
                try
                {
                    Byte[] bytes = new Byte[256];
                    while (true)
                    {
                        IPEndPoint zdalnyIP = new IPEndPoint(IPAddress.Any, 0);
                        UdpClient serwer = new UdpClient(13000);
                        Byte[] odczyt = serwer.Receive(ref zdalnyIP);
                        string dane = System.Text.Encoding.Unicode.GetString(odczyt);
                        this.SetText(dane);
                        serwer.Close();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            });
            clientListener.IsBackground = true;
            clientListener.Start();
        }

        private void SendMessage()
        {
            string username = Properties.Settings.Default["UserNameSetting"].ToString();
            string password = Properties.Settings.Default["UserPasswordSetting"].ToString();
            string usermessage = username +";+;"+ password + ";+;" + messageTextBox.Text;
            string message = messageTextBox.Text;
            int port = 0;
            port = (int)Properties.Settings.Default["ClientPortSetting"];
            string host = "";
            host = (string)Properties.Settings.Default["HostIPSetting"];
            try
            {
                client = new Client();

                client.Connect(port, IPAddress.Parse(host));
                client.SendMessage(usermessage);
                TalkTextBox.Text = TalkTextBox.Text + "\nMe: " + message;
                messageTextBox.Text = "";
                client.Disconnect();
            }
            catch (TimeoutException toex)
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

        private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }
    }
}
