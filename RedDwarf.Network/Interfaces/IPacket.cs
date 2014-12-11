using System.IO;

namespace RedDwarf.Network.Interfaces
{
    public interface IPacket
    {
        NetworkMode ReadPacket(BinaryReader reader, NetworkMode networkMode, PacketDirection direction);
        NetworkMode WritePacket(BinaryWriter writer, NetworkMode networkMode, PacketDirection direction);
    }
}
