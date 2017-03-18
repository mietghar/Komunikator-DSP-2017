﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace komunikator.Models
{
    public class Server : IServer
    {
        #region Properties

        public int Port { get; set; }
        public IPAddress IpAddress { get; set; }
        private TcpListener server { get; set;}
        private IPEndPoint ClientIP = null;
        public bool IsStarted {get; private set; }
        public bool PortInUse { get; private set; }

        #endregion

        #region Constructors

        public Server(int port, IPAddress ipAddress)
        {
            Port = port;
            IpAddress = ipAddress;
            server = new TcpListener(IpAddress,Port);
        }

        #endregion

        #region Methods

        public bool ValidateServer(int port, IPAddress ipAddress)
        {
            //Checking if current port is already in use
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Address==ipAddress && tcpi.LocalEndPoint.Port == port)
                {
                    PortInUse = true;
                    break;
                }
            }
            //Validation of port and ipaddress
            if (Port < 0 || Port > 65535)
            {
                return false;
                throw new Exception("Error: You cannot connect on this port.");
            }
            if (PortInUse)
            {
                return false;
                throw new Exception("Port is in use.");
            }
            if (IpAddress == null)
            {
                return false;
                throw new Exception("Error: You cannot connect on this IP.");
            }
                return true;
        }

        public void Start()
        {
            try { 
            if (ValidateServer(Port, IpAddress))
            {
                server.Start();
                HasStarted();
            }
            else
            {
                throw new Exception("Server cannot be started.");
            }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void HasStarted()
        {
            IsStarted = true;
            PortInUse = true;
        }

        public void HasStopped()
        {
            IsStarted = false;
            PortInUse = false;
        }

        public void ProcessConnection()
        {
            //New thread for endless while loop
            Thread listener = new Thread(delegate ()
            {
                try
                {

                    // Buffer for reading data
                    Byte[] bytes = new Byte[256];
                    String data = null;

                    while (IsStarted)
                    {
                        MessageBox.Show("Waiting for a connection.");
                        //Performing await for a connection
                        TcpClient client = server.AcceptTcpClient();
                        ClientIP = (IPEndPoint)client.Client.RemoteEndPoint;
                        MessageBox.Show("Connected from IP: " + ClientIP.ToString() + " .");
                        data = null;
                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        int i;

                        // Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            MessageBox.Show("Received: {0}", data);

                            /*// Process the data sent by the client.
                            data = data.ToUpper();

                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                            Console.WriteLine("Sent: {0}", data);*/
                        }


                        client.Close();
                    }
                }
                catch (SocketException sex)
                {
                    MessageBox.Show("Server has stopped.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    //server.Stop();
                }
                System.Windows.Threading.Dispatcher.Run();
            });
            listener.SetApartmentState(ApartmentState.STA);
            //listener.Priority = ThreadPriority.Highest;
            //przeniesienie wątku do tła, dzięki temu przy zamykaniu aplikacji zostanie on automatycznie zamknięty
            listener.IsBackground = true;
            listener.Start();
        }

        public void Stop()
        {
            if (IsStarted) { 
            server.Stop();
            HasStopped();
            }
        }

        

        #endregion
    }
}