﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("2DKot server");
            server.Start();
        }
    }
}
