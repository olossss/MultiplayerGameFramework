using MultiplayerFramework.Client;
using MultiplayerFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MultiplayerFramework.Server;

namespace UnitTests
{
    [TestFixture]
    public class ConnectionTests
    {
        public int PORT_NUMBER { get; set; }
        public IPAddress SERVER_ADDRESS { get; set; }
        public int SIMULATED_NOISE { get; set; }
        public int SIMULATED_PACKETLOSS { get; set; }

        [SetUp]
        public void Setup(){
        
            // Server Details
            PORT_NUMBER = 9001;
            SERVER_ADDRESS = IPAddress.Loopback;
            SIMULATED_NOISE = 0;
            SIMULATED_PACKETLOSS = 0;

        }

        [Test]
        public void RunLots() {

            int TIMES_TO_RUN = 5;

            for (int i = 1; i <= TIMES_TO_RUN; ++i) {
                Console.WriteLine("TEST RUN " + i);
                Helpers.Sleep(3);
                ConnectMaxClientsServerShutdown(false, 10);
                ConnectMaxClientsServerShutdown(true, 15);

            }
        }
        
        public void ConnectMaxClientsServerShutdown(bool ClientsDisconnectBeforeServerShutdown = false, int clientNumber = 15)
        {
            
            Client[] clients = new Client[clientNumber];


            Server server = new Server(PORT_NUMBER);
            server.Init();
            int ClientCountExpected = (server.maxClients > clientNumber ? clientNumber : server.maxClients);

            Helpers.Sleep();

            for (int i = 0; i < clientNumber; ++i)
            {
                clients[i] = new Client();
                clients[i].Connect(SERVER_ADDRESS, PORT_NUMBER);
            }

            // Wait For Clients To Connect
            while (!server.GetAllClientStatus(ClientStateEnum.READY) || server.ClientCount() != ClientCountExpected) ;
            server.maxClients = 0;

            // Validate All Clients Are Connected
            Helpers.CheckValue(server.ClientCount(), ClientCountExpected);
            Console.WriteLine("All Clients Connected And Ready");

            //Helpers.Sleep(10);

            // Clients Disconnect From Server
            if (ClientsDisconnectBeforeServerShutdown) { 
                for (UInt32 i = 1; i <= ClientCountExpected; ++i){

                    foreach (var client in clients)
                        if (client._clientID == i)
                            client.Disconnect();
                }

                //foreach (Client client in clients)
                    //client.Join();

                // Uncomment This To Pass Completely
                while (server.ClientCount() > 0) ;
                Helpers.CheckValue(server.ClientCount(), 0);
            }

            server.ShutDown();
            Helpers.CheckValue(server.ClientCount(), 0);
        }

        /*
         
        [Test]
        public void ConnectHardDisconnectSingleClient() { }

        

        [Test]
        public void ConnectMaxClients_ClientsDisconnect()
        {
            Client[] clients = new Client[10];

            Server server = new Server(PORT_NUMBER);
            server.Init();

            Helpers.Sleep();

            for (int i = 0; i < clients.Length; ++i )
            {
                clients[i] = new Client();
                clients[i].Connect(SERVER_ADDRESS, PORT_NUMBER);
            }

            for (int i = 0; i < clients.Length; ++i)
            {
                clients[i].Disconnect();
            }

            Console.WriteLine("All Clients Connected");
            Helpers.CheckValue(server.ClientCount(), 10);

            Helpers.Sleep(10);

            server.ShutDown();

            Helpers.CheckValue(server.ClientCount(), 0);
        }

        [Test]
        public void TestConcurrentDebugLog() {
            new DebugTest().RunTest();
        }

        [Test]
        public void HardDisconnectServer() {
            
        }

        [Test]
        public void RejectClientServerFull() { }

        [Test]
        public void RejectClientBanned() { }

        [Test]
        public void ConnectDisconnect500ClientsRandomly() { }
        
        [Test]
        public void ClientConnectionDropAckZero() { }

        [Test]
        public void ClientTimeout() { }
        */
    }
}
