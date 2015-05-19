using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NUnit.Framework;
using ChatClient;
using System.Net;
using System.Net.Sockets;
using NSubstitute;

namespace ChatClientTests
{
    [TestFixture]
    public class ClientTests
    {
        //---IsCorrectNickTEST---\\
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCase(null)]
        public void IsCorrectNickTest_ReceivesNullString_throwsArgmunetException(string arg)
        {
            //arrange //act //assert
            Client.IsCorrectNick(arg);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestCase("")]
        public void IsCorrectNickTest_ReceivesEmptyString_throwsArgmunetException(string arg)
        {
            //arrange //act //assert
            Client.IsCorrectNick(arg);
        }

        [TestCase("b")]
        public void IsCorrectNickTest_ReceivesOneLetter_returnsTrue(string arg)
        {
            bool realResult = Client.IsCorrectNick(arg);
            Assert.IsTrue(realResult);
        }
        [TestCase(" ")]
        public void IsCorrectNickTest_ReceivesOneSpaceSymbol_returnsFalse(string arg)
        {
            bool realResult = Client.IsCorrectNick(arg);
            Assert.IsFalse(realResult);
        }
        [TestCase("ssss_aasd1233_bfl;';")]
        public void IsCorrectNickTest_ReceivesLettersAndSymbols_returnsTrue(string arg)
        {
            bool realResult = Client.IsCorrectNick(arg);
            Assert.IsTrue(realResult);
        }

