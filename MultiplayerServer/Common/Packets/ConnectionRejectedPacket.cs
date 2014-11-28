
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
            return null;
        }

        public override ConnectionRejectedPacket DecodePacket(byte[] packet)
        {
            return this;
        }
    }
}
