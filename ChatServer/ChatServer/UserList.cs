using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer
{
    public delegate void ListChangedHandler(List<User> users);
    public class UserList
    {
        public event ListChangedHandler listChangedHandler;
        List<User> users;

        public UserList()
        {
            users = new List<User>();
        }

        public void OnListChanged()
        {
            if (listChangedHandler != null)
            {
                listChangedHandler(users);
            }
        }

        public void Add(User user)
        {
            users.Add(user);
            OnListChanged();
        }

        public void Remove(User user)
        {
            user.client.Close();
            if (!users.Remove(user))
            {
                throw new InvalidOperationException("Ошибка при удалении юзера из списка!");
            }
            OnListChanged();
        }

        public void RemoveAll()
        {
            lock (users)
            {
                foreach (User user in users)
                {
                    user.client.Close();
                }
                users.Clear();
                OnListChanged();
            }
        }

        public User this[int i]
        {
            get { return users[i]; }
        }

        public User Find(string name)
        {
            foreach (User user in users)
            {
                if (user.name == name) return user;
            }
            return null;
        }

        public void SendMessageToAll(string message)
        {
            //Console.WriteLine("Send to all: " + message);
            lock (users)
            {
                foreach (User target in users)
                {
                    target.SendMessage(message);
                }
            }
        }

        public void SendErrorToAll(string code)
        {
            SendMessageToAll("ERROR " + code);
        }

        public void SendNamesToAll()
        {
            string message = "NAMES";
            foreach (User one in users)
            {
                message += " " + one.name;
            }
            SendMessageToAll(message);
        }
    }
}
