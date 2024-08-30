using System;
using System.Windows.Media;

namespace SeeloewenCraft.entity
{
    public class Slime : MovingEntity
    {

        public const int animalSizeX = 1300;
        public const int animalSizeY = 1300;

        private int timeSinceJump;

        private Random rnd;


        public Slime(int posX, int posY, int velX, int velY) : base(animalSizeX, animalSizeY, posX, posY, velX, velY)
        {
            type = "Slime";
            rnd = new Random(DateTime.Now.Millisecond);
            frictionAir = 1;
            ACC_WALKING = 0;
            ACC_SPRINTING = 0;
        }

        public Slime(JsonToken token) : base(token, animalSizeX, animalSizeY)
        {
            type = "Slime";
            rnd = new Random(DateTime.Now.Millisecond);
            frictionAir = 1;
            ACC_WALKING = 0;
            ACC_SPRINTING = 0;
        }

        protected override void InitTexture()
        {
            texture.Background = GetSlimeTexture();
        }

        public override void Die()
        {
            Drop("sc:crafting_table_item");
            base.Die();
        }

        protected override void OnUpdateStart(int tps)
        {
            base.OnUpdateStart(tps);

            //ai
            if (timeSinceJump > 2000 && onGround)
            {
                int dir = rnd.Next(-3, 4);
                velY = -20000;
                velX = 3000 * dir;

                timeSinceJump = 0;
            }
            timeSinceJump += 1000 / tps;
        }


        public static ImageBrush GetSlimeTexture()
        {
            Random random = new Random(DateTime.Now.Millisecond);

            switch (random.Next(0, 5))
            {
                case 0:
                    return Images.Slime_Blue.GetTexture();
                case 1:
                    return Images.Slime_Red.GetTexture();
                case 2:
                    return Images.Slime_Green.GetTexture();
                case 3:
                    return Images.Slime_Magenta.GetTexture();
                case 4:
                    return Images.Slime_Yellow.GetTexture();
                default:
                    return Images.Slime_Blue.GetTexture();
            }
        }
    }
}