using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace ChatClient
{
    class Client
    {
        private static List<string> ignoredCommandsList;
        TcpClient clientToServer;
        NetworkStream nStream;
        static Client instance;
        public List<string> listOfNickNames;
        public string ownNickName;
        public static Client GetInstance()
        {
            if (instance == null)
            {
                instance = new Client();
            }
            return instance;
        }
        private Client()
        {
            listOfNickNames = new List<string>();
            InitIgnoredList();

        }
    
        private static void InitIgnoredList()
        {
            ignoredCommandsList = new List<string>();
            ignoredCommandsList.Add("NAMES");

        }
        public static bool IsCorrectNick(string nick)
        {
            bool flagSpaceSymbol = (nick.IndexOf(' ') != -1);

            return !(flagSpaceSymbol);
        }
        public static bool IsCorrectPassword(string password)
        {
            bool flagSpaceSymbol = (password.IndexOf(' ') != -1);

            return !(flagSpaceSymbol);
        }
        public void RequestToGetTempNickName()
        {
            string command = "NICK";
            SendText(command);
        }
        
        public void DoConnect(IPEndPoint IEP)
        {
            IPEndPoint remoteIEP = IEP;
            this.clientToServer = new TcpClient();
            this.clientToServer.Connect(remoteIEP);
            if (this.clientToServer.Client == null)
            {
                throw new SocketException();
            }
        }
        public void DoDisconnect()
        {
            try
            {
                if (this.clientToServer != null)
                {
                    this.clientToServer.Close();
                    this.clientToServer.Client= null;
                }
            }
            catch
            {
                throw new SocketException();
            }
        }
        public void SendText(string message)
        {
            try
            {
                byte[] buffWithMessage = StringToBytes(message);
                int length = buffWithMessage.Length;
                byte[] buffWithLength = BitConverter.GetBytes(length);
                
                this.clientToServer.GetStream().Write(buffWithLength, 0, buffWithLength.Length);
                this.clientToServer.GetStream().Write(buffWithMessage, 0, length);
            }
            catch
            {
                throw new SocketException();
            }
            
        }
        public void SendText(string[] messages)
        {
            if (messages == null || messages.Length == 0)
            {

                throw new ArgumentException();
            }
            foreach (string currentMessage in messages)
            {
                SendText(currentMessage);
            }
        }
        public string GetMessage()
        {
            string message = "";
            try
            {
                byte[] buffWithLength;
                int lengthOfBuffWithLength = 4;
                
                byte[] buffWithMessage;

                buffWithLength = new byte[lengthOfBuffWithLength];
                this.clientToServer.GetStream().Read(buffWithLength, 0, lengthOfBuffWithLength);
                int lengthOfMessage = BitConverter.ToInt32(buffWithLength, 0);
                buffWithMessage = new byte[lengthOfMessage];
                this.clientToServer.GetStream().Read(buffWithMessage, 0, lengthOfMessage);
                message = System.Text.Encoding.UTF8.GetString(buffWithMessage);
                if (IsFailedMessage(message))
                {
                    DoDisconnect();
                }
                return message;
            }
            catch
            {
                throw new SocketException();
            }
        }
        private bool IsFailedMessage(string text)
        {
            return text == "";
        }
        public string HandleRawDataText(string text)
        {
            string handledText = "";
            int firstIndexSpace = text.IndexOf(' ');
            string command = "";
            string restParameters = "";
            if (firstIndexSpace != -1)
            {
                command = text.Substring(0, firstIndexSpace);
                restParameters = text.Remove(0, firstIndexSpace+1);
            }
            else
            { 
                /*return "Сообщение со стороны клиента - работа с сервером могла быть прекращена.";*/
                throw new ArgumentException("Некорретное сообщение со стороны сервера - работа могла быть прекращена.");
            }
            if (!Reactions.commandToHandler.ContainsKey(command))
            {
                throw new ArgumentException("Неизвестная команда сервера.");
            }
            handledText = Reactions.commandToHandler[command](restParameters);
            if (ignoredCommandsList.Contains(command))
            {
                handledText = String.Empty;
            }
            return handledText;
        }
        public Message ConvertToMessage(string rawData)
        {
            string handledTextMessage = HandleRawDataText(rawData);
            Color textColor = DetermineColor(rawData.Substring(0, rawData.IndexOf(' ')));
            Message message = new Message(handledTextMessage, textColor);
            return message;
        }
        static private Color DetermineColor(string command)
        {
            Color rColor;
            switch (command)
            {
                case "YOUARE":
                    {
                        rColor = Color.Red;
                        break;
                    }
                case "ERROR":
                    {
                        rColor = Color.Red;
                        break;
                    }
                case "MSG":
                    {
                        rColor = Color.Black;
                        break;
                    }
                case "PRIVMSG":
                    {
                        rColor = Color.Indigo;
                        break;
                    }
                default:
                    {
                        /*throw new ArgumentException("");*/
                        rColor = Color.Black;
                        break;
                    }

            }
            return rColor;
        }
        static public byte[] StringToBytes(string text)
        {
            return (Encoding.UTF8.GetBytes(text));
        }

    }
}
