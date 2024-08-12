using System.Windows.Media;

namespace SeeloewenCraft
{
    class Skeleton : MovingEntity
    {

        public Skeleton(int posX, int posY, int velX, int velY, World world) : base(30000, 700, 1750, posX, posY, velX, velY, world, new SolidColorBrush(Colors.White))
        {
        }


        public override void Die()
        {
            Drop("sc:bone_item");
            Drop("sc:arrow_item");
            base.Die();
        }

    }
}
