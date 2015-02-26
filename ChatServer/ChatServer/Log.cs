using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatServer
{
    public delegate void LogHandler(string msg);
    class Log
    {
        public static event LogHandler LogEvent;

        public static void Write(string msg)
        {
            string time = DateTime.Now.ToShortDateString() + " "
                + DateTime.Now.ToLongTimeString();
            msg = time + " " + msg;
            if (LogEvent != null)
            {
                LogEvent(msg);
            }
            StreamWriter sw = new StreamWriter("log.txt", true);
            sw.WriteLine(msg);
            sw.Close();
        }

        public static void Delete()
        {
            if(File.Exists("log.txt"))
            {
                File.Delete("log.txt");
            }
        }
    }
}
