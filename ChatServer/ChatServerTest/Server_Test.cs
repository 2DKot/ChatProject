using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatServer;
using NUnit.Framework;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ChatServerTest
{
    [TestFixture]
    class Server_Test
    {
        [Test]
        public void Connecting()
        {
            Server server = new Server();
            server.Start();
            FakeClient fclient = new FakeClient();
            fclient.Connect();

            string recMsg = fclient.RecieveMessage();
            string expMsg = "YOUARE";
            Assert.AreEqual(expMsg, recMsg.Substring(0, expMsg.Length));
            string name = recMsg.Substring(expMsg.Length+1);

            recMsg = fclient.RecieveMessage();
            expMsg = "MSG Тебя приветствует сервер";
            Assert.AreEqual(expMsg, recMsg.Substring(0,expMsg.Length));
            
            recMsg = fclient.RecieveMessage();
            expMsg = "MSG " + name + " присоединился к чату.";
            Assert.AreEqual(expMsg, recMsg);

            recMsg = fclient.RecieveMessage();
            expMsg = "NAMES " + name;
            Assert.AreEqual(expMsg, recMsg);
            
            server.Stop();
        }

        [Test]
        public void Connecting()
        {
            Server server = new Server();
            server.Start();
            FakeClient fclient = new FakeClient();
            fclient.Connect();

            fclient.SkipFirstMessages();

            fclient.SendMessage("NICK SuperЮзер");

            string recMsg = fclient.RecieveMessage();
            string expMsg = "YOUARE";
            Assert.AreEqual(expMsg, recMsg.Substring(0, expMsg.Length));
            string name = recMsg.Substring(expMsg.Length + 1);

            server.Stop();
        }
    }
}
