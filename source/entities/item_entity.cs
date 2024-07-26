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
        public Item item;
        public ItemEntity(Item item, int posX, int posY, int velX, int velY, World world) : base(300, 300, posX, posY, velX, velY, world, Colors.Yellow){
            this.item = item;
            frictionAir = 2;
        }

    }
}
