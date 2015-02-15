using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatServer
{
    partial class Server
    {
        public string name;
        List<User> users = new List<User>();
        delegate void command(User user, string prms);
        Dictionary<string, command> commandsMap = new Dictionary<string, command>();
        TcpListener listener;
        bool stopped;
        Register register;

        public Server(string name = "chatServer")
        {
            this.name = name;
            commandsMap.Add("MSG", MSG);
            commandsMap.Add("NICK", NICK);
            commandsMap.Add("PRIVMSG", PRIVMSG);
            commandsMap.Add("NAMES", NAMES);
            commandsMap.Add("DATE", DATE);
            commandsMap.Add("REG", REG);
            commandsMap.Add("LOGIN", LOGIN);
        }

        public void Start()
        {
            register = new Register();
            register.Load();
            stopped = false;
            Console.WriteLine("Сервер {0} запущен!", name);
            listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            while (!stopped)
            {
                User user;
                try
                {
                    user = new User(listener.AcceptTcpClient());
                }
                catch
                {
                    break;
                }
                users.Add(user);
                user.name += users.Count();
                Thread thread = new Thread(() =>
                {
                    Console.WriteLine("Подключен клиент: {0}",
                        user.client.Client.RemoteEndPoint.ToString());
                    string date = DateTime.Now.Date.ToLongDateString();
                    SendMessage(user, String.Format("Тебя приветствует сервер {0}! Время на сервере: {1}",
                        name, date));
                    SendMessage("MSG " + user.name + " присоединился к чату.");
                    SendNamesToAll();
                    while (!stopped)
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
                            SendNamesToAll();
                            return;
                        }
                        int splitter = mess.IndexOf(' ');
                        if (splitter > 0)
                        {
                            string comm = mess.Substring(0, splitter);
                            string prms = mess.Substring(splitter + 1);
                            try
                            {
                                commandsMap[comm](user, prms);
                            }
                            catch
                            {
                                SendError(user, "001");
                                Console.WriteLine("Неверный формат: " + mess);
                            }
                        }
                        else
                        {
                            try
                            {
                                commandsMap[mess](user, "");
                            }
                            catch
                            {
                                SendError(user, "001");
                            }
                        }
                    }
                });
                thread.Start();
            }
            Console.WriteLine("Я закрылся!");
        }

        public void Stop()
        {
            SendError("100");
            foreach (User user in users)
            {
                user.client.Close();
            }
            listener.Stop();
            stopped = true;
            Console.WriteLine("Сервер был закрыт");
        }

        string GetNextMessage(NetworkStream ns)
        {
            byte[] sizeBuffer = new byte[4];
            ns.Read(sizeBuffer, 0, 4);
            int size = BitConverter.ToInt32(sizeBuffer, 0);
            byte[] messageBuffer = new byte[size];
            ns.Read(messageBuffer, 0, size);
            return Encoding.UTF8.GetString(messageBuffer);
        }

        string GetNextMessage(User user)
        {
            return GetNextMessage(user.GetStream());
        }

        void SendMessage(NetworkStream ns, string message)
        {
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
            int size = messageBuffer.Count();
            byte[] sizeBuffer = BitConverter.GetBytes(size);
            ns.Write(sizeBuffer, 0, 4);
            ns.Write(messageBuffer, 0, size);
        }

        void SendMessage(User user, string message)
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

        void SendMessage(string message)
        {
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
                        Console.WriteLine("Проблемка!");
                    }
                }
            }
        }

        void SendError(User user, string code)
        {
            SendMessage(user, "ERROR " + code);
        }

        void SendError(string code)
        {
            SendMessage("ERROR " + code);
        }

        User FindUserByName(string name)
        {
            foreach (User user in users)
            {
                if (user.name == name) return user;
            }
            return null;
        }

        void SendNamesToAll()
        {
            string message = "NAMES";
            foreach (User one in users)
            {
                message += " " + one.name;
            }
            SendMessage(message);
        }
    }
}
