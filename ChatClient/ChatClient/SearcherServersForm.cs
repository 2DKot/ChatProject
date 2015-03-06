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
    public partial class SearcherServersForm : Form
    {
        private SearcherServers searcher;
        private static readonly string stopSearchButtonText = "Стоп";
        private static readonly string startSearchButtonText = "Поиск";
        private List<string> mappedListServers;
        private Dictionary<string, IPEndPoint> copyOfListServer;
        private bool refreshingStatus;
        private Thread refreshingServerListBoxThread;
        public SearcherServersForm()
        {
            InitializeComponent();
            this.ConnectButton.Enabled = false;
            this.SearchButton.Text = startSearchButtonText;
            searcher = new SearcherServers();
            copyOfListServer = new Dictionary<string, IPEndPoint>();
            mappedListServers = new List<string>();
            InitRefreshingServerListBoxThread();
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
        private void InitRefreshingServerListBoxThread()
        {
            refreshingServerListBoxThread = new Thread(RefreshListServer);
        }
        private void RefreshListServer()
        {
            while(refreshingStatus)
            {
                if (copyOfListServer != SearcherServers.findedIEPs )
                {
                    copyOfListServer = SearcherServers.findedIEPs;
                    mappedListServers = MapToList(copyOfListServer);
                    this.Invoke(new Action(ChangeServerListBox));
                }
            }
            
        }
        public void ChangeServerListBox()
        {
            lock (SearcherServers.findedIEPs)
            {
                ServersListBox.DataSource = mappedListServers;
            }
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
            searcher.EndSearchingServers();
            refreshingStatus = false;
            refreshingServerListBoxThread.Join();
            this.SearchButton.Text = startSearchButtonText;
        }
        private void StartSearch()
        {
            searcher.StartSearchingServers();
            refreshingStatus = true;
            if (refreshingServerListBoxThread.ThreadState == ThreadState.Stopped ||
                refreshingServerListBoxThread.ThreadState == ThreadState.Aborted)
            {
                InitRefreshingServerListBoxThread();
            }
            refreshingServerListBoxThread.Start();
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
            
        }

    }
}
