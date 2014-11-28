using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common
{
    public class BaseUDP
    {
        public Socket _socket { get; set; }
        public IPEndPoint _localEndPoint { get; set; }
        public IPEndPoint _remoteEndPoint { get; set; }
        
        public int _simulatedPacketLoss { get; set; }
        public int _simulatedPacketRTT { get; set; }

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
            if (_localEndPoint == null)
                _localEndPoint = new IPEndPoint(IPAddress.Loopback, 0);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(_localEndPoint);

            Console.WriteLine("Local EndPoint: " + _localEndPoint.ToString());
            //Console.WriteLine("Remote EndPoint: " + _remoteEndPoint.ToString());
        }

        public void SendMessage(Byte[] message)
        {
            #if DEBUG
            Random random = new Random((int)DateTime.Now.Ticks);
            if (random.Next(0,100) < _simulatedPacketLoss)
                return;
            
            Thread.Sleep(_simulatedPacketRTT);
            #endif

            Console.WriteLine("Sending Message To: " + _remoteEndPoint.ToString());
            _socket.SendTo(message, _remoteEndPoint);
        }



    }

}