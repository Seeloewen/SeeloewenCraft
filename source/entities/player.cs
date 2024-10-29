using Newtonsoft.Json;
using SeeloewenCraft.gl_rendering;
using System;
using System.Windows.Media;

namespace SeeloewenCraft.entity
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
            t %= (float) (2 * Math.PI);
            //double a = Math.Max(0, 1 - 50 / (40+Math.Pow(Math.Abs(velX), 0.8)));
            double a = 1 - Math.Pow(Math.E, -0.001 * Math.Abs(velX));
            if (velX < 0) headDir = Direction.LEFT;
            if(velX > 0) headDir = Direction.RIGHT;
            playerRenderInfo = new PlayerRenderInfo(posX, posY,
                headDir,
                (float)(a * -0.6 * Math.Sin(t)),
                (float)(a * 0.6 * Math.Sin(t)),
                (float)(a*0.8*Math.Sin(t)),
                (float)(a*-0.8*Math.Sin(t)));
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

            bool changed = false;

            changed = changed || pressedLeft != Game.world.wndGame.pressedKeys.Contains(Settings.cMoveLeft);
            changed = changed || pressedRight != Game.world.wndGame.pressedKeys.Contains(Settings.cMoveLeft);
            changed = changed || pressedUp != Game.world.wndGame.pressedKeys.Contains(Settings.cMoveLeft);
            changed = changed || pressedSneak != Game.world.wndGame.pressedKeys.Contains(Settings.cMoveLeft);
            changed = changed || pressedSprint != Game.world.wndGame.pressedKeys.Contains(Settings.cMoveLeft);
            changed = changed || pressedThrow != Game.world.wndGame.pressedKeys.Contains(Settings.cMoveLeft);

            pressedLeft = Game.world.wndGame.pressedKeys.Contains(Settings.cMoveLeft);
            pressedRight = Game.world.wndGame.pressedKeys.Contains(Settings.cMoveRight);
            pressedUp = Game.world.wndGame.pressedKeys.Contains(Settings.cJump);
            pressedSneak = Game.world.wndGame.pressedKeys.Contains(Settings.cSneak);
            pressedSprint = Game.world.wndGame.pressedKeys.Contains(Settings.cSprint);
            pressedThrow = Game.world.wndGame.pressedKeys.Contains(Settings.cThrowItem);

            if (changed)
            {
                NetworkHandler.SendData(MultiplayerPacketType.PRESSED_CHANGE, Game.world.player.id.ToString(), pressedLeft.ToString(), pressedRight.ToString(), pressedUp.ToString(), pressedSneak.ToString(), pressedSprint.ToString());
            }

            /* Too laggy, needs a rework
            //will get synced
            //doesnt get synced
            bool newPressedThrow = Game.world.wndGame.pressedKeys.Contains(Settings.cThrowItem);

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
            }*/

            UpdateHeadPosition();

            //Do animation if necessary
            movingHorizontally = pressedLeft || pressedRight;
            DoMovementAnimation();
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

            ScaleTransform flipTransform = new ScaleTransform
            {
                ScaleX = directionScale,
                CenterX = cvsHead.ActualWidth / 2
            };

            cvsHead.RenderTransform = flipTransform;
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

            texture.Background = new SolidColorBrush(Colors.Transparent);
            InitAnimations();
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
            UpdateAnimation(1.0 / tps);
            DisplayDebugInformation();
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
