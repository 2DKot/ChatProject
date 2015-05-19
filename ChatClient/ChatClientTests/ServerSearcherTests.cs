using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ChatClient;
using NSubstitute;

namespace ChatClientTests
{
    [TestFixture]
    class ServerSearcherTests
    {
        IUdpClient client;
        ServerSearcher searcher;

        [SetUp]
        public void TestInit()
        {
            client = Substitute.For<IUdpClient>();
            searcher = new ServerSearcher(client, 667);
        }

        //--FindIPsServersTEST--\\
        [Test]
        public void FindIPsServersTest_ReceivesOneIEP_ChangesField()
        {
            //Arrange
            IPEndPoint iep = null;
            searcher.SetFindingStatus();
            IPEndPoint res = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 668);
            client.Receive(ref iep).Returns( x =>
            {
                x[0] = res;
                return Encoding.UTF8.GetBytes("IAMSERV RuRuRu");
            });
            client.When(x => x.Receive(ref res)).Throw(new SocketException());
            //Act
            searcher.FindIPsServers_PublicWrapper();
            //Assert
            Assert.IsFalse(searcher.FindingStatus);
            client.ReceivedWithAnyArgs(2).Receive(ref iep);
            res.Port = 666;
            Assert.IsTrue(searcher.FindedIpEPs.ContainsKey("RuRuRu"));
            Assert.AreEqual(searcher.FindedIpEPs["RuRuRu"], res);
        }

        [Test]
        public void FindIPsServersTest_ReceivesNoOne_ChangesField()
        {
            //Arrange
            IPEndPoint iep = null;
            searcher.SetFindingStatus();
            client.When(x => x.Receive(ref iep)).Throw(new SocketException());
            //Act
            searcher.FindIPsServers_PublicWrapper();
            //Assert
            Assert.IsFalse(searcher.FindingStatus);
            client.ReceivedWithAnyArgs(1).Receive(ref iep);
            Assert.AreEqual(0, searcher.FindedIpEPs.Count); 
        }

        [Test]
        public void FindIPsServersTest_ReceivesSomeIEP_ChangesField()
        {
            //Arrange
            searcher.SetFindingStatus();
            IPEndPoint iep = null;
            IPEndPoint res1 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 668);
            client.Receive(ref iep).Returns(x =>
                {
                    x[0] = res1;
                    return Encoding.UTF8.GetBytes("IAMSERV " + "Kongo");
                });
            res1.Port = 666;
            IPEndPoint res2 = new IPEndPoint(IPAddress.Parse("34.120.45.78"), 668);
            client.Receive(ref res1).Returns(x =>
            {
                x[0] = res2;
                return Encoding.UTF8.GetBytes("IAMSERV " + "BuGaGa");
            });
            res2.Port = 666;
            client.When(x => x.Receive(ref res2)).Throw(new SocketException());
            //Act
            searcher.FindIPsServers_PublicWrapper();
            //Assert
            Assert.IsFalse(searcher.FindingStatus);
            client.ReceivedWithAnyArgs(3).Receive(ref iep);
            Assert.AreEqual(2, searcher.FindedIpEPs.Count);
            Assert.IsTrue(searcher.FindedIpEPs.ContainsKey("Kongo"));
            Assert.AreSame(res1, searcher.FindedIpEPs["Kongo"]);
            Assert.IsTrue(searcher.FindedIpEPs.ContainsKey("BuGaGa"));
            Assert.AreSame(res2, searcher.FindedIpEPs["BuGaGa"]);
        }

        [Test]
        public void FindIPsServersTest_ReceivesSameIEP_ChangesField()
        {
            //Arrange
            searcher.SetFindingStatus();
            IPEndPoint iep = null;
            IPEndPoint res1 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 668);
            client.Receive(ref iep).Returns(x =>
            {
                x[0] = res1;
                return Encoding.UTF8.GetBytes("IAMSERV " + "Kongo");
            });
            res1.Port = 666;
            IPEndPoint res2 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 668);
            client.Receive(ref res1).Returns(x =>
            {
                x[0] = res2;
                return Encoding.UTF8.GetBytes("IAMSERV " + "Kongo1");
            });
            res2.Port = 666;
            client.When(x => x.Receive(ref res2)).Throw(new SocketException());
            //Act
            searcher.FindIPsServers_PublicWrapper();
            //Assert
            Assert.IsFalse(searcher.FindingStatus);
            client.ReceivedWithAnyArgs(2).Receive(ref iep);
            Assert.AreEqual(1, searcher.FindedIpEPs.Count);
            Assert.IsTrue(searcher.FindedIpEPs.ContainsKey("Kongo"));
            Assert.AreSame(res1, searcher.FindedIpEPs["Kongo"]);
            Assert.IsTrue(!searcher.FindedIpEPs.ContainsKey("BuGaGa"));
        }

        //--SendBroadcastMessageTEST--\\
        [Test]
        public void SendBroadcastMessageTest_CallsClientMethod_MatchesWithCalling()
        {
            //Arrangement
            string command = "FINDSERVER";
            //Act
            searcher.SendBroadcastMessage_PublicWrapper();
            //Arrangement
            client.Received(1).Send(Arg.Is<byte[]>(x => command == System.Text.Encoding.UTF8.GetString(x)), Arg.Any<int>(), Arg.Any<IPEndPoint>());
        }

        [Test]
        public void SendBroadcastMessageTest_ClientIsNull_MatchesWithCalling()
        {
            //Arrangement
            client.IsClientNull().ReturnsForAnyArgs(true);
            string command = "FINDSERVER";
            //Act
            searcher.SendBroadcastMessage_PublicWrapper();
            //Arrangement
            client.Received().InitializeUdpClient();
            client.Received(1).Send(Arg.Is<byte[]>(x => command == System.Text.Encoding.UTF8.GetString(x)), Arg.Any<int>(), Arg.Any<IPEndPoint>());
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Поиск уже производится!")]
        public void StartSearchingServersTest_DoubleCalls_ThrowsException()
        {
            //Act & Assert
            searcher.StartSearchingServers();
            searcher.StartSearchingServers();
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Клиент прекратил работу и не может быть завершен еще раз.")]
        public void EndSearchingServersTest_DoubleCalls_ThrowsException()
        {
            //Act & Assert
            searcher.EndSearchingServers();
        }

        [Test]
        public void StartAndEndSearchingServersTest_ReceivesOneIpAdress_ChangesField()
        {
            //Act
            searcher.StartSearchingServers();
            searcher.EndSearchingServers();
            //Assert
            Assert.IsFalse(searcher.FindingStatus);
        }

        [Test]
        public void StartAndEndSearchingServersTest_CallsSeveralTimes_ChangesField()
        {
            searcher.StartSearchingServers();
            searcher.EndSearchingServers();
            searcher.StartSearchingServers();
            searcher.EndSearchingServers();
            //Assert
            Assert.IsFalse(searcher.FindingStatus);
        }
    }
}
