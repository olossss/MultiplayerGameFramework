
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common.Packets
{
    internal class ConnectionRejectedPacket : AbstractBasePacket<ConnectionRejectedPacket>
    {
        public ConnectionRejectedReason _reason { get; set; }

        public ConnectionRejectedPacket()
        {
            _type = PacketType.ConnectionRejected;
        }

        public override byte[] EncodePacket()
        {
            return new PacketEncoder(PacketType.ConnectionRejected)
                .Add((Byte)_reason)
                .BuildPacket();
        }

        public override ConnectionRejectedPacket DecodePacket(byte[] packet)
        {
            PacketDecoder decoder = new PacketDecoder(packet);
            _type = decoder.NextPacketType();
            _reason = (ConnectionRejectedReason)decoder.NextByte();
            return this;
        }
    }
}
