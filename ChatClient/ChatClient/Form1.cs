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
        private delegate void strDel(string arg);
        private delegate void strListDel(List<string> args);

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
                    
                    string sendingMeassage = this.TypingBox.Text;
                    if (this.DebugCheckBox.Checked == false)
                    {
                        User.GetInstance().SendText("MSG " + sendingMeassage);
                    }
                    else
                    {
                        User.GetInstance().SendText(sendingMeassage);
                    }
                    this.TypingBox.Clear();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Ошибка. Скорее всего, сервер прекратил свою работу : " + exc.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                this.EnableGettingNewInformation();
            }
            catch (NullReferenceException exc)
            {
                MessageBox.Show("Подключение не было произведено. Проверьте IP-адрес и порт. Ошибка: " + exc.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Thread gettingMessagesThread;
            List<string> tempNicks = (List<string>)this.NickNamesListBox.DataSource;
            gettingMessagesThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        string newMessage = User.GetInstance().GetMessage();
                        if (this.DebugCheckBox.Checked == false)
                        {
                            newMessage = User.GetInstance().HandleMessage(newMessage);
                        }
                        this.Invoke(new strDel(this.AddTextInChatBox), new object[] { newMessage });
                        if (tempNicks != User.GetInstance().listOfNickNames)
                        {
                            this.Invoke(new strListDel (this.RefreshNickNamesListBox), new object[] {User.GetInstance().listOfNickNames});
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Ошибка. Скорее всего, сервер прекратил свою работу : " + e.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            });
            gettingMessagesThread.IsBackground = true;
            gettingMessagesThread.Start();
        }
        /*private void EnableGettingNickNames()*/

        private void RequestNickNameButton_Click(object sender, EventArgs e)
        {
            try
            {
                NickNameTextBox.Text.Trim();
                string newNick = NickNameTextBox.Text;
                if (NickNameTextBox.Text.Length != 0)
                {
                    User.GetInstance().RequestToChangeNickName(newNick);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка. Скорее всего, сервер прекратил свою работу : " + exc.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RefreshNickNamesListBox (List<string> nicks)
        {
            this.NickNamesListBox.DataSource = nicks;
            this.NickNamesListBox.Refresh();
        }
    }
}
