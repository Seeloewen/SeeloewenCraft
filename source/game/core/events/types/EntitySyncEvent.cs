using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.core.events
{
    internal class EntitySyncEvent : GameEvent
    {
        public EntitySyncEvent() : base(400) { }

        protected override void Run()
        {
            //Temporarily reverted to old system
            Player.Get().SendSyncData();
            foreach (Entity entity in World.Get().entityManager.entities)
            {
                if (entity is MovingEntity movEntity)
                {
                    movEntity.SendSyncData();
                }
            }
        }
    }
}
