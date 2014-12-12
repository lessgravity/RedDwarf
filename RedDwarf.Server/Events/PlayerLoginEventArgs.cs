using System;

namespace RedDwarf.Server.Events
{
    class PlayerLogInEventArgs : EventArgs
    {
        public string Username
        {
            get { return Client.UserName; }
        }

        public RemoteClient Client { get; set; }

        public bool Handled { get; set; }

        public PlayerLogInEventArgs(RemoteClient client)
        {
            Client = client;
        }
    }
}
