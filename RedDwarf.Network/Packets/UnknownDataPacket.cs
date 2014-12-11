using System;
using System.IO;
using RedDwarf.Network.Interfaces;

namespace RedDwarf.Network.Packets
{
    public class UnknownDataPacket : IPacket
    {
        public long Id { get; set; }

        public byte[] Data { get; set; }

        public NetworkMode ReadPacket(BinaryReader reader, NetworkMode networkMode, PacketDirection direction)
        {
            throw new NotImplementedException();
        }

        public NetworkMode WritePacket(BinaryWriter writer, NetworkMode networkMode, PacketDirection direction)
        {
            writer.Write(Id);
            writer.Write(Data);
            return networkMode;
        }
    }
}
