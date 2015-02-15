using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatServer
{
    class Register
    {
        const string regFile = "users.txt";
        ConcurrentDictionary<string, string> regUnits = new ConcurrentDictionary<string, string>();

        public bool Load()
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

        public bool Save()
        {
            try
            {
                StreamWriter writer = new StreamWriter(regFile);
                foreach (var unit in regUnits)
                {
                    writer.WriteLine("{0} {1}", unit.Key, unit.Value);
                }
                writer.Close();
            }
            catch { return false; }
            return true;
        }

        public bool Add(string name, string password)
        {
            if (regUnits.ContainsKey(name))
            {
                return false;
            }
            regUnits.AddOrUpdate(name, password, (key, val) => val);
            return true;
        }

        public bool Remove(string name)
        {
            /*
            if (!regUnits.ContainsKey(name))
            {
                return false;
            }
            regUnits.Remove(name);
            
            return true;
            */
            return false;
        }

        public bool Check(string name, string password)
        {
            return regUnits[name] == password;
        }

        public bool Contains(string name)
        {
            return regUnits.ContainsKey(name);
        }
    }
}
