using LessGravity.Common;

namespace RedDwarf.Network.Interfaces
{
    public interface IPacket
    {
        NetworkMode ReadPacket(DataStream stream, NetworkMode networkMode, PacketDirection direction);
        NetworkMode WritePacket(DataStream stream, NetworkMode networkMode, PacketDirection direction);
    }
}
