using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DanMuServer
{
    public class ClientInfo
    {
        public string Name { set; get; }
        public Socket Client { set; get; }
        public byte[] Buffer { set; get; }
    }
}
