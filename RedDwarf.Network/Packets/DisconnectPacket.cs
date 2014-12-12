using LessGravity.Common;
using RedDwarf.Network.Interfaces;

namespace RedDwarf.Network.Packets
{
    public struct DisconnectPacket : IPacket
    {
        public string Reason;

        public DisconnectPacket(string reason)
        {
            Reason = reason;
        }

        public NetworkMode ReadPacket(DataStream stream, NetworkMode mode, PacketDirection direction)
        {
            Reason = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(DataStream stream, NetworkMode mode, PacketDirection direction)
        {
            stream.WriteString(Reason);
            return mode;
        }
    }

}
