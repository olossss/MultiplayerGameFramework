/********************************************
 * Server.cs
 * The Server Class Handles Incomming Packets
 * and connects clients
 ********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MultiplayerFramework.Common;
using MultiplayerFramework.Common.Packets;

namespace MultiplayerFramework
{
    public class Server : BaseUDP
    {
        // Time It Takes To Timeout A New Connection
        private const int CONNECTION_TIMEOUT = 10;

        private int _portNumber;
        private List<ServerClientHandle> _clients { get; set; }
        private List<IPEndPoint> _BlackList { get; set; }
        public byte[] serverID { get; private set; }
        public bool isRunning { get; private set; }
        public int maxClients { get; set; }

        private AsyncWorker _listener;

        public Server(int port = 9000)
        {
            _portNumber = port;
            _clients = new List<ServerClientHandle>();
            maxClients = 10;
            _listener = new AsyncWorker();
            _BlackList = new List<IPEndPoint>();
        }

        public void Init()
        {
            // Set Up Port
            _localEndPoint = new IPEndPoint(IPAddress.Any, _portNumber);
            base.Setup();

            // Set Up Delegate Functions
            _listener._workerThread = new Thread(new ThreadStart(this.Listen));
        }

        // This is not thread safe and should be run syncrounusly


        public void DisconnectClient(UInt32 ClientID)
        {
            // Remove Client
            foreach (ServerClientHandle client in _clients)
            {
                if (client._clientID == ClientID)
                {
                    client.Disconnect();
                    _clients.Remove(client);
                }
            }
        }

        public void ShutDown()
        {
            isRunning = false;

            foreach (ServerClientHandle client in _clients)
                DisconnectClient(client._clientID);

            _socket.Close();
        }

        private bool ClientExsists(UInt32 clientID)
        {
            foreach (ServerClientHandle client in _clients)
                if (client._clientID == clientID)
                    return true;

            return false;
        }

        private UInt32 getNewClientID()
        {
            UInt32 newClientID = 0;

            while (ClientExsists(newClientID) || newClientID <= 0)
                newClientID++;

            return newClientID;
        }



        #region Packet Handling
        private void Listen()
        {
            isRunning = true;
            EndPoint incommingEndPoint = new IPEndPoint(IPAddress.Any, 0);//(EndPoint)_remoteEndPoint; // Might Need To Do A Check Here Agaisnt Client IP
            byte[] data = null;

            while (isRunning)
            {
                if (_socket.Available > 0)
                {
                    data = new byte[_socket.Available];
                    _socket.ReceiveFrom(data, ref incommingEndPoint);
                    HandlePacket((IPEndPoint)incommingEndPoint, data);
                }
            }
        }

        private void HandlePacket(IPEndPoint ClientAddress, byte[] packetData)
        {
            PacketType packetType = Packet.GetPacketType(packetData);

            if (packetType == PacketType.ConnectionRequest)
            {
                if (_BlackList.Contains(ClientAddress))
                    RejectClient(ClientAddress, ConnectionRejectedReason.Blocked);

                else if (_clients.Count >= maxClients)
                    RejectClient(ClientAddress, ConnectionRejectedReason.ServerFull);

                else
                    ConnectClient(ClientAddress);
            }
        }

        private void ConnectClient(IPEndPoint ClientAddress)
        {
            Console.WriteLine("Attempting To Connect Client " + ClientAddress.ToString() + "... ");
            ServerClientHandle newClient = new ServerClientHandle(ClientAddress, getNewClientID());
            _clients.Add(newClient);
            newClient.Connect();
        }

        private void RejectClient(IPEndPoint ClientAddresss, ConnectionRejectedReason reason)
        {
            Console.WriteLine("Client Rejected (" + reason +")");
            SendMessage(PacketFactory.ConnectionRejected(reason));
        }
        #endregion

        public void StartListening()
        {
            _listener.Start();
        }
    }
}
