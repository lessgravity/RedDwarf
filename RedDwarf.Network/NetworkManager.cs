using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedDwarf.Network
{
    public class NetworkManager
    {
        private object _streamLock = new object();

        public NetworkMode NetworkMode { get; private set; }

        private BufferedStream _bufferedStream;
        private Stream _baseStream;
        public Stream BaseStream
        {
            get {  return _baseStream;}
            set
            {
                lock (_baseStream)
                {
                    if (_bufferedStream != null)
                    {
                        _bufferedStream.Flush();
                    }
                    _baseStream = value;
                    _bufferedStream = new BufferedStream(_baseStream);

                }
            }
        }

        public NetworkManager(Stream stream)
        {
            
        }
    }
}
