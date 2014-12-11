using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using RedDwarf.Network.Interfaces;

namespace RedDwarf.Server
{
    class RemoteClient : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TcpClient NetworkClient { get; private set; }

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
