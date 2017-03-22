using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace komunikator.Models
{
    interface IServer
    {
        int Port { get; set; }
        IPAddress IpAddress { get; set; }
        bool IsStarted { get;}

        void Start();
        void ProcessConnection();
        void Stop();
    }
    
    interface IClient
    {
        int Port { get; set; }
        IPAddress IpAddress { get; set; }

        void Connect(int port, IPAddress ipAddress);
        void SendMessage(string message);
        void Disconnect();
    }
}
