using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace komunikator.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private readonly string _defaultMyIPAddress = "127.0.0.1";
        #region Properties
        private string _myIPAddress;

        public string myIPAddress
        {
            get
            {
                return SetMyIPAddress();
                //return !string.IsNullOrEmpty(_myIPAddress) ? _myIPAddress : _defaultMyIPAddress;
            }
            set
            {
                value = _defaultMyIPAddress;
            }
        }

        private string SetMyIPAddress()
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
                        return ip.ToString();
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            myIPAddress=myIPAddress;
        }
        #endregion
    }
}
