using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerServer.Common
{
    public class BaseUDP
    {
        public Socket _socket { get; set; }
        public IPEndPoint _localEndPoint { get; set; }
        public IPEndPoint _remoteEndPoint { get; set; }

        public void Setup(IPEndPoint local, IPEndPoint remote = null)
        {
            // CLient Handel - Any Local Port   - Specified remote
            // CLient - Any Local Port          - Specified remote
            // Server - Local Needs Binding     - None No Outgoing Packets Sent

            _localEndPoint = local;
            _remoteEndPoint = remote;
            Setup();
        }

        public void Setup()
        {
            if(_localEndPoint == null)
                _localEndPoint = new IPEndPoint(IPAddress.Loopback, 0);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(_localEndPoint);
        }

        public void SendMessage(String message)
        {
            byte[] send_buffer = Encoding.ASCII.GetBytes(message);
            _socket.SendTo(send_buffer, _remoteEndPoint);
        }

    }
}
