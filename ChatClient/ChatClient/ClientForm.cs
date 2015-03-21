using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ClientForm : Form
    {
        private delegate void strDel(string arg);
        private delegate void strListDel(List<string> args);
        private delegate void voidDel();
        private Thread ConnectionThread;
        private Thread GettingMessagesThread;
        private static bool clientIsConnected;
        private readonly string[] statusesOfConnectButtons = new string[] { "Подключить", "Отключить" };
        private static string currentNick = "";

        string serverName;
        IPEndPoint remoteIEP;
        public ClientForm(string serverName, IPEndPoint IEP)
        {
            InitializeComponent();
            this.serverName = serverName;
            this.remoteIEP = IEP;
            InitConnectionThread();
            InitGettingMessagesThread();
            PrepareFormToTyping();
            SetIdentifedLabels(IEP, serverName);
        }
        private void SetIdentifedLabels(IPEndPoint IEP, string serverName)
        {
            if (IEP != null)
            {
                this.IPAdressAndPortTextBox.Text = IEP.ToString();
            }
            else
            {
                this.IPAdressAndPortTextBox.Text = "Error data.";
            }
            this.ServerNameTextBox.Text = serverName;
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
                    Client.GetInstance().SendText(sendingMessages);
                }
            }
            catch (SocketException exc)
            {
                MessageBox.Show("Отсутствует подключение к серверу."/* + exc.Message*/,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Invoke(new voidDel(this.PrepareFormToTyping));
            }
            
        }

        //Производит подключение и активизирует получение сообщений от сервера
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            /*if ( IsConnectingThreadWorking )
            {
                MessageBox.Show("Подключение в процессе. Пожалуйста, дождитесь результата.",
                    "Подключение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
            if (! IsConnectingThreadWorking && ! IsGettingMessagesThreadWorking)
            {
                InitConnectionThread();
                ConnectionThread.Start();
                this.DisactivateConnectButton();
            }
        }
        private void InitConnectionThread()
        {
            ConnectionThread = new Thread(() =>
            {
                try
                {
                    Client.GetInstance().DoConnect(remoteIEP);
                    clientIsConnected = true;
                    this.EnableGettingNewInformation();
                    /*this.Invoke(new voidDel(DisactivateConnectButton));*/
                    this.Invoke(new voidDel(PrepareFormToTyping));
                }
                catch (SocketException exc)
                {
                    
                    this.CloseClientConnection();
                    if (!this.RTMainChatBox.IsDisposed && !this.RTMainChatBox.Disposing &&
                        !this.ConnectButton.IsDisposed && !this.ConnectButton.Disposing)
                    {
                        this.Invoke(new Action<string, Color>(AddTextInChatBox), new object[] 
                        {"Подключение не было произведено. Возможно, сервер прекратил свою работу.",
                        Color.Red});
                        this.Invoke(new voidDel(ActivateConnectButton));
                    }
                    /*MessageBox.Show("Подключение не было произведено. Проверьте IP-адрес и порт" +
                        "(возможно, сервер прекратил свою работу). Полная информация: " + exc.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                }
            });
        }
        private void InitGettingMessagesThread()
        {
            GettingMessagesThread = new Thread(() =>
            {
                Message newMessage;
                List<string> tempNicks = (List<string>)this.NickNamesListBox.DataSource;
                try
                {
                    while (clientIsConnected)
                    {
                        
                        string rawText = Client.GetInstance().GetMessage();
                        /*Временно!*/
                        if (this.DebugCheckBox.Checked == false)
                        {
                            newMessage = Client.GetInstance().ConvertToMessage(rawText);
                            this.Invoke(new Action<Message>(AddTextInChatBox),newMessage);
                        }
                        /*См. вверху^*/
                        else
                        {
                            this.Invoke(new Action<string>(this.AddTextInChatBox), rawText);
                        }
                        if (tempNicks != Client.GetInstance().listOfNickNames)
                        {
                            this.Invoke(new Action<List<string>>(this.RefreshNickNamesListBox), Client.GetInstance().listOfNickNames);
                        }
                        if (ClientForm.currentNick != Client.GetInstance().ownNickName)
                        {
                            currentNick = Client.GetInstance().ownNickName;
                            this.Invoke(new Action<string>(this.EditCurrentNickName), currentNick);
                        }
                    }
                }
                catch (SocketException exc)
                {
                    /*MessageBox.Show("Поток получения сообщений был экстренно завершен. Соединение с сервером разорвано. Информация: " + e.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                    /*CloseClientConnection();*/
                    if (!this.RTMainChatBox.IsDisposed && !this.RTMainChatBox.Disposing)
                    {
                        this.Invoke(new Action<string, Color>(AddTextInChatBox), new object[] 
                    {"Поток получения сообщений был экстренно завершен. Соединение с сервером разорвано.",
                    Color.Red});
                    }
                    
                }
                catch (ArgumentException exc)
                {
                    if (!this.RTMainChatBox.IsDisposed && !this.RTMainChatBox.Disposing)
                    {
                        this.Invoke(new Action<string, Color>(AddTextInChatBox), new object[] { exc.Message + 
                            " Соединение с сервером будет разорвано.", Color.Red });
                    }
                }
                /*catch (Exception e)
                {
                    MessageBox.Show("Скорее всего, сервер прекратил свою работу : " + e.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }*/
                finally
                {
                    CloseClientConnection();
                    if (!this.NickNamesListBox.IsDisposed && !this.NickNamesListBox.Disposing &&
                        !this.ConnectButton.Disposing && !this.ConnectButton.IsDisposed)
                    {
                        this.Invoke(new Action<List<string>>(RefreshNickNamesListBox), new List<string>());
                        this.Invoke(new voidDel(ActivateConnectButton));
                    }
                    
                }
            });
        }
        private void CloseClientConnection()
        {
            clientIsConnected = false;
            Client.GetInstance().ownNickName = "";
            Client.GetInstance().DoDisconnect();
            /*if (this.GettingMessagesThread.ThreadState == ThreadState.Running)
            {
                this.GettingMessagesThread.Join();
            }*/

            /*if (!this.IsDisposed || !this.Disposing)
            {
                this.Invoke(new Action<List<string>>(RefreshNickNamesListBox), new List<string>());
            }*/
        }
        
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            this.CloseClientConnection();
            if (!this.IsDisposed || !this.Disposing)
            {
                this.Invoke(new Action<List<string>>(RefreshNickNamesListBox), new List<string>());
            }
            this.ActivateConnectButton();
        }
        private bool IsConnectingThreadWorking
        {
            get
            {
                bool answer = true;
                if (ConnectionThread.ThreadState != ThreadState.Running)
                {
                    answer = false;
                }
                
                return answer;
            }
        }

        private bool IsGettingMessagesThreadWorking
        {
            get
            {
                bool answer = true;
                if (GettingMessagesThread.ThreadState != ThreadState.Running)
                {
                    answer = false;
                }
                return answer;
            }
        }
        private void AddTextInChatBox(string text)
        {
            this.RTMainChatBox.AppendText(text + Environment.NewLine);
            this.RTMainChatBox.ScrollToCaret();
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
            if (GettingMessagesThread.ThreadState == ThreadState.Stopped ||
                GettingMessagesThread.ThreadState == ThreadState.Aborted)
            {
                InitGettingMessagesThread();
            }
            GettingMessagesThread.Start();
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
                Client.GetInstance().RequestToGetTempNickName();
            }
            catch (SocketException exc)
            {
                MessageBox.Show("Отсутствует подключение к серверу.",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Client.GetInstance().ownNickName = "";
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
            this.CloseClientConnection();
            //GettingMessagesThread.Join();
            /*while (this.GettingMessagesThread.ThreadState == ThreadState.Running)
            {
                Thread.Sleep(200);
            }
            /*while (GettingMessagesThread.ThreadState == ThreadState.Running )
            {
                Thread.Sleep(100);
                //Ждем естественное завершение потока получения информации
                //Отказ от Join потому, что в этом потоке вызывается Invoke(),
                //который должен работать от имени главного потока. Т.е., если поставить Join(),
                //то все потоки будут ждать, а главный поток так и не сможет выполнить Invoke()
            }*/
            /*SearcherServersForm form = new SearcherServersForm();
            form.Show();*/
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
                bool ownNickNameFlag = (selectedNickName == Client.GetInstance().ownNickName);
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
            Client.GetInstance().SendText("REG " + regLogin + " " + regPassword);
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
        private void LogIn(string login, string password)
        {
            Client.GetInstance().SendText("LOGIN " + login + " " + password);
        }
        private void LogInButton_Click(object sender, EventArgs e)
        {
            try
            {
                string tempLogin = this.LoginTextBox.Text.Trim();
                string tempPass = this.PasswordTextBox.Text;
                if (Client.IsCorrectNick(tempLogin) && Client.IsCorrectPassword(tempPass))
                {
                    LogIn(tempLogin, tempPass);
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
                MessageBox.Show("Невозможно авторизовать пользователя. Обратите внимание на соединение. Подробная информация: " + exc.Message,
                    "Отклонено", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Client.GetInstance().ownNickName = "";
            }
            finally
            {
                this.LoginTextBox.Clear();
                this.PasswordTextBox.Clear();
            }
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //CloseClientConnection();
        }
    }
}
