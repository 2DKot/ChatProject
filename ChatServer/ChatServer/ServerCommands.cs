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
                        SendError(user, "002");
                    }
                }
            }
        }

        void NICK(User user, string prms)
        {
            string newName = rndNick.GetNew();
            Console.WriteLine(user.name + " changed nick to " + newName);
            SendMessage(("MSG " + user.name + " изменил ник на " + newName));
            user.name = newName;
            SendError(user, "050");
            SendNamesToAll();
            SendMessage(user, "YOUARE " + user.name);
        }

        void PRIVMSG(User user, string prms)
        {
            int splitter = prms.IndexOf(' ');
            if (splitter == -1)
            {
                SendError(user, "001");
                return;
            }
            string targetName = prms.Substring(0, splitter);
            string message = prms.Substring(splitter + 1);
            User target = FindUserByName(targetName);
            string formattedMessage = "PRIVMSG " + user.name + ": " + message;
            if (target == null)
            {
                SendError(user, "003");
                return;
            }
            try
            {
                SendMessage(target, formattedMessage);
            }
            catch
            {
                SendError(user, "002");
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

        void REG(User user, string prms)
        {
            string[] splitted = prms.Split(' ');
            if (register.Add(splitted[0], splitted[1]))
            {
                SendError(user, "053");
                register.Save();
            }
            else
            {
                SendError(user, "051");
            }
        }

        void LOGIN(User user, string prms)
        {
            string[] splitted = prms.Split(' ');
            string name = splitted[0];
            string password = splitted[1];
            if (register.Contains(name) && register.Check(name, password))
            {
                user.name = name;
                SendError(user, "055");
                SendNamesToAll();
            }
            else
            {
                SendError(user, "054");
            }
        }

        void WHOIAM(User user, string prms)
        {
            SendMessage(user, "YOUARE " + user.name);
        }
    }
}
