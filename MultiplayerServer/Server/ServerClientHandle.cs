using MultiplayerServer.Common;
/********************************************
 * Client.cs 
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

namespace MultiplayerServer
{
    class ServerClientHandle : BaseUDP
    {
        public int _clientID { get; private set; }
        private AsyncWorker _messanger { get; set; }
        private AsyncWorker _listener { get; set; }
        private int _currentSequenceNumber { get; set; }
        private int _lastAckSequenceNumber { get; set; }

        public ServerClientHandle(IPEndPoint clientAddress, int clientID)
        {
            _messanger = new AsyncWorker();
            _listener = new AsyncWorker();
            _remoteEndPoint = clientAddress;
            _clientID = clientID;
        }

        public void Connect() 
        {
            base.Setup();
            startPacketSender();
        }

        public void Disconnect()
        {
            closePacketSender();
            // Send Disconnect Signal
            _socket.Close();
        }

        private void UpdateClient()
        {
            while (true) { 
                SendMessage("Current Squence Number: " + _currentSequenceNumber);
                Thread.Sleep(1500);
                Console.WriteLine("Packet Sent");
            }
        }

        private void startPacketSender()
        {
            _messanger._workerThread = new Thread(new ThreadStart(this.UpdateClient));
            _messanger.Start();
        }

        private void closePacketSender()
        {
            _messanger.Stop();
        }

        public void AcknolagePacket(int sequenceNumber)
        {

        }

    }
}
