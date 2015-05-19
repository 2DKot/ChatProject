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

        //--InitReceivingBroadcastMessagesThreadTEST--\\
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
        /*
        object[] serverIPs = new object[]
            {
                new List<Tuple<string, string>> {
                    new Tuple<string,string>("127.0.0.1", "Kongo"), 
                    new Tuple<string,string>("22.05.11.55", "Bugaga"),
                    new Tuple<string,string>("123.22.05.230", "Zevs"),
                    new Tuple<string,string>("45.55.104.25", "Fobos"), 
                    new Tuple<string,string>("15.15.33.120", ";';asd_")},

                new List<Tuple<string, string>> {
                    new Tuple<string,string>("127.0.0.1", "Rama"), 
                    new Tuple<string,string>("22.05.11.55", "LoadSS"),
                    new Tuple<string,string>("123.22.05.230", "PGS")}
            };*/
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

        //--StartSearchingServersTEST--\\
        [Test]
        public void StartSearchingServersTest_FirstStart_ChangesField()
        {
            
            //Arrange
            IPEndPoint iep = null;
            IPEndPoint res = new IPEndPoint(IPAddress.Parse("155.34.11.123"), 666);
            byte[] name = Encoding.UTF8.GetBytes("IAMSERV ABRACADABRA");
            client.Receive(ref iep).Returns( x => 
            {
                x[0] = res;
                return name;
            });

            //Act
            searcher.StartSearchingServers();
            
            //Assert
            Assert.IsTrue(searcher.FindedIpEPs.ContainsKey("ABRACADABRA"));
            Assert.AreSame(res, searcher.FindedIpEPs["ABRACADABRA"]);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Поиск уже производится!")]
        public void StartSearchingServersTest_DoubleCalls_ThrowsException()
        {
            //Arrange
            
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
            //Arrange
            //Act
            searcher.StartSearchingServers();
            searcher.EndSearchingServers();

            //Assert
            Assert.IsFalse(searcher.FindingStatus);
        }

        /*[Test]
        public void StartAndEndSearchingServersTest_CallsSeveralTimes_ChangesField()
        {
            //Arrange1
            IPEndPoint iep = null;
            IPEndPoint res = new IPEndPoint(IPAddress.Parse("155.34.11.123"), 666);
            byte[] name = Encoding.UTF8.GetBytes("IAMSERV BBC");
            client.Receive(ref iep).Returns(x =>
            {
                x[0] = res;
                return name;
            });
            client.When(x => x.Receive(ref res)).Throw(new SocketException());

            //Act1
            searcher.StartSearchingServers();
            

            //Assert1
            Assert.IsTrue(searcher.FindedIpEPs.ContainsKey("BBC"));
            Assert.AreSame(res, searcher.FindedIpEPs["BBC"]);
            Assert.IsFalse(searcher.FindingStatus);

            //Arrange2
            res = new IPEndPoint(IPAddress.Parse("105.24.101.13"), 666);
            IPEndPoint res2 = new IPEndPoint(IPAddress.Parse("22.44.104.76"), 666);
            byte[] name2 = Encoding.UTF8.GetBytes("IAMSERV R1M1");
            name = Encoding.UTF8.GetBytes("IAMSERV SUPERZ");
            client.Receive(ref iep).Returns(x =>
            {
                x[0] = res;
                return name;
            });
            client.Receive(ref res).Returns(x =>
            {
                x[0] = res2;
                return name2;
            });
            client.When(x => x.Receive(ref res2)).Throw(new SocketException());

            //Act2
            searcher.StartSearchingServers();

            //Assert2
            Assert.IsTrue(searcher.FindedIpEPs.ContainsKey("SUPERZ"));
            Assert.AreSame(res, searcher.FindedIpEPs["SUPERZ"]);
            Assert.IsTrue(searcher.FindedIpEPs.ContainsKey("R1M1"));
            Assert.AreSame(res2, searcher.FindedIpEPs["R1M1"]);
            Assert.IsFalse(searcher.FindingStatus);
        }*/

        //--InitReceivingBroadcastMessagesThreadTEST--\\
        //[Test]
        //public void InitReceivingBroadcastMessagesThreadTEST_
    }
}
