using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class Player : Entity
    {
        //public Canvas cvsPlayer = new Canvas();
        //World world;
        public Inventory inventory;
        public HealthBar healthBar;

        //-- Variables for physics --/

        //constants:
        private const int accWalking = 70000; // a: acceleration
        private const int jumpStartSpeed = 15000;


        public bool pressedUp;
        public bool pressedRight;
        public bool pressedLeft;

        //variables

        /*int sizeX = 900;
        int sizeY = 1900;

        public int posX = 20050; //1/1000 of a block (1 mm)
        public int posY = 5000;
        int velX = 0; //(mm/s)
        int velY = 0;*/







        //-- Constructor --//

        public Player(World world, int x, int y) : base(900, 1900, x, y, 0, 0, world, Colors.Red)
        {
            //Set the attributes
            this.world = world;

            //Generate the player
            GeneratePlayer();
        }

        //-- Custom Methods --//

        public void GeneratePlayer()
        {
            //Setup the character canvas that is shown but does not count in movement checks
            /*cvsPlayer.Margin = new Thickness(0, 0, 0, 0);
            cvsPlayer.Width = 45;
            cvsPlayer.Height = 95;
            cvsPlayer.Background = new SolidColorBrush(Colors.Red);*/

            world.log.Write($"Created player at position x{posX} y{posY}", "Info");

            //Add initial debug menu lines
            world.debugMenu.AddLine(world.debugMenu.tblPlayerStats, "Player Stats:");
            if (Settings.enableHealth)
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
            if (Settings.enableHealth)
            {
                healthBar = new HealthBar(world, 10, 740);
            }
        }


        //physics
        public override void DoPhysicsStep(int tps)
        {

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
            }

            base.DoPhysicsStep(tps);

            //

            // -- check if moving into blocks --

            //move with amount of acual pixels

            DisplayDebugInformation();
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


       

        public void DisplayDebugInformation()
        {
            if (world.debugMenu.isEnabled)
            {
                if (Settings.enableHealth)
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
