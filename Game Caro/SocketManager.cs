using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Collections.Generic;

namespace Game_Caro
{
    class SocketManager
    {
        public delegate void ClientConnectedHandler(Socket client);
        public event ClientConnectedHandler ClientConnected;

        #region Client
        Socket client;
        List<Socket> clientsNotInQueue = new List<Socket>();
        Queue<Socket> clientsInQueue = new Queue<Socket>();

        public bool ConnectServer()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP), Port);

            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client.Connect(iep);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Server
        Socket server;

        public bool CreateServer()
        {
            try
            {
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP), Port);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                server.Bind(iep);
                server.Listen(10);

                Thread AcceptClient = new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            Socket newClient = server.Accept();
                            clientsNotInQueue.Add(newClient);
                            clientsInQueue.Enqueue(newClient);
                            ClientConnected?.Invoke(newClient); // Notify the ServerMonitor form

                            if (clientsInQueue.Count >= 2)
                            {
                                Socket client1 = clientsInQueue.Dequeue();
                                Socket client2 = clientsInQueue.Dequeue();
                                StartNewGame(client1, client2);
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }
                });
                AcceptClient.IsBackground = true;
                AcceptClient.Start();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void StartNewGame(Socket client1, Socket client2)
        {
            GameCaro game1 = new GameCaro(client1, client2);
            GameCaro game2 = new GameCaro(client2, client1);

            game1.Show();
            game2.Show();
        }
        #endregion

        #region Both
        public string IP = "127.0.0.1";
        public int Port = 9999;
        public bool IsServer = true;
        public const int BUFFER = 1024;

        private bool SendData(Socket target, byte[] data)
        {
            return target.Send(data) > 0;
        }

        private bool ReceiveData(Socket target, byte[] data)
        {
            return target.Receive(data) > 0;
        }

        public bool Send(Socket target, object data)
        {
            byte[] sendedData = SerializeData(data);
            return SendData(target, sendedData);
        }

        public object Receive(Socket target)
        {
            byte[] receivedData = new byte[BUFFER];
            bool IsOk = ReceiveData(target, receivedData);
            return DeserializeData(receivedData);
        }

        public byte[] SerializeData(Object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf1 = new BinaryFormatter();
            bf1.Serialize(ms, o);
            return ms.ToArray();
        }

        public object DeserializeData(byte[] theByteArray)
        {
            MemoryStream ms = new MemoryStream(theByteArray);
            BinaryFormatter bf1 = new BinaryFormatter();
            ms.Position = 0;
            return bf1.Deserialize(ms);
        }

        public string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            output = ip.Address.ToString();
            return output;
        }

        public void CloseConnect()
        {
            try
            {
                server.Close();
                foreach (Socket client in clientsNotInQueue)
                {
                    client.Close();
                }
                foreach (Socket client in clientsInQueue)
                {
                    client.Close();
                }
            }
            catch { }
        }
        #endregion
    }
}
