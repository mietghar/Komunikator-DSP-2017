using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace komunikator.Models
{
    class Client : IClient
    {
        #region Properties
        public int Port { get; set; }
        public IPAddress IpAddress { get; set; }

        private TcpClient client { get; set; }

        #endregion

        #region Constructors

        public Client()
        {
            client = new TcpClient();
        }

        #endregion

        #region Methods

        public void Connect(int port, IPAddress ipAddress)
        {
            Port = port;
            IpAddress = ipAddress;
            client.Connect(IpAddress.ToString(), Port);
        }

        public void Disconnect()
        {
            client.Close();
        }

        public void SendMessage(string message)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

            NetworkStream stream = client.GetStream(); 
            
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);
        }

        #endregion
    }
}
