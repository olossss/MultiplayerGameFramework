using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common.Packets
{
    internal class AcknolagementPacket : AbstractBasePacket<AcknolagementPacket>
    {
        public UInt32 _clientID { get; set; }

        public AcknolagementPacket()
        {
            _type = PacketType.Ack;
        }

        public override byte[] EncodePacket()
        {
            return new PacketEncoder(_type)
                .Add(_clientID)
                .Add(_sequenceNumber)
               .BuildPacket();
        }

        public override AcknolagementPacket DecodePacket(byte[] packet)
        {
            PacketDecoder decoder = new PacketDecoder(packet);
            _type = decoder.NextPacketType();
            _clientID = decoder.NextUnsignedInt32();
            _sequenceNumber = decoder.NextUnsignedInt32();
            return this;
        }
    }
}
