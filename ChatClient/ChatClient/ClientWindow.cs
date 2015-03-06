﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ClientWindow : Form
    {
        private delegate void strDel(string arg);
        private delegate void strListDel(List<string> args);
        private delegate void voidDel();
        private static Thread ConnectionThread;
        private static Thread GettingMessagesThread;
        private static bool clientIsConnected;
        private readonly string[] statusesOfConnectButtons = new string[] { "Подключить", "Отключить" };
        private static string currentNick = "";
        int serverPort;
        string serverIP;
        string serverName;
        public ClientWindow(string IP, int port, string serverName)
        {
            InitializeComponent();
            this.serverPort = port;
            this.serverIP = IP;
            this.serverName = serverName;
            SetIdentifedLabels(IP, port, serverName);
        }
        private void SetIdentifedLabels(string IP, int port, string serverName)
        {
            this.IPAdressTextBox.Text = IP;
            this.PortTextBox.Text = port.ToString();
            this.ServerNameTextBox.Text = serverName;
        }
        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {

                string typedText = this.TypingBox.Text;
                string[] sendingMessages;
                if (this.DebugCheckBox.Checked == false)
                {
                    sendingMessages = MapMessage(typedText);
                    
                }
                else
                {
                    sendingMessages = new string[] {typedText};
                }
                Client.GetInstance().SendText(sendingMessages);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Отсутствует подключение к серверу: " + exc.Message,
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
            if ( IsConnectingThreadWorking || IsGettingMessagesThreadWorking )
            {
                MessageBox.Show("Подключение в процессе. Пожалуйста, дождитесь результата.",
                    "Подключение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (! IsConnectingThreadWorking && ! IsGettingMessagesThreadWorking)
            {
                ConnectionThread.Start();
            }
        }
        private void InitConnectionThread()
        {
            ConnectionThread = new Thread(() =>
            {
                try
                {
                    Client.GetInstance().LogIn(serverIP, serverPort);
                    clientIsConnected = true;
                    this.EnableGettingNewInformation();
                    this.Invoke(new voidDel(DisactivateConnectButton));
                    this.Invoke(new voidDel(PrepareFormToTyping));
                }
                catch (Exception exc)
                {
                    this.CloseClientConnection();
                    MessageBox.Show("Подключение не было произведено. Проверьте IP-адрес и порт" +
                        "(возможно, сервер прекратил свою работу). Полная информация: " + exc.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }
        private void CloseClientConnection()
        {
            clientIsConnected = false;
            Client.GetInstance().LogOut();
            Client.GetInstance().ownNickName = "";
            if (!this.IsDisposed && !this.Disposing)
            {
                this.Invoke(new strListDel(RefreshNickNamesListBox), new List<string>());
                this.Invoke(new voidDel(ActivateConnectButton));
            }
        }
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            this.CloseClientConnection();
        }
        private static bool IsConnectingThreadWorking
        {
            get
            {
                /*bool answer = false;
                if (ConnectionThread != null)
                {
                    answer = ConnectionThread.IsAlive;
                }*/

                return ConnectionThread.IsAlive;
            }
        }

        private static bool IsGettingMessagesThreadWorking
        {
            get
            {
                /*bool answer = false;
                if (GettingMessagesThread != null)
                {
                    answer = GettingMessagesThread.IsAlive;
                }*/

                return GettingMessagesThread.IsAlive;
            }
        }
        private void AddTextInChatBox(string text)
        {
            this.RTMainChatBox.AppendText(text + Environment.NewLine);
            this.RTMainChatBox.ScrollToCaret();
        }
        /*
        private void AddTextInChatBox(string text, Style type)
        {

        }*/
        private void EnableGettingNewInformation()
        {
            
            List<string> tempNicks = (List<string>)this.NickNamesListBox.DataSource;
            GettingMessagesThread = new Thread(() =>
            {
                while (clientIsConnected)
                {
                    try
                    {
                        string newMessage = Client.GetInstance().GetMessage();
                        /*Временно!*/
                        if (this.DebugCheckBox.Checked == false)
                        {
                            newMessage = Client.GetInstance().HandleMessage(newMessage);
                        }
                        /*См. вверху^*/
                        this.Invoke(new strDel(this.AddTextInChatBox), newMessage);
                        if (tempNicks != Client.GetInstance().listOfNickNames)
                        {
                            this.Invoke(new strListDel(this.RefreshNickNamesListBox), Client.GetInstance().listOfNickNames);
                        }
                        if (ClientWindow.currentNick != Client.GetInstance().ownNickName)
                        {
                            currentNick = Client.GetInstance().ownNickName;
                            this.Invoke(new strDel(this.EditCurrentNickName), currentNick);
                        }
                    }
                    catch (SocketException e)
                    {
                        this.CloseClientConnection();
                        MessageBox.Show("Поток получения сообщений был экстренно завершен. Соединение с сервером разорвано. Информация: " + e.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception e)
                    {
                        this.CloseClientConnection();
                        MessageBox.Show("Скорее всего, сервер прекратил свою работу : " + e.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
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
                DoRequestTempNickName();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка. Скорее всего, сервер прекратил свою работу: \r\n" + exc.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Client.GetInstance().ownNickName = "";
            }
        }
        private void DoRequestTempNickName()
        {
            try
            {
                Client.GetInstance().RequestToGetTempNickName();
            }
            catch
            {
                throw new ArgumentException();
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
            this.GettersFlowLayoutPanel.Controls.Add(ClientWindow.CreateButtonForGetters("Отправить всем"));
        }

        private void ClientWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.CloseClientConnection();
            }
            catch { }
        }
        /*
        private void NickNamesListBox_DataSourceChanged(object sender, EventArgs e)
        {
            this.NickNamesListBox.SelectedItem = null;
        }*/
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
                        Button newGetter = ClientWindow.CreateButtonForGetters(selectedNickName);
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

            }
            catch (Exception exc)
            {
                MessageBox.Show("Невозможно зарегистрировать пользователя. Обратите внимание на соединение. Подробная информация: " + exc.Message,
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
    }
}
