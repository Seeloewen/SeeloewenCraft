using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class Player : MovingEntity
    {
        //public Canvas cvsPlayer = new Canvas();
        //World world;
        public Inventory inventory;
        public HealthBar healthBar;

        //-- Variables for physics --/

        //constants:










        //-- Constructor --//

        public Player(World world, int x, int y) : base(70000, 900, 1900, x, y, 0, 0, world, Colors.Red)
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

            Log.Write($"Created player at position x{posX} y{posY}", "Info");

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
            world.debugMenu.AddLine(world.debugMenu.tblPlayerStats, "touchingWater");



            //Setup health bar
            if (Settings.enableHealth)
            {
                healthBar = new HealthBar(world, 10, 740);
            }
        }


        //physics
        public override void DoPhysicsStep(int tps)
        {

            

            base.DoPhysicsStep(tps);

            //

            // -- check if moving into blocks --

            //move with amount of acual pixels

            DisplayDebugInformation();
        }



        public void SavePosition(string path)
        {
            Log.Write($"Saved player position to {path}", "Info");

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
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "blockPosX", $"blockPosX={(posX / 1000) % 8}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "blockPosY", $"blockPosY={posY / 1000}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "touchingWater", $"touchingWater={touchingWater}");
            }
        }
    }
}
