using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OgameWrapper.Model
{
    public class Proxy
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public NetworkCredential? Credentials { get; set; }
        public Proxy (string address, int port, NetworkCredential? credentials = null)
        {
            Address = address;
            Port = port;
            Credentials = credentials;
        }
    }
}
