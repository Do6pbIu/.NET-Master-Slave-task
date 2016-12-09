using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterSlave
{
    public struct Address
    {
        private readonly string _hostName;
        private readonly int _port;

        public string HostName { get { return _hostName; } }
        public int Port { get { return _port; } }

        public Address(string host, int port)
        {
            _hostName = host;
            _port = port;
        }
    }
}
