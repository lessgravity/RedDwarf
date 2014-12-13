using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedDwarf.Game;

namespace RedDwarf.Server
{
    class EntityManager
    {
        public RedDwarfServer Server { get; private set; }

        public UInt64 NextEntityId { get; private set; }

        public IList<Entity> Entities { get; private set; }

        public ConcurrentQueue<int> MarkedForDespawn { get; private set; }

        public EntityManager(RedDwarfServer server)
        {
            Server = server;
            NextEntityId = 1;
            Entities = new List<Entity>();
            MarkedForDespawn = new ConcurrentQueue<int>();
        }

        private void Despawn(object sender, EventArgs e)
        {
            Despawn(sender as Entity);
        }

        public void Despawn(Entity entity)
        {
            
        }

        public void SpawnEntity( /*World world,*/ Entity entity)
        {
            
        }

        public void Update()
        {
            
        }
    }
}
