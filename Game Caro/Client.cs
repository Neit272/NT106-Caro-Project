using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection.Emit;
using System.IO;

namespace Game_Caro
{
    public partial class Client : Form
    {
        private TcpClient _client;
        private string _username;
        private bool _shouldStayConnected = true;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public Client()
        {
            InitializeComponent();
        }

        private void connectionBtn_Click(object sender, EventArgs e)
        {
            if (connectionBtn.Text == "Connect")
            {
                _username = usernameTb.Text;
                connectionBtn.Text = "Disconnect";
                usernameTb.Enabled = false;
                ConnectToServer();
                queueBtn.Enabled = true;
            }
            else if (connectionBtn.Text == "Disconnect")
            {
                DisconnectFromServer();
                _shouldStayConnected = false;
                _cts.Cancel();
                connectionBtn.Text = "Reconnect";
                usernameTb.Enabled = true;
                queueBtn.Enabled = false;
            }
            else if (connectionBtn.Text == "Reconnect")
            {
                _username = usernameTb.Text;
                _shouldStayConnected = true;
                _cts = new CancellationTokenSource();
                ConnectToServer();
                connectionBtn.Text = "Disconnect";
                usernameTb.Enabled = false;
                queueBtn.Enabled = true;
            }
        }

        private void ConnectToServer()
        {
            _client = new TcpClient("localhost", 8080);
            var stream = _client.GetStream();
            var usernameMessage = "username:" + _username;
            var messageBytes = Encoding.UTF8.GetBytes(usernameMessage);
            stream.Write(messageBytes, 0, messageBytes.Length);
            ReceiveMessages();
        }

        private void DisconnectFromServer()
        {
            if (_client != null && _client.Connected)
            {
                _client.Close();
            }
        }

        private async void ReceiveMessages()
        {
            const int MaxMessageSize = 20 * 1024 * 1024; // 20MB
            var buffer = new byte[MaxMessageSize];
            var stream = _client.GetStream();

            while (_shouldStayConnected && _client.Connected)
            {
                try
                {
                    var bytesRead = 0;
                    try
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, _cts.Token);
                    }
                    catch (ObjectDisposedException)
                    {
                        // The NetworkStream has been disposed, stop reading from it
                        break;
                    }

                    var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (message.Contains(":"))
                    {
                        var messageType = message.Substring(0, message.IndexOf(":"));
                        message = message.Substring(message.IndexOf(":") + 1);

                        if (messageType == "opponentInfo")
                        {
                            var infoParts = message.Split(':');
                            var opponentUsername = infoParts[0];
                            var opponentIpAddress = infoParts[1];
                            var opponentPort = int.Parse(infoParts[2]);

                            GameCaro gameCaro = new GameCaro(); //opponentUsername, opponentIpAddress, opponentPort
                            gameCaro.Show();
                        }
                    }
                }
                catch (IOException)
                {
                    if (_shouldStayConnected)
                    {
                        throw;
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            _shouldStayConnected = false;
            _cts.Cancel();
            DisconnectFromServer();
        }

        private void usernameTb_TextChanged(object sender, EventArgs e)
        {
            connectionBtn.Enabled = !string.IsNullOrEmpty(usernameTb.Text);
        }

        private void queueBtn_Click(object sender, EventArgs e)
        {
            if (queueBtn.Text == "Queue")
            {
                AddToQueue();
                queueBtn.Text = "Dequeue";
                connectionBtn.Enabled = false;
            }
            else
            {
                RemoveFromQueue();
                queueBtn.Text = "Queue";
                connectionBtn.Enabled = true;
            }
        }

        private void AddToQueue()
        {
            if (_client != null && _client.Connected)
            {
                var stream = _client.GetStream();
                var queueMessage = "queue:" + _username;
                var messageBytes = Encoding.UTF8.GetBytes(queueMessage);
                stream.Write(messageBytes, 0, messageBytes.Length);
            }
        }

        private void RemoveFromQueue()
        {
            if (_client != null && _client.Connected)
            {
                var stream = _client.GetStream();
                var dequeueMessage = "dequeue:" + _username;
                var messageBytes = Encoding.UTF8.GetBytes(dequeueMessage);
                stream.Write(messageBytes, 0, messageBytes.Length);
            }
        }
    }
}