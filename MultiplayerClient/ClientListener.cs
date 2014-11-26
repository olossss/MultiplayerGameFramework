/*********************************************
 * ClientListener.cs
 * Handels Incomming packets for a client
 * 
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
    public class ClientListener
    {
        private Socket _socket;
        public IPEndPoint remoteEndPoint { get; set; } // Server
        public IPEndPoint localEndPoint { get; set; } //  Client

        public bool isListening { get; private set; }
        Thread ListenerWorkerThread;

        public ClientListener(IPEndPoint serverEndPoint)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            localEndPoint = new IPEndPoint(IPAddress.Any, 6666); // Address of server or of client??
            _socket.Bind(localEndPoint); 
        }
        
        private void ListenWorker() 
        {
            Console.WriteLine("Listen Worker Started");

            isListening = true;

            while (isListening)
            {
                byte[] data = new byte[256];
                EndPoint ep = (EndPoint)localEndPoint;
                _socket.ReceiveFrom(data, ref ep);

                Console.WriteLine("Recived from: " + ((IPEndPoint)ep).Address.ToString() + ":" + ((IPEndPoint)ep).Port);
                Console.WriteLine("Data recived:\n" + data.ToString() + Environment.NewLine);

            }
        }

        public void Listen()
        {
            isListening = true;
            ListenerWorkerThread = new Thread(new ThreadStart(this.ListenWorker));
            ListenerWorkerThread.Start();
        }

        public void StopListen()
        {
            isListening = false;
            ListenerWorkerThread.Join();
        }



    }
}
