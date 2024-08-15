using System.Windows.Media;

namespace SeeloewenCraft
{
    class Skeleton : MovingEntity
    {

        public Skeleton(int posX, int posY, int velX, int velY, World world) : base(700, 1750, posX, posY, velX, velY, world, new SolidColorBrush(Colors.White))
        {
            ACC_WALKING = 30000;
            ACC_SPRINTING = 45000;
        }


        public override void Die()
        {
            Drop("sc:bone_item");
            Drop("sc:arrow_item");
            base.Die();
        }

    }
}
