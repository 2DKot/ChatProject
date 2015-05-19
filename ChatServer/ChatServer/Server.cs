using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace ChatServer
{

    public partial class Server
    {
        public string name;
        delegate void command(User user, string prms);
        Dictionary<string, command> commandsMap = new Dictionary<string, command>();
        TcpListener listener;
        FindingService findingService;
        bool stopped;
#if TESTING
        public
#endif
        Register register;

        RandomNick rndNick = new RandomNick();
        public UserList userList = new UserList();
        Thread serverThread;

        public Server(string name = "chatServer")
        {
            this.name = name;
            commandsMap.Add("MSG", MSG);
            commandsMap.Add("NICK", NICK);
            commandsMap.Add("PRIVMSG", PRIVMSG);
            //commandsMap.Add("NAMES", NAMES);
            //commandsMap.Add("DATE", DATE);
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
            findingService = new FindingService();
            findingService.Start(name);
            listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            serverThread = new Thread(() =>
            {
                while (!stopped)
                {
                    User user;
                    try
                    {
                        user = new User(listener.AcceptTcpClient(), rndNick.GetNew());
                        userList.Add(user);
                    }
                    catch (SocketException)
                    {
                        return;
                    }
                    Thread clientThread = new Thread(new ParameterizedThreadStart(ClientThread));
                    clientThread.Start(user);
                }
                Log.Write("Сервер остановлен!");
            });
            serverThread.Start();
        }

        void ClientThread(Object userObject)
        {
            User user = (User)userObject;
            Log.Write(String.Format("Подключен клиент: {0}",
                user.client.Client.RemoteEndPoint.ToString()));
            user.SendMessage("YOUARE " + user.name);
            string date = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            user.SendMessage(String.Format("MSG Тебя приветствует сервер {0}! Время на сервере: {1}",
                name, date));
            userList.SendMessageToAll("MSG " + user.name + " присоединился к чату.");
            userList.SendNamesToAll();
            while (!stopped)
            {
                string mess = "";
                try
                {
                    mess = user.GetNextMessage();
                }
                catch (SocketException)
                {
                    Log.Write(String.Format("Ошибка получения сообщения! "
                        + "Подключение с {0} будет разорвано!", user.name));
                    userList.Remove(user);
                    userList.SendNamesToAll();
                    return;
                }
                catch (ObjectDisposedException)
                {
                    Log.Write(String.Format("Ошибка получения сообщения! "
                        + "Подключение с {0} было разорвано!", user.name));
                    userList.Remove(user);
                    userList.SendNamesToAll();
                    return;
                }
                //Log.Write("получил: "+mess);
                int splitter = mess.IndexOf(' ');
                string comm, prms;
                if (splitter > 0)
                {
                    comm = mess.Substring(0, splitter);
                    prms = mess.Substring(splitter + 1);
                }
                else
                {
                    comm = mess;
                    prms = "";
                }
                try
                {
                    commandsMap[comm](user, prms);
                }
                catch (KeyNotFoundException)
                {
                    user.SendError("001");
                    Log.Write("Неизвестная команда: " + mess);
                }
                catch (FormatException fe)
                {
                    user.SendError("001");
                    Log.Write("Ожидалось: " + fe.Message + ". Получено: " + mess);
                }
                catch (IOException)
                {
                    Log.Write("Ошибка отправки сообщения. Соединение будет разорвано.");
                    userList.Remove(user);
                    return;
                }
            }
        }

        public void Stop()
        {
            Log.Write("Остановка..");
            userList.SendErrorToAll("100");
            userList.RemoveAll();
            if (listener != null) listener.Stop();
            if (findingService != null) findingService.Stop();
            stopped = true;
            if(serverThread != null) serverThread.Join();
            Log.Write("Сервер остановлен!");
        }
#if !TESTING
        public void ClearUsersDB()
        {
            register.RemoveAll();
        }
#endif
    }
}
