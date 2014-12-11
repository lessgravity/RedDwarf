using System;
using System.IO;
using LessGravity.Common;
using RedDwarf.Network.Interfaces;

namespace RedDwarf.Network.Packets
{
    public class UnknownDataPacket : IPacket
    {
        public long Id { get; set; }

        public byte[] Data { get; set; }

        public NetworkMode ReadPacket(DataStream stream, NetworkMode networkMode, PacketDirection direction)
        {
            throw new NotImplementedException();
        }

        public NetworkMode WritePacket(DataStream stream, NetworkMode networkMode, PacketDirection direction)
        {
            stream.WriteInt64(Id);
            stream.Write(Data, 0, Data.Length);
            return networkMode;
        }
    }
}
