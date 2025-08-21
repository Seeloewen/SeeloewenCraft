using System.Windows.Media;

namespace SeeloewenCraft.game.core.entities
{
    class Skeleton : MovingEntity
    {

        public Skeleton(int posX, int posY, int velX, int velY) : base(700, 1750, posX, posY, velX, velY)
        {
            ACC_WALKING = 30000;
            ACC_SPRINTING = 45000;
        }

        protected override void InitTexture()
        {
            texture.Background = new SolidColorBrush(Colors.White);
        }

        public override void Die()
        {
            Drop("sc:bone_item");
            Drop("sc:arrow_item");
            base.Die();
        }

    }
}