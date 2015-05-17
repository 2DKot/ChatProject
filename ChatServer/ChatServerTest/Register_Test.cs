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
    class Register_Test
    {
        [Test]
        public void RegisterAndCheckSomeUsers()
        {
            Register reg = new Register();
            reg.RemoveAll();
            reg.Add("юзер1", "123");
            reg.Add("юзер2", "32");
            Assert.IsTrue(reg.Contains("юзер1"));
            Assert.IsTrue(reg.Contains("юзер2"));
            Assert.IsTrue(reg.Check("юзер1", "123"));
            Assert.IsTrue(reg.Check("юзер2", "32"));
        }

        [Test]
        public void CheckNotExistingUser()
        {
            Register reg = new Register();
            reg.RemoveAll();
            Assert.IsFalse(reg.Contains("юзер"));
            Assert.IsFalse(reg.Check("юзер", "пароль"));
        }

        [Test]
        public void AddExistingUser()
        {
            Register reg = new Register();
            reg.RemoveAll();
            reg.Add("юзер", "123");
            Assert.IsFalse(reg.Add("юзер", ""));
        }

        [Test]
        public void Load_DataBaseDoesNotExist()
        {
            Register reg = new Register();
            reg.RemoveAll();
            Assert.IsFalse(reg.Load());
        }

        [Test]
        public void Load_Success()
        {
            Register reg = new Register();
            reg.RemoveAll();
            reg.Add("юзер", "суперюзер");
            reg.Save();
            reg = new Register();
            Assert.IsFalse(reg.Check("юзер", "суперюзер"));
            Assert.IsTrue(reg.Load());
            Assert.IsTrue(reg.Check("юзер", "суперюзер"));
        }
    }
}
