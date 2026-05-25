using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.core.entities
{
    public class ItemEntity : Entity
    {

        public const int itemSizeX = 500;
        public const int itemSizeY = 500;

        public Item item;

        public string itemID { get { return item.id; } }

        protected override void SaveSpecialInfo(JObject obj)
        {
            obj.Add("item_id", item.id);
            obj.Add("item_tag", item.tag);
        }

        private void Init(Item item, string tag)
        {
            type = "ItemEntity";
            this.item = item;
            this.item.tag = tag;
            frictionAir = 2;
            frictionWater = 15;
        }


        protected override void OnUpdateStart(int tps)
        {
            base.OnUpdateStart(tps);

            if (touchingStatus[TOUCHING_CACTUS])
            {
                World.Get().RemoveEntity(id);
            }
        }

        public ItemEntity(Item item, string tag, int posX, int posY, int velX, int velY)
            : base(itemSizeX, itemSizeY, posX, posY, velX, velY)
        {
            Init(item, tag);
        }

        public ItemEntity(JObject token) : base(token, itemSizeX, itemSizeY)
        {
            Init(ItemRegister.Get(token.Get<string>("item_id")), token.Get<string>("item_tag"));
        }
    }
}