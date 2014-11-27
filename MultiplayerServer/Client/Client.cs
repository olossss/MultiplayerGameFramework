using MultiplayerServer.Common;
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
    public class Client : BaseUDP
    {
        private AsyncWorker _listener;

        public Client()
        {
            _listener = new AsyncWorker();
            _listener._workerThread = new Thread(new ThreadStart(this.Listen));
        }

        public void Connect(String IpAddress, int Port)
        {
            _localEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _remoteEndPoint = new IPEndPoint(IPAddress.Parse(IpAddress), Port);
            base.Setup();
        }

        public void StartListening() 
        {
            _listener.Start();
        }

        private void Listen()
        {
            Console.WriteLine("Listen Worker Started");

            while (true)
            {
                byte[] data = new byte[256];
                EndPoint ep = (EndPoint)_remoteEndPoint;
                _socket.ReceiveFrom(data, ref ep);

                Console.WriteLine("Recived from: " + ((IPEndPoint)ep).Address.ToString() + ":" + ((IPEndPoint)ep).Port);
                Console.WriteLine("Data recived:\n" + data.ToString() + Environment.NewLine);

            }
        }

    }
}
