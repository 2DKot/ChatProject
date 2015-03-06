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
        public static readonly string serverCommand = "IAMSERVER";
        public IPEndPoint remoteIEP;
        public static Dictionary<string, IPEndPoint> findedIEPs;
        private Thread receivingBroadcastMessagesThread;
        public bool findingStatus;
        private UdpClient commonClient;

        public SearcherServers()
        {
            remoteIEP = new IPEndPoint(IPAddress.Broadcast, transPort);
            commonClient = GetRecievClient();
            findedIEPs = new Dictionary<string, IPEndPoint>();
            findingStatus = false;
            InitReceivingBroadcastMessagesThread();

        }
        private UdpClient GetTransClient()
        {
            UdpClient client = new UdpClient(transPort);
            client.EnableBroadcast = true;
            return client;
        }
        private UdpClient GetRecievClient()
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
            findingStatus = true;
            SendBroadcastMessage();
            if (receivingBroadcastMessagesThread.ThreadState == ThreadState.Aborted)
            {
                InitReceivingBroadcastMessagesThread();
            }
            receivingBroadcastMessagesThread.Start();
        }

        public void EndSearchingServers()
        {
            findingStatus = false;
            Interrupt();
            receivingBroadcastMessagesThread.Abort();
            
        }
        private void SendBroadcastMessage()
        {
            if (commonClient.Client == null)
            {
                commonClient = GetTransClient();
            }
            try
            {
                commonClient.Connect(remoteIEP);
                byte[] messageInBytes = Client.StringToBytes(command);
                commonClient.Send(messageInBytes, messageInBytes.Length, remoteIEP);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                try
                {
                    commonClient.Close();
                }
                catch
                { }
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
            catch { };
        }
        private void FindIPsServers()
        {

            byte[] receivedData;
            string message;
            string tempCommand;
            IPEndPoint currentIPE = null;
            if (commonClient.Client == null)
            {
                commonClient = GetRecievClient();
            }
            try
            {
                
                while (findingStatus)
                {
                    receivedData = commonClient.Receive(ref currentIPE);
                    message = Encoding.UTF8.GetString(receivedData);
                    tempCommand = message.Substring(0, serverCommand.Length);
                    if (tempCommand.Equals(serverCommand))
                    {
                        message = message.Remove(0, serverCommand.Length + 1);
                        findedIEPs.Add(message, currentIPE);
                    }
                }
            }
            catch (SocketException)
            {
                //Все идет по плану
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                findingStatus = false;
                commonClient.Close();
            }
        }
    }
}
