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
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                    lIP.Text += ip.ToString();
            }

        }

        private void bStartServer_Click(object sender, EventArgs e)
        {
            server.name = tbServerName.Text;
            serverThread = new Thread(server.Start);
            serverThread.Start();
            bStartServer.Enabled = false;
            bStopServer.Enabled = true;
        }

        private void bStopServer_Click(object sender, EventArgs e)
        {
            if (server == null) return;
            server.Stop();
            serverThread.Join();
            bStartServer.Enabled = true;
            bStopServer.Enabled = false;
        }

    }
}
