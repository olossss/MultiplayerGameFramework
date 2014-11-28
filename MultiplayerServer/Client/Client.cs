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

namespace MultiplayerClient
{
    public class Client : BaseUDP
    {
        private const int CLIENT_TIMEOUT = 25;

        private AsyncWorker _listener;
        private UInt32 _clientID { get; set; }

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
            _listener.Start();
            SendMessage(PacketFactory.ConnectionRequest());
        }


        #region Packet Handeling
        private void Listen()
        {
            // Might Need To Do A Check Here Agaisnt Client IP
            byte[] data = null;
            EndPoint ep = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

            while (true)//(DateTime.Now < _lastAckTime.AddSeconds(CLIENT_TIMEOUT))
            {
                if (_socket.Available > 0)
                {
                    data = new byte[_socket.Available];
                    _socket.ReceiveFrom(data, ref ep);
                    HandlePacket(data, ep);
                }
            }

        }

        private void HandlePacket(byte[] data, EndPoint RecivedFrom)
        {
            PacketType packetType = Packet.GetPacketType(data);

            switch (packetType)
            {
                case PacketType.ConnectionAccepted:
                {
                    ConnectionAcceptedPacket packet = new ConnectionAcceptedPacket().DecodePacket(data);
                    _clientID = packet._clientID;
                    _remoteEndPoint = (IPEndPoint)RecivedFrom;
                    SendMessage(PacketFactory.Acknollagement(_clientID, 0));
                    break;
                }
                case PacketType.ConnectionRejected:
                    Disconnect();
                    break;

                case PacketType.TestPacket:
                {
                    TestPacket packet = new TestPacket().DecodePacket(data);
                    /*    
                    Console.WriteLine(packet._Int32);
                    Console.WriteLine(packet._UInt32);
                    Console.WriteLine(packet._Int16);
                    Console.WriteLine(packet._UInt16);
                    Console.WriteLine(packet._Boolean);
                    Console.WriteLine(packet._Byte);
                    Console.WriteLine(packet._String);
                    */
                    // Bounce Back
                    SendMessage(data);
                    break;
                }
            }

        }
        #endregion


        private void Disconnect()
        {
            throw new NotImplementedException();
        }







    }
}
