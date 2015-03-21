using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    public partial class ServerForm : Form
    {
        Server server;
        Thread serverThread;
        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            server = new Server();
            User.SetOnListChangedHandler(UserListChanged);
            Log.LogEvent += LogUpdated;
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    lIP.Text += ip.ToString();
            }
            lState.BackColor = Color.IndianRed;
        }

        private void bStartServer_Click(object sender, EventArgs e)
        {
            bStartServer.Enabled = false;
            server.name = tbServerName.Text;
            serverThread = new Thread(server.Start);
            serverThread.Start();
            bStartServer.Visible = false;
            bStopServer.Visible = true;
            bStopServer.Enabled = true;
            this.Text = "ChatServer (online)";
            lState.BackColor = Color.PaleGreen;
            lState.Text = "online";
        }

        private void bStopServer_Click(object sender, EventArgs e)
        {
            bStopServer.Enabled = false;
            if (server == null) return;
            server.Stop();
            serverThread.Join();
            bStartServer.Visible = true;
            bStartServer.Enabled = true;
            bStopServer.Visible = false;
            this.Text = "ChatServer";
            lState.BackColor = Color.IndianRed;
            lState.Text = "offline";
        }

        private void lHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Server was developed by Baydin (2DKot) Konstantin.\nKubSTU. 2015.");
        }

        void UserListChanged(List<User> users)
        {
            if (!this.lbUsers.Disposing && !this.lbUsers.IsDisposed)
                this.Invoke(new Action<List<User>>(UpdateUserList), users);

        }

        void UpdateUserList(List<User> users)
        {
            lbUsers.Items.Clear();
            foreach (User user in users)
            {
                lbUsers.Items.Add(user.name + " " + user.client.Client.RemoteEndPoint.ToString());
            }
        }

        void LogUpdated(string msg)
        {
            if(!this.tbLog.Disposing && !this.tbLog.IsDisposed) 
                this.Invoke(new Action<string>(UpdateLog), msg);
        }

        void UpdateLog(string msg)
        {
            tbLog.AppendText(msg + "\r\n");
        }
        
        private void bKick_Click(object sender, EventArgs e)
        {
            if (lbUsers.SelectedIndex < 0) return;
            User user = User.Get(lbUsers.SelectedIndex);
            user.Remove();
        }

        private void bSendToAll_Click(object sender, EventArgs e)
        {
            string msg = tbMessage.Text;
            if (!cbDebug.Checked) msg = "MSG " + msg;
            User.SendMessageToAll(msg);
        }

        private void bSendToCurrent_Click(object sender, EventArgs e)
        {
            if (lbUsers.SelectedIndex < 0) return;
            string msg = tbMessage.Text;
            if (!cbDebug.Checked) msg = "MSG " + msg;
            User.Get(lbUsers.SelectedIndex).SendMessage(msg);
        }

        private void bClearLog_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
        }

        private void bDeleteLogFile_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Вы уверены?", "Одумайся!", MessageBoxButtons.YesNo);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                Log.Delete();
            }
        }

        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            server.Stop();
        }
    }
}
