using MultiplayerFramework.Common;
using MultiplayerFramework.Common.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerFramework.Server
{
    internal class ServerListener : Listener<Server> 
    {
        public ServerListener(Server UDPHandle)
            : base(UDPHandle) 
        {
        }

        internal override void CloseRoutine()
        {
            Debug.Instance.Log(DebugLevel.DEBUG, "Server Listner Closed Gracefully");
        }

        internal override int HandlePacket(byte[] packetData, EndPoint ClientAddress)
        {
            PacketType packetType = Packet.GetPacketType(packetData);
            Debug.Instance.Log(DebugLevel.NETWORK, "(Server) Message Recived from " + ((IPEndPoint)ClientAddress).ToString() + " Type: " + packetType + "(Size:" + packetData.Length+ ")");

            if (packetType == PacketType.ConnectionRequest)
            {
                ConnectionRequestPacket packet = new ConnectionRequestPacket().DecodePacket(packetData);
                if (_UdpHandle._BlackList.Contains(ClientAddress))
                    _UdpHandle.RejectClient((IPEndPoint)ClientAddress, ConnectionRejectedReason.Blocked, (IPEndPoint)ClientAddress);

                else if (_UdpHandle.ClientCount() >= _UdpHandle.maxClients)
                    _UdpHandle.RejectClient((IPEndPoint)ClientAddress, ConnectionRejectedReason.ServerFull, (IPEndPoint)ClientAddress);

                else
                    _UdpHandle.ConnectClient((IPEndPoint)ClientAddress);

                return packet.packetSize();// this needs to change later
            }
            else if (packetType == PacketType.EmptyBuffer)
                return 1;
            else
                throw new Exception("Packet Size Missmatch");

        }
        
    }
}
