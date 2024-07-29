using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            writer.WritePropertyName("type");
            writer.WriteValue("ItemEntity");
            writer.WritePropertyName("item_id");
            writer.WriteValue(item.id);
        }

        public ItemEntity(Item item, int posX, int posY, int velX, int velY, World world) : base(itemSizeX, itemSizeY, posX, posY, velX, velY, world, Colors.Yellow){
            this.item = item;
            frictionAir = 2;
            frictionWater = 15;

            texture.Background = item.image;

        }

        public ItemEntity(JsonToken token, World world) : base(itemSizeX, itemSizeY, 0, 0, 0, 0, world, Colors.Yellow)
        {
            this.item = ItemRegister.GenerateItem(token.GetString("/item_id"), world);

            frictionAir = 2;
            frictionWater = 15;

            texture.Background = item.image;
        }
    }
}
