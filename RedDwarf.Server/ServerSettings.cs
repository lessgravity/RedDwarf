using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.Server
{
    class ServerSettings
    {
        public int MaxPlayers { get; set; }

        public bool EnableEncryption { get; set; }

        public static ServerSettings DefaultSettings()
        {
            var serverSettings = new ServerSettings
            {
                MaxPlayers = 32,
                EnableEncryption = true
            };
            return serverSettings;
        }
    }
}
