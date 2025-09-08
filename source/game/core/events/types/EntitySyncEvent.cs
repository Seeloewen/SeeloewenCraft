using SeeloewenCraft.game.core.entities;
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
            Game.world.player.SendSyncData();
            foreach (Entity entity in Game.world.entityManager.entities)
            {
                if (entity is MovingEntity movEntity)
                {
                    movEntity.SendSyncData();
                }
            }
        }
    }
}
