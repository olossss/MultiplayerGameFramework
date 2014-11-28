/********************************************
 * ServerClientHandle.cs 
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
using MultiplayerFramework.Common;
using MultiplayerFramework.Common.Packets;

namespace MultiplayerFramework
{
    class ServerClientHandle : BaseUDP
    {
        private const int CLIENT_TIMEOUT = 25;

        public UInt32 _clientID { get; private set; }
        private AsyncWorker _messanger { get; set; }
        private AsyncWorker _listener { get; set; }
        private UInt32 _currentSequenceNumber { get; set; }
        private UInt32 _lastAckSequenceNumber { get; set; }
        private DateTime _lastAckTime { get; set; }

        public ServerClientHandle(IPEndPoint clientAddress, UInt32 clientID)
        {
            _messanger = new AsyncWorker();
            _listener = new AsyncWorker();
            _lastAckTime = DateTime.Now;
            _remoteEndPoint = clientAddress;
            _clientID = clientID;
        }

        #region Basic Client Functions
        public void Connect()
        {
            // Initalize Socket
            base.Setup();

            // Tell Client He Will Be Connected
            SendMessage(PacketFactory.ConnectionAccepted(_clientID, _localEndPoint));
            StartPacketListener();
        }

        public void Disconnect()
        {
            closePacketSender();
            // Send Disconnect Signal
            _socket.Close();
        }

        private void UpdateClient()
        {
            while (true)
            {
                SendMessage(PacketFactory.TestPacket());
                Thread.Sleep(1500);
                Console.WriteLine("Packet Sent");
            }
        }

        private void StartPacketSender()
        {
            _messanger._workerThread = new Thread(new ThreadStart(this.UpdateClient));
            _messanger.Start();
        }

        private void StartPacketListener()
        {
            _listener._workerThread = new Thread(new ThreadStart(this.Listen));
            _listener.Start();
        }

        private void closePacketSender()
        {
            _messanger.Stop();
        }

        public void AcknolagePacket(int sequenceNumber)
        {

        }
        #endregion

        #region Packet Handeling
        private void Listen()
        {
            EndPoint ep = (EndPoint)_remoteEndPoint; // Might Need To Do A Check Here Agaisnt Client IP
            byte[] data = null;

            while (DateTime.Now < _lastAckTime.AddSeconds(CLIENT_TIMEOUT))
            {
                if(_socket.Available > 0)
                {
                    data = new byte[_socket.Available];
                    _socket.ReceiveFrom(data, ref ep);
                    HandlePacket(data);
                }
            }

            Console.WriteLine("Client {0} Timed Out", _clientID);
            Disconnect();
        }

        public void HandlePacket(byte[] packetData)
        {
            PacketType packetType = Packet.GetPacketType(packetData);

            Console.WriteLine("Packet Recived {0} (Size: {1})", packetType, packetData.Length);

            switch (packetType)
            {
                case PacketType.Ack:
                {
                    AcknolagementPacket packet = new AcknolagementPacket().DecodePacket(packetData);

                    // Connection Successfull
                    if (packet._sequenceNumber == 0)
                    {
                        Console.WriteLine("Success");
                        StartPacketSender();
                    }

                    _lastAckSequenceNumber = packet._sequenceNumber;
                    _lastAckTime = DateTime.Now;
                    break;
                }
                case PacketType.TestPacket:
                {
                    TestPacket packet = new TestPacket().DecodePacket(packetData);
                    break;
                }

                default:    
                    break;
            }

        }
        #endregion

    }
}
