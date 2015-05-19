#if !TESTING
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Net.Sockets;
using System.Net;
/*using System.Diagnostics;*/
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ClientForm : Form
    {
        private string serverName;
        private IPEndPoint remoteIEP;

        private Thread ConnectionThread;
        private Thread GettingMessagesThread;
        private Thread UpdatingControlsThread;

        private Client client;
        private /*static*/ bool clientIsConnected;
        private /*static*/ bool exitMode;
        private readonly string[] statusesOfConnectButtons = new string[] { "Подключить", "Отключить" };

        //Отслеживаемые переменные:
        private static string currentNick = "";
        private Message newMessage;
        private List<string> currentNicks;
        private Exception serviceException;

        public ClientForm(string serverName, IPEndPoint IEP)
        {
            InitializeComponent();
            if (IEP == null)
            {
                throw new ArgumentNullException("Недопустимое значение для IPEndPoint");
            }
            MTcpClient tempMTC = new MTcpClient(new TcpClient());
            client = new Client(tempMTC);
            this.serverName = serverName;
            this.remoteIEP = IEP;
            this.IPAdressAndPortTextBox.Text = IEP.ToString();
            this.ServerNameTextBox.Text = serverName;
            exitMode = false;
            PrepareFormToTyping();
            
        }

        public bool ClientIsConnected
        {
            get
            {
                return this.clientIsConnected;
            }
        }

        public List<string> CurrentNicks
        {
            get
            {
                return this.currentNicks;
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                string typedText = this.TypingBox.Text;
                if (typedText != "")
                {
                    string[] sendingMessages;
                    if (this.DebugCheckBox.Checked == false)
                    {
                        sendingMessages = MapMessage(typedText);
                    }
                    else
                    {
                        sendingMessages = new string[] { typedText };
                    }
                    client.SendTextData(sendingMessages);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(/*"Отсутствует подключение к серверу." +*/ exc.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Invoke(new Action(this.PrepareFormToTyping));
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            this.DisactivateConnectButton();
            InitConnectionThread();
            ConnectionThread.Start();
            
        }
        private void InitConnectionThread()
        {
            ConnectionThread = new Thread(() =>
            {
                try
                {
                    client.DoConnect(remoteIEP);
                    clientIsConnected = true;
                    this.EnableUpdatingControls();
                    this.EnableGettingNewInformation();

                    /*this.Invoke(new voidDel(DisactivateConnectButton));*/
                    this.Invoke(new Action(PrepareFormToTyping));
                }
                catch (Exception exc)
                {
                    this.CloseClientConnection();
                    if (!this.RTMainChatBox.IsDisposed && !this.RTMainChatBox.Disposing &&
                        !this.ConnectButton.IsDisposed && !this.ConnectButton.Disposing)
                    {
                        this.Invoke(new Action<string, Color>(AddTextInChatBox), new object[] 
                        {"Подключение не было произведено. Возможно, сервер прекратил свою работу: " + exc.Message,
                        Color.Red});
                        this.Invoke(new Action(ActivateConnectButton));
                    }
                }
            });
        }
        private void InitUpdatingControlsThread()
        {
            UpdatingControlsThread = new Thread(() =>
                {
                    Message lastMessage = newMessage;
                    List<string> lastNicks = currentNicks;
                    string lastNick = currentNick;
                    while (clientIsConnected)
                    {
                        if (newMessage != lastMessage)
                        {
                            lastMessage = newMessage;
                            if (this.DebugCheckBox.Checked == false)
                            {
                                this.Invoke(new Action<Message>(AddTextInChatBox), newMessage);
                            }
                            else
                            {
                                this.Invoke(new Action<string>(this.AddTextInChatBox), newMessage.RawMessage);
                            }
                        }
                        if (lastNicks != currentNicks)
                        {
                            lastNicks = currentNicks;
                            this.Invoke(new Action<List<string>>(this.RefreshNickNamesListBox), currentNicks);
                        }
                        if (lastNick != currentNick)
                        {
                            lastNick = currentNick;
                            this.Invoke(new Action<string>(this.EditCurrentNickName), lastNick);
                        }
                    }
                    if (!exitMode)
                    {
                        this.Invoke(new Action<List<string>>(RefreshNickNamesListBox), new List<string>());
                        this.Invoke(new Action(ActivateConnectButton));
                        if (serviceException != null)
                        {

                            this.Invoke(new Action<string, Color>(AddTextInChatBox), new object[] { serviceException.Message, Color.Red });
                            serviceException = null;
                        }
                        this.Invoke(new Action<string>(EditCurrentNickName), "");
                    }
                });
        }

        private void InitGettingMessagesThread()
        {
            GettingMessagesThread = new Thread(GetMessages);
        }
        private void GetMessages()
        {
            try
                {
                    while (clientIsConnected)
                    {

                        string rawText = client.GetTextData();
                        newMessage = client.ConvertTextDataToMessage(rawText);
                        if (currentNicks != client.OnlineUsers)
                        {
                            currentNicks = client.OnlineUsers;
                        }
                        if (currentNick != client.OwnNickName)
                        {
                            currentNick = client.OwnNickName;
                        }

                    }
                }
                catch (ArgumentException exc)
                {
                    serviceException = exc;
                }

                catch (Exception exc)
                {
                    serviceException = exc;
                }

                finally
                {
                    CloseClientConnection();
                }
        }
        /*
        [Conditional("DEBUG")]
        public void GetMessages_PublicWrapper()
        {

        }
        [Conditional("DEBUG")]
        public void SetClientIsConnected(bool value)
        {
            this.clientIsConnected = value;
        }*/

        private void CloseClientConnection()
        {
            clientIsConnected = false;
            //client.ownNickName = "";
            client.DoDisconnect();
        }
        
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            this.CloseClientConnection();
            this.RefreshNickNamesListBox(new List<string>());
            this.ActivateConnectButton();
        }
        private void AddTextInChatBox(string text)
        {
            if (text != null && text != "")
            {
                this.RTMainChatBox.AppendText(text + Environment.NewLine);
                this.RTMainChatBox.ScrollToCaret();
            }
        }
        
        private void AddTextInChatBox(string text, Color color)
        {
            this.RTMainChatBox.SelectionColor = color;
            this.RTMainChatBox.AppendText(text +Environment.NewLine);
            this.RTMainChatBox.SelectionColor = this.RTMainChatBox.ForeColor;
            this.RTMainChatBox.ScrollToCaret();
        }
        private void AddTextInChatBox(Message receivedMessage)
        {
            if (receivedMessage != null && receivedMessage.TextMessage != String.Empty)
            {
                AddTextInChatBox(receivedMessage.TextMessage, receivedMessage.TextColor);
            }
        }
        private void EnableGettingNewInformation()
        {
            if (GettingMessagesThread == null || GettingMessagesThread.ThreadState == ThreadState.Stopped ||
                GettingMessagesThread.ThreadState == ThreadState.Aborted)
            {
                InitGettingMessagesThread();
            }
            GettingMessagesThread.Start();
        }
        private void EnableUpdatingControls()
        {
            if (UpdatingControlsThread == null || UpdatingControlsThread.ThreadState == ThreadState.Stopped ||
                UpdatingControlsThread.ThreadState == ThreadState.Aborted)
            {
                InitUpdatingControlsThread();
            }
            UpdatingControlsThread.Start();
        }
        private void EditCurrentNickName(string nickName)
        {
            if (nickName != "")
            {
                this.CurrentNickNameTextBox.Text = nickName;
            }
            else
            {
                this.CurrentNickNameTextBox.Clear();
            }
        }
        private void RequestNickNameButton_Click(object sender, EventArgs e)
        {
            try
            {
                client.RequestToGetTempNickName();
            }
            catch (Exception exc)
            {
                MessageBox.Show(/*"Отсутствует подключение к серверу."*/exc.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //client.OwnNickName = "";
            }
        }
        private void RefreshNickNamesListBox (List<string> nicks)
        {
            this.NickNamesListBox.DataSource = nicks;
        }
        private void ActivateConnectButton()
        {
            this.ConnectButton.Enabled = true;
            this.DisconnectButton.Enabled = false;
        }
        private void DisactivateConnectButton()
        {

            this.ConnectButton.Enabled = false;
            this.DisconnectButton.Enabled = true;
        }
        private bool HasGettersPanelButtons()
        {
            Button temp = new Button();
            temp.Name = "Отправить всем";
            temp.Text = "Всем";
            return (this.GettersFlowLayoutPanel.Controls != null && !this.GettersFlowLayoutPanel.Controls.ContainsKey(temp.Name));
        }
        private string[] MapMessage(string text)
        {
            string[] discreteMessages = null;
            if (this.HasGettersPanelButtons())
            {
                int quantityOfButtons = this.GettersFlowLayoutPanel.Controls.Count;
                discreteMessages = new string[quantityOfButtons];
                int i = 0;
                foreach (Button thisButton in this.GettersFlowLayoutPanel.Controls)
                {
                    discreteMessages[i] = "PRIVMSG " + thisButton.Name + " " + text;
                    i++;
                }
            }
            else
            {
                discreteMessages = new string[] {"MSG " + text};
            }
            return discreteMessages;
        }
        private void PrepareFormToTyping()
        {
            this.NickNamesListBox.SelectedItem = null;
            this.TypingBox.Text = "";
            this.GettersFlowLayoutPanel.Controls.Clear();
            this.GettersFlowLayoutPanel.Controls.Add(ClientForm.CreateButtonForGetters("Отправить всем"));
        }

        private void ClientWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            exitMode = true;
            this.CloseClientConnection();
            
            if (UpdatingControlsThread != null && UpdatingControlsThread.ThreadState == ThreadState.Running)
            {
                UpdatingControlsThread.Join();
            }
        }
        private static Button CreateButtonForGetters(string nameOfGetter)
        {
            Button rButtton = new Button();
            if (nameOfGetter == "Отправить всем")
            {
                rButtton.Text = "Всем";
            }
            else
            {
                rButtton.Text = nameOfGetter;
            }
            rButtton.Name = nameOfGetter;
            return rButtton;
        }
        private void AddNewGetterInGettersPanel()
        {
            if (this.NickNamesListBox.SelectedItem != null)
            {
                string selectedNickName = this.NickNamesListBox.SelectedItem.ToString();
                bool ownNickNameFlag = (selectedNickName == client.OwnNickName);
                if (!ownNickNameFlag)
                {
                    bool toAllFlag = (this.GettersFlowLayoutPanel.Controls.ContainsKey("Отправить всем") || selectedNickName == "Отправить всем");
                    if (toAllFlag)
                    {
                        this.GettersFlowLayoutPanel.Controls.Clear();
                    }
                    bool itHasSameNickFlag = this.GettersFlowLayoutPanel.Controls.ContainsKey(selectedNickName);
                    if (!itHasSameNickFlag)
                    {
                        Button newGetter = ClientForm.CreateButtonForGetters(selectedNickName);
                        if (selectedNickName != "Отправить всем")
                        {
                            newGetter.Click += GetterButton_Click;
                        }
                        this.GettersFlowLayoutPanel.Controls.Add(newGetter);
                    }
                }
            }
        }
        private void GetterButton_Click(object sender, EventArgs e)
        {
            try
            {
                Button clickedButton = (Button)sender;
                this.RemoveGetter(clickedButton);
                if (this.GettersFlowLayoutPanel.Controls.Count == 0)
                {
                    this.GettersFlowLayoutPanel.Controls.Add(CreateButtonForGetters("Отправить всем"));
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Невозможно удалить получателя.", "Ошибка аргумента");
            }

        }
        private void RemoveGetter(Button rmvButton)
        {
            try
            {
                this.GettersFlowLayoutPanel.Controls.Remove(rmvButton);
            }
            catch
            {
                throw new ArgumentOutOfRangeException();
            }

        }
        private void AddNewGetterButton_Click(object sender, EventArgs e)
        {
            AddNewGetterInGettersPanel();
        }
        private void RegisterNewLogin(string regLogin, string regPassword)
        {
            client.SendTextData("REG " + regLogin + " " + regPassword);
        }
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            try
            {
                string regLogin = this.RegLoginTextBox.Text.Trim();
                string regPass = this.RegPasswordTextBox.Text;
                if (Client.IsCorrectNick(regLogin) && Client.IsCorrectPassword(regPass))
                {
                    RegisterNewLogin(regLogin, regPass);
                }
                else
                {
                    throw new ArgumentException("Некорректный ник-нейм или пароль.");
                }

            }
            catch (SocketException exc)
            {
                MessageBox.Show("Невозможно зарегистрировать пользователя. Обратите внимание на соединение. Подробная информация: " + exc.Message,
                    "Отклонено", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (ArgumentException exc)
            {
                MessageBox.Show("Невозможно зарегистрировать пользователя. Подробная информация: " + exc.Message,
                    "Отклонено", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                this.RegLoginTextBox.Clear();
                this.RegPasswordTextBox.Clear();
            }
        }
        private void LogInButton_Click(object sender, EventArgs e)
        {
            try
            {
                string tempLogin = this.LoginTextBox.Text.Trim();
                string tempPass = this.PasswordTextBox.Text;
                if (Client.IsCorrectNick(tempLogin) && Client.IsCorrectPassword(tempPass))
                {
                    client.SendTextData("LOGIN " + tempLogin + " " + tempPass);
                }
                else
                {
                    throw new ArgumentException("Некорректный ник-нейм или пароль.");
                }
            }
            catch (ArgumentException exc)
            {
                MessageBox.Show("Невозможно авторизовать пользователя. Подробная информация: " + exc.Message,
                    "Отклонено", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception exc)
            {
                EditCurrentNickName("");
                //client.OwnNickName = "";
                MessageBox.Show("Невозможно авторизовать пользователя. Обратите внимание на соединение. Подробная информация: " + exc.Message,
                    "Отклонено", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                this.LoginTextBox.Clear();
                this.PasswordTextBox.Clear();
            }
        }
    }
}
#endif