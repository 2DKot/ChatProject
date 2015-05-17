using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace ChatServer
{
    public class User
    {
        public string name;
        public TcpClient client;

        public User(TcpClient client, string name)
        {
            this.client = client;
            this.name = name;
        }

        public NetworkStream GetStream()
        {
            return client.GetStream();
        }

        public string GetNextMessage() 
        {
            NetworkStream ns = GetStream();
            byte[] sizeBuffer = new byte[4];
            ns.Read(sizeBuffer, 0, 4);
            int size = BitConverter.ToInt32(sizeBuffer, 0);
            if (size < 1)
            {
                throw new SocketException();
            }
            byte[] messageBuffer = new byte[size];
            ns.Read(messageBuffer, 0, size);
            string message = Encoding.UTF8.GetString(messageBuffer);
            /*if (message.Trim() == "" && !client.Connected)
            {
                throw new SocketException();
            }*/
            return message;
        }

        public void SendMessage(string message)
        {
            //Log.Write("Отправляю: " + message);
            NetworkStream ns = GetStream();
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
            int size = messageBuffer.Count();
            byte[] sizeBuffer = BitConverter.GetBytes(size);
            ns.Write(sizeBuffer, 0, 4);
            ns.Write(messageBuffer, 0, size);
        }  

        public void SendYouAre()
        {
            SendMessage("YOUARE " + name);
        }

        public void SendError(string code)
        {
            SendMessage("ERROR " + code);
        }   
    }
}
