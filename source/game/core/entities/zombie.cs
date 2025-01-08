using System;
using System.Windows.Media;

namespace SeeloewenCraft.entity
{
    class Zombie : MovingEntity
    {

        private int timeSinceMove;


        public Zombie(int posX, int posY, int velX, int velY)
            : base(900, 1800, posX, posY, velX, velY)
        {
            idGenerator = new Random(DateTime.Now.Millisecond);
            ACC_WALKING = 20000;
            ACC_SPRINTING = 35000;
        }

        protected override void InitTexture()
        {
            texture.Background = new SolidColorBrush(Colors.LimeGreen);
        }

        internal Zombie(JsonToken token) : base(token, 900, 1800)
        {
            ACC_WALKING = 20000;
            ACC_SPRINTING = 35000;
        }
        
        public override void Die()
        {
            Drop("sc:dirt_item");
            base.Die();
        }

        protected override void OnUpdateStart(int tps)
        {
            base.OnUpdateStart(tps);
            if (timeSinceMove > 1000)
            {
                int dir = idGenerator.Next(-1, 2);

                switch (dir)
                {
                    case 1:
                        pressedLeft = false;
                        pressedRight = true;
                        break;
                    case -1:
                        pressedLeft = true;
                        pressedRight = false;
                        break;
                    case 0:
                        pressedLeft = false;
                        pressedRight = false;
                        break;
                }
                timeSinceMove = 0;
            }
            timeSinceMove += 1000 / tps;
        }


    }
}