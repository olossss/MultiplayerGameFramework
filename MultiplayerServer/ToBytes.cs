using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerServer
{
    static class HelperExtentions
    {
        public static byte[] StringToBytes(this String value)
        {
            return null;
        }

        public static byte[] IntToBytes(this Int32 value)
        {
            return null;
        }

        public static byte[] IntToBytes(this Int16 value)
        {
            return null;
        }

        public static Int32 ReverseBytes(this Int32 value)
        {
            return (Int32)((value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                 (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24);
        }

        public static byte[] Append(this byte[] value, byte[] appendArray) {

            return null;
        }
    }
}
