﻿using System;
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
    class Client
    {
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
                throw new Exception();
            }
            return rInstance;
        }
        static private TcpClient GetNewTCPClient()
        {
            return (new TcpClient());
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
        private NetworkStream GetNetworkStream(IPEndPoint remoteIEP)
        {
            NetworkStream rInstance = null;
            this.clientToServer.Connect(remoteIEP);
            rInstance = this.clientToServer.GetStream();
            return rInstance;
        }

        public void RequestToGetTempNickName()
        {
            string command = "NICK";
            SendText(command);
        }
        
        public void LogIn(string ip, int port)
        {
            IPEndPoint remoteIEP = this.GetIEP(ip, port);
            this.clientToServer = Client.GetNewTCPClient();
            this.nStream = this.GetNetworkStream(remoteIEP);
            if (this.nStream == null)
            {
                throw new NullReferenceException();
            }
        }
        public void LogOut()
        {
            try
            {
                if (this.clientToServer != null)
                {
                    this.clientToServer.Close();
                    this.nStream = null;
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
                //First sending is data about size of a message
                GetInstance().nStream.Write(buffWithLength, 0, buffWithLength.Length);
                //Second sending is the message
                GetInstance().nStream.Write(buffWithMessage, 0, length);
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
            catch
            {
                throw new SocketException();
            }
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
                return "Сообщение со стороны клиента - работа с сервером могла быть прекращена.";
            }
            return Reactions.commandToHandler[command](restParameters);
        }
        static private byte[] StringToBytes(string text)
        {
            return (Encoding.UTF8.GetBytes(text));
        }
    }
}
