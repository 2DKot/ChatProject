using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace ChatServer
{
    class User
    {
        public string name;
        public TcpClient client;
        public User(TcpClient client)
        {
            this.client = client;
        }

        public NetworkStream GetStream()
        {
            return client.GetStream();
        }
    }
}
