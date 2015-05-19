using System;
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
        FakeClient fclient;
        User user;

        [SetUp]
        public void ConnectFakeClientToServerClient()
        {
            fclient = new FakeClient();
            Thread thread = new Thread(() => fclient.Connect());
            thread.Start();
            TcpListener listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            user = new User(listener.AcceptTcpClient(), "Юзер");
            listener.Stop();
            thread.Join();
        }

        [TearDown]
        public void CloseConnection()
        {
            fclient.client.Close();
            fclient = null;
            user.client.Close();
            user = null;
        }

        [Test]
        public void SendMessage()
        {
            string msg = "Привет!";
            
            Thread thread = new Thread(()=>user.SendMessage(msg));
            thread.Start();
            string result = fclient.RecieveMessage();
            thread.Join();
            Assert.AreEqual(msg, result);
        }

        [Test]
        public void SendError()
        {
            string expected = "ERROR 101";
           
            Thread thread = new Thread(() => user.SendError("101"));
            thread.Start();
            string result = fclient.RecieveMessage();
            thread.Join();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SendYouAre()
        {
            string expected = "YOUARE Юзер";
            
            Thread thread = new Thread(() => user.SendYouAre());
            thread.Start();
            string result = fclient.RecieveMessage();
            thread.Join();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetNextMessage_CorrectMessage()
        {
            string msg = "Привет!";
           
            Thread thread = new Thread(() => fclient.SendMessage(msg));
            thread.Start();
            string result = user.GetNextMessage();
            thread.Join();
            Assert.AreEqual(msg, result);
        }

        [Test]
        [ExpectedException(typeof(SocketException))]
        public void GetNextMessage_NullSize()
        {
            byte[] bytes = new byte[4];
            bytes[0] = 0;
            bytes[1] = 0;
            bytes[2] = 0;
            bytes[3] = 0;
            Thread thread = new Thread(() => fclient.SendBytes(bytes));
            thread.Start();
            user.GetNextMessage();
        }
    }
}
