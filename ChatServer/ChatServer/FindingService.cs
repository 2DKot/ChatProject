using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatServer
{
    class FindingService
    {
        int port = 667;
        bool stopped = false;
        string serverName;
        UdpClient udpClient;
        public void Start(string serverName)
        {
            this.serverName = serverName;
            Thread recThread = new Thread(RecieveRequests);
            recThread.Start();
        }

        void RecieveRequests()
        {
            udpClient = new UdpClient(port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, port);
            while (!stopped)
            {
                try
                {
                    byte[] bytes = udpClient.Receive(ref groupEP);
                    byte[] msg = Encoding.UTF8.GetBytes("IAMSERV " + serverName);
                    udpClient.Send(msg, msg.Count(), groupEP);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (SocketException)
                {
                    break;
                }
            } 
        }

        public void Stop()
        {
            stopped = true;
            udpClient.Close();
        }
    }
}
