/********************************************
 * Abstract Base Packet.cs
 * Does Stuff
 * 
 ********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common.Packets
{
    public enum PacketType
    {
        EmptyBuffer,
        Ack,
        ConnectionRequest,
        ConnectionAccepted,
        ConnectionRejected,
        DisconnectRequest,
        TestPacket
    }

    public enum ConnectionRejectedReason
    {
        Blocked,
        ServerFull
    }

    internal class Packet {
        private const int PACKET_TYPE_INDEX = 0;

        public static PacketType GetPacketType(Byte[] packetData)
        { 
            return (PacketType)packetData[PACKET_TYPE_INDEX];
        }
    }

    internal abstract class AbstractBasePacket<T> where T : AbstractBasePacket<T>
    {
        public UInt32 _gameID { get; set; }
        public UInt32 _serverID { get; set; }
        public PacketType _type { get; protected set; }
        public UInt32 _sequenceNumber { get; set; }

        public abstract byte[] EncodePacket();
        public abstract T DecodePacket(byte[] packet);
        public int packetSize()
        {
            return EncodePacket().Length;
        }
        
    }

}