        //---IsCorrectPasswordTEST---\\
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCase(null)]
        public void IsCorrectPasswordTest_ReceivesNullString_throwsArgmunetException(string arg)
        {
            //arrange //act //assert
            Client.IsCorrectPassword(arg);
        }
        [ExpectedException(typeof(ArgumentException))]
        [TestCase("")]
        public void IsCorrectPasswordTest_ReceivesEmptyString_throwsArgmunetException(string arg)
        {
            //arrange //act //assert
            Client.IsCorrectPassword(arg);
        }
        [TestCase("ssa__sdw2;'.Б//В  ?")]
        public void IsCorrectPasswordTest_ReceivesVaroiusLetters_throwsArgmunetException(string arg)
        {
            //arrange //act //assert
            bool result = Client.IsCorrectPassword(arg);
            Assert.IsTrue(result);
        }


        //---DoConnectTEST---\\
        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void DoConnectTest_IEPIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            IPEndPoint iep =null;
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(false);
            Client clientObj = new Client(mockClient);
            //Act
            clientObj.DoConnect(iep);
        }

        [Test]
        public void DoConnectTest_ConnectionWithUnconnectedStatus_NTR()
        {
            //Arrange
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"),
                662);
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(false);
            Client clientObj = new Client(mockClient);
            //Act
            clientObj.DoConnect(iep);
            //Assert
            mockClient.Received().Connect(iep);
        }

        [Test]
        public void DoConnectTest_ConnectionWithConnectedStatus_NTR()
        {
            //Arrange
            IPEndPoint secondIEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"),
                662);
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(true);
            Client clientObj = new Client(mockClient);
            //Act
            clientObj.DoConnect(secondIEP);
            //Assert
            mockClient.DidNotReceive().Connect(secondIEP);

        }

        //---DoDisconnectTEST---\\
        [Test]
        public void DoDisconnectTest_DisonnectionWithConnectedStatus_NTR()
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(true);
            Client clientObj = new Client(mockClient);
            //Act
            clientObj.DoDisconnect();
            //Assert
            mockClient.ReceivedWithAnyArgs().Close();

        }
        [Test]
        public void DoDisconnectTest_DisonnectionWithUnConnectedStatus_NTR()
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(false);
            Client clientObj = new Client(mockClient);
            //Act
            clientObj.DoDisconnect();
            //Assert
            mockClient.DidNotReceiveWithAnyArgs().Close();

        }

        //---SendTextTEST(string)---\\\
        [ExpectedException(typeof(Exception))]
        [Test]
        public void SendTextDataTest_DisconnectedStatus_throwsException()
        {
            //Arrange
            string superStr = "I wanna crash you!";
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(false);
            Client clientObj = new Client(mockClient);
            //Act & Assert
            clientObj.SendTextData(superStr);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestCase(null)]
        [TestCase("")]
        public void SendTextDataTest_ReceivesNullOrEmpty_throwsArgumentException(string arg)
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(true);
            Client clientObj = new Client(mockClient);
            //Act & Assert
            clientObj.SendTextData(arg);
        }
        [TestCase("cmd sssw www")]
        [TestCase("ss''aw';;c//")]
        [TestCase("__ASSS_@@@#112356")]
        public void SendTextDataTest_ReceivesUnemptyStringConnectedStatus_NTR(string arg)
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(true);
            Client clientObj = new Client(mockClient);
            byte[] buffWithMessage = Encoding.UTF8.GetBytes(arg);
            int length = buffWithMessage.Length;
            byte[] buffWithLength = BitConverter.GetBytes(length);

            //Act
            clientObj.SendTextData(arg);

            //Assert
            mockClient.Received(1).Write(Arg.Is<byte[]>(x => length == BitConverter.ToInt32(x, 0)),
                0, buffWithLength.Length);
            mockClient.Received(1).Write(Arg.Is<byte[]>(x => arg == System.Text.Encoding.UTF8.GetString(x)), 
                0, length);
        }

        [ExpectedException(typeof(Exception))]
        [Test]
        public void SendTextDataTest_FailsWhileWriting_throwsException()
        {
            //Arrange
            string superStr = "I wanna crash you!";
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(true);
            mockClient.When(mock => mock.Write(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>())).
                Throw<Exception>();
            Client clientObj = new Client(mockClient);
            byte[] buffWithMessage = Encoding.UTF8.GetBytes(superStr);
            int length = buffWithMessage.Length;
            byte[] buffWithLength = BitConverter.GetBytes(length);

            //Act & Assert
            clientObj.SendTextData(superStr);
        }

        //---SendTextTEST(string[])---\\\
        [ExpectedException(typeof(ArgumentException))]
        [TestCase(null)]
        public void SendTextData1Test_ReceivesNull_throwsArgumentException(string[] arg)
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(true);
            Client clientObj = new Client(mockClient);
            //Act & Assert
            clientObj.SendTextData(arg);
        }

        [ExpectedException(typeof(ArgumentException))]
        [Test]
        public void SendTextData1Test_ReceivesEmpty_throwsArgumentException()
        {
            //Arrange
            string[] arg = new string[] { }; 
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(true);
            Client clientObj = new Client(mockClient);
            //Act & Assert
            clientObj.SendTextData(arg);
        }

        [ExpectedException(typeof(Exception))]
        [Test]
        public void SendTextData1Test_ReceivesStringsWithDisconnectedStatus_throwsException()
        {
            //Arrange
            string[] arg = new string[] { "first", "second", "sdad_22fs adwl12wr cca//.,<<>?" };
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(false);
            Client clientObj = new Client(mockClient);
            //Act & Assert
            clientObj.SendTextData(arg);
            mockClient.DidNotReceiveWithAnyArgs().Write(Arg.Any<byte[]>(),
                Arg.Any<int>(), Arg.Any<int>());
        }
        [Test]
        public void SendTextData1Test_ReceivesStringsWithConnectedStatus_6CallsOfWriteMethod()
        {
            //Arrange
            string[] arg = new string[] { "first", "second", "sdad_22fs adwl12wr cca//.,<<>?" };
            int[] length = new int[] { arg[0].Length, arg[1].Length, arg[2].Length };
            List<byte[]> buffWithMessage = new List<byte[]>();
            List<byte[]> buffWithLength = new List<byte[]>();
            buffWithMessage.Add(Encoding.UTF8.GetBytes(arg[0]));
            buffWithMessage.Add(Encoding.UTF8.GetBytes(arg[1]));
            buffWithMessage.Add(Encoding.UTF8.GetBytes(arg[2]));

            buffWithLength.Add(BitConverter.GetBytes(length[0]));
            buffWithLength.Add(BitConverter.GetBytes(length[1]));
            buffWithLength.Add(BitConverter.GetBytes(length[2]));

            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(true);
            Client clientObj = new Client(mockClient);
            //Act
            clientObj.SendTextData(arg);
            //Assert
            mockClient.Received(1).Write(Arg.Is<byte[]>(x => length[0] == BitConverter.ToInt32(x, 0)),
                0, buffWithLength[0].Length);
            mockClient.Received(1).Write(Arg.Is<byte[]>(x => arg[0] == System.Text.Encoding.UTF8.GetString(x)), 
                0, length[0]);

            mockClient.Received(1).Write(Arg.Is<byte[]>(x => length[1] == BitConverter.ToInt32(x, 0)),
                0, buffWithLength[1].Length);
            mockClient.Received(1).Write(Arg.Is<byte[]>(x => arg[1] == System.Text.Encoding.UTF8.GetString(x)),
                0, length[1]);

            mockClient.Received(1).Write(Arg.Is<byte[]>(x => length[2] == BitConverter.ToInt32(x, 0)),
                0, buffWithLength[2].Length);
            mockClient.Received(1).Write(Arg.Is<byte[]>(x => arg[2] == System.Text.Encoding.UTF8.GetString(x)),
                0, length[2]);
        }


        //---RequestToGetTempNickNameTEST---\\
        [Test]
        public void RequestToGetTempNickNameTest_ConnectedStatus_NTR()
        {
            //Arrange
            string command = "NICK";
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(true);
            Client clientObj = new Client(mockClient);
            byte[] buffWithMessage = Encoding.UTF8.GetBytes(command);
            int length = buffWithMessage.Length;
            byte[] buffWithLength = BitConverter.GetBytes(length);

            //Act
            clientObj.RequestToGetTempNickName();

            //Assert

            mockClient.Received(1).Write(Arg.Is<byte[]>(x => command == System.Text.Encoding.UTF8.GetString(x)),
                0, length);
            
            mockClient.Received(1).Write(Arg.Is<byte[]>(x => length == BitConverter.ToInt32(x, 0)),
                0, buffWithLength.Length);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void RequestToGetTempNickNameTest_DisconnectedStatus_throwsException()
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(false);
            Client clientObj = new Client(mockClient);

            //Act & Assert
            clientObj.RequestToGetTempNickName();
            mockClient.DidNotReceiveWithAnyArgs().Write(Arg.Any<byte[]>(), 
                Arg.Any<int>(), Arg.Any<int>());
        }


        //---GetMessageTEST---\\
        [Test]
        [ExpectedException(typeof(Exception))]
        public void GetTextDataTest_DisconnectedStatus_throwsException()
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(false);
            Client clientObj = new Client(mockClient);

            //Act & Assert
            clientObj.GetTextData();
        }
        [TestCase("TOPSECRET! 2wad")]
        [TestCase("SSSAA 123 /'//-'';;z12")]
        public void GetTextDataTest_GettingCorrectMessageWithConnectedStatus_AllFine(string arg)
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(true);
            
            byte[] buffWithMessage0 = Encoding.UTF8.GetBytes(arg);
            byte[] buffWithLength0 = BitConverter.GetBytes(buffWithMessage0.Length);

            mockClient.Read(Arg.Any<int>(), Arg.Any<int>())
                .ReturnsForAnyArgs(buffWithLength0);
            mockClient.Read(Arg.Any<int>(), Arg.Any<int>())
                .ReturnsForAnyArgs(buffWithMessage0);
            Client clientObj = new Client(mockClient);

            //Act
            string result = clientObj.GetTextData();

            //Assert
            Assert.AreEqual(arg, result);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void GetTextDataTest_CatchingExceptionWhileGettingOne_ThrowsException()
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            mockClient.IsConnected().Returns(true);

            mockClient.When(x => x.Read(Arg.Any<int>(), Arg.Any<int>()))
                .Throw<SocketException>();
            Client clientObj = new Client(mockClient);

            //Act and Assert
            string result = clientObj.GetTextData();
        }

        //---ConvertTextDataToMessageTEST\\---
        //Colors: 0 - black, 1 - red, 2 - indigo
        [TestCase("MSG Hello World!", "Hello World", 0)]
        [TestCase("PRIVMSG RANDOMTEXT OLOLO", "RANDOMTEXT: OLOLO", 2)]
        [TestCase("IAMSERV dddzzz", "dddzzz", 1)]
        [TestCase("YOUARE NINJA", "Сервер Вас приветствует, NINJA", 1)]
        [TestCase("ERROR 1000000", "Ошибка неизвестного вида.", 1)]
        [TestCase("ERROR ", "Ошибка неизвестного вида.", 1)]
        [TestCase("ERROR 1", "Некорретный формат сообщения", 1)]
        [TestCase("ERROR 2", "Неизвестная ошибка при передаче сообщения.", 1)]
        [TestCase("ERROR 3", "Личное сообщение не было", 1)]
        [TestCase("ERROR 50", "Ник-нейм пользователя был", 1)]
        [TestCase("ERROR 51", "Данный ник-нейм занят.", 1)]
        [TestCase("ERROR 52", "Данный ник-нейм не", 1)]
        [TestCase("ERROR 53", "Ник был удачно зарегестрирован.", 1)]
        [TestCase("ERROR 54", "Неверный пароль или логин.", 1)]
        [TestCase("ERROR 55", "Успешная авторизация.", 1)]
        [TestCase("ERROR 100", "Сервер собирается приостановить", 1)]
        public void ConvertTextDataToMessageTEST_ReceivesCorrectTextData_TheyAreMatched(string arg1, string arg2, int color)
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            Client clientObj = new Client(mockClient);
            Color currentColor = Color.Black;
            switch (color)
            {
                case 0:
                    {
                        currentColor = Color.Black;
                        break;
                    }
                case 1:
                    {
                        currentColor = Color.Red;
                        break;
                    }
                case 2:
                    {
                        currentColor = Color.Indigo;
                        break;
                    }
            }
            //Act
            Message actualMessage = clientObj.ConvertTextDataToMessage(arg1);
            //Assert
            
            StringAssert.Contains(arg2, actualMessage.TextMessage);
            StringAssert.Contains(arg1, actualMessage.RawMessage);
            Assert.AreEqual(currentColor, actualMessage.TextColor);
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("CRASH!")]
        [TestCase("uncommand WTF!")]
        [TestCase("PRIVMSG ")]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertTextDataToMessageTEST_ReceivesIncorrectTextData_ThrowsArgumentException(string arg)
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            Client clientObj = new Client(mockClient);
            //Act & Assert
            Message actualMessage = clientObj.ConvertTextDataToMessage(arg);
        }

        [TestCase(null)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConvertTextDataToMessageTEST_ReceivesNull_ThrowsNullException(string arg)
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            Client clientObj = new Client(mockClient);
            //Act & Assert
            Message actualMessage = clientObj.ConvertTextDataToMessage(arg);
        }

        [TestCase("YOUARE PANDA", "PANDA")]
        [TestCase("YOUARE d", "d")]
        public void ConvertTextDataToMessageTEST_ReceivesSpecialTextData_ChangesNickNameField(string arg1, string arg2)
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            Client clientObj = new Client(mockClient);
            string textMessage = "Сервер Вас приветствует, " + arg2 + " !";
            //Act
            Message actualMessage = clientObj.ConvertTextDataToMessage(arg1);
            //Assert
            StringAssert.Contains(textMessage, actualMessage.TextMessage);
            StringAssert.Contains(arg2, clientObj.OwnNickName);
        }

        [TestCase("NAMES PANDA JOHN COURT", "PANDA JOHN COURT")]
        [TestCase("NAMES DDD ", "DDD ")]
        [TestCase("NAMES ", "")]
        public void ConvertTextDataToMessageTEST_ReceivesSpecialTextData_ChangesOnlineUsersField(string arg1, string arg2)
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            Client clientObj = new Client(mockClient);
            List<string> nickNames = arg2.Split(' ').ToList();
            nickNames.Insert(0, "Отправить всем");
            //Act
            Message actualMessage = clientObj.ConvertTextDataToMessage(arg1);
            //Assert
            Assert.AreEqual(nickNames.Count, clientObj.OnlineUsers.Count);
            for (int i = 0; i < nickNames.Count; i++)
            {
                StringAssert.Contains(nickNames[i], clientObj.OnlineUsers[i]);
            }
        }


        //---GetterSetterOwnNickNameTEST---\\
        [Test]
        public void OwnNickNameTEST_returnsNullNickName()
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            Client clientObj = new Client(mockClient);
            //Assert
            StringAssert.Contains(String.Empty, clientObj.OwnNickName);
        }

        [TestCase("purple")]
        public void OwnNickNameTest_UsingAsSetterFromYOUARE_ReturnsCorrectNickName(string arg)
        {
            //Arrange
            ITcpClient mockClient = Substitute.For<ITcpClient>();
            Client clientObj = new Client(mockClient);
            string rawTextData = "YOUARE " + arg;
            //Act
            clientObj.ConvertTextDataToMessage(rawTextData);
            //Assert
            StringAssert.Contains("", clientObj.OwnNickName);
        }

    }
}
