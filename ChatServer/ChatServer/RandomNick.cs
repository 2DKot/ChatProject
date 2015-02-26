using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer
{
    class RandomNick
    {
        List<string> names = new List<string>() {
            "Жыраф",
            "Креведко",
            "Медвед",
            "Ниндзя",
            "Школоло",
            "Панда",
            "Панк"
        };

        List<string> adjectivies = new List<string>() {
            "Упоротый",
            "Весёлый",
            "Отважный",
            "Гламурный",
            "Чоткий"
        };

        HashSet<string> takedNicks = new HashSet<string>();

        public string GetNew()
        {
            Random rnd = new Random();
            string nick;
            do
            {
                int adI = rnd.Next(adjectivies.Count);
                int nameID = rnd.Next(names.Count);
                nick = adjectivies[adI] + names[nameID];
            } while (takedNicks.Contains(nick));
            takedNicks.Add(nick);
            return nick;
        }

        public void Remove(string name)
        {
            takedNicks.Remove(name);
        }
    }
}
