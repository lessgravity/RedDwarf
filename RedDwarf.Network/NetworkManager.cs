using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LessGravity.Common;
using RedDwarf.Network.Interfaces;
using RedDwarf.Network.Packets;

namespace RedDwarf.Network
{
    public class NetworkManager
    {
        private object _streamLock = new object();

        public NetworkMode NetworkMode { get; private set; }

        private DataStream _dataStream;
        private BufferedStream _bufferedStream;
        private Stream _baseStream;

        public Stream BaseStream
        {
            get {  return _baseStream;}
            set
            {
                lock (_baseStream)
                {
                    if (_bufferedStream != null)
                    {
                        _bufferedStream.Flush();
                    }
                    _baseStream = value;
                    _bufferedStream = new BufferedStream(_baseStream);
                    _dataStream = new DataStream(_bufferedStream);
                }
            }
        }

        private static readonly Type[][] _handShakePackets;
        private static readonly Type[][] _statusPackets;
        private static readonly Type[][] _loginPackets;
        private static readonly Type[][] _playPackets;

        private static readonly Type[][][] _networkModes;

        private static Type[][] Populate(IList<Type> toServerPackets, IList<Type> toClientPackets)
        {
            var typeArray = new Type[Math.Max(toServerPackets.Count, toClientPackets.Count)][];
            for (var i = 0; i < typeArray.Length; ++i)
            {
                typeArray[i] = new[]
                {
                    i < toServerPackets.Count ? toServerPackets[i] : null,
                    i < toClientPackets.Count ? toClientPackets[i] : null
                };
            }
            toServerPackets.Clear();
            toClientPackets.Clear();
            return typeArray;
        }

        static NetworkManager()
        {
            var toClientPackets = new List<Type>();
            var toServerPackets = new List<Type>();

            //
            // Handshake 
            //
            _handShakePackets = Populate(toServerPackets, toClientPackets);

            //
            // Status
            //
            _statusPackets = Populate(toServerPackets, toClientPackets);

            // 
            // Login
            //
            _loginPackets = Populate(toServerPackets, toClientPackets);

            //
            // Play
            //
            toServerPackets.Add(typeof(KeepAlivePacket));
            toClientPackets.Add(typeof(KeepAlivePacket));

            _playPackets = Populate(toServerPackets, toClientPackets);

            _networkModes = new Type[][][]
            {
                _handShakePackets,
                _statusPackets,
                _loginPackets,
                _playPackets
            };
        }

        public NetworkManager(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            NetworkMode = NetworkMode.Handshake;
            BaseStream = stream;
        }

        public IPacket ReadPacket(PacketDirection packetDirection)
        {
            lock (_streamLock)
            {
                
            }
        }

        public void WritePacket(IPacket packet, PacketDirection packetDirection)
        {
            
        }
    }
}
