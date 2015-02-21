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
            "Медвед"
        };

        List<string> adjectivies = new List<string>() {
            "Упоротый",
            "Весёлый",
            "Отважный"
        };

        HashSet<string> takedNicks = new HashSet<string>();

        public string GetNew()
        {
            Random rnd = new Random();
            string nick;
            do
            {
                nick = adjectivies[rnd.Next(names.Count)] + names[rnd.Next(names.Count)];
            } while (takedNicks.Contains(nick));
            return nick;
        }
    }
}
