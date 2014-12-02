using MultiplayerFramework.Common.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common
{
    class PacketFactory
    {
        /*
         * Connection Packets
         */

        public static byte[] Acknollagement(UInt32 clientID, UInt32 sequenceNumber)
        {
            var packet = new AcknolagementPacket();
            packet._clientID = clientID;
            packet._sequenceNumber = sequenceNumber;
            return packet.EncodePacket();
        }

        public static byte[] ConnectionRequest()
        {
            return new ConnectionRequestPacket().EncodePacket();
        }
        
        public static byte[] ConnectionAccepted(UInt32 clientID, IPEndPoint address)
        {
            var packet = new ConnectionAcceptedPacket();
            packet._clientID = clientID;
            return packet.EncodePacket();
        }

        public static byte[] ConnectionRejected(ConnectionRejectedReason reason)
        {
            var packet = new ConnectionRejectedPacket();
            packet._reason = reason;
            return packet.EncodePacket();
        }

        internal static byte[] TestPacket()
        {
            var packet = new TestPacket();
            packet._Int32  = 1;
            packet._UInt32 = 2;
            packet._Int16  = 3;
            packet._UInt16 = 4;
            packet._String = "-Catch A Nigger-";
            packet._Boolean = false;
            packet._Byte = 32;
            return packet.EncodePacket();
        }

        internal static byte[] Disconnect(UInt32 clientID)
        {
            var packet = new DisconnectionRequestPacket();
            packet._clientID = clientID;
            return packet.EncodePacket();
        }
    }
}
