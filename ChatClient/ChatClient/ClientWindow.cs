using System;
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
        private static object selectedNick;
        private static bool clientIsConnected;
        private static bool itWasRefreshingNicks = false;
        readonly string[] statusesOfConnectButtons = new string[] { "Подключить", "Отключить" };
        public ClientWindow()
        {
            InitializeComponent();
            selectedNick = new object();
            this.IPAdressTextBox.Text = "192.168.";
        }

        private void TypingBox_TextChanged(object sender, EventArgs e)
        {
            if (selectedNick != null)
            {
                string thisNick = selectedNick.ToString();
                string comparedText = this.TypingBox.Text;
                if (comparedText.Length > thisNick.Length)
                {
                    comparedText = comparedText.Substring(0, thisNick.Length);
                    if (!String.Equals(thisNick, comparedText))
                    {
                        selectedNick = null;
                    }
                }
                else
                {
                    selectedNick = null;
                }
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {

                string sendingMessage = this.TypingBox.Text;
                if (this.DebugCheckBox.Checked == false)
                {
                    sendingMessage = MapMessage(sendingMessage);
                }
                Client.GetInstance().SendText(sendingMessage);
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
            ConnectionThread = new Thread(() =>
            {
                try
                {
                    string newMessage = "";
                    string ipAdrs = this.IPAdressTextBox.Text;
                    int port = Convert.ToInt32(this.PortTextBox.Text);
                    Client.GetInstance().LogIn(ipAdrs, port);
                    /*newMessage = Client.GetInstance().GetMessage();*/
                    this.Invoke(new strDel(AddTextInChatBox), newMessage);
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

            if ( IsConnectingThreadWorking || IsGettingMessagesThreadWorking )
            {
                MessageBox.Show("Подключение в процессе. Пожалуйста, дождитесь результата.", "Подключение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (! IsConnectingThreadWorking && ! IsGettingMessagesThreadWorking)
            {
                ConnectionThread.Start();
            }
        }
        private void CloseClientConnection()
        {
            clientIsConnected = false;
            Client.GetInstance().LogOut();
            this.Invoke(new strListDel(RefreshNickNamesListBox), new List<string>());
            this.Invoke(new voidDel(ActivateConnectButton));
        }
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            this.CloseClientConnection();
        }
        private static bool IsConnectingThreadWorking
        {
            get
            {
                bool answer = false;
                if (ConnectionThread != null)
                {
                    answer = ConnectionThread.IsAlive;
                }
                return answer;
            }
        }
        private static bool IsGettingMessagesThreadWorking
        {
            get
            {
                bool answer = false;
                if (GettingMessagesThread != null)
                {
                    answer = GettingMessagesThread.IsAlive;
                }
                return answer;
            }
        }
        private void AddTextInChatBox(string text)
        {
            this.RTMainChatBox.AppendText(text + Environment.NewLine);
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

        private void RequestNickNameButton_Click(object sender, EventArgs e)
        {
            try
            {
                DoRequestNickName();
            }
            catch (ArgumentException exc)
            {
                MessageBox.Show("Ник-нейм не должен состоять из нескольких слов, которые отделены пробелом.",
                    "Некорретный ник-нейм", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка. Скорее всего, сервер прекратил свою работу: \r\n" + exc.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DoRequestNickName()
        {

            NickNameTextBox.Text.Trim();
            string newNick = NickNameTextBox.Text;
            if (NickNameTextBox.Text.Length != 0 && this.IsCorrectNick(newNick))
            {
                Client.GetInstance().RequestToChangeNickName(newNick);
            }
            else
            {
                throw new ArgumentException();
            }
        }
        private bool IsCorrectNick(string nick)
        {
            bool flagSpaceSymbol = (nick.IndexOf(' ') != -1);

            return !(flagSpaceSymbol);
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
        private void NickNamesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!itWasRefreshingNicks && this.NickNamesListBox.SelectedItem != null)
            {
                selectedNick = this.NickNamesListBox.SelectedItem;
                this.Invoke(new strDel(DirectMessageToNickNameInTypingBox), selectedNick.ToString());
            }            
        }
        private void DirectMessageToNickNameInTypingBox (string text)
        {
            this.TypingBox.Clear();
            this.TypingBox.AppendText(text + ": ");
        }
        private static string MapMessage(string text)
        {
            if (selectedNick != null)
            {
                return ("PRIVMSG " + text.Remove(selectedNick.ToString().Length, 1));
            }   
            else
            {
                return ("MSG " + text);
            }
        }
        private void PrepareFormToTyping()
        {
            this.NickNamesListBox.SelectedItem = null;
            this.TypingBox.Text = "";
        }

        private void ClientWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.CloseClientConnection();
            }
            catch { }
        }

        private void NickNamesListBox_DataSourceChanged(object sender, EventArgs e)
        {
            itWasRefreshingNicks = true;
            this.NickNamesListBox.SelectedItem = null;
        }

        private void NickNamesListBox_MouseClick(object sender, MouseEventArgs e)
        {
            object item = this.NickNameTextBox.GetChildAtPoint(e.Location);
            ClientWindow.itWasRefreshingNicks = false;
            this.NickNamesListBox_SelectedIndexChanged(sender, e);
        }

    }
}
