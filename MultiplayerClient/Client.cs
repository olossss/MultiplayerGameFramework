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

namespace MultiplayerClient
{
    public class Client
    {
        private Socket _socket;
        private IPEndPoint _serverEndPoint;
        private ClientListener listener;
        

        public Client() { }

        public void Connect(String IpAddress, int Port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(IpAddress), Port);
        }

        public void StartListening() 
        {
            listener = new ClientListener(_serverEndPoint);
            listener.Listen();
        }

        public void sendMessage(String message)
        {
            _socket.SendTo(new byte[] { }, _serverEndPoint);
        
        }

        




    }
}
