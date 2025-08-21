using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.legacy;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.Windows.Media;
using JsonWriter = SeeloewenCraft.game.util.JsonWriter;

namespace SeeloewenCraft.game.core.entities
{
    public partial class Player : MovingEntity
    {
        public Inventory inventory;
        public HealthBar healthBar;

        public Gamemode gamemode = Gamemode.Survival;

        public const double HIT_RANGE = 4000.0;
        public const double HIT_DAMAGE = 2.0;

        private const int PLAYER_WIDTH = 475;
        private const int PLAYER_HEIGHT = 1900;

        private int directionScale = 1;

        float t = 0.0f;
        Direction headDir = Direction.LEFT;

        public PlayerRenderInfo playerRenderInfo = new PlayerRenderInfo(0, 0, Direction.LEFT, 0.0f, 0.0f, 0.0f, 0.0f);

        void UpdateAnimation(double dt)
        {
            t += (float)dt * Math.Abs(velX) * 0.003f;
            t %= (float)(2 * Math.PI);
            //double a = Math.Max(0, 1 - 50 / (40+Math.Pow(Math.Abs(velX), 0.8)));
            double a = 1 - Math.Pow(Math.E, -0.001 * Math.Abs(velX));
            if (velX < 0) headDir = Direction.LEFT;
            if (velX > 0) headDir = Direction.RIGHT;
            playerRenderInfo = new PlayerRenderInfo(posX, posY,
                headDir,
                (float)(a * -0.6 * Math.Sin(t)),
                (float)(a * 0.6 * Math.Sin(t)),
                (float)(a * 0.8 * Math.Sin(t)),
                (float)(a * -0.8 * Math.Sin(t)));
        }

        public Player(int x, int y) : base(PLAYER_WIDTH, PLAYER_HEIGHT, x, y, 0, 0)
        {
            //Generate the player
            type = "Player";
            InitPlayer();
        }

        public Player(JsonToken token) : base(token, PLAYER_WIDTH, PLAYER_HEIGHT, new SolidColorBrush(Colors.Red))
        {
            InitPlayer();
        }

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
                        //TODO calculate direction with mouseoffset
                        /*double xOffset = mousePosX - posX - 450;
                        double yOffset = mousePosY - posY;
                        double n = Math.Sqrt(xOffset * xOffset + yOffset * yOffset);
                        double xDir = xOffset / n;
                        double yDir = yOffset / n;

                        ItemEntity itemEntity = new ItemEntity(item, item.tag, posX + 500 - ItemEntity.itemSizeX / 2, posY, (int)(15000 * xDir) + velX, (int)(20000 * yDir) + velY);
                        Game.world.AddEntity(itemEntity);
                        thrown = true;
                        selectedSlot.Remove(1);
                        selectedSlot.inventory.UpdateHotbar();*/
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

            bool changed = false;

            changed = changed || pressedLeft != KeyBinds.pressed[KeyBinds.MOVE_LEFT] && Screen.allowIngameInputs;
            changed = changed || pressedRight != KeyBinds.pressed[KeyBinds.MOVE_RIGHT] && Screen.allowIngameInputs;
            changed = changed || pressedUp != KeyBinds.pressed[KeyBinds.JUMP] && Screen.allowIngameInputs;
            changed = changed || pressedSneak != KeyBinds.pressed[KeyBinds.SNEAK] && Screen.allowIngameInputs;
            changed = changed || pressedSprint != KeyBinds.pressed[KeyBinds.SPRINT] && Screen.allowIngameInputs;
            changed = changed || pressedThrow != KeyBinds.pressed[KeyBinds.THROW_ITEM] && Screen.allowIngameInputs;

            pressedLeft = KeyBinds.pressed[KeyBinds.MOVE_LEFT] && Screen.allowIngameInputs;
            pressedRight = KeyBinds.pressed[KeyBinds.MOVE_RIGHT] && Screen.allowIngameInputs;
            pressedUp = KeyBinds.pressed[KeyBinds.JUMP] && Screen.allowIngameInputs;
            pressedSneak = KeyBinds.pressed[KeyBinds.SNEAK] && Screen.allowIngameInputs;
            pressedSprint = KeyBinds.pressed[KeyBinds.SPRINT] && Screen.allowIngameInputs;
            pressedThrow = KeyBinds.pressed[KeyBinds.THROW_ITEM] && Screen.allowIngameInputs;

            if (changed)
            {
                NetworkHandler.SendData(MultiplayerPacketType.PRESSED_CHANGE, Game.world.player.id.ToString(), pressedLeft.ToString(), pressedRight.ToString(), pressedUp.ToString(), pressedSneak.ToString(), pressedSprint.ToString());
            }

