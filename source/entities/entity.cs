using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft.entity
{
    //base class for all entities
    public abstract class Entity
    {
        public static Random idGenerator = new Random(DateTime.Now.Millisecond);
        protected Random rnd;

        public TextBlock tblId;

        public string type;

        //touching status constants
        public const int TOUCHING_STATUS_COUNT = 7;
        public const int TOUCHING_WATER = 0;
        public const int TOUCHING_WATER_LEFT = 1;
        public const int TOUCHING_WATER_RIGHT = 2;
        public const int TOUCHING_CACTUS = 3;
        public const int TOUCHING_AIR = 4;
        public const int TOUCHING_MAGMA = 5;
        public const int TOUCHING_LADDER = 6;

        //physics constants
        public const int DEFAULT_GRAV = 70000;
        protected int accGrav = DEFAULT_GRAV;
        protected int frictionGround = 10;
        protected int frictionAir = 10;
        protected int frictionWater = 25;
        private const int slowestGroundSpeed = 100;

        public long lifeTime;

        protected bool allowOverCliffWalking = true;

        public int id { get; private set; }

        public Canvas texture;

        public int sizeX;
        public int sizeY;

        public int posX;
        public int posY;

        public int velX;
        public int velY;

        public bool onGround;

        public bool[] touchingStatus;

        //calculate friction and apply it to velocity
        private void DoFrictionStep(int tps)
        {
            if (touchingStatus[TOUCHING_WATER])
            {
                double velTotal = Math.Sqrt(velX * velX + velY * velY); //tactical pythagoras
                if (velTotal != 0)
                {
                    if (velX > 0)
                    {
                        velX -= (int)(frictionWater * velX / tps);
                    }
                    else if (velX < 0)
                    {
                        velX -= (int)(frictionWater * velX / tps);
                    }
                    if (velY > 0)
                    {

                        velY -= (int)(frictionWater * velY / tps);
                    }
                    else if (velY < 0)
                    {
                        velY -= (int)(frictionWater * velY / tps);
                    }
                }
            }
            else
            {
                if (onGround)
                {
                    if (velX > 0)
                    {
                        //this reduces the velocity by f_ground * v_x * dt until v_x reaches a threshold with value slowest_ground_speed, when it gets set to zero
                        int v_reduction = frictionGround * velX / tps;
                        velX -= Math.Max(Math.Min(slowestGroundSpeed, velX), v_reduction);
                    }
                    else if (velX < 0)
                    {
                        int v_reduction = -frictionGround * velX / tps;
                        velX += Math.Max(Math.Min(slowestGroundSpeed, -velX), v_reduction);
                    }
                }
                else
                {
                    int v_reduction = frictionAir * velX / tps;
                    velX -= v_reduction;
                }
            }
        }

        //do one tick
        public virtual void OnUpdate(int tps) //temp virtual
        {
            OnUpdateStart(tps);

            DoPhysicsStep(tps);

            Move(tps);

            OnUpdateEnd(tps);
        }

        protected virtual void OnUpdateStart(int tps)
        {
            lifeTime += 1000 / tps;

            (onGround, _) = DoCollisionCheck(Direction.DOWN, posX, posY + sizeY, posX + sizeX, posY + sizeY + 1);

            DoTouchCheck();
        }

        protected virtual void OnUpdateEnd(int tps)
        {

        }

        public virtual void DoPhysicsStep(int tps)
        {


            if (!onGround)
            {
                velY += accGrav / tps;
            }

            if (touchingStatus[TOUCHING_WATER])
            {
                if (touchingStatus[TOUCHING_WATER_LEFT])
                {
                    velX -= 20000 / tps;
                }
                if (touchingStatus[TOUCHING_WATER_RIGHT])
                {
                    velX += 20000 / tps;
                }
            }

            DoFrictionStep(tps);


        }

        protected void Move(int tps)
        {
            int dX = velX / tps;
            int dY = velY / tps;

            Move(dX, dY, tps);
        }

        protected void Move(int dX, int dY, int tps)
        {
            if (dX > 0)
            {
                int prevPosX = posX;
                int prevPosY = posY;
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.RIGHT, posX + sizeX, posY, posX + sizeX + dX, posY + sizeY);
                if (collided)
                {
                    posX += maxMovement;
                    if (dX - maxMovement != 0 && !CheckUpStep(Direction.RIGHT, dX - maxMovement, tps))
                    {
                        velX = 0;
                    }
                }
                else
                {
                    posX += dX;
                }
                if (!allowOverCliffWalking && onGround)
                {
                    (bool onGround, _) = DoCollisionCheck(Direction.DOWN, posX, posY + sizeY, posX + sizeX, posY + sizeY + 1);
                    if (!onGround)
                    {
                        posX = prevPosX;
                        posY = prevPosY;
                        Move(dX / 2, dY / 2, tps);
                    }
                }
            }
            if (dX < 0)
            {
                int prevPosX = posX;
                int prevPosY = posY;
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.LEFT, posX, posY, posX + dX, posY + sizeY);
                if (collided)
                {
                    posX -= maxMovement;
                    if (dX + maxMovement != 0 && !CheckUpStep(Direction.LEFT, dX + maxMovement, tps))
                    {
                        velX = 0;
                    }
                }
                else
                {
                    posX += dX;
                }
                if (!allowOverCliffWalking && onGround)
                {
                    (bool onGround, _) = DoCollisionCheck(Direction.DOWN, posX, posY + sizeY, posX + sizeX, posY + sizeY + 1);
                    if (!onGround)
                    {
                        posX = prevPosX;
                        posY = prevPosY;
                        Move(dX / 2, dY / 2, tps);
                    }
                }
            }

            if (dY > 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.DOWN, posX, posY + sizeY, posX + sizeX, posY + sizeY + dY);
                if (collided)
                {
                    posY += maxMovement;
                    DoFallDamage();
                    velY = 0;
                }
                else
                {
                    posY += dY;
                }
            }
            if (dY < 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.UP, posX, posY, posX + sizeX, posY + dY);
                if (collided)
                {
                    velY = 0;
                    posY -= maxMovement;
                }
                else
                {
                    posY += dY;
                }
            }
        }

        protected virtual void DoFallDamage()
        {
            return;
        }

        //return true if stepped up
        protected virtual bool CheckUpStep(Direction direction, int remaining, int tps)
        {
            return false;
        }

        public int ConvertToBlockX(int i)
        {
            return i >= 0
                ? i / 1000
                : (i - 999) / 1000;
        }

        public int GetChunkIndex()
        {
            return posX >= 0
                ? posX / 8000
                : (posX - 7999) / 8000;
        }

        protected void DoTouchCheck()
        {
            touchingStatus = new bool[TOUCHING_STATUS_COUNT];
            if (posY < 0 || posY > 75000) return;


            for (int x = ConvertToBlockX(posX); x <= ConvertToBlockX(posX + sizeX - 1); x++)
            {
                for (int y = posY / 1000; y <= (posY + sizeY - 1) / 1000; y++)
                {

                    int startX = posX - x * 1000;
                    int endX = posX + sizeX - 1 - x * 1000;
                    int startY = posY - y * 1000;
                    int endY = posY + sizeY - 1 - y * 1000;
                    bool[] blockTouching = Game.world.GetBlock(x, y).CheckTouch(startX, startY, endX, endY);
                    for (int i = 0; i < blockTouching.Length; i++)
                    {
                        touchingStatus[i] = touchingStatus[i] || blockTouching[i];
                    }
                }
            }
        }

        protected (bool, int) DoCollisionCheck(Direction direction, int startX, int startY, int endX, int endY)
        {
            if (endY < 0 || endY > 75000 || posY < 0 || posY > 75000) return (false, 0);

            if (direction.IsRight())
            {
                for (int x = ConvertToBlockX(startX); x <= ConvertToBlockX(endX); x++)
                {
                    bool collision = false;
                    int maxMovement = int.MaxValue;

                    for (int y = startY / 1000; y <= endY / 1000; y++)
                    {
                        Block b = Game.world.GetBlock(x, y);
                        (bool newCollision, int newMaxMovement) = b.CheckCollision(Direction.RIGHT, startX, endX, startY, endY);
                        if (newCollision)
                        {
                            collision = true;
                            maxMovement = Math.Min(maxMovement, newMaxMovement);
                        }
                    }
                    if (collision)
                    {
                        return (true, maxMovement);
                    }
                }
                return (false, 0);
            }

            if (direction.IsLeft())
            {
                for (int x = ConvertToBlockX(startX); x >= ConvertToBlockX(endX); x--)
                {
                    bool collision = false;
                    int maxMovement = int.MaxValue;

                    for (int y = startY / 1000; y <= endY / 1000; y++)
                    {
                        (bool newCollision, int newMaxMovement) = Game.world.GetBlock(x, y).CheckCollision(Direction.LEFT, startX, endX, startY, endY);
                        if (newCollision)
                        {
                            collision = true;
                            maxMovement = Math.Min(maxMovement, newMaxMovement);
                        }

                    }
                    if (collision)
                    {
                        return (true, maxMovement);
                    }
                }
                return (false, 0);
            }


            if (direction.IsDown())
            {
                for (int y = startY / 1000; y <= endY / 1000; y++)
                {
                    bool collision = false;
                    int maxMovement = int.MaxValue;

                    for (int x = ConvertToBlockX(startX); x <= ConvertToBlockX(endX); x++)
                    {
                        (bool newCollision, int newMaxMovement) = Game.world.GetBlock(x, y).CheckCollision(Direction.DOWN, startX, endX, startY, endY);
                        if (newCollision)
                        {
                            collision = true;
                            maxMovement = Math.Min(maxMovement, newMaxMovement);
                        }

                    }
                    if (collision)
                    {
                        return (true, maxMovement);
                    }
                }
                return (false, 0);
            }

            if (direction.IsUp())
            {
                for (int y = startY / 1000; y >= endY / 1000; y--)
                {
                    bool collision = false;
                    int maxMovement = int.MaxValue;

                    for (int x = ConvertToBlockX(startX); x <= ConvertToBlockX(endX); x++)
                    {
                        (bool newCollision, int newMaxMovement) = Game.world.GetBlock(x, y).CheckCollision(Direction.UP, startX, endX, startY, endY);
                        if (newCollision)
                        {
                            collision = true;
                            maxMovement = Math.Min(maxMovement, newMaxMovement);
                        }

                    }
                    if (collision)
                    {
                        return (true, maxMovement);
                    }
                }
                return (false, 0);
            }
            return (false, 0);
        }

        public Entity(JsonToken token, int sizeX, int sizeY, Brush image)
            : this(sizeX, sizeY,
                token.GetInt("/posX"),
                token.GetInt("/posY"),
                token.GetInt("/velX"),
                token.GetInt("/velY"),
                 image)
        {
            lifeTime = token.GetInt("/life_time");
            id = token.GetInt("/id");

            texture.Children.Clear();
            tblId = new TextBlock() { FontSize = 20, FontWeight = FontWeights.DemiBold };
            tblId.Text = id.ToString();
            if (this is MovingEntity)
            {
                texture.Children.Add(tblId);
                Canvas.SetTop(tblId, -30);
                Canvas.SetLeft(tblId, 8);
            }

            rnd = new Random(id);
        }

        public Entity(int sizeX, int sizeY, int posX, int posY, int velX, int velY, Brush image)
        {
            lifeTime = 0;
            id = idGenerator.Next(0, int.MaxValue);
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.posX = posX;
            this.posY = posY;
            this.velX = velX;
            this.velY = velY;

            texture = new Canvas();
            texture.Margin = new Thickness(0, 0, 0, 0);
            texture.Width = sizeX / 20;
            texture.Height = sizeY / 20;
            texture.Background = image;

            touchingStatus = new bool[TOUCHING_STATUS_COUNT];

            texture.Children.Clear();
            tblId = new TextBlock() { FontSize = 20, FontWeight = FontWeights.DemiBold };
            tblId.Text = id.ToString();
            if (this is MovingEntity)
            {
                texture.Children.Add(tblId);
                Canvas.SetTop(tblId, -30);
                Canvas.SetLeft(tblId, 8);
            }

            rnd = new Random(id);
        }

        public static Entity LoadFromJson(JsonToken token)
        {
            Entity entity;
            string type = token.GetString("/type");
            switch (type)
            {
                case "FallingBlockEntity":
                    entity = new FallingBlockEntity(token);
                    break;
                case "ItemEntity":
                    entity = new ItemEntity(token);
                    break;
                case "Slime":
                    entity = new Slime(token);
                    break;
                case "Player":
                    entity = new Player(token);
                    break;
                default:
                    throw new Exception($"Loading Error: entity type {type} not found");
            }
            entity.type = type;

            return entity;
        }

        //save all attributes of entity base class; override SaveSpecialInfo()
        //to save custom values
        internal void SaveToJson(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue(type);
            writer.WritePropertyName("id");
            writer.WriteValue(id);
            writer.WritePropertyName("posX");
            writer.WriteValue(posX);
            writer.WritePropertyName("posY");
            writer.WriteValue(posY);
            writer.WritePropertyName("velX");
            writer.WriteValue(velX);
            writer.WritePropertyName("velY");
            writer.WriteValue(velY);
            writer.WritePropertyName("life_time");
            writer.WriteValue(lifeTime);

            SaveSpecialInfo(writer);
            writer.WriteEndObject();
        }

        protected virtual void SaveSpecialInfo(JsonWriter writer)
        {

        }


        public override bool Equals(object obj)
        {
            return (obj is Entity e && e.id == id);
        }

        public override int GetHashCode()
        {
            return id;
        }

        //Hahn und Jazz waren hier
    }
}