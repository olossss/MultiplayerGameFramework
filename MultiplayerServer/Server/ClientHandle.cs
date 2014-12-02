/********************************************
 * ServerClientHandle.cs 
 * The Client Class Handels Outgoing
 * Communications with that client
 *********************************************/
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

namespace MultiplayerFramework.Server
{
    class ClientHandle : BaseUDP
    {
        private const int CLIENT_TIMEOUT = 0;

        public UInt32 _clientID { get; private set; }
        private AsyncWorker _messanger { get; set; }
        private ClientHandleListener _listener { get; set; }
        private UInt32 _currentSequenceNumber { get; set; }
        internal UInt32 _lastAckSequenceNumber { get; set; }
        internal DateTime _lastAckTime { get; set; }
        private Server _server { get; set; }
        public ClientStateEnum clientState { get; internal set; }

        

        public ClientHandle(Server server, IPEndPoint clientAddress, UInt32 clientID)
        {
            clientState = ClientStateEnum.IDLE;
            _server = server;
            _messanger = new AsyncWorker();

            _lastAckTime = DateTime.Now;
            _remoteEndPoint = clientAddress;
            _clientID = clientID;
        }

        public void Connect()
        {
            clientState = ClientStateEnum.CONNECTING;
            // Initalize Socket
            base.Initalize();

            // Tell Client He Will Be Connected
            SendMessage(PacketFactory.ConnectionAccepted(_clientID, _localEndPoint));
            StartPacketListener();
        }

        internal override void Close()
        {
            // DONT JOIN IN CLOSE - THIS IS A LISTENER FUNCTION
            _messanger.Stop();
            _listener.Stop();
        }

        public void Shutdown()
        {
            Close();
            _listener.Join(); // You can't Join here
            //_messanger.Join();

            _socket.Close();
            
            _server.RequestRemove(_clientID);
            Debug.Instance.Log(DebugLevel.DEBUG, "Client Handle Disconnected: " + _clientID);
        }

        public bool isDisconnected()
        {
            if (_listener.isAlive())
            {
                Debug.Instance.Log(DebugLevel.WARNING, "Can Not Disconnect; Listener is active");
                return false;
            }

            if (_messanger.isAlive())
            {
                Debug.Instance.Log(DebugLevel.WARNING, "Can Not Disconnect; Messanger is active");
                return false;
            }

            if (!_socket.IsBound)
            {
                Debug.Instance.Log(DebugLevel.WARNING, "Can Not Disconnect; Socket Is Bound");
                return true;
            }

            return true;
        }

        private void UpdateClient()
        {
            while (_messanger.isRunning())
            {
                try
                {
                    SendMessage(PacketFactory.TestPacket());
                    Thread.Sleep(5);
                }
                catch(ObjectDisposedException){
                    Debug.Instance.Log(DebugLevel.WARNING, "Client Handle Attempted To Send To A Closed Socket");
                    break;
                }
            }

            Debug.Instance.Log(DebugLevel.DEBUG, "Client Handle With ClientID: " + _clientID + " Updater Closed Gracefully");
        }

        internal void StartPacketSender()
        {
            _messanger._workerThread = new Thread(new ThreadStart(this.UpdateClient));
            //_messanger.Start();
        }
        
        private void StartPacketListener()
        {
            _listener = new ClientHandleListener(this);
            _listener.Start();
        }

        public void RequestDisconnect()
        {
            SendMessage(PacketFactory.Disconnect(_clientID));
        }

        

    }
}
