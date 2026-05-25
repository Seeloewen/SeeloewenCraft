using Color = SeeloewenCraft.game.graphics.Color;

namespace SeeloewenCraft.game.core.entities
{
    class Skeleton : MovingEntity
    {

        public Skeleton(int posX, int posY, int velX, int velY) : base(700, 1750, posX, posY, velX, velY, new Color(0.7f, 0.7f, 0.7f, 0.7f))
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