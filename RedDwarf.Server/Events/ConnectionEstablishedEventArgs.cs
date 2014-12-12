using System;

namespace RedDwarf.Server.Events
{
    class ConnectionEstablishedEventArgs : EventArgs
    {
        public RemoteClient Client { get; set; }
        public bool PermitConnection { get; set; }
        public string DisconnectReason { get; set; }

        public ConnectionEstablishedEventArgs(RemoteClient client)
        {
            Client = client;
            PermitConnection = true;
            DisconnectReason = "You are banned.";
        }
    }
}
