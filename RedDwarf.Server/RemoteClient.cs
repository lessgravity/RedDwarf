using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using RedDwarf.Game;
using RedDwarf.Network;
using RedDwarf.Network.Interfaces;

namespace RedDwarf.Server
{
    class RemoteClient : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool EncryptionEnabled { get; set; }

        protected internal byte[] VerificationToken { get; set; }

        public string UserName { get; set; }

        public string HostName { get; set; }

        public int Ping { get; set; }

        public PlayerEntity Entity { get; set; }

        protected internal DateTime LastKeepAlive { get; set; }

        protected internal DateTime LastKeepAliveSent { get; set; }

        public TcpClient NetworkClient { get; private set; }

        public Stream NetworkStream { get; set; }

        public NetworkManager NetworkManager { get; set; }

        public PlayerManager PlayerManager { get; set; }

        public ConcurrentQueue<IPacket> PacketQueue { get; private set; }

        public bool IsLoggedIn { get; set; }

        public RemoteClient(TcpClient networkClient)
        {
            NetworkClient = networkClient;
            PacketQueue = new ConcurrentQueue<IPacket>();
        }

        public void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Disconnect(string reason)
        {
            
        }

        public void Dispose()
        {
            if (PlayerManager != null)
            {
                PlayerManager.Dispose();
            }
        }

        public void SendPacket(IPacket packet)
        {
            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }
            PacketQueue.Enqueue(packet);
        }
    }
}
