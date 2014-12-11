using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedDwarf.Network.Interfaces;

namespace RedDwarf.Server
{
    class RedDwarfServer : IDisposable
    {
        private readonly ServerSettings _serverSettings;

        public IDictionary<Type, Action<RemoteClient, RedDwarfServer, IPacket>> PacketHandlers { get; private set; }




        public RedDwarfServer(ServerSettings serverSettings)
        {
            _serverSettings = serverSettings;

            PacketHandlers = new Dictionary<Type, Action<RemoteClient, RedDwarfServer, IPacket>>();
        }

        public void Dispose()
        {

        }
    }
}
