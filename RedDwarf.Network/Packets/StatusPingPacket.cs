using LessGravity.Common;
using RedDwarf.Network.Interfaces;

namespace RedDwarf.Network.Packets
{
    public struct StatusPingPacket : IPacket
    {
        public StatusPingPacket(long time)
        {
            Time = time;
        }

        public long Time;

        public NetworkMode ReadPacket(DataStream stream, NetworkMode mode, PacketDirection direction)
        {
            Time = stream.ReadInt64();
            return mode;
        }

        public NetworkMode WritePacket(DataStream stream, NetworkMode mode, PacketDirection direction)
        {
            stream.WriteInt64(Time);
            return mode;
        }
    }
}
