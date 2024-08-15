using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Media;

namespace SeeloewenCraft
{
    public class Player : MovingEntity
    {
        public Inventory inventory;
        public HealthBar healthBar;

        public Gamemode gamemode = Gamemode.Survival;

        public const double HIT_RANGE = 4000.0;
        public const double HIT_DAMAGE = 2.0;

        //-- Constructor --//

        public Player(World world, int x, int y) : base(900, 1900, x, y, 0, 0, world, new SolidColorBrush(Colors.Red))
        {
            //Set the attributes
            this.world = world;

            //Generate the player
            InitPlayer();
        }

        //-- Custom Methods --//

        private void HandleThrow()
        {
            if (pressedThrow)
            {
                if (!thrown)
                {
                    //Get the selected slot and selected item
                    InventorySlot selectedSlot = inventory.GetSelectedHotbarSlot().slot;
                    Item item = null;
                    if (!selectedSlot.IsEmpty())
                    {
                        item = ItemRegister.GenerateItem(selectedSlot.itemId, world);
                    }

                    if (item != null)
                    {
                        (double mousePosX, double mousePosY) = world.worldRenderer.GetMouseOffset();
                        double xOffset = mousePosX - posX - 450;
                        double yOffset = mousePosY - posY;
                        double n = Math.Sqrt(xOffset * xOffset + yOffset * yOffset);
                        double xDir = xOffset / n;
                        double yDir = yOffset / n;

                        ItemEntity itemEntity = new ItemEntity(item, posX + 500 - ItemEntity.itemSizeX / 2, posY, (int)(15000 * xDir) + velX, (int)(20000 * yDir) + velY, world);
                        world.AddEntity(itemEntity);
                        thrown = true;
                        selectedSlot.Remove(1);
                        selectedSlot.inventory.UpdateHotbar();
                    }
                }
            }
            else
            {
                thrown = false;
            }
        }

        private void HandleInputs()
        {
            pressedLeft = world.wndGame.pressedKeys.Contains(Settings.cMoveLeft);
            pressedRight = world.wndGame.pressedKeys.Contains(Settings.cMoveRight);
            pressedUp = world.wndGame.pressedKeys.Contains(Settings.cJump);
            pressedSneak = world.wndGame.pressedKeys.Contains(Settings.cSneak);
            pressedSprint = world.wndGame.pressedKeys.Contains(Settings.cSprint);
            pressedThrow = world.wndGame.pressedKeys.Contains(Settings.cThrowItem);
        }

        protected override void DoFallDamage()
        {
            if (lifeTime > 5000)
            {
                base.DoFallDamage();
            }
        }

        public void SetGamemode(Gamemode gamemode)
        {
            this.gamemode = gamemode;
            if (gamemode == Gamemode.Creative)
            {
                healthBar.Hide();
            }
            else if (gamemode == Gamemode.Survival)
            {
                healthBar.Show();
            }
        }

        public void InitPlayer()
        {
            //Setup the character canvas that is shown but does not count in movement checks

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
            world.debugMenu.AddLine(world.debugMenu.tblPlayerStats, "breathing");

            //Setup health bar
            healthBar = new HealthBar(world, 10, 740);

            if (world.gamemode == Gamemode.Creative)
            {
                healthBar.Hide();
            }
        }

        public override void Die()
        {
            MessageBox.Show("You experienced a severe skill issue and as a consequence have vanished from this world (death has not been implemented yet)");
        }

        public override void SetHP(double hp)
        {
            base.SetHP(hp);
            healthBar.SetValue((int)(hp * 2) * 0.5);
        }

        public override void Damage(double damage)
        {
            if (gamemode == Gamemode.Survival) base.Damage(damage);
        }


        protected override void OnUpdateStart(int tps)
        {
            HandleInputs();
            HandleThrow();
            base.OnUpdateStart(tps);
            DisplayDebugInformation();
        }

        protected override void OnUpdateEnd(int tps)
        {
            base.OnUpdateEnd(tps);
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
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "touchingWater", $"touchingWater={touchingStatus[TOUCHING_WATER]}");
                world.debugMenu.ChangeLine(world.debugMenu.tblPlayerStats, "breathing", $"breathing={breathing}");
            }
        }
    }
}
