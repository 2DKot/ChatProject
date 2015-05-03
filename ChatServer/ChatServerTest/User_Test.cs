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
    public class User_Test
    {
        [Test]
        public void SendMessage()
        {
            string msg = "Привет!";
            FakeClient fclient = new FakeClient();
            Thread thread = new Thread(()=>fclient.Connect());
            thread.Start();
            TcpListener listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            User user = new User(listener.AcceptTcpClient(), "Юзер");
            listener.Stop();
            thread.Join();
            thread = new Thread(()=>user.SendMessage(msg));
            thread.Start();
            string result = fclient.RecieveMessage();
            thread.Join();
            Assert.AreEqual(msg, result);
        }

        [Test]
        public void SendError()
        {
            string expected = "ERROR 101";
            FakeClient fclient = new FakeClient();
            new Thread(() => fclient.Connect()).Start();
            TcpListener listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            User user = new User(listener.AcceptTcpClient(), "Юзер");
            listener.Stop();
            new Thread(() => user.SendError("101")).Start();
            string result = fclient.RecieveMessage();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SendYouAre()
        {
            string expected = "YOUARE Юзер";
            FakeClient fclient = new FakeClient();
            new Thread(() => fclient.Connect()).Start();
            TcpListener listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            User user = new User(listener.AcceptTcpClient(), "Юзер");
            listener.Stop();
            new Thread(() => user.SendYouAre()).Start();
            string result = fclient.RecieveMessage();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetNextMessage_CorrectMessage()
        {
            string msg = "Привет!";
            FakeClient fclient = new FakeClient();
            new Thread(() => fclient.Connect()).Start();
            TcpListener listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            User user = new User(listener.AcceptTcpClient(), "Юзер");
            listener.Stop();
            new Thread(() => fclient.SendMessage(msg)).Start();
            string result = user.GetNextMessage();
            Assert.AreEqual(msg, result);
        }

        [Test]
        [ExpectedException(typeof(SocketException))]
        public void GetNextMessage_NullSize()
        {
            FakeClient fclient = new FakeClient();
            new Thread(() => fclient.Connect()).Start();
            TcpListener listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            User user = new User(listener.AcceptTcpClient(), "Юзер");
            listener.Stop();
            byte[] bytes = new byte[4];
            bytes[0] = 0;
            bytes[1] = 0;
            bytes[2] = 0;
            bytes[3] = 0;
            new Thread(() => fclient.SendBytes(bytes)).Start();
            user.GetNextMessage();
        }
    }
}
