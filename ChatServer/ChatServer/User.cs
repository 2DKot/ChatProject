using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace ChatServer
{
    class User
    {
        public string name;
        public TcpClient client;
        static UserList users = new UserList();

        public static void RemoveAll()
        {
            lock (users)
            {
                foreach (User user in users)
                {
                    user.client.Close();
                }
                users.Clear();
            }
        }

        public static void OnListChanged()
        {
            users.OnListChanged();
        }

        public static void SetOnListChangedHandler(ListChangedHandler h)
        {
            users.listChangedHandler += h;
        }

        public static void SendMessageToAll(string message)
        {
            //Console.WriteLine("Send to all: " + message);
            lock (users)
            {
                foreach (User target in users)
                {
                    try
                    {
                        target.SendMessage(message);
                    }
                    catch
                    {
                        Log.Write("Проблемка!");
                    }
                }
            }
        }

        public static void SendNamesToAll()
        {
            string message = "NAMES";
            foreach (User one in users)
            {
                message += " " + one.name;
            }
            User.SendMessageToAll(message);
        }

        public static void SendErrorToAll(string code)
        {
            User.SendMessageToAll("ERROR " + code);
        }

        public static User Find(string name)
        {
            foreach (User user in users)
            {
                if (user.name == name) return user;
            }
            return null;
        }

        public static User Get(int i)
        {
            return users[i];
        }

        public User(TcpClient client, string name)
        {
            this.client = client;
            this.name = name;
            users.Add(this);
        }

        public void Remove()
        {
            lock (users)
            {
                users.Remove(this);
                this.client.Close();
            }
        }

        public NetworkStream GetStream()
        {
            return client.GetStream();
        }

        public string GetNextMessage()
        {
            NetworkStream ns = GetStream();
            byte[] sizeBuffer = new byte[4];
            try
            {
                ns.Read(sizeBuffer, 0, 4);
            }
            catch (IOException)
            {
                throw new SocketException();
            }
            int size = BitConverter.ToInt32(sizeBuffer, 0);
            if (size < 1)
            {
                throw new SocketException();
            }
            byte[] messageBuffer = new byte[size];
            ns.Read(messageBuffer, 0, size);
            string message = Encoding.UTF8.GetString(messageBuffer);
            if (message.Trim() == "" && !client.Connected)
            {
                throw new SocketException();
            }
            return message;
        }

        public void SendMessage(string message)
        {
            //Log.Write("Отправляю: " + message);
            NetworkStream ns = GetStream();
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
            int size = messageBuffer.Count();
            byte[] sizeBuffer = BitConverter.GetBytes(size);
            ns.Write(sizeBuffer, 0, 4);
            ns.Write(messageBuffer, 0, size);
        }  

        public void SendYouAre()
        {
            SendMessage("YOUARE " + name);
        }

        public void SendError(string code)
        {
            SendMessage("ERROR " + code);
        }        
    }
}
