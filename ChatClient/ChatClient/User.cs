using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ChatClient
{
    class User
    {
        //Dict: Service Code to Instance of Delegate
        TcpClient clientToServer;
        NetworkStream nStream;
        static User instance;
        public List<string> listOfNickNames;
        
        public static User GetInstance()
        {
            if (instance == null)
            {
                instance = new User();
            }
            return instance;
        }
        private User()
        {
            clientToServer = new TcpClient();
            listOfNickNames = new List<string>();

        }
        private IPEndPoint GetIEP(string ip, int port)
        {
            IPEndPoint rInstance = null;
            try
            {
                rInstance = new IPEndPoint(IPAddress.Parse(ip), port);
            }
            catch
            {
                //Make note in log
            }
            return rInstance;
        }
        private NetworkStream GetNetworkStream(IPEndPoint remoteIEP)
        {
            NetworkStream rInstance = null;
            try
            {
                GetInstance().clientToServer.Connect(remoteIEP);
                rInstance = GetInstance().clientToServer.GetStream();
            }
            catch
            {
                throw new SocketException();
            }
            return rInstance;
        }

        public void RequestToChangeNickName(string nick)
        {
            string command = "NICK ";
            SendText(command + nick);
        }
        // int - number of error
        public void LogIn(string ip, int port)
        {
            IPEndPoint remoteIEP = GetInstance().GetIEP(ip, port);
            GetInstance().nStream = GetInstance().GetNetworkStream(remoteIEP);
            if (GetInstance().nStream == null)
            {
                throw new Exception();
            }
          
        }
        public void SendText(string message)
        {
            try
            {
                byte[] buffWithMessage = StringToBytes(message);
                int length = buffWithMessage.Length;
                byte[] buffWithLength = BitConverter.GetBytes(length);
                //First sending is data about size of a message
                GetInstance().nStream.Write(buffWithLength, 0, buffWithLength.Length);
                //Second sending is the message
                GetInstance().nStream.Write(buffWithMessage, 0, length);
            }
            catch
            {
                throw new NullReferenceException();
            }
            
        }
        public string GetMessage()
        {
            byte[] buffWithLength;
            int lengthOfBuffWithLength = 4;
            string message;
            byte[] buffWithMessage;

            buffWithLength = new byte[lengthOfBuffWithLength];
            nStream.Read(buffWithLength, 0, lengthOfBuffWithLength);
            int lengthOfMessage = BitConverter.ToInt32(buffWithLength, 0);
            buffWithMessage = new byte[lengthOfMessage];
            nStream.Read(buffWithMessage, 0, lengthOfMessage);
            message = System.Text.Encoding.UTF8.GetString(buffWithMessage);
            if (IsFailedMessage(message))
            {
                GetInstance().clientToServer.Close();
                GetInstance().nStream = null;
            }
            return message;
        }
        private bool IsFailedMessage(string text)
        {
            return text == "";
        }
        public string HandleMessage(string text)
        {
            int firstIndexTab = text.IndexOf(' ');
            string command = "";
            string restParameters = "";
            if (firstIndexTab != -1)
            {
                command = text.Substring(0, firstIndexTab);
                restParameters = text.Remove(0, firstIndexTab+1);
            }
            else
            { 
                return "Некорректный формат сообщения. Работа с сервером могла быть прекращена.";
            }
            return Actions.commandToHandler[command](restParameters);
        }
        static private byte[] StringToBytes(string text)
        {
            return (Encoding.UTF8.GetBytes(text));
        }
    }
}
