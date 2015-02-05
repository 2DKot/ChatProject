using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ClientWindow : Form
    {
        public delegate void strDel(string arg);

        public ClientWindow()
        {
            InitializeComponent();
        }

        private void TypingBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                string sendingMeassage = TypingBox.Text;
                User.GetInstance().SendText("MSG " + sendingMeassage);
            }
            catch
            {
                MessageBox.Show("Проверьте сервер.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Производит подключение и активизирует получение сообщений от сервера
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                string ipAdrs = this.IPAdressTextBox.Text;
                int port = Convert.ToInt32(this.PortTextBox.Text);
                User.GetInstance().LogIn(ipAdrs, port);
                string newMessage = "";
                newMessage = User.GetInstance().GetMessage();
                this.AddTextInChatBox(newMessage);
                this.EnableGettingMessages();
            }
            catch
            {
                //Log about bad connection to box
                MessageBox.Show("Connect is failed. Check ip-adress and port.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void EnableGettingMessages()
        {
            Thread gettingMessagesThread;
            gettingMessagesThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        string newMessage = "";
                        newMessage = User.GetInstance().GetMessage();
                        newMessage = User.GetInstance().HandleMessage(newMessage);
                        this.Invoke(new strDel(this.AddTextInChatBox), new object[] { newMessage });
                    }
                    catch (NullReferenceException e)
                    {
                        MessageBox.Show("Ошибка. Скорее всего, сервер прикратил свою работу. \r\n" + e.ToString(),
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });
            gettingMessagesThread.IsBackground = true;
            gettingMessagesThread.Start();
        }

        private void RequestNickNameButton_Click(object sender, EventArgs e)
        {

            NickNameTextBox.Text.Trim();
            string newNick = NickNameTextBox.Text;
            if (NickNameTextBox.Text.Length != 0)
            {
                User.GetInstance().RequestToChangeNickName(newNick);
            }
        }

    }
}
