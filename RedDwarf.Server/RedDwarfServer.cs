using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.Server
{
    class RedDwarfServer : IDisposable
    {
        private readonly ServerSettings _serverSettings;
        public RedDwarfServer(ServerSettings serverSettings)
        {
            _serverSettings = serverSettings;
        }

        public void Dispose()
        {

        }
    }
}
