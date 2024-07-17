using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class Player
    {
        public Canvas cvsPlayer = new Canvas();
        World world;
        public Inventory inventory;
        public HealthBar healthBar;

        //-- Variables for physics --/

        //constants:
        private const int accWalking = 50000; // a: acceleration
        private const int accGrav = 60000;
        private const int jumpStartSpeed = 15000;
        private const int friction = 10;
        private const int slowestGroundSpeed = 400;

        //variables

        int sizeX = 900;
        int sizeY = 1900;

        public int posX = 20050; //1/1000 of a block (mm)
        public int posY = 5000;
        int velX = 0; //(mm/s)
        int velY = 0;







        //-- Constructor --//

        public Player(World world, int x, int y)
        {
            //Set the attributes
            this.world = world;

            //Generate the player
            GeneratePlayer(x, y);
            posY = y * 20;
        }

        //-- Custom Methods --//

        public void GeneratePlayer(int x, int y)
        {
            //Setup the character canvas that is shown but does not count in movement checks
            cvsPlayer.Margin = new Thickness(x, y, 0, 0);
            cvsPlayer.Width = 45;
            cvsPlayer.Height = 95;
            cvsPlayer.Background = new SolidColorBrush(Colors.Red);

            world.log.Write($"Created player at position x{x} y{y}", "Info");

            //Add initial debug menu lines
            world.debugMenu.AddLine(world.debugMenu.tblPlayerStats, "Player Stats:");
            if (world.settings.enableHealth)
            {
                world.debugMenu.AddLine(world.debugMenu.tblPlayerStats, "health");
            }
            world.debugMenu.AddLine(world.debugMenu.tblPlayerStats, "posX");
            world.debugMenu.AddLine(world.debugMenu.tblPlayerStats, "posY");
            world.debugMenu.AddLine(world.debugMenu.tblPlayerStats, "velX");
            world.debugMenu.AddLine(world.debugMenu.tblPlayerStats, "velY");
            world.debugMenu.AddLine(world.debugMenu.tblPlayerStats, "blockPosX");
            world.debugMenu.AddLine(world.debugMenu.tblPlayerStats, "blockPosY");



            //Setup health bar
            if (world.settings.enableHealth)
            {
                healthBar = new HealthBar(world, 10, 740);
            }
        }

        private int ConvertToBlockX(int i)
        {
            return i >= 0
                ? i / 1000 
                : (i-999) / 1000;
        }

        private (bool, int) DoCollisionCheck(Direction direction, int startX, int startY, int endX, int endY)
        {
            if (endY < 0 || endY > 75000) return (false, 0);

            if (direction.IsRight())
            {
                for (int x = ConvertToBlockX(startX); x <= ConvertToBlockX(endX); x++)
                {
                    bool collision = false;
                    int maxMovement = int.MaxValue;

                    for (int y = startY / 1000; y <= endY / 1000; y++)
                    {
                        (bool newCollision, int newMaxMovement) = world.GetBlock(x, y).CheckCollision(Direction.RIGHT, startX, endX, startY, endY);
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
                        (bool newCollision, int newMaxMovement) = world.GetBlock(x, y).CheckCollision(Direction.LEFT, startX, endX, startY, endY);
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
                        (bool newCollision, int newMaxMovement) = world.GetBlock(x, y).CheckCollision(Direction.DOWN, startX, endX, startY, endY);
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
                        (bool newCollision, int newMaxMovement) = world.GetBlock(x, y).CheckCollision(Direction.UP, startX, endX, startY, endY);
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


        //physics
        public void DoPhysicsStep(bool pressedLeft, bool pressedRight, bool pressedUp, double deltaTime)
        {

            int tps = (int)(1.0 / deltaTime);

            // -- determine which sides of the player are touched by solid blocks --

            //reset
            (bool onGround, _) = DoCollisionCheck(Direction.DOWN, posX, posY + sizeY, posX + sizeX, posY + sizeY + 1);
            (bool touchingLeft, _) = DoCollisionCheck(Direction.LEFT, posX, posY, posX - 1, posY + sizeY);
            (bool touchingRight, _) = DoCollisionCheck(Direction.RIGHT, posX + sizeX, posY, posX + sizeX + 1, posY + sizeY);




            // -- change velocity depending on inputs --
            if (pressedRight && !touchingRight)
            {
                velX += accWalking / tps;
            }
            if (pressedLeft && !touchingLeft)
            {
                velX -= accWalking / tps;
            }

            //jump
            if (pressedUp && onGround)
            {
                velY = -jumpStartSpeed;
                onGround = false;
            }

            // -- friction --
            if (true) //normally: onGround
            {
                if (velX > 0)
                {
                    //this reduces the velocity by f_ground * v_x * dt until v_x reaches a threshold with value slowest_ground_speed, when it gets set to zero
                    int v_reduction = friction * velX/tps;
                    velX -= Math.Max(Math.Min(slowestGroundSpeed/tps, velX), v_reduction);
                }
                else if (velX < 0)
                {
                    int v_reduction = -friction * velX/tps;
                    velX += Math.Max(Math.Min(slowestGroundSpeed/tps, -velX), v_reduction);
                }
            }



            if (!onGround)
            {
                velY += accGrav / tps;
            }

            int dX = velX / tps;
            int dY = velY / tps;

            if (velX > 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.RIGHT, posX + sizeX, posY, posX + sizeX + dX, posY + sizeY);
                if (collided)
                {
                    velX = 0;
                    posX += maxMovement;
                    //MoveHorizontal(maxMovement / 20);
                }
                else
                {
                    posX += velX / tps;
                    //MoveHorizontal(dX / 20);
                }
            }
            if (velX < 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.LEFT, posX, posY, posX + dX, posY + sizeY);
                if (collided)
                {
                    velX = 0;
                    posX -= maxMovement;
                    //MoveHorizontal(-maxMovement / 20);
                }
                else
                {
                    posX += velX / tps;
                    //MoveHorizontal(dX / 20);
                }
            }

            if (velY > 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.DOWN, posX, posY + sizeY, posX + sizeX, posY + sizeY + dY);
                if (collided)
                {
                    velY = 0;
                    posY += maxMovement;
                    //MoveVertical(-maxMovement / 20);
                }
                else
                {
                    posY += velY / tps;
                    //MoveVertical(-dY / 20);
                }
            }
            if (velY < 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.UP, posX, posY, posX + sizeX, posY + dY);
                if (collided)
                {
                    velY = 0;
                    posY -= maxMovement;
                    //MoveVertical(maxMovement / 20);
                }
                else
                {
                    posY += velY / tps;
                    //MoveVertical(-dY / 20);
                }
            }

            // -- check if moving into blocks --

            //move with amount of acual pixels

            DisplayDebugInformation();
        }

        //block origin means top left corner of the block relative to the top left corner of player
        private bool InBlock(Point blockOrigin, Point point)
        {
            return blockOrigin.Y <= point.Y
                && blockOrigin.Y + 50 > point.Y
                && blockOrigin.X <= point.X
                && blockOrigin.X + 50 > point.X;
        }


        public void SavePosition(string path)
        {
            world.log.Write($"Saved player position to {path}", "Info");

            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Formatting.Indented;
                writer.WriteStartObject();

                writer.WritePropertyName("pos_x");
                writer.WriteValue(posX);

                writer.WritePropertyName("pos_y");
                writer.WriteValue(posY);

                writer.WriteEndObject();

                writer.WriteToFile($"{path}/player_position.json");
            }

        }

        public void SaveInventory(string path)
        {
            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Formatting.Indented;
                inventory.SaveToJson(writer);
                writer.WriteToFile($"{path}/player_inventory.json");
            }
        }


        /*public void MoveHorizontal(int amount)
        {
            //Move all chunks the specified amount to the left
            foreach (Chunk chunk in world.currentChunkList)
            {
                Canvas.SetLeft(chunk.grdChunk, Canvas.GetLeft(chunk.grdChunk) - amount);
            }

            //Sort the list to account for chunk movement
            world.currentChunkList = world.currentChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();

            //Check if the chunk has moved too far
            if (Canvas.GetLeft(world.currentChunkList[2].grdChunk) <= 0)
            {
                double offset = -Canvas.GetLeft(world.currentChunkList[2].grdChunk);
                //Save the chunk that has moved to far and remove it. Add a new one at the opposite site.
                world.currentChunkList.Remove(world.GetFromCurrentChunks(world.currentChunkList[0].index));
                world.currentChunkList.Add(world.GetChunk(world.currentChunkList[3].index + 1));
                try
                {
                    world.wndGame.cvsWorld.Children.Add(world.currentChunkList[4].grdChunk);
                }
                catch { }

                Canvas.SetLeft(world.currentChunkList[4].grdChunk, 1200 - offset);

                //Sort the chunklist again
                world.currentChunkList = world.currentChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
            }
            else if (Canvas.GetLeft(world.currentChunkList[2].grdChunk) >= 800)
            {
                double offset = Canvas.GetLeft(world.currentChunkList[2].grdChunk) - 800;
                //Move the chunk on the right all the way to the left
                world.currentChunkList.Remove(world.GetFromCurrentChunks(world.currentChunkList[4].index));
                world.currentChunkList.Add(world.GetChunk(world.currentChunkList[0].index - 1));
                try
                {
                    world.wndGame.cvsWorld.Children.Add(world.currentChunkList[4].grdChunk);
                }
                catch { }
                Canvas.SetLeft(world.currentChunkList[4].grdChunk, -400 + offset);

                //Sort the list again
                world.currentChunkList = world.currentChunkList.OrderBy(obj => Canvas.GetLeft(obj.grdChunk)).ToList();
            }
        }*/

        /*public void MoveVertical(int amount)
        {
            Thickness currentMarginPlayer = cvsPlayer.Margin;
            currentMarginPlayer.Top -= amount;
            cvsPlayer.Margin = currentMarginPlayer;

            //Scroll along to match the movement
            world.wndGame.svWorld.ScrollToVerticalOffset(world.player.cvsPlayer.Margin.Top - 400);
        }*/

        

        public void DisplayDebugInformation()
        {
            if (world.debugMenu.isEnabled)
            {
                if (world.settings.enableHealth)
                {
                    world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "health", $"health={healthBar.value}");
                }
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "posX", $"posX={posX}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "posY", $"posY={posY}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "velX", $"velX={velX}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "velY", $"velY={velY}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "blockPosX", $"blockPosX={(posX/1000)%8}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "blockPosY", $"blockPosY={posY/1000}");
            }
        }
    }
}
