
using System;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class Slime : MovingEntity
    {

        public const int animalSizeX = 1300;
        public const int animalSizeY = 1300;

        private int timeSinceJump;

        private Random rnd;
        

        public Slime(int posX, int posY, int velX, int velY, World world) : base(0, animalSizeX, animalSizeY, posX, posY, velX, velY, world, GetSlimeTexture())
        {
            rnd = new Random(DateTime.Now.Millisecond);
            frictionAir = 1;
        }

        public override void DoPhysicsStep(int tps)
        {
            if(timeSinceJump > 2000)
            {
                int dir = rnd.Next(-3, 4);
                velY = -20000;
                velX = 3000 * dir;

                timeSinceJump = 0;
            }

            timeSinceJump += 1000/tps;

            base.DoPhysicsStep(tps);
        }

        public static ImageBrush GetSlimeTexture()
        {
            Random random = new Random(DateTime.Now.Millisecond);

            switch (random.Next(0, 5))
            {
                case 0:
                    return Images.Slime_Blue;
                case 1:
                    return Images.Slime_Red;
                case 2:
                    return Images.Slime_Green;
                case 3:
                    return Images.Slime_Magenta;
                case 4:
                    return Images.Slime_Yellow;
                default:
                    return Images.Slime_Blue;    
            }
        }
    }
}
