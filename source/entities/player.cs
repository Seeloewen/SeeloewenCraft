using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Media;

namespace SeeloewenCraft.entity
{
    public class Player : MovingEntity
    {
        public Inventory inventory;
        public HealthBar healthBar;

        public Gamemode gamemode = Gamemode.Survival;

        public const double HIT_RANGE = 4000.0;
        public const double HIT_DAMAGE = 2.0;

        //-- Constructor --//

        public Player(int x, int y) : base(900, 1900, x, y, 0, 0)
        {
            //Generate the player
            type = "Player";
            InitPlayer();
        }

        public Player(JsonToken token) : base(token, 900, 1900, new SolidColorBrush(Colors.Red))
        {
            InitPlayer();
        }

        //-- Custom Methods --//

        protected override void InitTexture()
        {
            texture.Background = new SolidColorBrush(Colors.Red);
        }

        private void HandleThrow()
        {
            if (pressedThrow)
            {
                if (!thrown && inventory != null)
                {
                    //Get the selected slot and selected item
                    InventorySlot selectedSlot = inventory.GetSelectedHotbarSlot().slot;
                    Item item = null;
                    if (!selectedSlot.IsEmpty())
                    {
                        item = ItemRegister.GenerateItem(selectedSlot.itemId);
                    }

                    if (item != null)
                    {
                        (double mousePosX, double mousePosY) = Game.world.worldRenderer.GetMouseOffset();
                        double xOffset = mousePosX - posX - 450;
                        double yOffset = mousePosY - posY;
                        double n = Math.Sqrt(xOffset * xOffset + yOffset * yOffset);
                        double xDir = xOffset / n;
                        double yDir = yOffset / n;

                        ItemEntity itemEntity = new ItemEntity(item, item.tag, posX + 500 - ItemEntity.itemSizeX / 2, posY, (int)(15000 * xDir) + velX, (int)(20000 * yDir) + velY);
                        Game.world.AddEntity(itemEntity);
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

        public void HandleInputs()
        {
            if (this != Game.world.player) return;
            //doesnt get synced
            bool newPressedThrow = Game.world.wndGame.pressedKeys.Contains(Settings.cThrowItem);

            //will get synced
            bool newPressedLeft = Game.world.wndGame.pressedKeys.Contains(Settings.cMoveLeft);
            bool newPressedRight = Game.world.wndGame.pressedKeys.Contains(Settings.cMoveRight);
            bool newPressedUp = Game.world.wndGame.pressedKeys.Contains(Settings.cJump);
            bool newPressedSneak = Game.world.wndGame.pressedKeys.Contains(Settings.cSneak);
            bool newPressedSprint = Game.world.wndGame.pressedKeys.Contains(Settings.cSprint);

            PressedChangeEvent e = PressedChangeEvent.Create(id,
                pressedUp, newPressedUp,
                pressedRight, newPressedRight,
                pressedLeft, newPressedLeft,
                pressedSneak, newPressedSneak,
                pressedSprint, newPressedSprint);

            if (e != null)
            {
                HandlePressedChangeEvent(e);
                e.Send();
            }
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
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblPlayerStats, "Player Stats:");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblPlayerStats, "health");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblPlayerStats, "posX");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblPlayerStats, "posY");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblPlayerStats, "velX");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblPlayerStats, "velY");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblPlayerStats, "blockPosX");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblPlayerStats, "blockPosY");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblPlayerStats, "touchingWater");
            Game.world.debugMenu.AddLine(Game.world.debugMenu.tblPlayerStats, "breathing");

            //Setup health bar
            healthBar = new HealthBar(10, 740);

            if (Game.world.gamemode == Gamemode.Creative)
            {
                healthBar.Hide();
            }
        }

        public override void Die()
        {
            if (this == Game.world.player)
            {
                //Drop all items and clear the inventory
                foreach (InventorySlot slot in inventory.slotList)
                {
                    for (int i = 0; i < slot.Amount; i++)
                    {
                        Drop(slot.itemId);
                    }
                    slot.Remove(slot.Amount);
                }

                //Move the player to the spawn
                posX = Game.world.worldSpawnX;
                posY = Game.world.worldSpawnY;

                //Set the hp back to 10
                base.SetHP(10);

                NotificationHandler.ShowNotification("You died and were moved back to the world spawn.", 5000, Images.Bone.GetTexture());
            }
        }

        public override void SetHP(double hp)
        {
            base.SetHP(hp);
            healthBar.SetValue((int)(this.hp * 2) * 0.5);
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
            if (Game.world.debugMenu.isEnabled)
            {
                Game.world.debugMenu.ChangeLine(Game.world.debugMenu.tblPlayerStats, "health", $"health={healthBar.value}");
                Game.world.debugMenu.ChangeLine(Game.world.debugMenu.tblPlayerStats, "posX", $"posX={posX}");
                Game.world.debugMenu.ChangeLine(Game.world.debugMenu.tblPlayerStats, "posY", $"posY={posY}");
                Game.world.debugMenu.ChangeLine(Game.world.debugMenu.tblPlayerStats, "velX", $"velX={velX}");
                Game.world.debugMenu.ChangeLine(Game.world.debugMenu.tblPlayerStats, "velY", $"velY={velY}");
                Game.world.debugMenu.ChangeLine(Game.world.debugMenu.tblPlayerStats, "blockPosX", $"blockPosX={(posX / 1000) % 8}");
                Game.world.debugMenu.ChangeLine(Game.world.debugMenu.tblPlayerStats, "blockPosY", $"blockPosY={posY / 1000}");
                Game.world.debugMenu.ChangeLine(Game.world.debugMenu.tblPlayerStats, "touchingWater", $"touchingWater={touchingStatus[TOUCHING_WATER]}");
                Game.world.debugMenu.ChangeLine(Game.world.debugMenu.tblPlayerStats, "breathing", $"breathing={breathing}");
            }
        }
    }
}
