using LessGravity.Common;
using RedDwarf.Network.Interfaces;

namespace RedDwarf.Network.Packets
{
    public struct LoginDisconnectPacket : IPacket
    {
        public LoginDisconnectPacket(string jsonData)
        {
            JsonData = jsonData;
        }

        /// <summary>
        /// Note: This will eventually be replaced with a strongly-typed represenation of this data.
        /// </summary>
        public string JsonData;

        public NetworkMode ReadPacket(DataStream stream, NetworkMode mode, PacketDirection direction)
        {
            JsonData = stream.ReadString();
            return mode;
        }

        public NetworkMode WritePacket(DataStream stream, NetworkMode mode, PacketDirection direction)
        {
            stream.WriteString(JsonData);
            return mode;
        }
    }
}
