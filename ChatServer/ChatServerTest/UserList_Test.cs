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
    public class UserList_Test
    {
        [Test]
        public void AddUsers_FindUser()
        {
            User expected = new User(null, "СуперЮзер");
            UserList ulist = new UserList();
            ulist.Add(expected);
            ulist.Add(new User(new TcpClient(), "ДругойЮзер"));
            User result = ulist.Find("СуперЮзер");
            Assert.AreSame(expected, result, expected.name + " и " + result.name);
        }

        [Test]
        public void FindUserByName_FindNotExistingUser()
        {
            UserList ulist = new UserList();
            User result = ulist.Find("Не существующий Юзер");
            Assert.AreSame(null, result);
        }

        [Test]
        public void RemoveUser_UserExists()
        {
            UserList ulist = new UserList();
            User user = new User(new TcpClient(), "СуперЮзер");
            ulist.Add(user);
            User result = ulist.Find("СуперЮзер");
            Assert.AreEqual(user, result, "Юзер не добавился!");
            ulist.Remove(user);
            result = ulist.Find("СуперЮзер");
            Assert.AreSame(null, result, "Удаление юзера не сработало!");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveUser_UserNotExists()
        {
            UserList ulist = new UserList();
            User user = new User(new TcpClient(), "СуперЮзер");
            ulist.Remove(user);
        }

        [Test]
        public void RemoveAllUsers()
        {
            UserList ulist = new UserList();
            ulist.Add(new User(new TcpClient(), "ЮзерОдин"));
            ulist.Add(new User(new TcpClient(), "ЮзерДва"));
            ulist.RemoveAll();
            User result = ulist.Find("ЮзерОдин");
            Assert.AreEqual(null, result);
            result = ulist.Find("ЮзерДва");
            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetUserById_UserExists()
        {
            UserList ulist = new UserList();
            ulist.Add(new User(null, "ЮзерНоль"));
            User expected = new User(null, "ЮзерОдин");
            ulist.Add(expected);
            ulist.Add(new User(null, "ЮзерДва"));
            User result = ulist[1];
            Assert.AreSame(expected, result, expected.name + " и " + result.name);
        }

        [Test]
        public void ListChangedHandler()
        {
            UserList ulist = new UserList();
            int changesCount = 0;
            ulist.listChangedHandler += (list) => changesCount++;
            User user = new User(new TcpClient(), "юзер1");
            ulist.Add(user);
            ulist.Remove(user);
            ulist.Add(user);
            ulist.RemoveAll();
            Assert.AreEqual(4, changesCount);
        }

        [Test]
        public void SendMessageToAll()
        {
            string msg = "Привет!";
            UserList ulist = new UserList();
            FakeClient fclient_1 = new FakeClient();
            FakeClient fclient_2 = new FakeClient();
            TcpListener listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            Thread thread = new Thread(() => fclient_1.Connect());
            thread.Start();
            ulist.Add(new User(listener.AcceptTcpClient(), "Юзер1"));
            thread.Join();
            thread = new Thread(() => fclient_2.Connect());
            thread.Start();
            ulist.Add(new User(listener.AcceptTcpClient(), "Юзер2"));
            thread.Join();
            listener.Stop();
            
            thread = new Thread(()=> ulist.SendMessageToAll(msg));
            thread.Start();
            string result_1 = fclient_1.RecieveMessage();
            string result_2 = fclient_2.RecieveMessage();
            thread.Join();
            Assert.AreEqual(msg, result_1);
            Assert.AreEqual(msg, result_2);
        }

        [Test]
        public void SendErrorToAll()
        {
            UserList ulist = new UserList();
            FakeClient fclient_1 = new FakeClient();
            FakeClient fclient_2 = new FakeClient();
            TcpListener listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            Thread thread = new Thread(() => fclient_1.Connect());
            thread.Start();
            ulist.Add(new User(listener.AcceptTcpClient(), "Юзер1"));
            thread.Join();
            thread = new Thread(() => fclient_2.Connect());
            thread.Start();
            ulist.Add(new User(listener.AcceptTcpClient(), "Юзер2"));
            thread.Join();
            listener.Stop();

            thread = new Thread(() => ulist.SendErrorToAll("101"));
            thread.Start();
            string result_1 = fclient_1.RecieveMessage();
            string result_2 = fclient_2.RecieveMessage();
            thread.Join();
            Assert.AreEqual("ERROR 101", result_1);
            Assert.AreEqual("ERROR 101", result_2);
        }

        [Test]
        public void SendNamesToAll()
        {
            string expected = "NAMES Юзер1 Юзер2";
            UserList ulist = new UserList();
            FakeClient fclient_1 = new FakeClient();
            FakeClient fclient_2 = new FakeClient();
            TcpListener listener = new TcpListener(IPAddress.Any, 666);
            listener.Start();
            Thread thread = new Thread(() => fclient_1.Connect());
            thread.Start();
            ulist.Add(new User(listener.AcceptTcpClient(), "Юзер1"));
            thread.Join();
            thread = new Thread(() => fclient_2.Connect());
            thread.Start();
            ulist.Add(new User(listener.AcceptTcpClient(), "Юзер2"));
            thread.Join();
            listener.Stop();

            thread = new Thread(() => ulist.SendNamesToAll());
            thread.Start();
            string result_1 = fclient_1.RecieveMessage();
            string result_2 = fclient_2.RecieveMessage();
            thread.Join();
            Assert.AreEqual(expected, result_1);
            Assert.AreEqual(expected, result_2);
        }
    }
}
