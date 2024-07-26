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
        public ItemEntity(Item item, int posX, int posY, int velX, int velY, World world) : base(itemSizeX, itemSizeY, posX, posY, velX, velY, world, Colors.Yellow){
            this.item = item;
            frictionAir = 2;

            texture.Background = item.image;

        }

    }
}