            /* Too laggy, needs a rework
            //will get synced
            //doesnt get synced
            bool newPressedThrow = InputHandler.pressedKeys.Contains(Settings.cThrowItem);

            bool newPressedLeft = InputHandler.pressedKeys.Contains(Settings.cMoveLeft);
            bool newPressedRight = InputHandler.pressedKeys.Contains(Settings.cMoveRight);
            bool newPressedUp = InputHandler.pressedKeys.Contains(Settings.cJump);
            bool newPressedSneak = InputHandler.pressedKeys.Contains(Settings.cSneak);
            bool newPressedSprint = InputHandler.pressedKeys.Contains(Settings.cSprint);

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
            }*/

            UpdateHeadPosition();

        }

        public void UpdateHeadPosition()
        {
            //Update player head position based on the direction he's looking
            if (pressedRight)
            {
                directionScale = -1;
            }
            else if (pressedLeft)
            {
                directionScale = 1;
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

            Log.Write($"Created player at position x{posX} y{posY}.", LogType.ENTITIES, LogLevel.INFO);

            //Add initial debug menu lines
            DebugMenu.AddLine(DebugMenu.Section.PLAYER, "Player Stats:");
            DebugMenu.AddLine(DebugMenu.Section.PLAYER, "health");
            DebugMenu.AddLine(DebugMenu.Section.PLAYER, "posX");
            DebugMenu.AddLine(DebugMenu.Section.PLAYER, "posY");
            DebugMenu.AddLine(DebugMenu.Section.PLAYER, "velX");
            DebugMenu.AddLine(DebugMenu.Section.PLAYER, "velY");
            DebugMenu.AddLine(DebugMenu.Section.PLAYER, "blockPosX");
            DebugMenu.AddLine(DebugMenu.Section.PLAYER, "blockPosY");
            DebugMenu.AddLine(DebugMenu.Section.PLAYER, "touchingWater");
            DebugMenu.AddLine(DebugMenu.Section.PLAYER, "breathing");

            //Setup health bar
            healthBar = new HealthBar(10, 740);

            if (Game.world.gamemode == Gamemode.Creative)
            {
                healthBar.Hide();
            }

            texture.Background = new SolidColorBrush(Colors.Transparent);
        }

        public override void Die()
        {
            if (this == Game.world.player)
            {
                //Drop all items and clear the inventory
                foreach (InventorySlot slot in inventory.slotList)
                {
                    for (int i = 0; i < slot.amount; i++)
                    {
                        Drop(slot.itemId);
                    }
                    slot.Remove(slot.amount);
                }

                //Move the player to the spawn
                posX = Game.world.worldSpawnX;
                posY = Game.world.worldSpawnY;

                //Set the hp back to 10
                base.SetHP(10);

                NotificationHandler.ShowNotification("You died and were moved back to the world spawn.", 5000);
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
            UpdateAnimation(1.0 / tps);
        }

        protected override void OnUpdateEnd(int tps)
        {
            base.OnUpdateEnd(tps);
            DisplayDebugInformation();
        }

        public void SavePosition(string path)
        {
            Log.Write($"Saved player position to {path}.", LogType.ENTITIES, LogLevel.INFO);

            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Newtonsoft.Json.Formatting.Indented;
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
                writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                inventory.SaveToJson(writer);
                writer.WriteToFile($"{path}/player_inventory.json");
            }
        }

        public void DisplayDebugInformation()
        {
            DebugMenu.UpdateLine(DebugMenu.Section.PLAYER, "health", $"health={healthBar.value}");
            DebugMenu.UpdateLine(DebugMenu.Section.PLAYER, "posX", $"posX={posX}");
            DebugMenu.UpdateLine(DebugMenu.Section.PLAYER, "posY", $"posY={posY}");
            DebugMenu.UpdateLine(DebugMenu.Section.PLAYER, "velX", $"velX={velX}");
            DebugMenu.UpdateLine(DebugMenu.Section.PLAYER, "velY", $"velY={velY}");
            DebugMenu.UpdateLine(DebugMenu.Section.PLAYER, "blockPosX", $"blockPosX={(posX / 1000) % 8}");
            DebugMenu.UpdateLine(DebugMenu.Section.PLAYER, "blockPosY", $"blockPosY={posY / 1000}");
            DebugMenu.UpdateLine(DebugMenu.Section.PLAYER, "touchingWater", $"touchingWater={touchingStatus[TOUCHING_WATER]}");
            DebugMenu.UpdateLine(DebugMenu.Section.PLAYER, "breathing", $"breathing={breathing}");

        }
    }
}
