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
    public class Client
    {
        private static List<string> ignoredCommands;
        protected Dictionary<string, Action<string>> sideEffectCommandsToDelegate;
        private ITcpClient serviceClient;
        private List<string> onlineUsers;
        private string ownNickName;
        public Client(ITcpClient client)
        {
            serviceClient = client;
            ownNickName = String.Empty;
            onlineUsers = new List<string>();
            ignoredCommands = new List<string>();
            ignoredCommands.Add("NAMES");
            sideEffectCommandsToDelegate = new Dictionary<string, Action<string>>();
            sideEffectCommandsToDelegate.Add("YOUARE", (nick) => { this.ownNickName = nick; });
            sideEffectCommandsToDelegate.Add("NAMES", (nicks) => 
            { 
                onlineUsers = new List<string>(nicks.Split(' '));
                onlineUsers.Insert(0, "Отправить всем");
            });
        }
        public static bool IsCorrectNick(string nick)
        {
            if (nick == null)
            {
                throw new ArgumentNullException("Недопустимый входной параметр (null))");
            }
            if (nick.Length == 0)
            {
                throw new ArgumentException("Недопустимый входной параметр (пустая строка)");
            }
            bool flagSpaceSymbol = (nick.IndexOf(' ') != -1);
            return !(flagSpaceSymbol);
        }
        public static bool IsCorrectPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("Недопустимый входной параметр (null)");
            }
            if (password.Length == 0)
            {
                throw new ArgumentException("Недопустимый входной параметр (пустая строка для пароля)");
            }
            return true;
        }
        public void RequestToGetTempNickName()
        {
            string command = "NICK";
            SendTextData(command);
        }
        public string OwnNickName
        {
            get
            {
                return this.ownNickName;
            }
        }

        public List<string> OnlineUsers
        {
            get
            {
                return this.onlineUsers;
            }
        }
        public void DoConnect(IPEndPoint IEP)
        {
            if (IEP == null)
            {
                throw new ArgumentNullException("Значение объекта IPEndPoint равно null");
            }
            if (!this.serviceClient.IsConnected())
            {
                this.serviceClient.Connect(IEP);
            }
        }
        public void DoDisconnect()
        {
            
            if (this.serviceClient.IsConnected())
            {
                this.serviceClient.Close();
            }
            this.ownNickName = "";
        }
        public void SendTextData(string message)
        {
            if (!this.serviceClient.IsConnected())
            {
                throw new Exception("Клиент не имел подключения");
            }
            if (message == null || message == "")
            {
                throw new ArgumentException("Неверные входные данные : пустое сообщение для передачи.");
            }
            byte[] buffWithMessage = Encoding.UTF8.GetBytes(message);
            int length = buffWithMessage.Length;
            byte[] buffWithLength = BitConverter.GetBytes(length);
            try
            {
                this.serviceClient.Write(buffWithLength, 0, buffWithLength.Length);
                this.serviceClient.Write(buffWithMessage, 0, length);
            }
            catch (Exception)
            {
                throw new Exception("Ошибка при передаче сообщения серверу. Возможно отсутствует подключение.");
            }
            
        }
        public void SendTextData(string[] messages)
        {
            if (messages == null || messages.Length == 0)
            {
                throw new ArgumentException("Пустой массив сообщений.");
            }
            foreach (string currentMessage in messages)
            {
                SendTextData(currentMessage);
            }
        }
        public string GetTextData()
        {
            if (!this.serviceClient.IsConnected())
            {
                throw new Exception("Невозможно получить сообщение с потока - клиент не подключен.");
            }
            string message = "";
            byte[] buffWithLength;
            int lengthOfBuffWithLength = 4;
            byte[] buffWithMessage;
            buffWithLength = new byte[lengthOfBuffWithLength];

            try
            {
                buffWithLength = this.serviceClient.Read(0, lengthOfBuffWithLength);
                int lengthOfMessage = BitConverter.ToInt32(buffWithLength, 0);
                buffWithMessage = this.serviceClient.Read(0, lengthOfMessage);
                message = System.Text.Encoding.UTF8.GetString(buffWithMessage);
            }
            catch (Exception)
            {
                throw new Exception("Разрыв соединения при получении сообщения.");
            }
            if (IsFailedTextData(message))
            {
                throw new Exception("Бескомандное или пустое сообщение со стороны сервера - возможно сервер прекратил свою работу.");
            }
            return message;
        }
        private bool IsFailedTextData(string text)
        {
            return text == "";
        }
        private string HandleRawDataText(string text)
        {
            
            string handledText;
            int firstIndexSpace = text.IndexOf(' ');
            string command = "";
            string restParameters = "";
            if (firstIndexSpace != -1)
            {
                command = text.Substring(0, firstIndexSpace);
                restParameters = text.Remove(0, firstIndexSpace + 1);
                if (command == "PRIVMSG")
                {
                    firstIndexSpace = restParameters.IndexOf(' ');
                    if (firstIndexSpace == -1)
                    {
                        throw new ArgumentException("Сообщение не по протоколу - отсутствуют необходимые аргументы.");
                    }
                }
            }
            else
            { 
                throw new ArgumentException("Сообщение не по протоколу - отсутствует команда со стороны сервера.");
            }
            if (sideEffectCommandsToDelegate.ContainsKey(command))
            {
                sideEffectCommandsToDelegate[command](restParameters);
            }
            if (!Reactions.ContainsHandlerForCommand(command))
            {
                throw new ArgumentException("Сообщение не по протоколу - Неизвестная команда '" + command +
                "' со стороны сервера. Ее невозможно обработать.");
            }
            handledText = Reactions.GetCommandHandler(command)(restParameters);
            if (ignoredCommands.Contains(command))
            {
                handledText = "";
            }
            return handledText;
        }
        public Message ConvertTextDataToMessage(string rawData)
        {
            if (rawData == null)
            {
                throw new ArgumentNullException("null-строка при обработке.");
            }
            string handledTextMessage = this.HandleRawDataText(rawData);
            Color textColor = Message.DetermineColor(rawData.Substring(0, rawData.IndexOf(' ')));
            Message message = new Message(rawData, handledTextMessage, textColor);
            return message;
        }
    }
}
