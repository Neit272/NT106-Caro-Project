using System;
using System.Windows.Forms;

namespace Game_Caro
{
    public partial class Dashboard : Form
    {
        private SocketManager socketManager;

        public Dashboard()
        {
            InitializeComponent();
            socketManager = new SocketManager();
        }

        private void btnCreateServer_Click(object sender, EventArgs e)
        {
            socketManager.IsServer = true;
            if (socketManager.CreateServer())
            {
                MessageBox.Show("Server created successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ServerMonitor serverMonitor = new ServerMonitor();
                serverMonitor.Show();
            }
            else
            {
                MessageBox.Show("Failed to create server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConnectAsClient_Click(object sender, EventArgs e)
        {
            socketManager.IsServer = false;
            if (socketManager.ConnectServer())
            {
                MessageBox.Show("Connected to server successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClientInterface clientInterface = new ClientInterface();
                clientInterface.Show();
            }
            else
            {
                MessageBox.Show("Failed to connect to server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
