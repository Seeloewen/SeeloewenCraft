using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.util;
using System.Windows.Media;

namespace SeeloewenCraft.game.core.entities
{
    public class ItemEntity : Entity
    {

        public const int itemSizeX = 500;
        public const int itemSizeY = 500;

        public Item item;

        public string itemID { get { return item.id; } }

        protected override void SaveSpecialInfo(JsonWriter writer)
        {
            writer.WritePropertyName("item_id");
            writer.WriteValue(item.id);

            writer.WritePropertyName("item_tag");
            writer.WriteValue(item.tag);
        }

        private void Init(Item item, string tag)
        {
            type = "ItemEntity";
            this.item = item;
            this.item.tag = tag;
            frictionAir = 2;
            frictionWater = 15;

            texture.Background = item.image;
        }


        protected override void OnUpdateStart(int tps)
        {
            base.OnUpdateStart(tps);

            if (touchingStatus[TOUCHING_CACTUS])
            {
                Game.world.RemoveEntity(id);
            }
        }

        public ItemEntity(Item item, string tag, int posX, int posY, int velX, int velY)
            : base(itemSizeX, itemSizeY, posX, posY, velX, velY, new SolidColorBrush(Colors.Yellow))
        {
            Init(item, tag);
        }

        public ItemEntity(JsonToken token) : base(token, itemSizeX, itemSizeY, new SolidColorBrush(Colors.Yellow))
        {
            Init(ItemRegister.Get(token.GetString("/item_id")), token.GetString("/item_id"));
        }
    }
}