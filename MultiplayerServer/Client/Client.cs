/*********************************************
 * Client.cs
 * Handels A Client Connection And Sends
 * Messages To The Server.
 *********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MultiplayerFramework;
using MultiplayerFramework.Common;
using MultiplayerFramework.Common.Packets;

namespace MultiplayerFramework.Client
{
    public class Client : BaseUDP
    {
        private const int CLIENT_TIMEOUT = 25;
        private Listener<Client> _listener { get; set; }
        public UInt32 _clientID { get; set; }

        public void Connect(String IpAddress, int Port)
        {
            Connect(IPAddress.Parse(IpAddress), Port);
        }

        public void Connect(IPAddress IpAddress, int Port)
        {
            _localEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _remoteEndPoint = new IPEndPoint(IpAddress, Port);
            base.Initalize();

            SendMessage(PacketFactory.ConnectionRequest());
            _listener = new ClientListener(this);
            _listener.Start();
        }

        internal override void Close()
        {
            // DONT JOIN IN CLOSE - THIS IS A LISTENER FUNCTION
            _listener.Stop();
            _socket.Close();
        }

        public void Disconnect()
        {
            SendMessage(PacketFactory.Disconnect(_clientID));
            //Join();
        }

        public void Join()
        {
            _listener.Join();
        }
    }
}
