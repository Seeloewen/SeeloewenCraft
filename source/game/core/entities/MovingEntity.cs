using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using SeeloewenCraft.game.graphics;

namespace SeeloewenCraft.game.core.entities
{
    public abstract class MovingEntity : Entity
    {
        public const int MAX_STEPUP_HEIGHT = 505;

        public const double MAX_HP = 10.0;
        public double hp;

        protected int ACC_WALKING = 50000;
        protected int ACC_SPRINTING = 90000;
        protected int ACC_SNEAKING = 30000;


        protected int currentAcc;
        private const int jumpStartSpeed = 15000;

        private int fallMaxHeight = 0;

        public bool pressedUp;
        public bool pressedRight;
        public bool pressedLeft;
        public bool pressedSneak;
        public bool pressedSprint;
        public bool pressedThrow;
        protected bool thrown;

        bool touchingRight;
        bool touchingLeft;

        public bool flying;

        public bool breathing;
        
        public Color hitboxColor { private init; get; }


        public MovingEntity(int sizeX, int sizeY, int posX, int posY, int velX, int velY, Color hitboxColor)
            : base(sizeX, sizeY, posX, posY, velX, velY, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green))
        {
            type = "MovingEntity";
            hp = MAX_HP;
            currentAcc = ACC_WALKING;
            this.hitboxColor = hitboxColor;

            texture.MouseLeftButtonDown += Texture_MouseLeftButtonDown;

            InitTexture();
        }

