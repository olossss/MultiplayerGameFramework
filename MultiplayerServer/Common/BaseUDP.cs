using MultiplayerFramework;
using MultiplayerFramework.Common.Packets;
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
    public abstract class BaseUDP
    {
        public Socket _socket { get; set; }
        public IPEndPoint _localEndPoint { get; set; }
        public IPEndPoint _remoteEndPoint { get; set; }
        
        public int _simulatedPacketLoss { get; set; }
        public int _simulatedPacketRTT { get; set; }

        /// <summary>
        /// Sets Up The Endpoints And Starts The UDP Socket
        /// </summary>
        /// <param name="local"></param>
        /// <param name="remote"></param>
        public void Initalize(IPEndPoint local, IPEndPoint remote = null)
        {
            // CLient Handel - Any Local Port   - Specified remote
            // CLient - Any Local Port          - Specified remote
            // Server - Local Needs Binding     - None No Outgoing Packets Sent

            _localEndPoint = local;
            _remoteEndPoint = remote;
            Initalize();
        }

        public void Initalize()
        {
            if (_localEndPoint == null)
                _localEndPoint = new IPEndPoint(IPAddress.Loopback, 0);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(_localEndPoint);
        }

        /// <summary>
        /// Sends A Message To The Remote EndPoint
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(Byte[] message)
        {
            SendMessage(message, _remoteEndPoint);
        }

        /// <summary>
        /// Sends A Message To A Specified EndPoint
        /// </summary>
        /// <param name="message"></param>
        /// <param name="remoteEndPoint"></param>
        public void SendMessage(Byte[] message, IPEndPoint remoteEndPoint)
        {
            #if DEBUG
            Random random = new Random((int)DateTime.Now.Ticks);
            if (random.Next(0, 100) < _simulatedPacketLoss)
                return;

            Thread.Sleep(_simulatedPacketRTT);
            #endif

            Debug.Instance.Log(DebugLevel.NETWORK, "Message Sent from " + _socket.LocalEndPoint.ToString() + " Type: " + ((PacketType)message[0]).ToString());
            _socket.SendTo(message, remoteEndPoint);
        }

        /// <summary>
        /// Closes All Running Threads On This EndPoint
        /// </summary>
        internal abstract void Close();

    }

}