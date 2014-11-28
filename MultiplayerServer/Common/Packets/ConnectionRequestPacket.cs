using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common.Packets
{
    internal class ConnectionRequestPacket : AbstractBasePacket<ConnectionRequestPacket>
    {
        public ConnectionRequestPacket()
        {
            _type = PacketType.ConnectionRequest;
        }

        public override byte[] EncodePacket()
        {
            return new PacketEncoder(_type)
                .BuildPacket();
        }

        public override ConnectionRequestPacket DecodePacket(byte[] packet)
        {
            PacketDecoder decoder = new PacketDecoder(packet);
            _type = (PacketType)decoder.NextByte();
            return this;
        }
    }
}
