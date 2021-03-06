﻿using LessGravity.Common;
using RedDwarf.Network.Interfaces;

namespace RedDwarf.Network.Packets
{
    public struct KeepAlivePacket : IPacket
    {
        public int KeepAlive;

        public KeepAlivePacket(int keepAlive)
        {
            KeepAlive = keepAlive;
        }

        public NetworkMode ReadPacket(DataStream stream, NetworkMode networkMode, PacketDirection direction)
        {
            KeepAlive = stream.ReadInt32();
            return networkMode;
        }

        public NetworkMode WritePacket(DataStream stream, NetworkMode networkMode, PacketDirection direction)
        {
            stream.WriteInt32(KeepAlive);
            return networkMode;
        }
    }
}
