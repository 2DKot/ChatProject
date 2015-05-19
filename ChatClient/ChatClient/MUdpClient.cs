using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ChatClient
{
    public interface IUdpClient
    {
        int ReceivePort
        {
            get;
        }
        void Send(byte[] dgram, int bytes, IPEndPoint endPoint);
        //Null'ится после каждого рассоединения
        bool IsClientNull();
        void InitializeUdpClient();
        byte[] Receive(ref IPEndPoint remoteEP);
        void Close();
    }
    public class MUdpClient : IUdpClient
    {
        private UdpClient client;
        private int receivePort;
        public MUdpClient(int receivePort)
        {
            this.receivePort = receivePort;
            InitializeUdpClient();

        }
        public void InitializeUdpClient()
        {
            this.client = new UdpClient();
            this.client.EnableBroadcast = true;
        }
        public int ReceivePort
        {
            get
            {
                return receivePort;
            }
        }
        public bool IsClientNull()
        {
            if (client.Client == null)
            {
                return true;
            }
            return false;
        }
        public void Send(byte[] dgram, int bytes, IPEndPoint endPoint)
        {
            this.client.Send(dgram, bytes, endPoint);
        }
        public byte[] Receive(ref IPEndPoint remoteEP)
        {
            return this.client.Receive(ref remoteEP);
        }
        public void Close()
        {
            client.Close();
        }
    }
}
