
using System.Collections.Generic;
using Windows.Media.Protection.PlayReady;

namespace SeeloewenCraft.entity {
    //this class stores all entities that exist
    //responsible for:
    // - adding and removing entities
    // - doing gameloop steps on them
    public class EntityManager
    {
        //list of all entities
        private List<Entity> entities;
        //current player
        public Player player;

        //entities that will be removed at end of tick
        private List<int> removeBuffer;
        //entities that will be added at end of tick
        private List<Entity> addBuffer;
        //flag used to indicate if currently looping through entities and thus
        //unable to remove/add from the list. remove/add calls are stored in remove/add buffer
        bool allowModify;

        //does one tick for every entity and delays all entity removals/adds until after
        public void DoStep(int tps)
        {
            allowModify = false;
            foreach (Entity entity in entities)
            {
                //do the tick
                entity.OnUpdate(63);

                //checks if item entity player intersect and let player pick up the item
                if (entity is ItemEntity itemEntity && entity.lifeTime > 300 && entity.posX < Game.world.player.posX + Game.world.player.sizeX && entity.posX + entity.sizeX > Game.world.player.posX && entity.posY < Game.world.player.posY + Game.world.player.sizeY && entity.posY + entity.sizeY > Game.world.player.posY)
                {
                    Game.world.player.inventory.AddItem(itemEntity.item.id, 1, itemEntity.item.tag, out int remainingItem);
                    if (remainingItem == 0)
                    {
                        Remove(itemEntity.id);
                    }
                }
            }
            allowModify = true;

            //do all buffered remove and add calls
            foreach(int id in removeBuffer)
            {
                Remove(id);
            }
            foreach (Entity entity in addBuffer)
            {
                Add(entity);
            }
            removeBuffer.Clear(); //clear buffers after
            addBuffer.Clear();
        }


        
        public void ReceivePressedChange(string[] eventArgs)
        {
            PressedChangeEvent e = PressedChangeEvent.Create(eventArgs);
            ((MovingEntity)GetEntity(e.id)).HandlePressedChangeEvent(e);
        }

        public Entity GetEntity(int id)
        {
            foreach(Entity entity in entities)
            {
                if(entity.id == id)
                {
                    return entity;
                }
            }
            return null;
        }

        //stores every entity into json array with key "entities"
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

        //load constructor
        public EntityManager(JsonToken token) : this()
        {
            JsonToken list = token.GetToken("/entities");
            for(int i = 0; i < list.GetArrayLength(); i++)
            {
                var entity = Entity.LoadFromJson(list.GetToken($"/{i}"));
                if(entity is not Player)
                {
                    Add(entity);
                }
            }
        }

        //generate constructor
        public EntityManager()
        {
            entities = new List<Entity>();
            removeBuffer = new List<int>();
            addBuffer = new List<Entity>();
            allowModify = true;
        }


        //adds an entity or stores the add call until after finishing a
        //gameloop tick
        public void Add(Entity entity)
        {
            if (allowModify)
            {
                entities.Add(entity);
                Game.world.worldRenderer.AddEntity(entity);
            }
            else
            {
                addBuffer.Add(entity);
            }
        }

        //removes an entity or stores the remove call until after finishing a
        //gameloop tick
        public void Remove(int id)
        {
            if (allowModify)
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


        public void SendInitLoadData(int clientID)
        {
            using (JsonWriter writer = JsonWriter.Create())
            {
                Game.world.player.SaveToJson(writer);
                Game.server.SendDataSingleClient(clientID, $"CreateEntity;{writer.ToString()}");
            }
        }

    }
}
