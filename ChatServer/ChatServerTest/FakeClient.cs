using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using NUnit.Framework;

namespace ChatServerTest
{
    class FakeClient
    {
        public TcpClient client = new TcpClient();

        public void Connect()
        {
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 666));
        }

        public void SendMessage(string message)
        {
            NetworkStream ns = client.GetStream();
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
            int size = messageBuffer.Count();
            byte[] sizeBuffer = BitConverter.GetBytes(size);
            ns.Write(sizeBuffer, 0, 4);
            ns.Write(messageBuffer, 0, size);
        }

        public void SendBytes(byte[] bytes)
        {
            NetworkStream ns = client.GetStream();
            ns.Write(bytes, 0, bytes.Count());
        }

        public string RecieveMessage()
        {
            NetworkStream ns = client.GetStream();
            byte[] sizeBuffer = new byte[4];
            try
            {
                ns.Read(sizeBuffer, 0, 4);
            }
            catch (IOException)
            {
                throw new SocketException();
            }
            int size = BitConverter.ToInt32(sizeBuffer, 0);
            if (size < 1)
            {
                throw new SocketException();
            }
            byte[] messageBuffer = new byte[size];
            ns.Read(messageBuffer, 0, size);
            string message = Encoding.UTF8.GetString(messageBuffer);
            return message;
        }

        public void SkipFirstMessages()
        {
            RecieveMessage();
            RecieveMessage();
            RecieveMessage();
            RecieveMessage();
        }

        public void SendAndRecieve(string send, string recieve)
        {
            SendMessage(send);
            string recMsg = RecieveMessage();
            Assert.AreEqual(recieve, recMsg);
        }
    }
}
