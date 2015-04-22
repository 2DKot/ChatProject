using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatClient
{
    public class SearcherServers
    {
        public const int transPort = 667;
        public const int recievPort = 668;
        public static readonly string command = "FINDSERVER";
        public static readonly string serverCommand = "IAMSERV";
        public IPEndPoint remoteIEP;
        public static Dictionary<string, IPEndPoint> findedIEPs;
        private Thread receivingBroadcastMessagesThread;
        public bool findingStatus;
        private UdpClient commonClient;

        public SearcherServers()
        {
            remoteIEP = new IPEndPoint(IPAddress.Broadcast, transPort);
            commonClient = GetReceivedClient();
            findedIEPs = new Dictionary<string, IPEndPoint>();
            findingStatus = false;
        }
        private UdpClient GetReceivedClient()
        {
            UdpClient client = new UdpClient(recievPort);
            client.EnableBroadcast = true;
            return client;
        }
        private void InitReceivingBroadcastMessagesThread()
        {
            receivingBroadcastMessagesThread = new Thread(FindIPsServers);
        }
        public void StartSearchingServers()
        {
            findedIEPs = new Dictionary<string, IPEndPoint>();
            findingStatus = true;
            SendBroadcastMessage();
            if (receivingBroadcastMessagesThread == null || receivingBroadcastMessagesThread.ThreadState == ThreadState.Aborted || 
                receivingBroadcastMessagesThread.ThreadState == ThreadState.Stopped)
            {
                InitReceivingBroadcastMessagesThread();
            }
            receivingBroadcastMessagesThread.Start();
        }

        public void EndSearchingServers()
        {
            findingStatus = false;
            Interrupt();
            if (receivingBroadcastMessagesThread != null && receivingBroadcastMessagesThread.ThreadState == ThreadState.Running)
            {
                receivingBroadcastMessagesThread.Join();
            }
        }
        private void SendBroadcastMessage()
        {
            if (commonClient.Client == null)
            {
                commonClient = GetReceivedClient();
            }
            try
            {
                byte[] messageInBytes = Encoding.UTF8.GetBytes(command);
                commonClient.Send(messageInBytes, messageInBytes.Length, remoteIEP);
            }
            catch (Exception)
            {
                
            }
        }
        private void Interrupt()
        {
            try
            {
                lock (commonClient)
                {
                    if (commonClient != null)
                    {
                        commonClient.Close();
                        commonClient.Client = null;
                    }
                }
            }
            catch (Exception) 
            { };
        }
        private void FindIPsServers()
        {

            byte[] receivedData;
            string message;
            string tempCommand;
            IPEndPoint currentIEP = null;
            try
            {
                while (findingStatus)
                {
                    receivedData = commonClient.Receive(ref currentIEP);
                    currentIEP.Port = 666;
                    message = Encoding.UTF8.GetString(receivedData);
                    tempCommand = message.Substring(0, serverCommand.Length);
                    if (tempCommand.Equals(serverCommand))
                    {
                        message = message.Remove(0, serverCommand.Length + 1);
                        if (!findedIEPs.ContainsKey(message) && !findedIEPs.ContainsValue(currentIEP))
                        {
                            findedIEPs.Add(message, currentIEP);
                        }
                    }
                }
            }
            catch (SocketException)
            {
                //Все идет по плану
            }
            finally
            {
                findingStatus = false;
            }
        }
    }
}
