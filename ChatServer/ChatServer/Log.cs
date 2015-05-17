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
        public static Object handler = new Object();
        static string fileName = "log.txt";

        public static void Write(string msg)
        {
            lock (handler)
            {
                string time = DateTime.Now.ToShortDateString() + " "
                    + DateTime.Now.ToLongTimeString();
                msg = time + " " + msg;
                if (LogEvent != null)
                {
                    LogEvent(msg);
                }
                StreamWriter sw = new StreamWriter(fileName, true);
                sw.WriteLine(msg);
                sw.Close();
            }
        }

        public static void Delete()
        {
            lock (handler)
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }
    }
}
