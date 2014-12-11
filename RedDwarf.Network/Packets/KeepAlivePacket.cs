using System.IO;
using RedDwarf.Network.Interfaces;

namespace RedDwarf.Network.Packets
{
    public class KeepAlivePacket : IPacket
    {
        public int KeepAlive { get; set; }

        public NetworkMode ReadPacket(BinaryReader reader, NetworkMode networkMode, PacketDirection direction)
        {
            KeepAlive = reader.ReadInt32();
            return networkMode;
        }

        public NetworkMode WritePacket(BinaryWriter writer, NetworkMode networkMode, PacketDirection direction)
        {
            writer.Write(KeepAlive);
            return networkMode;
        }
    }
}
