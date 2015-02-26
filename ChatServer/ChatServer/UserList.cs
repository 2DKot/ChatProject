using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer
{
    delegate void ListChangedHandler(List<User> users);
    class UserList
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
            users.Remove(user);
            OnListChanged();
        }

        public void Clear()
        {
            users.Clear();
            OnListChanged();
        }

        public List<User>.Enumerator GetEnumerator()
        {
            return users.GetEnumerator();
        }

        public User this[int i]
        {
            get { return users[i]; }
        }
    }
}
