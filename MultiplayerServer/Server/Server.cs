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
using MultiplayerFramework;
using System.Collections.Concurrent;

namespace MultiplayerFramework.Server
{
    public enum ClientStateEnum
    {
        IDLE,
        CONNECTING,
        READY,
        DISCONNECTED
    }
    public class Server : BaseUDP
    {
        // Time It Takes To Timeout A New Connection
        private const int CONNECTION_TIMEOUT = 0;

        public int _portNumber { get; set; }
        //private List<ClientHandle> _clients { get; set; }
        //private ConcurrentQueue<ClientHandle> _clients { get; set; }
        private ClientPool _clients { get; set; }
        internal List<IPEndPoint> _BlackList { get; set; }
        public byte[] serverID { get; private set; }
        public bool isRunning { get; private set; }
        public int maxClients { get; set; }
        internal AsyncWorker _cleaner;

        private ServerListener _listener;

        public Server(int port = 9000)
        {
            _portNumber = port;
            maxClients = 10;
            _clients = new ClientPool();
            _BlackList = new List<IPEndPoint>();
            _cleaner = new AsyncWorker();
        }
        #region StartUp ShutDown
        public void Init()
        {
            // Set Up Port
            _localEndPoint = new IPEndPoint(IPAddress.Any, _portNumber);
            base.Initalize();

            // Start Listener Thread
            _listener = new ServerListener(this);
            _listener.Start();
            _cleaner._workerThread = new Thread(new ThreadStart(this.CleanDisconnections));
            //_cleaner._workerThread.Priority = ThreadPriority.Lowest;
            _cleaner.Start();
        }

        public void ShutDown()
        {
            Debug.Instance.Log(DebugLevel.DEBUG, "Shutting Down Server...");
            // Refuse all incomming clients
            maxClients = 0;

            _clients.SendAllClients(PacketFactory.Disconnect(0));

            // Join On All Clients
            while (_clients.Count() > 0) ;
                //_clients.GetFirstClient().Shutdown();

                Debug.Instance.Log(DebugLevel.DEBUG, "All Clients Disconnected");
            // Close Listener Thread and Socket
            _cleaner.Stop();
            _listener.Stop();
            
            _listener.Join();
            _cleaner.Join();
            _socket.Close();
            Debug.Instance.Log(DebugLevel.DEBUG, "Server Completed Shutdown");
        }

        internal void RequestRemove(UInt32 ClientID)
        {
            ClientHandle client = GetClientHandle(ClientID);

            if (client == null)
            {
                Debug.Instance.Log(DebugLevel.ERROR, "Attempting To Delete A Nonexsitent Client ID; A Race Condition May Have Occured!");
                return;
            }

            //if (client.isDisconnected())
                RemoveClientHandle(client._clientID);
            //else
                //Debug.Instance.Log(DebugLevel.ERROR, "Failed To Remove Client Handle (ClientID: " + ClientID +")");
        }

        #endregion

        private ClientHandle GetClientHandle(UInt32 clientID)
        {
            return _clients.Get(clientID);
        }

        /*
        private int GetClientHandleIndex(UInt32 clientID)
        {
            for (int i = 0; i < _clients.Count(); ++i)
                if (_clients[i]._clientID == clientID)
                    return i;

            return -1;
        }*/

        private void RemoveClientHandle(UInt32 clientID)
        {
           // int index = GetClientHandleIndex(clientID);
            //_clients.RemoveAt(index);
            _clients.RemoveClient(clientID);

        }

        

        internal void ConnectClient(IPEndPoint ClientAddress)
        {
            Debug.Instance.Log(DebugLevel.DEBUG, "Client Connecting To Server (Client Address: " + ClientAddress.ToString() + ")");
            _clients.Add(this, ClientAddress).Connect();
        }

        internal void RejectClient(IPEndPoint ClientAddresss, ConnectionRejectedReason reason, IPEndPoint address)
        {
            Debug.Instance.Log(DebugLevel.DEBUG, "Client Rejected (" + reason + ") " + address.ToString());
            SendMessage(PacketFactory.ConnectionRejected(reason),  address);
        }

        public bool GetAllClientStatus(ClientStateEnum value) {

            return _clients.HasClientWithStatus(value);
        }

        #region Unit Test Functions
        public int ClientCount()
        {
            return _clients.Count();
        }

        internal override void Close()
        {
            // DONT JOIN IN CLOSE - THIS IS A LISTENER FUNCTION
            _listener.Stop();
        }
        #endregion

        private void CleanDisconnections()
        {
            while(_cleaner.isRunning())
            {
                _clients.RemoveDisconnected();
            }
        }
    }
}
