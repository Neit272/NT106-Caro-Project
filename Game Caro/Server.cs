using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Caro
{
    public partial class Server : Form
    {
        private TcpListener _listener;
        private const int Port = 8080;
        private bool _isRunning;

        private Dictionary<string, TcpClient> _connectedClients = new Dictionary<string, TcpClient>();
        private Queue<TcpClient> _clientQueue = new Queue<TcpClient>();

        public Server()
        {
            InitializeComponent();
            StartServer();
        }

        private void StartServer()
        {
            _listener = new TcpListener(IPAddress.Any, Port);
            _listener.Start();
            _isRunning = true;

            AppendToMonitor("Server started.\nWaiting for connection...");

            Task.Run(async () =>
            {
                while (_isRunning)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    await Task.Run(() => HandleClient(client));
                }
            });
        }

        private async void HandleClient(TcpClient client)
        {
            const int MaxMessageSize = 20 * 1024 * 1024; // 20MB
            var buffer = new byte[MaxMessageSize];
            var stream = client.GetStream();
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            if (!message.StartsWith("username:"))
            {
                // Handle error: first message from client should be username
                return;
            }

            var username = message.Substring(9);
            _connectedClients[username] = client;

            AppendToMonitor("Client " + username + " connected!");

            while (_isRunning && client.Connected)
            {
                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    AppendToMonitor("Client " + username + " disconnected!");
                    _connectedClients.Remove(username);
                    client.Close();
                    return;
                }

                message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (message.StartsWith("queue:"))
                {
                    _clientQueue.Enqueue(client);

                    AppendToMonitor("Client " + username + " added to queue!");
                }
                else if (message.StartsWith("dequeue:"))
                {
                    if (_clientQueue.Contains(client))
                    {
                        _clientQueue = new Queue<TcpClient>(_clientQueue.Where(c => c != client));

                        AppendToMonitor("Client " + username + " removed from queue!");
                    }
                }
            }
        }
        private void AppendToMonitor(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendToMonitor), new object[] { message });
                return;
            }

            serverMonitor.AppendText(message + Environment.NewLine);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            _isRunning = false;
            _listener.Stop();
        }
    }
}
