using MultiplayerFramework.Common.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common
{
    public class PacketEncoder
    {
        private List<Byte[]> _packetItemList { get; set; }
        private int _size { get; set; }
        private PacketType _type { get; set; }
        public PacketEncoder(PacketType type)
        {
            _type = type;
            clear();
        }

        internal PacketEncoder clear()
        {
            _packetItemList = new List<Byte[]>();
            _size = 0;
            Add(_type);
            return this;
        }

        internal PacketEncoder Add(PacketType data)
        {
            return Add((Byte)data);
        }

        internal PacketEncoder Add(Byte[] data)
        {
            _packetItemList.Add(data);
            _size += data.Length;
            return this;
        }

        internal PacketEncoder Add(Byte data)
        {
            Byte[] packetData = new byte[1];
            packetData[0] = data;
            _packetItemList.Add(packetData);
            _size += sizeof(Byte);
            return this;
        }

        internal PacketEncoder Add(Int32 data)
        {
            Byte[] packetData = BitConverter.GetBytes(data);
            _packetItemList.Add(packetData);
            _size += packetData.Length;
            return this;
        }

        internal PacketEncoder Add(UInt32 data)
        {
            Byte[] packetData = BitConverter.GetBytes(data);
            _packetItemList.Add(packetData);
            _size += packetData.Length;
            return this;
        }

        internal PacketEncoder Add(Int16 data)
        {
            Byte[] packetData = BitConverter.GetBytes(data);
            _packetItemList.Add(packetData);
            _size += packetData.Length;
            return this;
        }

        internal PacketEncoder Add(UInt16 data)
        {
            Byte[] packetData = BitConverter.GetBytes(data);
            _packetItemList.Add(packetData);
            _size += packetData.Length;
            return this;
        }

        internal PacketEncoder Add(Boolean data)
        {
            Byte[] packetData = BitConverter.GetBytes(data);
            _packetItemList.Add(packetData);
            _size += packetData.Length;
            return this;
        }

        internal PacketEncoder Add(String data)
        {
            // Check String Length
            if (data.Length > Byte.MaxValue)
                throw new Exception("String Length Exceeds Max Length Of " + Byte.MaxValue);

            // Add A Single Byte To Indicate String Length
            Add((byte)data.Length);

            // Add The Packet
            Byte[] packetData = Encoding.ASCII.GetBytes(data);
            _packetItemList.Add(packetData);
            _size += packetData.Length;
            return this;
        }

        internal byte[] BuildPacket()
        {
            Byte[] packet = new Byte[_size];
            int currentIndex = 0;

            for (int i = 0; i < _packetItemList.Count; ++i)
            {
                //packet.CopyTo(_packetItemList[i], currentIndex);
                _packetItemList[i].CopyTo(packet, currentIndex);
                currentIndex += _packetItemList[i].Length;
            }
            
            return packet;
        }

        internal int packetSize()
        {
            int totalSize = 0;

            foreach (byte[] size in _packetItemList)
                totalSize += size.Length;

            if (_size != totalSize)
                throw new Exception("Inconsistent Packet Sizes");

            return totalSize;
        }

    }
}
