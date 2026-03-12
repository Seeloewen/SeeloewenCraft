using SeeloewenCraft.game.util;
using System;
using SeeloewenCraft.game.graphics;
using Newtonsoft.Json.Linq;

namespace SeeloewenCraft.game.core.entities
{
    public class Slime : MovingEntity
    {

        public const int animalSizeX = 1300;
        public const int animalSizeY = 1300;

        private int timeSinceJump;


        public Slime(int posX, int posY, int velX, int velY) : base(animalSizeX, animalSizeY, posX, posY, velX, velY, new Color(0f, 0.4f, 0f, 0.7f))
        {
            type = "Slime";
            frictionAir = 1;
            ACC_WALKING = 0;
            ACC_SPRINTING = 0;
        }

        public Slime(JObject obj) : base(obj, animalSizeX, animalSizeY)
        {
            type = "Slime";

            frictionAir = 1;
            ACC_WALKING = 0;
            ACC_SPRINTING = 0;
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
                int dir = Game.rnd.Next(-3, 4);
                velY = -20000;
                velX = 3000 * dir;

                timeSinceJump = 0;
            }
            timeSinceJump += 1000 / tps;
        }


    }
}
