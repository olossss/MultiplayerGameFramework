using MultiplayerFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerFramework.Common
{
    internal abstract class Listener<T> : AsyncWorker where T : BaseUDP
    {
        protected T _UdpHandle;
        //protected PacketDecoder decoder;
        protected EndPoint _senderAddress;

        public Listener(T udpHandle)
        {
            _UdpHandle = udpHandle;
            _senderAddress = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
            _workerThread = new Thread(new ThreadStart(this.Listen));
        }

        private void Listen()
        {
            byte[] data = null;

            while (_isRunning)
            {
                if (_UdpHandle._socket.Available > 0)
                {
                    data = new byte[_UdpHandle._socket.Available];
                    _UdpHandle._socket.ReceiveFrom(data, ref _senderAddress);
                    processPackets(data);
                    //HandlePacket(data, _senderAddress);
                }
            }

            CloseRoutine();
        }

        private void processPackets(byte[] packetData)
        {
            int packetStartOffset = 0;

            while (packetStartOffset < packetData.Length)
            {
                packetStartOffset += HandlePacket(packetData.SubArray(packetStartOffset, packetData.Length - packetStartOffset), _senderAddress);
            }

        }

        internal abstract int HandlePacket(byte[] packetData, EndPoint RecivedFrom);

        internal virtual void CloseRoutine(){

            Debug.Instance.Log(DebugLevel.DEBUG, "Listener Closed Gracefully");
        }
    }
}
