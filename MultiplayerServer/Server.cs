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
using System.Threading.Tasks;

namespace MultiplayerServer
{
    public class Server
    {
        private Socket _socket;
        private int _portNumber;

        private List<Client> Clients;

        public byte[] serverID { get; private set; }
        public bool isRunning { get; private set; }
        public int maxClients { get; set; }

        public Server(int port = 9000)
        {
            _portNumber = port;
            Clients = new List<Client>();
            maxClients = 10;
        }

        IPEndPoint remoteEndPoint;
        public void Setup()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, _portNumber);
            _socket.Bind((EndPoint)remoteEndPoint);
        }

        // This is not thread safe and should be run syncrounusly
        public void ConnectClient(IPEndPoint ClientAddress)
        {
            ClientAddress.Port = 6666; // Do Something With This
            Client newClient = new Client(ClientAddress, getNewClientID());
            Clients.Add(newClient);
            newClient.Connect();
            Console.WriteLine("Client Connected");
        }

        public void DisconnectClient(int ClientID)
        {
            // Remove Client
            foreach (Client client in Clients)
            {
                if (client._clientID == ClientID)
                {
                    client.Disconnect();
                    Clients.Remove(client);
                }
            }
        }

        public void ShutDown() {

            isRunning = false;

            foreach (Client client in Clients)
                DisconnectClient(client._clientID);
            
            _socket.Close();
        }

        private bool ClientExsists(int clientID)
        {
            foreach (Client client in Clients)
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

        public void StartServer()
        {
            isRunning = true;

            while (isRunning)
            {
                byte[] data = new byte[256]; // Need to check this with data
                EndPoint ep = (EndPoint)remoteEndPoint;
                _socket.ReceiveFrom(data, ref ep);

                Console.WriteLine("Recived from: " + ((IPEndPoint)ep).Address.ToString() + ":" + ((IPEndPoint)ep).Port);
                Console.WriteLine("Data recived:\n" + data.ToString());

                ConnectClient((IPEndPoint)ep);
            }
        }

    }
}
