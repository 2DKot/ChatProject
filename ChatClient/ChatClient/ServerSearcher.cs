using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ChatClient
{
    public class ServerSearcher
    {
        /*public protected int transPort = 667;
        /*public protected int receivePort = 668;*/

        private readonly string findCommand = "FINDSERVER";
        private readonly string serverCommand = "IAMSERV";
        private IPEndPoint remoteIpEP;
        private Dictionary<string, IPEndPoint> findedIpEPs;
        private Thread receivingBroadcastMessagesThread;
        private bool findingStatus;
        //private UdpClient commonClient;
        
        private IUdpClient commonClient;

        public ServerSearcher(IUdpClient client, int transPort)
        {
            remoteIpEP = new IPEndPoint(IPAddress.Broadcast, transPort);
            commonClient = client;
            findedIpEPs = new Dictionary<string, IPEndPoint>();
            findingStatus = false;
        }

        public Dictionary<string, IPEndPoint> FindedIpEPs
        {
            get
            {
                return this.findedIpEPs;
            }
        }
        public bool FindingStatus
        {
            get
            {
                return this.findingStatus;
            }
        }

        private void InitReceivingBroadcastMessagesThread()
        {
            receivingBroadcastMessagesThread = new Thread(FindIPsServers);
        }

        public void StartSearchingServers()
        {
            if (findingStatus)
            {
                throw new Exception("Поиск уже производится!");
            }
            findedIpEPs = new Dictionary<string, IPEndPoint>();
            //findingStatus = true;
            SendBroadcastMessage();
            if (receivingBroadcastMessagesThread == null || 
                receivingBroadcastMessagesThread.ThreadState == System.Threading.ThreadState.Aborted || 
                receivingBroadcastMessagesThread.ThreadState == System.Threading.ThreadState.Stopped)
            {
                InitReceivingBroadcastMessagesThread();
            }
            findingStatus = true;
            receivingBroadcastMessagesThread.Start();
        }

        public void EndSearchingServers()
        {
            if (!findingStatus)
            {
                throw new Exception("Клиент прекратил работу и не может быть завершен еще раз.");
            }
            findingStatus = false;
            this.Interrupt();
            if (receivingBroadcastMessagesThread != null &&
                receivingBroadcastMessagesThread.ThreadState == System.Threading.ThreadState.Running)
            {
                receivingBroadcastMessagesThread.Join();
            }
        }
        private void SendBroadcastMessage()
        {
            if (commonClient.IsClientNull())
            {
                commonClient.InitializeUdpClient();
            }
            try
            {
                byte[] messageInBytes = Encoding.UTF8.GetBytes(findCommand);
                commonClient.Send(messageInBytes, messageInBytes.Length, remoteIpEP);
            }
            catch (Exception)
            {}
        }
        [Conditional("DEBUG")]
        public void SendBroadcastMessage_PublicWrapper()
        {
            this.SendBroadcastMessage();
        }
        
        private void Interrupt()
        {
            /*try
            {*/
                lock (commonClient)
                {
                    if (!commonClient.IsClientNull())
                    {
                        commonClient.Close();
                    }
                }
            /*}
            catch (Exception) 
            { };*/
        }
        [Conditional("DEBUG")]
        public void FindIPsServers_PublicWrapper()
        {
            FindIPsServers();
        }

        [Conditional("DEBUG")]
        public void SetFindingStatus()
        {
            this.findingStatus = true;
        }

        private void FindIPsServers()
        {
            byte[] receivedData;
            string message;
            string actualCommand;
            IPEndPoint currentIEP = null;
            //this.findingStatus = true;
            try
            {
                while (findingStatus)
                {
                    receivedData = commonClient.Receive(ref currentIEP);
                    currentIEP.Port = 666;
                    message = Encoding.UTF8.GetString(receivedData);
                    actualCommand = message.Substring(0, serverCommand.Length);
                    if (actualCommand.Equals(serverCommand))
                    {
                        message = message.Remove(0, serverCommand.Length + 1);
                        if (!findedIpEPs.ContainsKey(message) && !findedIpEPs.ContainsValue(currentIEP))
                        {
                            findedIpEPs.Add(message, currentIEP);
                        }
                    }
                }
            }
            catch (SocketException)
            {
                //Произошел принудительный разрыв udp-соединения через Interrupt
                findingStatus = false;
            }
        }
    }
}
