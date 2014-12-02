using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common.Packets
{
    internal class DisconnectionRequestPacket : AbstractBasePacket<DisconnectionRequestPacket>
    {
        public UInt32 _clientID { get; set; }
        public DisconnectionRequestPacket()
        {
            _type = PacketType.DisconnectRequest;
        }

        public override byte[] EncodePacket()
        {
            return new PacketEncoder(PacketType.DisconnectRequest)
                .Add(_clientID)
                .BuildPacket();
        }

        public override DisconnectionRequestPacket DecodePacket(byte[] packet)
        {
            PacketDecoder decoder = new PacketDecoder(packet);
            _type = decoder.NextPacketType();
            _clientID = decoder.NextUnsignedInt32();
            return this;
        }
    }
}
