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
        public UserList users = new UserList();
        delegate void command(User user, string prms);
        Dictionary<string, command> commandsMap = new Dictionary<string, command>();
        TcpListener listener;
        bool stopped;
        Register register;
        RandomNick rndNick = new RandomNick();


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
            commandsMap.Add("WHOIAM", WHOIAM);
        }

        public void Start()
        {
            register = new Register();
            register.Load();
            stopped = false;
            Log.Write("Сервер " + name + " запущен!");
            listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            while (!stopped)
            {
                User user;
                try
                {
                    user = new User(listener.AcceptTcpClient());
                }
                catch (SocketException)
                {
                    return;
                }
                user.name = rndNick.GetNew();
                users.Add(user);
                Thread clientThread = new Thread(new ParameterizedThreadStart(ClientThread));
                clientThread.Start(user);
            }
            Log.Write("Сервер остановлен!");
        }

        void ClientThread(Object userObject)
        {
            User user = (User)userObject;
            Thread thread = new Thread(() =>
            {
                Log.Write(String.Format("Подключен клиент: {0}",
                    user.client.Client.RemoteEndPoint.ToString()));
                string date = DateTime.Now.Date.ToLongDateString();
                SendMessage(user, String.Format("MSG Тебя приветствует сервер {0}! Время на сервере: {1}",
                    name, date));
                SendMessage("MSG " + user.name + " присоединился к чату.");
                SendNamesToAll();
                SendMessage(user, "YOUARE " + user.name);
                while (!stopped)
                {
                    string mess = "";
                    try
                    {
                        mess = GetNextMessage(user);
                    }
                    catch
                    {
                        Log.Write(String.Format("Ошибка получения сообщения! "
                            + "Подключение с {0} будет разорвано!", user.name));
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
                            Log.Write("Неверный формат: " + mess);
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

        public void Stop()
        {
            Log.Write("Остановка..");
            SendError("100");
            lock (users)
            {
                foreach (User user in users)
                {
                    user.client.Close();
                }
                users.Clear();
            }
            if (listener != null) listener.Stop();
            stopped = true;
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

        public void SendMessage(User user, string message)
        {
            try
            {
                SendMessage(user.GetStream(), message);
            }
            catch
            {
                Log.Write("Ошибка отправки сообщения " + message);
                Log.Write("Подключение с " + user.name + " будет разорвано!");
                lock (users) users.Remove(user);
                user.client.Close();
                return;
            }
        }

        public void SendMessage(string message)
        {
            //Console.WriteLine("Send to all: " + message);
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
                        Log.Write("Проблемка!");
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
