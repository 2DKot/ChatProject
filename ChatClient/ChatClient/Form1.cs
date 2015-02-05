using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ClientWindow : Form
    {
        public ClientWindow()
        {
            InitializeComponent();

            User.GetInstance().LogIn("127.0.0.1", 666);

            User.GetInstance().ShowMessages();
        }

        private void TypingBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            User.GetInstance().SendText(TypingBox.Text);
        }
    }
}
