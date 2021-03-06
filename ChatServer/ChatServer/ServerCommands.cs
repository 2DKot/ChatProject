﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer
{
    partial class Server
    {
        void MSG(User user, string prms)
        {
            if (prms=="")
            {
                user.SendError("001");
                throw new FormatException("MSG <msg>");
            }
            string message = "MSG " + user.name + ": " + prms;
            userList.SendMessageToAll(message);
        }

        void NICK(User user, string prms)
        {
            rndNick.Remove(user.name);
            string newName = rndNick.GetNew();
            Log.Write(user.name + " changed nick to " + newName);
            userList.SendMessageToAll(("MSG " + user.name + " изменил ник на " + newName));
            user.name = newName;
            user.SendError("050");
            userList.SendNamesToAll();
            user.SendYouAre();
            userList.OnListChanged();
        }

        void PRIVMSG(User user, string prms)
        {
            int splitter = prms.IndexOf(' ');
            if (splitter == -1)
            {
                user.SendError("001");
                throw new FormatException("PRIVMSG <username> <msg>");
            }
            string targetName = prms.Substring(0, splitter);
            string message = prms.Substring(splitter + 1);
            User target = userList.Find(targetName);
            string formattedMessage = "PRIVMSG " + user.name + ": " + message;
            if (target == null)
            {
                user.SendError("003");
                return;
            }
            try
            {
                target.SendMessage(formattedMessage);
            }
            catch
            {
                user.SendError("002");
                return;
            }
            user.SendMessage(formattedMessage);
        }

        /*
        void NAMES(User user, string prms)
        {
            string message = "NAMES";
            foreach (User one in users)
            {
                message += " " + one.name;
            }
            SendMessage(user, message);
        }
        */
        
        /*
        void DATE(User user, string prms)
        {
            user.SendMessage("MSG " + DateTime.Now.Date.ToLongDateString());
        }
        */

        void REG(User user, string prms)
        {
            string[] splitted = prms.Split(' ');
            if (splitted.Count() < 2)
            {
                throw new FormatException("REG <name> <password>");
            }
            if (register.Add(splitted[0], splitted[1]))
            {
                user.SendError("053");
                register.   Save();
            }
            else
            {
                user.SendError("051");
            }
        }

        void LOGIN(User user, string prms)
        {
            string[] splitted = prms.Split(' ');
            if (splitted.Count() < 2)
            {
                throw new FormatException("LOGIN <name> <password>");
            }
            string name = splitted[0];
            string password = splitted[1];
            if (register.Contains(name) && register.Check(name, password))
            {
                user.name = name;
                user.SendError("055");
                userList.SendNamesToAll();
                userList.SendMessageToAll("MSG " + name + " вернулся к нам!");
                user.SendYouAre();
                Log.Write(name + " зашёл.");
                userList.OnListChanged();
            }
            else
            {
                user.SendError("054");
            }
        }

        void WHOIAM(User user, string prms)
        {
            user.SendMessage("YOUARE " + user.name);
        }
    }
}
