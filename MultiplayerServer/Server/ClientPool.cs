using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Server
{
    class ClientPool
    {

        private Dictionary<UInt32, ClientHandle> _clients;
        private Object syncLock;

        private UInt32 newClientID = 0;
        public ClientPool() {

            _clients = new Dictionary<UInt32, ClientHandle>();
            syncLock = new Object();
        }


        public ClientHandle Add(Server server, IPEndPoint ClientAddress)
        {
            lock (syncLock)
            {
                // Generate New Client ID
                //UInt32 newClientID = 0;
                while (_clients.ContainsKey(newClientID) || newClientID <= 0)
                    newClientID++;

                // Add Client
                ClientHandle newClient = new ClientHandle(server, ClientAddress, newClientID);
                _clients.Add(newClient._clientID, newClient);

                return _clients[newClientID];
            }
        }

        public ClientHandle Get(UInt32 ClientID)
        { 
            return _clients[ClientID];
        }

        public void RemoveClient(UInt32 clientID) {
        
            lock(syncLock){
                _clients.Remove(clientID);
            }
        }

        public int Count() { 
            lock(syncLock){
                return _clients.Count;
            }
        }

        public void RemoveDisconnected() {

            lock (syncLock) {
                UInt32 remove = 0;
                foreach(UInt32 key in _clients.Keys)
                {
                    if( _clients[key].clientState == ClientStateEnum.DISCONNECTED ){
                        remove = key;
                        break;
                    }
                }

                if(remove > 0){
                    Debug.Instance.Log(DebugLevel.DEBUG, "Removing Client ID: " + remove);
                    _clients.Remove(remove);
                }
            }

        }

        public bool HasClientWithStatus(ClientStateEnum value)
        {
            lock (syncLock) { 
            foreach (UInt32 key in _clients.Keys)
            {
                if (_clients[key].clientState != value)
                    return false;
            }

            return true;
            }
        }

        internal void SendAllClients(byte[] message, UInt32 exception = 0)
        {
            lock (syncLock)
            {
                foreach (UInt32 key in _clients.Keys)
                {
                    if (key != exception)
                        _clients[key].SendMessage(message);
                }
            }
        }
    }
}
