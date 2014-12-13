using System;
using RedDwarf.Network.Interfaces;
using RedDwarf.Network.Packets;

namespace RedDwarf.Server.Handlers
{
    static class PacketHandlerRegistry
    {
        public static void RegisterHandlers(RedDwarfServer server)
        {
            server.RegisterPacketHandler(typeof(StatusPingPacket), StatusPingHandler);
            server.RegisterPacketHandler(typeof(KeepAlivePacket), KeepAliveHandler);
        }

        public static void StatusPingHandler(RemoteClient client, RedDwarfServer server, IPacket packet)
        {
            client.SendPacket(packet);
        }

        public static void KeepAliveHandler(RemoteClient client, RedDwarfServer server, IPacket packet)
        {
            client.LastKeepAlive = DateTime.Now;
            client.Ping = (int)(client.LastKeepAlive - client.LastKeepAliveSent).TotalMilliseconds;
        }
    }
}
