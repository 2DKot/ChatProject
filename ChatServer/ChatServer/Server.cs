using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatServer
{
    class Server
    {
        string name;
        List<User> users = new List<User>();
        delegate void command(User user, string prms);
        Dictionary<string, command> commandsMap = new Dictionary<string, command>();

        public Server(string name = "chatServer")
        {
            this.name = name;
            commandsMap.Add("MSG", MSG);
            commandsMap.Add("NICK", NICK);
            commandsMap.Add("PRIVMSG", PRIVMSG);
        }

        public void Start()
        {
            Console.WriteLine("Сервер запущен!");
            TcpListener listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            while (true)
            {
                User user = new User(listener.AcceptTcpClient());
                users.Add(user);
                user.name += users.Count();
                Thread thread = new Thread(() =>
                {
                    Console.WriteLine("Подключен клиент: {0}",
                        user.client.Client.RemoteEndPoint.ToString());
                    SendMessage(user, "Тебя приветствует сервер " + name + "!");
                    while (true)
                    {
                        string mess = "";
                        try
                        {
                            mess = GetNextMessage(user);
                        }
                        catch
                        {
                            Console.WriteLine("Ошибка получения сообщения!");
                            Console.WriteLine("Подключение с {0} будет разорвано!",
                                user.name);
                            lock (users) users.Remove(user);
                            user.client.Close();
                            return;
                        }
                        try
                        {
                            int splitter = mess.IndexOf(' ');
                            string comm = mess.Substring(0, splitter);
                            string prms = mess.Substring(splitter + 1);
                            commandsMap[comm](user, prms);
                        }
                        catch
                        {
                            Console.WriteLine("Неверная команда или параметры! ({0}: {1})",
                                user.name, mess);
                            SendMessage(user, "ERROR 001");
                        }
                    }
                });
                thread.Start();
            }
        }

        public string GetNextMessage(NetworkStream ns)
        {
            byte[] sizeBuffer = new byte[4];
            ns.Read(sizeBuffer, 0, 4);
            int size = BitConverter.ToInt32(sizeBuffer, 0);
            byte[] messageBuffer = new byte[size];
            ns.Read(messageBuffer, 0, size);
            return Encoding.UTF8.GetString(messageBuffer);
        }

        public string GetNextMessage(User user)
        {
            return GetNextMessage(user.GetStream());
        }

        public void SendMessage(NetworkStream ns, string message)
        {
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
            int size = messageBuffer.Count();
            byte[] sizeBuffer = BitConverter.GetBytes(size);
            ns.Write(sizeBuffer, 0, 4);
            ns.Write(messageBuffer, 0, size);
        }

        public void SendMessage(User user, string message)
        {
            try
            {
                SendMessage(user.GetStream(), message);
            }
            catch
            {
                Console.WriteLine("Ошибка отправки сообщения!");
                Console.WriteLine("Подключение с {0} будет разорвано!", user.name);
                lock (users) users.Remove(user);
                user.client.Close();
                return;
            }
        }

        public User FindUserByName(string name)
        {
            foreach (User user in users)
            {
                if (user.name == name) return user;
            }
            return null;
        }

        public void MSG(User user, string prms)
        {
            string message = "MSG " + user.name + ": " + prms;
            Console.WriteLine("Send to all: " + message);
            lock (users)
            {
                foreach (User target in users)
                {
                    try
                    {
                        SendMessage(target, message);
                    }
                    catch
                    {
                        SendMessage(user, "ERROR 002");
                    }
                }
            }
        }

        public void NICK(User user, string prms)
        { 
            string newName = prms.Split(' ')[0];
            if(FindUserByName(newName)!=null)
            {
                SendMessage(user, "ERROR 051");
                return;
            }
            Console.WriteLine(user.name + " changed nick to " + newName);
            user.name = newName;
            SendMessage(user, "ERROR 050");
        }

        public void PRIVMSG(User user, string prms)
        {
            int splitter = prms.IndexOf(' ');
            if (splitter == -1)
            {
                SendMessage(user, "ERROR 001");
                return;
            }
            string targetName = prms.Substring(0, splitter);
            string message = prms.Substring(splitter + 1);
            User target = FindUserByName(targetName);
            if (target == null)
            {
                SendMessage(user, "ERROR 003");
                return;
            }
            try
            {
                SendMessage(target, "MSG "+user.name+": "+message);
            }
            catch
            {
                SendMessage(user, "ERROR 002");
                return;
            }
        }
    }
}
