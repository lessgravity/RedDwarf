using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using LessGravity.Common;
using RedDwarf.Network;
using RedDwarf.Network.Interfaces;
using RedDwarf.Network.Packets;
using RedDwarf.Server.Events;

namespace RedDwarf.Server
{
    class RedDwarfServer : IDisposable
    {
        private readonly ServerSettings _serverSettings;

        private readonly object _networkLock = new object();

        public event EventHandler<ChatMessageEventArgs> ChatMessage;
        protected internal virtual void OnChatMessage(ChatMessageEventArgs e)
        {
            if (ChatMessage != null) ChatMessage(this, e);
        }

        public event EventHandler<ConnectionEstablishedEventArgs> ConnectionEstablished;
        protected internal virtual void OnConnectionEstablished(ConnectionEstablishedEventArgs e)
        {
            if (ConnectionEstablished != null) ConnectionEstablished(this, e);
        }

        public event EventHandler<PlayerLogInEventArgs> PlayerLoggedIn;
        protected internal virtual void OnPlayerLoggedIn(PlayerLogInEventArgs e)
        {
            if (PlayerLoggedIn != null) PlayerLoggedIn(this, e);
        }

        public event EventHandler<PlayerLogInEventArgs> PlayerLoggedOut;
        protected internal virtual void OnPlayerLoggedOut(PlayerLogInEventArgs e)
        {
            if (PlayerLoggedOut != null) PlayerLoggedOut(this, e);
        }


        protected internal RSACryptoServiceProvider CryptoServiceProvider { get; set; }
        protected internal RSAParameters ServerKey { get; set; }

        public IList<RemoteClient> Clients { get; private set; }

        public TcpListener Listener { get; private set; }

        public DateTime StartTime { get; private set; }
        private DateTime NextPlayerUpdate { get; set; }
        private DateTime NextChunkUpdate { get; set; }
        private DateTime LastTimeUpdate { get; set; }
        private DateTime NextScheduledSave { get; set; }

        public Thread NetworkThread { get; set; }

        public Thread EntityThread { get; set; }

        public IDictionary<Type, Action<RemoteClient, RedDwarfServer, IPacket>> PacketHandlers { get; private set; }


        public RedDwarfServer()
        {
            _serverSettings = ServerSettings.DefaultSettings();

            Clients = new List<RemoteClient>();
            PacketHandlers = new Dictionary<Type, Action<RemoteClient, RedDwarfServer, IPacket>>();
        }

        public RedDwarfServer(ServerSettings serverSettings) : this()
        {
            _serverSettings = serverSettings;
        }

        private void AcceptClientAsync(IAsyncResult ar)
        {
            lock (_networkLock)
            {
                if (Listener == null)
                {
                    return;
                }
                var client = new RemoteClient(Listener.EndAcceptTcpClient(ar));
                client.NetworkStream = client.NetworkClient.GetStream();
                client.NetworkManager = new NetworkManager(client.NetworkStream);
                Clients.Add(client);
                Listener.BeginAcceptTcpClient(AcceptClientAsync, null);
            }
        }

        public void Dispose()
        {

        }

        private void DoClientUpdates(RemoteClient client)
        {
            if (client.LastKeepAliveSent.AddSeconds(20) < DateTime.Now)
            {
                client.SendPacket(new KeepAlivePacket(Helpers.Random.Next()));
                client.LastKeepAliveSent = DateTime.Now;
            }

            if (NextChunkUpdate < DateTime.Now)
            {
                
            }
        }

        private void HandlePacket(RemoteClient remoteClient, IPacket packet)
        {
            var packetType = packet.GetType();
            if (!PacketHandlers.ContainsKey(packetType))
            {
                return;
            }
            PacketHandlers[packetType](remoteClient, this, packet);
        }

        public void RegisterPacketHandler(Type packetType, Action<RemoteClient, RedDwarfServer, IPacket> packetHandler)
        {
            if (!typeof (IPacket).IsAssignableFrom(packetType))
            {
                throw new ArgumentException("Packet must derive from IPacket");
            }
            PacketHandlers.Add(packetType, packetHandler);
        }

        public void Start(IPEndPoint endPoint)
        {
            CryptoServiceProvider = new RSACryptoServiceProvider(2048);
            ServerKey = CryptoServiceProvider.ExportParameters(true);

            StartTime = DateTime.Now;

            Listener = new TcpListener(endPoint);
            Listener.Start();
            Listener.BeginAcceptTcpClient(AcceptClientAsync, null);

            NetworkThread = new Thread(NetworkThreadProc);
            EntityThread = new Thread(EntityThreadProc);

            NetworkThread.Start();
            EntityThread.Start();
        }

        public void Stop()
        {
            lock (_networkLock)
            {
                if (Listener != null)
                {
                    Listener.Stop();
                    Listener = null;
                }
                if (NetworkThread != null)
                {
                    NetworkThread.Abort();
                    NetworkThread = null;
                }
                if (EntityThread != null)
                {
                    EntityThread.Abort();
                    EntityThread = null;
                }
            }
        }

        private void UpdateScheduledEvents()
        {
            if (DateTime.Now > NextPlayerUpdate)
            {
                Console.WriteLine("Updating Players");
                NextPlayerUpdate = DateTime.Now.AddMinutes(1);
            }
        }
    }
}
