using MultiplayerFramework.Common.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common
{
    internal class PacketDecoder
    {
        private Byte[] _packet;
        private int _index { get; set; }
        public PacketDecoder(Byte[] packet)
        {
            _packet = packet;
            _index = 0;
        }

        #region Type Functions

        internal PacketType NextPacketType()
        {
            return (PacketType)_packet[_index++];
        }

        internal Byte NextByte()
        {
            return _packet[_index++];
        }

        internal Byte[] NextByteArray(int size)
        {
            Byte[] value = _packet.SubArray(_index, size);
            _index += size;
            return value;
        }

        internal Int32 NextInt32()
        {
            Int32 value = BitConverter.ToInt32(_packet, _index);
            _index += sizeof(Int32);
            return value;
        }

        internal UInt32 NextUnsignedInt32()
        {
            UInt32 value = BitConverter.ToUInt32(_packet, _index);
            _index += sizeof(UInt32);
            return value;
        }

        internal Int16 NextInt16()
        {
            Int16 value = BitConverter.ToInt16(_packet, _index);
            _index += sizeof(Int16);
            return value;
        }

        internal UInt16 NextUnsignedInt16()
        {
            UInt16 value = BitConverter.ToUInt16(_packet, _index);
            _index += sizeof(UInt16);
            return value;
        }

        internal bool NextBool()
        {
            Boolean value = BitConverter.ToBoolean(_packet, _index);
            _index += sizeof(Boolean);
            return value;
        }

        internal String NextString() 
        {
            byte StringSize = _packet[_index++];
            String outString = Encoding.ASCII.GetString(_packet, _index, StringSize);
            _index += StringSize;
            return outString;
        }

        #endregion

        #region Next Overrides

        internal PacketDecoder Next(ref PacketType value)
        {
            value = (PacketType)NextByte();
            return this;
        }

        internal PacketDecoder Next(ref Byte value)
        {
            value = NextByte();
            return this;
        }

        internal PacketDecoder Next(ref Byte[] value, int size)
        {
            value = NextByteArray(size);
            return this;
        }

        internal PacketDecoder Next(ref Int32 value)
        {
            value = NextInt32();
            return this;
        }

        internal PacketDecoder Next(ref UInt32 value)
        {
            value = NextUnsignedInt32();
            return this;
        }

        internal PacketDecoder Next(ref Int16 value)
        {
            value = NextInt16();
            return this;
        }

        internal PacketDecoder Next(ref UInt16 value)
        {
            value = NextUnsignedInt16();
            return this;
        }

        internal PacketDecoder Next(ref Boolean value)
        {
            value = NextBool();
            return this;
        }

        internal PacketDecoder Next(ref String value)
        {
            value = NextString();
            return this;
        }

        #endregion 

    }


}
