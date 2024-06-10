using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Game_Caro
{
    class SocketManager
    {
        #region Client
        Socket client;
        List<Socket> clientsNotInQueue = new List<Socket>();
        List<Socket> clientsInQueue = new List<Socket>();

        public bool ConnectServer()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP), Port);

            if (IsServer)
            {
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
                        }
                        catch
                        {
                            break;
                        }
                    }
                });
                AcceptClient.IsBackground = true;
                AcceptClient.Start();

                Form serverMonitor = new ServerMonitor();
                serverMonitor.Show();

                IsServer = false;
                return true;
            }
            else
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    client.Connect(iep);

                    ClientInterface clientInterface = new ClientInterface();

                    clientInterface.Invoke(new Action(() =>
                    {
                        clientInterface.Show();
                    }));

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        #endregion

        #region Server
        Socket server;
        public void CreateServer()
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
                    }
                    catch
                    {
                        break;
                    }
                }
            });
            AcceptClient.IsBackground = true;
            AcceptClient.Start();
        }
        #endregion

        #region Both
        public string IP = "127.0.0.1";
        public int Port = 9999;
        public bool IsServer = true;
        public const int BUFFER = 1024;

        private bool SendData(Socket target, byte[] data)
        {
            return target.Send(data) == 1;
        }

        private bool ReceiveData(Socket target, byte[] data)
        {
            return target.Receive(data) == 1;
        }

        public bool Send(Socket target, object data)
        {
            byte[] sendedData = SerializeData(data);
            return SendData(target, sendedData);
        }

        public object Receive(Socket target)
        {
            byte[] receivedData = new byte[BUFFER]; // 1 lần nhận tin là cỡ bao nhiêu
            bool IsOk = ReceiveData(target, receivedData);
            return DeserializeData(receivedData);
        }

        /// <summary>
        /// Nén đối tượng thành mảng byte[]
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public byte[] SerializeData(Object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf1 = new BinaryFormatter();
            bf1.Serialize(ms, o);
            return ms.ToArray();
        }

        /// <summary>
        /// Giải nén mảng byte[] thành đối tượng object
        /// </summary>
        /// <param name="theByteArray"></param>
        /// <returns></returns>
        public object DeserializeData(byte[] theByteArray)
        {
            MemoryStream ms = new MemoryStream(theByteArray);
            BinaryFormatter bf1 = new BinaryFormatter();
            ms.Position = 0;
            return bf1.Deserialize(ms);
        }

        /// <summary>
        /// Lấy ra IP V4 của card mạng đang dùng
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
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