        public MovingEntity(JsonToken token, int sizeX, int sizeY)
            : base(token, sizeX, sizeY, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green))
        {
            type = "MovingEntity";
            hp = token.GetDouble("/hp");
            currentAcc = token.GetInt("/current_acc");
            fallMaxHeight = token.GetInt("/fall_max_height");
            thrown = token.GetBool("/thrown");
            flying = token.GetBool("/flying");


            texture.MouseLeftButtonDown += Texture_MouseLeftButtonDown;

            InitTexture();
        }



        protected abstract void InitTexture();

        public void SendSyncData()
        {
            if (this is Player && this != Player.Get() && !NetworkHandler.IsServer())
            {
                return;
            }

            //Only send sync data of the current player or entities
            NetworkHandler.SendData(MultiplayerPacketType.SYNC_POS, id.ToString(), posX.ToString(), posY.ToString(), velX.ToString(), velY.ToString());
        }

        public void HandleSyncData(string[] args)
        {
            posX = Convert.ToInt32(args[1]);
            posY = Convert.ToInt32(args[2]);
            velX = Convert.ToInt32(args[3]);
            velY = Convert.ToInt32(args[4]);
        }

        internal void HandlePressedChangeEvent(PressedChangeEvent e)
        {
            pressedUp = e.pressedUp;
            pressedRight = e.pressedRight;
            pressedLeft = e.pressedLeft;
            pressedUp = e.pressedUp;
            pressedSneak = e.pressedSneak;
            pressedSprint = e.pressedSprint;
        }


        public MovingEntity(JsonToken token, int sizeX, int sizeY, System.Windows.Media.Brush image)
            : base(token, sizeX, sizeY, image)
        {
            type = "MovingEntity";
            hp = token.GetDouble("/hp");
            currentAcc = token.GetInt("/current_acc");
            fallMaxHeight = token.GetInt("/fall_max_height");
            flying = token.GetBool("/flying");

            texture.MouseLeftButtonDown += Texture_MouseLeftButtonDown;
        }


        protected override void DoFallDamage()
        {
            int fallHeight = posY - fallMaxHeight;
            if (fallHeight > 3950)
            {
                Damage((fallHeight - 2000) / 3000.0);
                Log.Write($"Applied fall damage, entity {type} ({id}) now has {hp} HP.", LogType.ENTITIES, LogLevel.INFO);
            }
        }

        protected override bool CheckUpStep(Direction direction, int remaining, int tps)
        {
            if (!onGround) return false;

            (_, int open) = direction.IsRight()
                ? DoCollisionCheck(Direction.DOWN, posX + sizeX, posY, posX + sizeX + 1, posY + sizeY)
                : DoCollisionCheck(Direction.DOWN, posX - 1, posY, posX, posY + sizeY);

            int amount = sizeY - open;
            if (amount <= MAX_STEPUP_HEIGHT)
            {
                bool collided;
                int maxMovement;
                if (direction.IsRight())
                {
                    (collided, maxMovement) = DoCollisionCheck(Direction.UP, posX + sizeX, posY, posX + sizeX + 1, posY - amount);
                }
                else
                {
                    (collided, maxMovement) = DoCollisionCheck(Direction.UP, posX - 1, posY, posX, posY - amount);
                }
                if (!collided) maxMovement = amount;
                if (maxMovement < amount)
                {
                    return false;
                }
                else
                {
                    Move(0, -maxMovement, tps);
                    Move(direction.IsRight() ? remaining : -remaining, 0, tps);
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        //hit by player
        private void Texture_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this is not Player)
            {
                int playerX = Player.Get().posX;
                int playerY = Player.Get().posY;

                if (playerX - (posX + sizeX) < Player.HIT_RANGE
                    && playerY - (posY + sizeY) < Player.HIT_RANGE
                    && (playerX + Player.Get().sizeX) - posX < Player.HIT_RANGE
                    && (playerY + Player.Get().sizeY) - posY < Player.HIT_RANGE)
                {
                    Damage(Player.HIT_DAMAGE);
                }
            }
        }


        public override void OnUpdate(int tps) //temporary because player death is not handled properly
        {
            if (hp != 0)
            {
                base.OnUpdate(tps);
            }
        }

        protected override void OnUpdateEnd(int tps)
        {
            if (velY <= 0)
            {
                fallMaxHeight = posY;
            }

            base.OnUpdateEnd(tps);
        }

        public virtual void Drop(string id)
        {
            Item item = ItemRegister.Get(id);

            World.Get().AddEntity(new ItemEntity(item, item.tag, //item type
                posX + sizeX / 2 - ItemEntity.itemSizeX / 2, //posX
                posY + sizeY * 2 / 3 - ItemEntity.itemSizeY / 2, //posY
                Game.rnd.Next(-6000, 6000), Game.rnd.Next(-15000, -10000))); //velX and velY
        }

        public virtual void Die()
        {
            World.Get().RemoveEntity(id);
        }

        public virtual void SetHP(double hp)
        {
            hp = Math.Round(hp * 2) / 2;

            this.hp = Math.Min(hp, MAX_HP);
            if (hp <= 0)
            {
                Die();
            }
        }

        public virtual void Heal(double amount)
        {
            SetHP(hp + amount);

            NetworkHandler.SendData(MultiplayerPacketType.HEAL_ENTITY, $"{id};{amount}");
        }

        public virtual void MultiplayerHeal(double amount)
        {
            SetHP(hp + amount);
        }

        public virtual void Damage(double damage)
        {
            SetHP(hp - damage);

            NetworkHandler.SendData(MultiplayerPacketType.DAMAGE_ENTITY, $"{id};{damage}");
        }

        public virtual void MultiplayerDamage(double damage)
        {
            SetHP(hp - damage);
        }

        protected override void SaveSpecialInfo(JsonWriter writer)
        {
            writer.WritePropertyName("hp");
            writer.WriteValue(hp);
            writer.WritePropertyName("current_acc");
            writer.WriteValue(currentAcc);
            writer.WritePropertyName("fall_max_height");
            writer.WriteValue(fallMaxHeight);
            writer.WritePropertyName("thrown");
            writer.WriteValue(thrown);
            writer.WritePropertyName("flying");
            writer.WriteValue(flying);
        }

        protected override void OnUpdateStart(int tps)
        {
            base.OnUpdateStart(tps);
            (touchingLeft, _) = DoCollisionCheck(Direction.LEFT, posX, posY, posX - 1, posY + sizeY);
            (touchingRight, _) = DoCollisionCheck(Direction.RIGHT, posX + sizeX, posY, posX + sizeX + 1, posY + sizeY);

            allowOverCliffWalking = !pressedSneak;

            breathing = touchingStatus[TOUCHING_AIR];

            if (touchingStatus[TOUCHING_CACTUS] || touchingStatus[TOUCHING_MAGMA])
            {
                Damage((1000 / tps) * 0.001);
            }

            if (flying)
            {
                accGrav = 0;
            }
            else
            {
                accGrav = DEFAULT_GRAV;
            }

            if (touchingStatus[TOUCHING_LADDER])
            {
                fallMaxHeight = posY;
            }
        }




        public override void DoPhysicsStep(int tps)
        {
            if (this is Player)
            {
                if (pressedSneak && !flying)
                {
                    currentAcc = ACC_SNEAKING;
                    if (sizeY == 1900)
                    {
                        posY += 450;
                    }
                    sizeY = 1450;
                }
                else if (pressedSprint)
                {
                    currentAcc = ACC_SPRINTING;
                    if (sizeY == 1450)
                    {
                        posY -= 450;
                    }
                    sizeY = 1900;
                }
                else
                {
                    currentAcc = ACC_WALKING;
                    if (sizeY == 1450)
                    {
                        posY -= 450;
                    }
                    sizeY = 1900;
                }
            }

            texture.Height = sizeY / 20;

            // -- change velocity depending on inputs --
            if (touchingStatus[TOUCHING_LADDER])
            {
                if (pressedRight && !touchingRight)
                {
                    velX += currentAcc / (tps * 3);
                }
                if (pressedLeft && !touchingLeft)
                {
                    velX -= currentAcc / (tps * 3);
                }
            }
            else
            {
                if (pressedRight)
                {
                    if (!touchingRight || CheckUpStep(Direction.RIGHT, 1, tps))
                    {
                        velX += currentAcc / tps;
                    }
                }
                if (pressedLeft)
                {
                    if (!touchingLeft || CheckUpStep(Direction.LEFT, -1, tps))
                    {
                        velX -= currentAcc / tps;
                    }
                }
            }

            //handle jump key
            if (flying)
            {
                velY = 0;
                if (pressedUp)
                {
                    velY = -4000;
                }
                if (pressedSneak)
                {
                    velY = 4000;
                }
            }
            else
            {
                if (pressedUp)
                {
                    if (onGround && !touchingStatus[TOUCHING_LADDER])
                    {
                        velY = -jumpStartSpeed;
                    }

                    if (touchingStatus[TOUCHING_WATER])
                    {
                        velY -= 200000 / tps;
                    }
                }
            }

            base.DoPhysicsStep(tps);

            if (touchingStatus[TOUCHING_LADDER])
            {
                if (pressedUp)
                {
                    velY = -5000;
                }
                else
                {
                    if (pressedSneak)
                    {
                        velY = 0;
                    }
                }
                velY = Math.Min(velY, 4000);
            }
        }
    }
}