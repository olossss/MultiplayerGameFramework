using MultiplayerServer.Common;
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

namespace MultiplayerServer
{
    public class Server : BaseUDP
    {

        private int _portNumber;

        private List<ServerClientHandle> _clients { get; set; }

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
        }

        public void Setup()
        {
            // Set Up Port
            _localEndPoint = new IPEndPoint(IPAddress.Any, _portNumber);
            base.Setup();

            // Set Up Delegate Functions
            _listener._workerThread = new Thread(new ThreadStart(this.Listen));
        }



        // This is not thread safe and should be run syncrounusly
        public void ConnectClient(IPEndPoint ClientAddress)
        {
            ServerClientHandle newClient = new ServerClientHandle(ClientAddress, getNewClientID());
            _clients.Add(newClient);
            newClient.Connect();
            Console.WriteLine("Client Connected");
        }

        public void DisconnectClient(int ClientID)
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

        private bool ClientExsists(int clientID)
        {
            foreach (ServerClientHandle client in _clients)
                if (client._clientID == clientID)
                    return true;

            return false;
        }

        private int getNewClientID()
        {
            int newClientID = -1; // Generate Random Number??

            while (ClientExsists(newClientID) || newClientID <= 0)
                newClientID++;

            return newClientID;
        }




        private void Listen()
        {
            isRunning = true;

            while (isRunning)
            {
                byte[] data = new byte[256]; // Need to check this with data
                EndPoint ep = (EndPoint)_localEndPoint;
                _socket.ReceiveFrom(data, ref ep);

                Console.WriteLine("Recived from: " + ((IPEndPoint)ep).Address.ToString() + ":" + ((IPEndPoint)ep).Port);
                Console.WriteLine("Data recived:\n" + data.ToString());

                ConnectClient((IPEndPoint)ep);
            }
        }

        public void StartListening()
        {
            _listener.Start();
        }
    }
}
