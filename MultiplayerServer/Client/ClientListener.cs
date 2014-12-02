using MultiplayerFramework.Common;
using MultiplayerFramework.Common.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MultiplayerFramework;

namespace MultiplayerFramework.Client
{
    internal class ClientListener : Listener<Client>
    {
        public ClientListener(Client UdpHandle)
            : base(UdpHandle) 
        {
        
        }

        internal override void CloseRoutine()
        {
            Debug.Instance.Log(DebugLevel.DEBUG, "Client ID: " + _UdpHandle._clientID + " Listner Closed Gracefully");
        }

        internal override int HandlePacket(byte[] packetData, EndPoint RecivedFrom)
        {
            {
                PacketType packetType = Packet.GetPacketType(packetData);
                Debug.Instance.Log(DebugLevel.NETWORK, "(Client ID:" + _UdpHandle._clientID + ") Message Recived from " + ((IPEndPoint)RecivedFrom).ToString() + " Type: " + packetType + "(Size:" + packetData.Length + ")");

                switch (packetType)
                {

                    case PacketType.EmptyBuffer:
                        {
                            Debug.Instance.Log(DebugLevel.NETWORK, "Empty Packet Buffer");
                            return 1;
                        }
                    case PacketType.Ack:
                        {
                            AcknolagementPacket packet = new AcknolagementPacket().DecodePacket(packetData);
                            var sequenceNumber = packet._sequenceNumber;

                            if (sequenceNumber == (UInt32)AcknolagementPacketSignals.CONFIRM_DISCONNECTED)
                                _UdpHandle.Close();
                            return packet.packetSize();
                        }
                    case PacketType.ConnectionAccepted:
                        {
                            ConnectionAcceptedPacket packet = new ConnectionAcceptedPacket().DecodePacket(packetData);
                            _UdpHandle._clientID = packet._clientID;
                            _UdpHandle._remoteEndPoint = (IPEndPoint)_senderAddress;
                            _UdpHandle.SendMessage(PacketFactory.Acknollagement(_UdpHandle._clientID, 0));
                            return packet.packetSize();
                        }

                    case PacketType.ConnectionRejected:
                        {
                            ConnectionRejectedPacket packet = new ConnectionRejectedPacket().DecodePacket(packetData);
                            _UdpHandle.Close();
                            return packet.packetSize();
                        }

                    case PacketType.DisconnectRequest:
                        {
                            DisconnectionRequestPacket packet = new DisconnectionRequestPacket().DecodePacket(packetData);
                            _UdpHandle.SendMessage(PacketFactory.Acknollagement(_UdpHandle._clientID, (UInt32)AcknolagementPacketSignals.CONFIRM_DISCONNECTED));
                            _UdpHandle.Close();
                            return packet.packetSize();
                        }

                    case PacketType.TestPacket:
                        {
                            TestPacket packet = new TestPacket().DecodePacket(packetData);

                            // Bounce Back
                            _UdpHandle.SendMessage(packetData);
                            return packet.packetSize();
                        }

                    default:
                        break;
                }
                throw new Exception("Packet Size Missmatch");
            }
        }
    }
}
