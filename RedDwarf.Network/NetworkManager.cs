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
            get { return _baseStream; }
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
                int idLength;
                long length = _dataStream.ReadVariableInt();
                long packetId = _dataStream.ReadVariableInt(out idLength);
                var packetData = _dataStream.ReadUInt8Array((int)(length - idLength));
                if (_networkModes[(int) NetworkMode].Length < packetId ||
                    _networkModes[(int) NetworkMode][packetId][(int) packetDirection] == null)
                {
                    return new UnknownDataPacket
                    {
                        Id = packetId, 
                        Data = packetData
                    };
                }
                var dataStream = new DataStream(new MemoryStream(packetData));
                var packetType = _networkModes[(int) NetworkMode][packetId][(int) packetDirection];
                var packet = Activator.CreateInstance(packetType) as IPacket;
                NetworkMode = packet.ReadPacket(dataStream, NetworkMode, packetDirection);
                if (dataStream.Position < dataStream.Length)
                {
                    Console.WriteLine("Warning: did not completely read packet: {0}", packet.GetType().Name);
                }
                return packet;
            }
        }

        public void WritePacket(IPacket packet, PacketDirection packetDirection)
        {
            lock (_streamLock)
            {
                var networkMode = packet.WritePacket(_dataStream, NetworkMode, packetDirection);
                _bufferedStream.WriteImmediately = true;
                var packetId = -1;
                var packetType = packet.GetType();
                for (var packetTypeIndex = 0;
                    packetTypeIndex < _networkModes[(int) NetworkMode].LongLength;
                    packetTypeIndex++)
                {
                    if (_networkModes[(int) NetworkMode][packetTypeIndex][(int) packetDirection] == packetType)
                    {
                        packetId = packetTypeIndex;
                        break;
                    }
                }
                if (packetId == -1)
                {
                    throw new InvalidOperationException("Attempted to write invalid packet type.");
                }
                var pendingWrites = (int) _bufferedStream.PendingWrites + _dataStream.GetVariantIntLength(packetId);
                _dataStream.WriteVariableInt(pendingWrites);
                _dataStream.WriteVariableInt(packetId);
                _bufferedStream.WriteImmediately = false;
                _bufferedStream.Flush();
                NetworkMode = networkMode;
            }
        }
    }
}
