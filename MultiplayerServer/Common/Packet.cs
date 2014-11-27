/********************************************
 * Packet.cs
 * Reads Packets Sent From Clients??
 * 
 ********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerServer
{
    public enum PacketType { 
        
        ConnectionRequest = 1,
        ConnectionAccepted,
        ConnectionRejected

    }

    public abstract class packet
    {
        public String serverID { get; set; }
        public int packetTypeID { get; set; }

        public virtual byte[] buildPacket(PacketType type) {
            
            return null;
        }
    }

    public class ConnectionPacket : packet
    {
        public IPEndPoint address { get; set; }

        public override byte[] buildPacket(PacketType type)
        {
            byte[] packetData = new Byte[10];
            address.Address.GetAddressBytes();
            //saddress.Port;
            


            return packetData;
        }
    }
}
