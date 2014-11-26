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
    class Client
    {
        private Socket _clientSocket;
        private IPEndPoint _clientEndPoint;
        public int _clientID { get; private set; }
        private Thread packetSenderThread;
        private bool isPacketSenderActive { get; set; }
        private int currentSequenceNumber { get; set; }

        public Client(IPEndPoint ClientAddress, int ClientID)
        {
            _clientEndPoint = ClientAddress;
            _clientID = ClientID;
        }

        public void Connect() {

            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint _localEndPoint = new IPEndPoint(IPAddress.Loopback, 5678);
            _clientSocket.Bind(_localEndPoint);
            startPacketSender();
        }

        public void Disconnect()
        {
            closePacketSender();
            // Send Disconnect Signal
            _clientSocket.Close();
        }

        public void SendMessage(String message)
        {
            byte[] send_buffer = Encoding.ASCII.GetBytes(message);
            _clientSocket.SendTo(send_buffer, _clientEndPoint);
        }

        private void PacketSenderWorker()
        {
            while (true) { 
                SendMessage("Current Squence Number: " + currentSequenceNumber);
                Thread.Sleep(1500);
                Console.WriteLine("Packet Sent");
            }
        }

        private void startPacketSender()
        {
            isPacketSenderActive = true;
            packetSenderThread = new Thread(new ThreadStart(this.PacketSenderWorker));
            packetSenderThread.Start();
        }

        private void closePacketSender()
        {
            isPacketSenderActive = false;
            packetSenderThread.Join();
        }

        public void AcknolagePacket(int sequenceNumber)
        {

        }

    }
}
