using System.Windows.Media;

namespace SeeloewenCraft
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

        public override void DoPhysicsStep(int tps)
        {
            base.DoPhysicsStep(tps);

            if (touchingStatus[TOUCHING_CACTUS])
            {
                world.toDieEntities.Add(this);
            }
        }

        public ItemEntity(Item item, int posX, int posY, int velX, int velY, World world) : base(itemSizeX, itemSizeY, posX, posY, velX, velY, world, new SolidColorBrush(Colors.Yellow))
        {
            Init(item);
        }

        public ItemEntity(JsonToken token, World world) : base(token, itemSizeX, itemSizeY, world, new SolidColorBrush(Colors.Yellow))
        {
            Init(ItemRegister.GenerateItem(token.GetString("/item_id"), world));
        }
    }
}
