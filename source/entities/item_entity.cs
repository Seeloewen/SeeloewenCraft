using System.Windows.Media;

namespace SeeloewenCraft.entity
{
    public class ItemEntity : Entity
    {

        public const int itemSizeX = 500;
        public const int itemSizeY = 500;

        public Item item;

        protected override void SaveSpecialInfo(JsonWriter writer)
        {
            writer.WritePropertyName("item_id");
            writer.WriteValue(item.id);
        }

        private void Init(Item item)
        {
            type = "ItemEntity";
            this.item = item;
            frictionAir = 2;
            frictionWater = 15;

            texture.Background = item.image;
        }


        protected override void OnUpdateStart(int tps)
        {
            base.OnUpdateStart(tps);

            if (touchingStatus[TOUCHING_CACTUS])
            {
                Game.world.toDieEntities.Add(this);
            }
        }

        public ItemEntity(Item item, int posX, int posY, int velX, int velY) : base(itemSizeX, itemSizeY, posX, posY, velX, velY, new SolidColorBrush(Colors.Yellow))
        {
            Init(item);
        }

        public ItemEntity(JsonToken token) : base(token, itemSizeX, itemSizeY, new SolidColorBrush(Colors.Yellow))
        {
            Init(ItemRegister.GenerateItem(token.GetString("/item_id")));
        }
    }
}