#if !TESTING
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ServerSearcherForm : Form
    {
        private ServerSearcher searcher;
        private static readonly string stopSearchButtonText = "Стоп";
        private static readonly string startSearchButtonText = "Поиск";
        private List<string> mappedListServers;
        private Dictionary<string, IPEndPoint> copyOfListServer;
        public ServerSearcherForm()
        {
            InitializeComponent();
            this.ConnectButton.Enabled = false; //!!!
            this.SearchButton.Text = startSearchButtonText;
            searcher = new ServerSearcher(new MUdpClient(), 667);
            searcher.ListChanged += ChangeServerListBox;
            mappedListServers = new List<string>();
        }
        private List<string> MapToList(Dictionary<string, IPEndPoint> dict)
        {
            List<string> rList = new List<string>();
            Dictionary<string, IPEndPoint>.Enumerator enumerator = dict.GetEnumerator();
            string elemOfList;
            while (enumerator.MoveNext())
            {
                elemOfList = enumerator.Current.Key + " " + enumerator.Current.Value.ToString();
                rList.Add(elemOfList);
            }
            return rList;
        }
        private void ChangeServerListBox()
        {
            mappedListServers = MapToList(searcher.FindedIpEPs);
            this.Invoke(new Action(() => ServersListBox.DataSource = mappedListServers));
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (this.SearchButton.Text == stopSearchButtonText)
            {
                StopSearch();
            }
            else
            {
                StartSearch();
            }
        }
        private void StopSearch()
        {
            if (searcher.FindingStatus)
            {
                searcher.EndSearchingServers();
            }
           
            this.SearchButton.Text = startSearchButtonText;
        }
        private void StartSearch()
        {
            ResetServersList();
            searcher.StartSearchingServers();
            this.SearchButton.Text = stopSearchButtonText;
        }

        private void ServersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ServersListBox.Items != null && this.ServersListBox.SelectedItem != null)
            {
                ConnectButton.Enabled = true;
            }
            else
            {
                ConnectButton.Enabled = false;
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            StopSearch();
            string servName = this.mappedListServers[this.ServersListBox.SelectedIndex].Split(new char[]{' '})[0];
            IPEndPoint tempIEP = searcher.FindedIpEPs[servName];
            ClientForm form = new ClientForm(servName, tempIEP);
            this.Hide();
            form.ShowDialog();
            this.Show();
        }
        private void ResetServersList()
        {
            mappedListServers = new List<string>();
            copyOfListServer = new Dictionary<string, IPEndPoint>();
            this.ServersListBox.DataSource = mappedListServers;
        }

        private void SearcherServersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopSearch();
        }

    }
}
#endif