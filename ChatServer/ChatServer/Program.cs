using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
#if !TESTING
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerForm());
#endif
        }
    }
}

