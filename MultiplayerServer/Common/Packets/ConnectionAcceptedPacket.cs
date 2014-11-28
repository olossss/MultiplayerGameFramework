using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common.Packets
{
    internal class ConnectionAcceptedPacket : AbstractBasePacket<ConnectionAcceptedPacket>
    {
        public UInt32 _clientID { get; set; }

        public ConnectionAcceptedPacket()
        {
            _type = PacketType.ConnectionAccepted;
        }

        public override byte[] EncodePacket()
        {
            return new PacketEncoder(_type)
                .Add(_clientID)
                .BuildPacket();
        }

        public override ConnectionAcceptedPacket DecodePacket(byte[] packet)
        {
            PacketDecoder decoder = new PacketDecoder(packet);
            _type = (PacketType)decoder.NextByte();
            _clientID = decoder.NextUnsignedInt32();
            return this;
        }

    }
}
