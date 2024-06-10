using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Game_Caro
{
    public partial class ServerMonitor : Form
    {
        private SocketManager socketManager;

        public ServerMonitor()
        {
            InitializeComponent();
            socketManager = new SocketManager();
            socketManager.ClientConnected += SocketManager_ClientConnected;

            if (socketManager.CreateServer())
            {
                serverMonitorRtb.AppendText("Server started and listening for connections...\n");
            }
            else
            {
                serverMonitorRtb.AppendText("Failed to start the server.\n");
            }
        }

        private void SocketManager_ClientConnected(Socket client)
        {
            string clientInfo = $"Client connected: {client.RemoteEndPoint}\n";
            serverMonitorRtb.Invoke((MethodInvoker)delegate
            {
                serverMonitorRtb.AppendText(clientInfo);
            });
        }

        private void ServerMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            socketManager.CloseConnect();
        }
    }
}
