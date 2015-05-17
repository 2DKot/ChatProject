using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatServer
{
    public class Register
    {
        const string regFile = "users.txt";
        ConcurrentDictionary<string, string> regUnits = new ConcurrentDictionary<string, string>();
        Object handler = new Object();

        public void RemoveAll()
        {
            lock (handler)
            {
                File.Delete(regFile);
            }
        }

        public bool Load()
        {
            lock (handler)
            {
                if (!File.Exists(regFile)) return false;

                regUnits = new ConcurrentDictionary<string, string>();
                StreamReader reader = new StreamReader(regFile);
                while (!reader.EndOfStream)
                {
                    string[] unitRaw = reader.ReadLine().Split(' ');
                    regUnits.AddOrUpdate(unitRaw[0], unitRaw[1], (key, value) => value);
                }
                reader.Close();
                return true;
            }
        }

        public void Save()
        {
            lock (handler)
            {
                StreamWriter writer = new StreamWriter(regFile);
                foreach (var unit in regUnits)
                {
                    writer.WriteLine("{0} {1}", unit.Key, unit.Value);
                }
                writer.Close();
            }
        }

        public bool Add(string name, string password)
        {
            if (regUnits.ContainsKey(name))
            {
                return false;
            }
            bool result = true;
            regUnits.AddOrUpdate(name, password, (key, val) => {
                result = false;
                return val;
            });
            return result;
        }

        public bool Check(string name, string password)
        {
            try
            {
                return regUnits[name] == password;
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return false;
            }
        }

        public bool Contains(string name)
        {
            return regUnits.ContainsKey(name);
        }
    }
}
