using System;
using ChatServer;
using NUnit.Framework;

namespace ChatServerTest
{
    [TestFixture]
    class Server_Test
    {
        Server server;
        FakeClient fclient;
        [SetUp]
        public void CreateAndStartServer()
        {
            server = new Server();
            server.Start();
            fclient = new FakeClient();
            fclient.Connect();
        }

        [TearDown]
        public void StopAndDeleteServer()
        {
            server.Stop();
            server = null;
        }

        [Test]
        public void Connecting()
        {
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
        }

        [Test]
        public void Registration_Success()
        {
            server.register.RemoveAll();

            fclient.SkipFirstMessages();

            fclient.SendAndRecieve("REG myUser 56789", "ERROR 053");
            Assert.IsTrue(server.register.Check("myUser", "56789"));
        }

        [Test]
        public void Registration_UserAlreadyExists()
        {
            server.register.RemoveAll();
            server.register.Add("myUser", "56789");
            fclient.SkipFirstMessages();

            fclient.SendAndRecieve("REG myUser 1234", "ERROR 051");
        }

        [Test]
        public void Registration_WrongFormat()
        {
            fclient.SkipFirstMessages();

            fclient.SendAndRecieve("REG абвгд", "ERROR 001");
        }

        [Test]
        public void Login_Success()
        {
            server.register.RemoveAll();
            server.register.Add("myUser", "56789");

            fclient.SkipFirstMessages();

            fclient.SendAndRecieve("LOGIN myUser 56789", "ERROR 055");
        }

        [Test]
        public void Login_UserDoesNotExist()
        {
            server.register.RemoveAll();

            fclient.SkipFirstMessages();

            fclient.SendAndRecieve("LOGIN myUser 1234", "ERROR 054");
        }

        [Test]
        public void Login_WrongPassword()
        {
            server.register.RemoveAll();
            server.register.Add("myUser", "56789");

            fclient.SkipFirstMessages();

            fclient.SendAndRecieve("LOGIN myUser 1234", "ERROR 054");
        }

        [Test]
        public void Login_WrongFormat()
        {
            fclient.SkipFirstMessages();

            fclient.SendAndRecieve("LOGIN my", "ERROR 001");
        }

        [Test]
        public void PrivMsg_Success()
        {
            fclient.SkipFirstMessages();
            string name1 = server.userList[0].name;

            FakeClient fclient2 = new FakeClient();
            fclient2.Connect();
            fclient2.SkipFirstMessages();
            string name2 = server.userList[1].name;

            fclient.RecieveMessage();
            fclient.RecieveMessage();
            string sendMSG = "PRIVMSG " + name2 + " привет, юзер!";
            string recMSG = "PRIVMSG " + name1 + ": привет, юзер!";
            fclient.SendAndRecieve(sendMSG, recMSG);

            string msg = fclient2.RecieveMessage();
            Assert.AreEqual(recMSG, msg);
        }

        [Test]
        public void PrivMsg_UserDoesNotExist()
        {
            fclient.SkipFirstMessages();

            fclient.SendAndRecieve("PRIVMSG notExistingUser mess asd", "ERROR 003");
        }


        [Test]
        public void PrivMsg_WrongFormat()
        {
            fclient.SkipFirstMessages();

            fclient.SendAndRecieve("PRIVMSG абвгд", "ERROR 001");
        }

        [Test]
        public void Msg_Success()
        {
            fclient.SkipFirstMessages();
            string name1 = server.userList[0].name;

            FakeClient fclient2 = new FakeClient();
            fclient2.Connect();
            fclient2.SkipFirstMessages();
            string name2 = server.userList[1].name;

            fclient.RecieveMessage();
            fclient.RecieveMessage();
            string sendMSG = "MSG привет всем!";
            string recMSG = "MSG " + name1 + ": привет всем!";
            fclient.SendAndRecieve(sendMSG, recMSG);

            string msg = fclient2.RecieveMessage();
            Assert.AreEqual(recMSG, msg);
        }

        [Test]
        public void Msg_WrongFormat()
        {
            fclient.SkipFirstMessages();

            fclient.SendAndRecieve("MSG", "ERROR 001");
        }

        [Test]
        public void WhoIAm()
        {
            fclient.SkipFirstMessages();

            string name = server.userList[0].name;

            fclient.SendAndRecieve("WHOIAM", "YOUARE " + name);
        }

        [Test]
        public void Nick()
        {
            string name = server.userList[0].name;
            fclient.SkipFirstMessages();
            
            fclient.SendMessage("NICK");
            string msg = fclient.RecieveMessage();
            StringAssert.StartsWith("MSG", msg);
            StringAssert.Contains("изменил ник на", msg);
            msg = fclient.RecieveMessage();
            Assert.AreEqual("ERROR 050", msg);
            Assert.IsNotEmpty(server.userList[0].name);
        }
    }
}
