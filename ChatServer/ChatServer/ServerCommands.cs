using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer
{
    partial class Server
    {
        void MSG(User user, string prms)
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

        void NICK(User user, string prms)
        {
            string newName = prms.Split(' ')[0];
            if (FindUserByName(newName) != null)
            {
                SendMessage(user, "ERROR 051");
                return;
            }
            Console.WriteLine(user.name + " changed nick to " + newName);
            SendMessage(("MSG " + user.name + " изменил ник на " + newName));
            user.name = newName;
            SendMessage(user, "ERROR 050");
            SendNamesToAll();
        }

        void PRIVMSG(User user, string prms)
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
            string formattedMessage = "PRIVMSG " + user.name + ": " + message;
            if (target == null)
            {
                SendMessage(user, "ERROR 003");
                return;
            }
            try
            {
                SendMessage(target, formattedMessage);
            }
            catch
            {
                SendMessage(user, "ERROR 002");
                return;
            }
            SendMessage(user, formattedMessage);
        }

        void NAMES(User user, string prms)
        {
            string message = "NAMES";
            foreach (User one in users)
            {
                message += " " + one.name;
            }
            SendMessage(user, message);
        }

        void DATE(User user, string prms)
        {
            SendMessage(user, "MSG "+DateTime.Now.Date.ToLongDateString());
        }
    }
}
