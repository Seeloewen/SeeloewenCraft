using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;

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
        private const int accGrav = 70000;
        private const int jumpStartSpeed = 15000;

        //variables

        int sizeX;
        int sizeY;

        int posX; //1/1000 of a block (mm)
        int posY;
        int velX; //(mm/s)
        int velY;







        //-- Constructor --//

        public Player(World world, int x, int y)
        {
            //Set the attributes
            this.world = world;

            //Generate the player
            GeneratePlayer(x, y);
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

            //Setup health bar
            if (world.settings.enableHealth)
            {
                healthBar = new HealthBar(world, 10, 740);
            }
        }

        private (bool, int) DoCollisionCheck(Direction direction, int startX, int startY, int endX, int endY)
        {
            if (direction == Direction.RIGHT)
            {
                for (int x = startX / 1000; x <= endX / 1000; x++)
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

            if (direction == Direction.LEFT)
            {
                for (int x = startX / 1000; x >= endX / 1000; x--)
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


            if (direction == Direction.DOWN)
            {
                for (int y = startY / 1000; y <= endY / 1000; y++)
                {
                    bool collision = false;
                    int maxMovement = int.MaxValue;

                    for (int x = startX / 1000; x <= endX / 1000; x++)
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

            if (direction == Direction.UP)
            {
                for (int y = startY / 1000; y >= endY / 1000; y--)
                {
                    bool collision = false;
                    int maxMovement = int.MaxValue;

                    for (int x = startX / 1000; x <= endX / 1000; x++)
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
            (bool touchingTop, _) = DoCollisionCheck(Direction.UP, posX, posY, posX+sizeX, posY-1);
            (bool touchingRight, _) = DoCollisionCheck(Direction.RIGHT, posX+sizeX, posY, posX+sizeX+1, posY+sizeY);




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
            /*if (true) //normally: onGround
            {
                if (v_x > 0)
                {
                    //this reduces the velocity by f_ground * v_x * dt until v_x reaches a threshold with value slowest_ground_speed, when it gets set to zero
                    double v_reduction = f_ground * v_x * dt;
                    v_x -= Math.Max(Math.Min(slowest_ground_speed * dt, v_x), v_reduction);
                }
                else if (v_x < 0)
                {
                    double v_reduction = -f_ground * v_x * dt;
                    v_x += Math.Max(Math.Min(slowest_ground_speed * dt, -v_x), v_reduction);
                }
            }*/
            
            //friction temporary
            velX = velX*8/10;


            if (!onGround)
            {
                velY += accGrav / tps;
            }


            if(velX > 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.RIGHT, posX+sizeX,posY,posX+sizeX+velX,posY+sizeY);
                if(collided)
                {
                    velX = 0;
                    posX += maxMovement;
                    MoveHorizontal(maxMovement/20);
                }else
                {
                    posX += velX;
                    MoveHorizontal(velX / 20);
                }
            }
            if (velX < 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.LEFT, posX, posY, posX+velX, posY + sizeY);
                if (collided)
                {
                    velX = 0;
                    posX -= maxMovement;
                    MoveHorizontal(-maxMovement / 20);
                }
                else
                {
                    posX += velX;
                    MoveHorizontal(velX / 20);
                }
            }

            if (velY > 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.DOWN, posX, posY+sizeY, posX + sizeX, posY + sizeY+velY);
                if (collided)
                {
                    velY = 0;
                    posY += maxMovement;
                    MoveHorizontal(maxMovement / 20);
                }
                else
                {
                    posY += velY;
                    MoveHorizontal(velY / 20);
                }
            }
            if (velX < 0)
            {
                (bool collided, int maxMovement) = DoCollisionCheck(Direction.UP, posX, posY, posX+sizeX, posY+velY);
                if (collided)
                {
                    velY = 0;
                    posY -= maxMovement;
                    MoveHorizontal(-maxMovement / 20);
                }
                else
                {
                    posY += velY;
                    MoveHorizontal(velY / 20);
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


        public void MoveHorizontal(int amount)
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
        }

        public void MoveVertical(int amount)
        {
            Thickness currentMarginPlayer = cvsPlayer.Margin;
            currentMarginPlayer.Top -= amount;
            cvsPlayer.Margin = currentMarginPlayer;

            //Scroll along to match the movement
            world.wndGame.svWorld.ScrollToVerticalOffset(world.player.cvsPlayer.Margin.Top - 400);
        }

        public List<Chunk> GetCurrentChunks()
        {
            //Create a list of the chunks the player is currently in by checking collision
            List<Chunk> chunkList = new List<Chunk>();
            foreach (Chunk chunk in world.currentChunkList)
            {
                if (world.wndGame.GetRectangle(cvsPlayer).IntersectsWith(world.wndGame.GetRectangle(chunk.grdChunk)))
                {
                    chunkList.Add(chunk);
                }
            }

            return chunkList;
        }

        public void DisplayDebugInformation()
        {
            if (world.debugMenu.isEnabled)
            {
                if (world.settings.enableHealth)
                {
                    world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "health", $"health={healthBar.value}");
                }
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "v_x", $"v_x={v_x}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "v_y", $"v_y={v_y}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "d_x", $"d_x={d_x}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "d_y", $"d_y={d_y}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "d_offset_x", $"d_offset_x={d_offset_x}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "d_offset_y", $"d_offset_y={d_offset_y}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "posX", $"posX={posXInChunk}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "posY", $"posY={posY + 1}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "chunk", $"chunk={currentChunk}");
            }
        }
    }
}
