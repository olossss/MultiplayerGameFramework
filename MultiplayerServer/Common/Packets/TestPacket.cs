using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common.Packets
{
    class TestPacket : AbstractBasePacket<TestPacket>
    {
        public Int32 _Int32 { get; set; }
        public UInt32 _UInt32 { get; set; }
        public Int16 _Int16 { get; set; }
        public UInt16 _UInt16 { get; set; }
        public Boolean _Boolean { get; set; }
        public Byte _Byte { get; set; }
        public String _String { get; set; }

        public TestPacket() 
        {
            _type = PacketType.TestPacket;
        }

        public override byte[] EncodePacket()
        {
            return new PacketEncoder(_type)
                .Add(_Int32)
                .Add(_UInt32)
                .Add(_Int16)
                .Add(_UInt16)
                .Add(_String)
                .Add(_Boolean)
                .Add(_Byte)
                .BuildPacket();
        }

        public override TestPacket DecodePacket(byte[] packet)
        {
            PacketDecoder decoder = new PacketDecoder(packet);
            _type = (PacketType)decoder.NextByte();
            _Int32 = decoder.NextInt32();
            _UInt32 = decoder.NextUnsignedInt32();
            _Int16 = decoder.NextInt16();
            _UInt16 = decoder.NextUnsignedInt16();
            _String = decoder.NextString();
            _Boolean = decoder.NextBool();
            _Byte = decoder.NextByte();
            
            return this;
        }

    }
}
