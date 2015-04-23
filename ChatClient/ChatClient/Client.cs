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
        private TcpClient clientToServer;
        //private static Client instance;
        public List<string> listOfConnectedNickNames;
        public string ownNickName;
        /*public static Client GetInstance()
        {
            if (instance == null)
            {
                instance = new Client();
            }
            return instance;
        }*/
        /*private*/ public Client()
        {
            listOfConnectedNickNames = new List<string>();
            //InitIgnoredList();
            ignoredCommandsList = new List<string>();
            ignoredCommandsList.Add("NAMES");
        }
    
        /*private static void InitIgnoredList()
        {
            ignoredCommandsList = new List<string>();
            ignoredCommandsList.Add("NAMES");
        }*/
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
            //Второе подключение - ошибка?
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
               /**/ if (this.clientToServer != null && 
                   this.clientToServer.Client != null && 
                   this.clientToServer.Client.Connected)
                {
                    this.clientToServer.Close();
                    this.clientToServer.Client= null;
                }
            }
            catch
            {
                /*Exception SE =*/ throw new Exception("Невозможно закрыть открытое соединение");
            }
        }
        public void SendText(string message)
        {
            if (message == null || message == "")
            {
                throw new ArgumentException("Неверные входные данные : пустое сообщение для передачи.");
            }
            byte[] buffWithMessage = Encoding.UTF8.GetBytes(message);
            int length = buffWithMessage.Length;
            byte[] buffWithLength = BitConverter.GetBytes(length);
            try
            {
                this.clientToServer.GetStream().Write(buffWithLength, 0, buffWithLength.Length);
                this.clientToServer.GetStream().Write(buffWithMessage, 0, length);
            }
            catch (Exception)
            {
                //Обычный ли будет exception ?
                throw new Exception("Ошибка при передаче сообщения серверу. Возможно отсутствует подключение.");
            }
        }
        public void SendText(string[] messages)
        {
            if (messages == null || messages.Length == 0)
            {
                throw new ArgumentException("Пустой массив сообщений.");
            }
            foreach (string currentMessage in messages)
            {
                SendText(currentMessage);
            }
        }
        public string GetMessage()
        {
            string message = "";
            byte[] buffWithLength;
            int lengthOfBuffWithLength = 4;
            byte[] buffWithMessage;
            buffWithLength = new byte[lengthOfBuffWithLength];
            if (this.clientToServer == null || !this.clientToServer.Connected)
            {
                throw new Exception("Невозможно получить сообщение с потока - клиент не подключен.");
            }
            try
            {
                this.clientToServer.GetStream().Read(buffWithLength, 0, lengthOfBuffWithLength);
                int lengthOfMessage = BitConverter.ToInt32(buffWithLength, 0);
                buffWithMessage = new byte[lengthOfMessage];
                this.clientToServer.GetStream().Read(buffWithMessage, 0, lengthOfMessage);
                message = System.Text.Encoding.UTF8.GetString(buffWithMessage);
                
            }
            catch (Exception exc)
            {
                //Переопределить свой exception
                throw new Exception("Разрыв соединения при получении сообщения.");
            }
            if (IsFailedMessage(message))
            {
                throw new Exception("Бескомандное или пустое сообщение со стороны сервера - возможно сервер прекратил свою работу.");
            }
            return message;
        }
        private bool IsFailedMessage(string text)
        {
            if (text != null)
            {
                return text == "";
            }
            else
            {
                return false;
            }
        }
        public string HandleRawDataText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("null-строка при обработке.");
            }
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
                throw new ArgumentException("Сообщение не по протоколу - отсутствует команда со стороны сервера."/* - работа могла быть прекращена."*/);
            }
            if (!Reactions.commandToHandler.ContainsKey(command))
            {
                throw new ArgumentException("Сообщение не по протоколу - Неизвестная команда '" + command +
                "' со стороны сервера. Ее невозможно обработать.");
            }
            if (!ignoredCommandsList.Contains(command))
            {
                handledText = Reactions.commandToHandler[command](restParameters, this);
            }
            return handledText;
        }
        public Message ConvertToMessage(string rawData)
        {
            string handledTextMessage = HandleRawDataText(rawData);
            Color textColor = Message.DetermineColor(rawData.Substring(0, rawData.IndexOf(' ')));
            Message message = new Message(rawData, handledTextMessage, textColor);
            return message;
        }
    }
}
