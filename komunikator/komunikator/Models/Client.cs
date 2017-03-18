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

        #endregion
    }
}
