using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ChatClient
{
    public interface ITcpClient
    {
        void Connect(IPEndPoint remoteEP);
        void Close();
        byte[] Read(int offset, int size);
        void Write(byte[] buffer, int offset, int size);
        bool IsConnected();
    }
    //КЛАСС БЕЗ ТЕСТОВ - ОН ПРОСТО АДАПТИРУЕТ ВСТРОЕННЫЕ МЕТОДЫ TCP-КЛИЕНТА
    public class MTcpClient : ITcpClient
    {
        TcpClient tcpClient;
        public MTcpClient(TcpClient client)
        {
            tcpClient = client;
        }
        public void Connect(IPEndPoint remoteEP)
        {
            tcpClient.Connect(remoteEP);
        }
        public void Close()
        {
            tcpClient.Close();
            tcpClient = new TcpClient();
        }

        public byte[] Read(int offset, int size)
        {
            byte[] buffer = new byte[size];
            tcpClient.GetStream().Read(buffer, offset, size);
            return buffer;
        }

        public void Write(byte[] buffer, int offset, int size)
        {
            tcpClient.GetStream().Write(buffer, offset, size);
        }
        public bool IsConnected()
        {
            return tcpClient.Connected;
        }
    }
}
