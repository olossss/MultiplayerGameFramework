using MultiplayerFramework.Common;
using MultiplayerFramework.Common.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Server
{
    internal class ClientHandleListener : Listener<ClientHandle>
    {
        public ClientHandleListener(ClientHandle UDPHandle)
            : base(UDPHandle) 
        {
        }

        internal override void CloseRoutine()
        {
            Debug.Instance.Log(DebugLevel.DEBUG, "Client Handle ID: " + _UdpHandle._clientID + " Listner Closed Gracefully");
        }

        internal override int HandlePacket(byte[] packetData, EndPoint RecivedFrom)
        {
            PacketType packetType = Packet.GetPacketType(packetData);
            Debug.Instance.Log(DebugLevel.NETWORK, "(Client Handle ID:" + _UdpHandle._clientID + ") Message Recived from " + ((IPEndPoint)RecivedFrom).ToString() + " Type: " + packetType + "(Size:" + packetData.Length + ")");

            switch (packetType)
            {
                case PacketType.EmptyBuffer:
                    {
                        return 1;
                    }
                case PacketType.Ack:
                    {
                        AcknolagementPacket packet = new AcknolagementPacket().DecodePacket(packetData);

                        // Connection Successfull
                        if (packet._sequenceNumber == (UInt32)AcknolagementPacketSignals.CONFIRM_CONNECTED)
                        {
                            Debug.Instance.Log(DebugLevel.DEBUG, "Connected Client (ID:" + _UdpHandle._clientID + ")");
                            _UdpHandle.StartPacketSender();
                            _UdpHandle.clientState = ClientStateEnum.READY;
                        }
                        else if (packet._sequenceNumber == (UInt32)AcknolagementPacketSignals.CONFIRM_DISCONNECTED)
                        {
                            Debug.Instance.Log(DebugLevel.DEBUG, "Disconnected Client (ID:" + _UdpHandle._clientID + ")");
                            _UdpHandle.SendMessage(PacketFactory.Acknollagement(_UdpHandle._clientID, (UInt32)AcknolagementPacketSignals.CONFIRM_DISCONNECTED));
                            _UdpHandle.Close();
                            _UdpHandle.clientState = ClientStateEnum.DISCONNECTED;

                        }
                        else
                        {
                            _UdpHandle._lastAckSequenceNumber = packet._sequenceNumber;
                            _UdpHandle._lastAckTime = DateTime.Now;
                        }

                        return packet.packetSize();
                    }

                case PacketType.TestPacket:
                    {
                        TestPacket packet = new TestPacket().DecodePacket(packetData);
                        return packet.packetSize();
                    }

                case PacketType.DisconnectRequest:
                    {
                        DisconnectionRequestPacket packet = new DisconnectionRequestPacket().DecodePacket(packetData);
                        _UdpHandle.SendMessage(PacketFactory.Acknollagement(_UdpHandle._clientID, (UInt32)AcknolagementPacketSignals.CONFIRM_DISCONNECTED));
                        _UdpHandle.Close();
                        _UdpHandle.clientState = ClientStateEnum.DISCONNECTED;
                        return packet.packetSize();
                    }

                default:
                    break;
            }
            throw new Exception("Packet Size Missmatch");
        }
    }
}
