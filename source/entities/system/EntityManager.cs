
using System.Collections.Generic;

namespace SeeloewenCraft.entity {
    //this class stores all entities that exist
    //responsible for:
    // - adding and removing entities
    // - doing gameloop steps on them
    public class EntityManager
    {
        //list of all entities
        private List<Entity> entities = new List<Entity>();

        //entities that will be removed at end of tick
        private List<int> removeBuffer = new List<int>();
        //flag used to indicate if currently looping through entities and thus
        //unable to remove from the list. remove calls are stored in remove buffer
        bool allowRemove;

        //does one tick for every entity and delays all enntity removals until after
        public void DoStep(int tps)
        {
            allowRemove = false;
            foreach (Entity entity in entities)
            {
                entity.OnUpdate(63);
                if (entity is ItemEntity itemEntity && entity.lifeTime > 300 && entity.posX < Game.world.player.posX + Game.world.player.sizeX && entity.posX + entity.sizeX > Game.world.player.posX && entity.posY < Game.world.player.posY + Game.world.player.sizeY && entity.posY + entity.sizeY > Game.world.player.posY)
                {
                    Game.world.player.inventory.AddItem(itemEntity.item.id, 1, out int remainingItem);
                    if (remainingItem == 0)
                    {
                        Remove(itemEntity.id);
                    }
                }
            }
            allowRemove = true;
            //do all buffered remove calls
            foreach(int id in removeBuffer)
            {
                Remove(id);
            }


        }


        public void SaveToJson(JsonWriter writer)
        {
            writer.WritePropertyName("entities");
            writer.WriteStartArray();
            foreach (Entity entity in entities)
            {
                entity.SaveToJson(writer);
            }
            writer.WriteEndArray();
        }


        //adds an entity to the world
        public void Add(Entity entity)
        {
            entities.Add(entity);
            Game.world.worldRenderer.AddEntity(entity);
        }

        //removes an entity or stores the remove call until after finishing a
        //gameloop tick
        public void Remove(int id)
        {
            if (allowRemove)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    Entity entity = entities[i];
                    if (entity.id == id)
                    {
                        entities.Remove(entity);
                        Game.world.worldRenderer.RemoveEntity(entity);

                        i--;
                    }
                }
            }
            else
            {
                removeBuffer.Add(id);
            }
        }



    }
}
