using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;
using System;
using SeeloewenCraft.entity;
using System.Text;
using System.Runtime.CompilerServices;
using SeeloewenCraft.game.ui;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;

namespace SeeloewenCraft
{
    public abstract partial class Block : IDebugMenuTargetable
    {
        //references
        private HighPrecisionTimer.MultimediaTimer tmrBreak = new HighPrecisionTimer.MultimediaTimer();
        private HighPrecisionTimer.MultimediaTimer tmrHammer = new HighPrecisionTimer.MultimediaTimer();
        public List<string> tags = new List<string>();
        public ImageBrush image = new ImageBrush();
        public Chunk chunk;
        public Inventory blockInventory;
        private Block foregroundBlock;
        public List<(int xOffset, int yOffset, string blockId)> connectedBlocks = new List<(int, int, string)>();
        public (int? xOffset, int? yOffset) baseBlock;
        public (LootTable lootTable, int minRolls, int maxRolls) lootTable;
        public Gui gui;
        public CraftingHandler craftingHandler;

        //block type info
        public string name;
        public string id;
        public string itemId;
        public bool canBeMovedToBackground = true;
        public bool isReplacable = false;
        public bool isBreakable = true;
        public bool hasInventory = false;
        public bool isLightSource = false;
        public bool isBase = false;
        public bool hasRightClickAction = false;
        public bool dropsOnWrongTool = true;
        public int breakTime = 150;
        public List<(string id, int min, int max)> drops = new List<(string, int, int)>(); //Can be empty, means that item id will drop
        public Collision collision;
        public Tool effectiveTool;
        public Material? effectiveMaterial;
        public (bool doesNeed, string tag) needsGround = (false, "");
        public bool willFall = false;
        public bool doesntDrop = false;

        //variables
        public int xPos;
        public int yPos;
        public bool isSolid = true;
        public bool isBackground = false;
        public double lightLevel;
        public bool isForeground = false;
        public int rangeToNearestLightSource = int.MaxValue;
        public bool hasAirLightSource;
        public int xOffset;
        public int yOffset;
        public string state = "";
        public bool breaking;
        public int breakProgress;

        //Water
        public int waterLevel = 0; //constant depending on block type
        public bool isWaterSource = false; //variables
        public int waterSourceXPos = 0;
        public int waterSourceYPos = 0;
        public int waterSourceChunkIndex = 0;
        public bool hasWaterSource = false;

        //Temporary, only important during generation
        public bool isSurface = false;

        //-- Constructor --//

        public Block(bool isBackground)
        {
            collision = new EntireBlockCollision();

            //Set the attributes
            this.isBackground = isBackground;

            tmrBreak.Elapsed += tmrBreak_Tick;
            tmrHammer.Elapsed += tmrHammer_Tick;
        }

        //-- Custom Methods --//

        public BlockRenderInfo GetBlockRenderInfo()
        {
            if (this is FurnaceBlock)
            {
                Debug.Assert(false);
            }
            var info = new BlockRenderInfo(xPos + chunk.index * 8, yPos, id, state, isBackground);
            if (foregroundBlock != null) info.AddForegroundBlock(foregroundBlock.id, foregroundBlock.state);
            return info;
        }

        public virtual void Init(string name, string id, int breakTime, string? itemId, Tool effectiveTool)
        {
            this.name = name;
            this.id = id;
            this.breakTime = breakTime;
            this.itemId = itemId;
            this.effectiveTool = effectiveTool;
        }

        public virtual bool[] CheckTouch(int startX, int startY, int endX, int endY)
        {
            if (isBackground && foregroundBlock != null)
            {
                return foregroundBlock.CheckTouch(startX, startY, endX, endY);
            }
            bool[] touchingStatus = new bool[Entity.TOUCHING_STATUS_COUNT];
            touchingStatus[Entity.TOUCHING_AIR] = true;
            return touchingStatus;
        }

        public virtual (bool, int) CheckCollision(Direction direction, int startX, int endX, int startY, int endY)
        {
            startX -= (xPos + chunk.index * 8) * 1000;
            endX -= (xPos + chunk.index * 8) * 1000;
            startY -= yPos * 1000;
            endY -= yPos * 1000;

            if (!isSolid || isBackground)
            {
                if (foregroundBlock != null && foregroundBlock.isSolid)
                {
                    return foregroundBlock.collision.CheckCollision(direction, startX, endX, startY, endY);
                }
                else
                {
                    return (false, 0);
                }
            }
            else
            {
                return collision.CheckCollision(direction, startX, endX, startY, endY);
            }
        }


        public virtual void SaveToJson(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(id);

            writer.WritePropertyName("pos_x");
            writer.WriteValue(xPos);

            writer.WritePropertyName("pos_y");
            writer.WriteValue(yPos);

            writer.WritePropertyName("is_in_background");
            writer.WriteValue(isBackground);

            if (hasInventory)
            {
                writer.WritePropertyName("inventory");
                blockInventory.SaveToJson(writer);
            }

            if (foregroundBlock != null)
            {
                writer.WritePropertyName("foreground_block");
                foregroundBlock.SaveToJson(writer);
            }

            if (connectedBlocks.Count > 0)
            {
                writer.WritePropertyName("connected_blocks");
                writer.WriteStartArray();

                foreach (var block in connectedBlocks)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("x_offset");
                    writer.WriteValue(block.xOffset);
                    writer.WritePropertyName("y_offset");
                    writer.WriteValue(block.yOffset);
                    writer.WritePropertyName("block_id");
                    writer.WriteValue(block.blockId);
                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
            }

            if (baseBlock.xOffset != null && baseBlock.yOffset != null)
            {
                writer.WritePropertyName("baseblock_x_offset");
                writer.WriteValue(baseBlock.xOffset);
                writer.WritePropertyName("baseblock_y_offset");
                writer.WriteValue(baseBlock.yOffset);
            }

            if (tags.Contains("liquids/water"))
            {
                writer.WritePropertyName("water_is_source");
                writer.WriteValue(isWaterSource);
                writer.WritePropertyName("water_source_pos_x");
                writer.WriteValue(waterSourceXPos);
                writer.WritePropertyName("water_source_pos_y");
                writer.WriteValue(waterSourceYPos);
                writer.WritePropertyName("water_source_chunk_index");
                writer.WriteValue(waterSourceChunkIndex);
                writer.WritePropertyName("water_has_source");
                writer.WriteValue(hasWaterSource);
            }
            if (tags.Contains("workstation"))
            {
                writer.WritePropertyName("recipe_running");
                if (craftingHandler.recipeRunning)
                {
                    writer.WriteValue(true);
                    writer.WritePropertyName("recipe_id");
                    writer.WriteValue(craftingHandler.selectedRecipe.id);
                    writer.WritePropertyName("recipe_progress");
                    writer.WriteValue(craftingHandler.recipeProgress);
                    writer.WritePropertyName("recipe_amount");
                    writer.WriteValue(craftingHandler.amount);
                }
                else if (craftingHandler.recipeClaimable)
                {
                    writer.WriteValue(true);
                    writer.WritePropertyName("recipe_id");
                    writer.WriteValue(craftingHandler.selectedRecipe.id);
                    writer.WritePropertyName("recipe_progress");
                    writer.WriteValue(craftingHandler.selectedRecipe.requiredTime - 100);
                    writer.WritePropertyName("recipe_amount");
                    writer.WriteValue(craftingHandler.amount);
                }
                else
                {
                    writer.WriteValue(false);
                }
            }

            if (tags.Count > 0)
            {
                //Get all tags into a singular string so it can be saved as one attribute
                StringBuilder tagString = new StringBuilder();
                foreach (string tag in tags)
                {
                    tagString.Append(tag);
                    tagString.Append(';');
                }
                tagString.Remove(tagString.Length - 1, 1); //Remove the last char (seperator) as it would be misread

                writer.WritePropertyName("tags");
                writer.WriteValue(tagString.ToString());
            }

            if (this is CropBlock cBlock)
            {
                writer.WritePropertyName("growth_time");
                writer.WriteValue(cBlock.growthTime);
                writer.WritePropertyName("progress");
                writer.WriteValue(cBlock.progress);
            }

            writer.WriteEndObject();
        }

        static public Block LoadFromJson(JsonToken blockToken, Chunk chunk)
        {
            int posX = blockToken.GetInt("/pos_x");
            int posY = blockToken.GetInt("/pos_y");
            bool isInBackground = blockToken.GetBool("/is_in_background");
            string id = blockToken.GetString("/id");


            Block block = BlockRegister.GenerateBlock(id);

            if (block == null)
            {
                block = new AirBlock(false);
            }
            else
            {
                block.isBackground = isInBackground;
            }

            if (block.hasInventory)
            {
                JsonToken invToken = blockToken.GetToken("/inventory");
                Inventory inventory = Inventory.LoadFromJson(invToken, false);

                block.blockInventory = inventory;
                Game.world.inventoryList.Add(block.blockInventory);
                if (block.gui != null && block.gui.inventory != null)
                {
                    block.gui.inventory = block.blockInventory;
                    block.blockInventory.block = block;
                }
            }

            if (block.tags.Contains("liquids/water"))
            {
                block.waterSourceXPos = blockToken.GetInt("/water_source_pos_x");
                block.waterSourceYPos = blockToken.GetInt("/water_source_pos_y");
                block.isWaterSource = blockToken.GetBool("/water_is_source");
                block.waterSourceChunkIndex = blockToken.GetInt("/water_source_chunk_index");
                block.hasWaterSource = blockToken.GetBool("/water_has_source");
            }

            if (block.tags.Contains("workstation"))
            {
                bool recipeRunning = blockToken.GetBool("/recipe_running");

                if (recipeRunning)
                {
                    CraftingRecipe recipe = block.craftingHandler.GetRecipe(blockToken.GetString("/recipe_id"));
                    int progress = blockToken.GetInt("/recipe_progress");
                    int amount = blockToken.GetInt("/recipe_amount");
                    block.craftingHandler.selectedRecipe = recipe;
                    block.craftingHandler.amount = amount;
                    block.craftingHandler.Craft(recipe, false);
                    block.craftingHandler.recipeProgress = progress;
                    block.craftingHandler.pbCrafting.Value = progress;
                    block.craftingHandler.pbCraftingBlock.Value = progress;
                }

            }

            if (blockToken.ContainsKey("foreground_block"))
            {
                block.foregroundBlock = Block.LoadFromJson(blockToken.GetToken("/foreground_block"), chunk);
            }

            if (blockToken.ContainsKey("connected_blocks"))
            {
                JsonToken conArrayToken = blockToken.GetToken("/connected_blocks");

                for (int i = 0; i < conArrayToken.GetArrayLength(); i++)
                {
                    JsonToken conBlockToken = conArrayToken.GetToken($"/{i}");

                    int xOffset = conBlockToken.GetInt("/x_offset");
                    int yOffset = conBlockToken.GetInt("/y_offset");
                    string blockId = conBlockToken.GetString("/block_id");

                    block.connectedBlocks.Add((xOffset, yOffset, blockId));
                }
            }

            if (blockToken.ContainsKey("baseblock_x_offset") && blockToken.ContainsKey("baseblock_y_offset"))
            {
                block.baseBlock = (blockToken.GetInt("/baseblock_x_offset"), blockToken.GetInt("/baseblock_y_offset"));
            }

            if (blockToken.ContainsKey("tags"))
            {
                //Remove tags that were added automatically on creation
                block.tags.Clear();

                string[] tagSplit = blockToken.GetString("/tags").Split(';');
                foreach (string tag in tagSplit)
                {
                    block.tags.Add(tag);
                }
            }

            if (blockToken.ContainsKey("growth_time") && blockToken.ContainsKey("progress"))
            {
                if (block is CropBlock cBlock)
                {
                    cBlock.progress = blockToken.GetInt("/progress");
                    cBlock.growthTime = blockToken.GetInt("/growth_time");
                    cBlock.UpdateProgress(0); //Call update to load the correct state of the block, if needed
                }
            }

            //Set block stats
            block.xPos = posX;
            block.yPos = posY;
            block.chunk = chunk;

            return block;
        }



        public bool IsCollidingWithPlayer()
        {
            /*if (element is Canvas)
            {
                //Check for collision
                if (//Game.world.wndGame.GetRectangle(Game.world.player.texture).IntersectsWith(//Game.world.wndGame.GetRectangle(blockContainer.cvsBlock)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }*/
            return false;
        }

        public void MoveToBackground()
        {
            isBackground = true;


            Block blockAbove = GetBlockFromOffset(0, -1);
            if (blockAbove != null && blockAbove.willFall)
            {
                blockAbove.BreakBlock(true, true, false);
                Game.world.AddEntity(new FallingBlockEntity(xPos + 8 * chunk.index, yPos - 1, blockAbove.id));
            }
        }

        public void MoveToNormal()
        {
            isBackground = false;

        }

        public bool IsInRange()
        {
            Block playerBlock = Game.world.GetBlock(Game.world.player.posX / 1000, (Game.world.player.posY / 1000) + 1);
            if (playerBlock != null)
            {
                return (GetXRangeToBlock(playerBlock) < 5 && GetYRangeToBlock(playerBlock) < 5);
            }
            else
            {
                return false;
            }
        }

        public bool CanStayOnBlockBelow(Block block, Block blockBelow)
        {
            if (block != null && blockBelow != null && block.needsGround.doesNeed)
            {
                //If the block doesn't need a specific ground block (besides being solid) or the ground block has the needed tag, the block can stay
                if ((block.needsGround.tag == "" && blockBelow.isSolid)
                 || blockBelow.tags.Contains(block.needsGround.tag))
                {
                    return true;
                }
            }

            return false;
        }


        public void RemoveForegroundBlock()
        {
            foregroundBlock = null;
        }

        public Block GetForegroundBlock()
        {
            return foregroundBlock;
        }

        public Item GetItem()
        {
            //If the item has an id, generate an item and return it
            if (!string.IsNullOrEmpty(itemId))
            {
                return ItemRegister.GenerateItem(itemId);
            }

            return null;
        }


        public virtual void RightClickAction()
        {
            return;
        }

        #region Debug menu implementation



        public virtual void AddDebugMenu()
        {
            //Show the debug information for the block in debug menu
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "id", $"{id}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "name", $"{name}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "x", $"{xPos + chunk.index * 8}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "y", $"{yPos}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "x;chunk", $"{xPos};{chunk.index}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "isSolid");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "isBackground");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"lightLevel");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"isLightSource", $"{isLightSource}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"rangeToNearestLightSource");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"hasRightClickAction", $"{hasRightClickAction}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"hasInventory", $"{hasInventory}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"isBase", $"{isBase}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"isSurface", $"{isSurface}");
            if (foregroundBlock != null)
            {
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"foregroundBlock", $"{foregroundBlock.id}");
            }
            if (GetBaseBlock() != null)
            {
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"baseBlock", $"{GetBaseBlock().id} [{xPos + baseBlock.xOffset}|y{yPos + baseBlock.yOffset}]");
            }

            if (tags.Count > 0)
            {
                string s = tags.Count != 0 ? tags[0] : "";
                for (int i = 1; i < tags.Count; i++)
                {
                    s += "; " + tags[i];
                }
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, "Tags", s);

            }
        }


        public virtual void UpdateDebugMenu()
        {
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "isSolid", $"{isSolid} ");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "isBackground", $"{isBackground} ");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "lightLevel", $"{lightLevel} ");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "rangeToNearestLightSource", $"{rangeToNearestLightSource} ");
        }


        #endregion

        public List<Block> GetBlocksInRange(int range)
        {
            List<Block> blocksInRange = new List<Block>();

            //Gets all blocks above 
            for (int yOffset = 0; yOffset < range; yOffset++)
            {
                for (int xOffset = -yOffset; xOffset <= yOffset; xOffset++)
                {
                    blocksInRange.Add(GetBlockFromOffset(xOffset, range - yOffset));
                    blocksInRange.Add(GetBlockFromOffset(xOffset, -range + yOffset));
                }
            }

            for (int xOffset = 1; xOffset < range + 1; xOffset++)
            {
                blocksInRange.Add(GetBlockFromOffset(xOffset, 0));
                blocksInRange.Add(GetBlockFromOffset(-xOffset, 0));
            }

            return blocksInRange;
        }

        public int GetRangeToBlock(Block block)
        {
            int xDiff = block.xPos + block.chunk.index * 8 - xPos - chunk.index * 8;
            return Math.Abs(xDiff) + Math.Abs(block.yPos - yPos);
        }

        public int GetXRangeToBlock(Block block)
        {
            int xDiff = block.xPos + block.chunk.index * 8 - xPos - chunk.index * 8;
            return Math.Abs(xDiff);
        }

        public int GetYRangeToBlock(Block block)
        {
            int yDiff = block.yPos - yPos;
            return Math.Abs(yDiff);
        }

        protected virtual void Drop()
        {
            //Get the block that should drop

            if (doesntDrop)
            {
                return;
            }

            if ((Game.world.player.inventory.GetSelectedItem() is ToolItem tool && tool.type == effectiveTool && ToolIsCorrectMaterial(tool.material) && !dropsOnWrongTool) || dropsOnWrongTool)
            {
                //If the block has a loot table, roll an entry and give the items to player
                if (lootTable.lootTable != null)
                {
                    //Get the amount of times the item gets dropped
                    int rolls = Game.rnd.Next(lootTable.minRolls, lootTable.maxRolls + 1);

                    for (int i = 0; i < rolls; i++)
                    {
                        List<Item> items = lootTable.lootTable.RollEntry().RollItems();
                        foreach (Item item in items)
                        {
                            SpawnItem(item);
                        }
                    }
                }
                //If it has a specified droplist
                else if (drops.Count > 0)
                {
                    foreach (var entry in drops)
                    {
                        //Roll an amount of drops for each item and drop that amount of that item
                        int amount = Game.rnd.Next(entry.min, entry.max + 1);

                        for (int j = 0; j < amount; j++)
                        {
                            SpawnItem(ItemRegister.GenerateItem(entry.id));
                        }
                    }
                }
                //If it has only an item, only give that item
                else if (GetItem() != null)
                {
                    SpawnItem(GetItem());
                }
            }
        }

        private void SpawnItem(Item item)
        {
            if (item != null)
            {
                //Spawn the item entity in the world
                Game.world.AddEntity(new ItemEntity(item, item.tag, //item type
                    (xPos + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, //posX
                    yPos * 1000 + 500 - ItemEntity.itemSizeY / 2, //posY
                Game.rnd.Next(-6000, 6000), Game.rnd.Next(-15000, -10000))); //velX and velY 
            }
        }

        public void BreakBlock(bool skipRangeCheck, bool skipBreakableCheck, bool dropItems)
        {
            //Check if is in range
            if ((IsInRange() || skipRangeCheck))
            {
                //If it has a foreground block, check if that one is breakable
                if (foregroundBlock != null)
                {
                    if (foregroundBlock.isBreakable || skipBreakableCheck)
                    {
                        if (dropItems)
                        {
                            foregroundBlock.Drop();

                            if (foregroundBlock.hasInventory)
                            {
                                foregroundBlock.blockInventory.Drop((xPos + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, yPos * 1000 + 500 - ItemEntity.itemSizeY / 2);
                            }
                        }

                        RemoveForegroundBlock();

                        Block blockAbove = GetBlockFromOffset(0, -1);
                        if (blockAbove.willFall)
                        {
                            blockAbove.BreakBlock(true, true, false);
                            Game.world.AddEntity(new FallingBlockEntity(xPos + 8 * chunk.index, yPos, id));
                        }
                    }
                }
                //If it has no foreground block, check if the normal block is breakable
                else if (isBreakable || skipBreakableCheck)
                {
                    //Remove the block from the chunks blocklist and add an airblock
                    Block block = new AirBlock(false);
                    SetBlock(block);

                    if (dropItems)
                    {
                        Drop();

                        if (hasInventory)
                        {
                            blockInventory.Drop((xPos + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, yPos * 1000 + 500 - ItemEntity.itemSizeY / 2);
                        }
                    }

                    Block blockAbove = GetBlockFromOffset(0, -1);
                    if (blockAbove.willFall)
                    {
                        blockAbove.BreakBlock(true, true, false);
                        Game.world.AddEntity(new FallingBlockEntity(xPos + 8 * chunk.index, yPos - 1, blockAbove.id));
                    }
                }
            }
        }

        public void SetBlock(Block block)
        {
            //Show the progressbar based on if it's workstation or not
            if (craftingHandler != null && (craftingHandler.recipeRunning || craftingHandler.recipeClaimable))
            {
                craftingHandler.HideBlockProgressbar();
            }

            //If it's air, check if it should be a light source  
            if (block.id == "sc:air_block")
            {
                block.isLightSource = IsAirLightSource(block);
            }

            //Add the block to the chunk
            block.rangeToNearestLightSource = rangeToNearestLightSource;
            chunk.SetBlock(block, xPos, yPos);
            UpdateAirLightsources(block);

            if (block.isBackground)
            {
                block.MoveToBackground();
            }
            else
            {
                block.MoveToNormal();
            }

            Block blockBelow = block.GetBlockFromOffset(0, 1);
            if (block.willFall
                && (blockBelow is AirBlock || blockBelow is WaterBlock || blockBelow.isBackground))
            {
                block.BreakBlock(true, true, false);
                Game.world.AddEntity(new FallingBlockEntity(xPos + 8 * chunk.index, yPos, block.id));
            }

            chunk.GetBlock(xPos, yPos).AddDebugMenu();

            //Send the data on the network if it's multiplayer
            NetworkHandler.SendData(MultiplayerPacketType.SET_BLOCK, block.id, chunk.index.ToString(), block.xPos.ToString(), block.yPos.ToString());
        }

        public Block GetBlockBelow()
        {
            return chunk.GetBlock(xPos, yPos + 1);
        }

        public Block GetBlockAbove()
        {
            return chunk.GetBlock(xPos, yPos - 1);
        }

        public Block GetBlockRight()
        {
            return chunk.GetBlock(xPos + 1, yPos);
        }

        public Block GetBlockLeft()
        {
            return chunk.GetBlock(xPos - 1, yPos);
        }

        public void SetForegroundBlock(Block block)
        {
            if (block == null)
            {
                return;
            }

            foregroundBlock = block;
            block.isForeground = true;
            block.xPos = xPos;
            block.yPos = yPos;
            block.chunk = chunk;

        }
        

        public void InsertLootTable(LootTable lootTable, int amount)
        {
            //Get all loot into a list
            List<Item> loot = new List<Item>();
            for (int i = 0; i < amount; i++)
            {
                loot.AddRange(lootTable.RollEntry().RollItems());
            }

            //Put the loot into the inventory
            foreach (Item item in loot)
            {
                blockInventory.AddItem(item.id, 1, item.tag);
            }
        }

        public void InsertLootTable(LootTable lootTable, int amount, Random rnd)
        {
            //Get all loot into a list
            List<Item> loot = new List<Item>();
            for (int i = 0; i < amount; i++)
            {
                loot.AddRange(lootTable.RollEntry(rnd).RollItems(rnd));
            }

            //Put the loot into the inventory
            foreach (Item item in loot)
            {
                blockInventory.AddItem(item.id, 1, item.tag);
            }
        }

        public bool ConBlocksHaveSpace(Block baseBlock, bool isForeground)
        {
            if (!isForeground)
            {
                foreach (var conBlockInfo in baseBlock.connectedBlocks)
                {
                    //Goes through all connected blocks and checks whether the block at the location, that they should be placed, at is solid
                    Block block = Game.world.GetBlock(xPos + 8 * chunk.index + conBlockInfo.xOffset, yPos + conBlockInfo.yOffset);
                    if (block != null && (block.isSolid || block.isBackground))
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (isForeground)
            {
                foreach (var conBlockInfo in baseBlock.connectedBlocks)
                {
                    //Goes through all connected blocks and checks whether the block at the location, that they should be placed, has a foreground block
                    Block block = Game.world.GetBlock(xPos + 8 * chunk.index + conBlockInfo.xOffset, yPos + conBlockInfo.yOffset);
                    if (block != null && (block.foregroundBlock != null || !block.isBackground))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetCoords(int xPos, int yPos, Chunk chunk)
        {
            //Warning: Only sets coords inside blocks, not inside chunk/blocklist
            this.chunk = chunk;
            this.xPos = xPos;
            this.yPos = yPos;
        }

        public List<Block> GetConnectedBlocks(bool inForeground) //Assumes this is a base block
        {
            //Get all connected blocks from the given coordinates
            List<Block> connectedBlocks = new List<Block>();
            foreach (var entry in this.connectedBlocks)
            {
                if (!inForeground)
                {
                    connectedBlocks.Add(Game.world.GetBlock(xPos + 8 * chunk.index + entry.xOffset, yPos + entry.yOffset));
                }
                else
                {
                    connectedBlocks.Add(Game.world.GetBlock(xPos + 8 * chunk.index + entry.xOffset, yPos + entry.yOffset).foregroundBlock);
                }
            }

            return connectedBlocks;
        }

        public Block GetBaseBlock()
        {
            if (baseBlock.xOffset != null && baseBlock.yOffset != null)
            {
                return Game.world.GetBlock(xPos + chunk.index * 8 + (int)baseBlock.xOffset, yPos + (int)baseBlock.yOffset);
            }
            return null;
        }

        public void PlaceConnectedForegroundBlocks(Block baseBlock)
        {
            foreach (var conBlock in baseBlock.connectedBlocks)
            {
                //Place the connected block
                Block oldBlock = Game.world.GetBlock(xPos + 8 * chunk.index + conBlock.xOffset, yPos + conBlock.yOffset);
                Block newBlock = BlockRegister.GenerateBlock(conBlock.blockId);
                newBlock.baseBlock = (-conBlock.xOffset, -conBlock.yOffset);

                oldBlock.SetForegroundBlock(newBlock);
            }
        }

        public void PlaceConnectedBlocks(Block baseBlock)
        {
            foreach (var conBlock in baseBlock.connectedBlocks)
            {
                //Place the connected block
                Block oldBlock = Game.world.GetBlock(xPos + 8 * chunk.index + conBlock.xOffset, yPos + conBlock.yOffset);
                Block newBlock = BlockRegister.GenerateBlock(conBlock.blockId);
                newBlock.baseBlock = (-conBlock.xOffset, -conBlock.yOffset);

                oldBlock.SetBlock(newBlock);
            }
        }


        public void RegisterTool(Tool tool)
        {
            switch (tool)
            {
                case Tool.Hammer:
                    tags.Add("effectiveTools/hammer");
                    break;
                case Tool.Scythe:
                    tags.Add("effectiveTools/scythe");
                    break;
                case Tool.Shovel:
                    tags.Add("effectiveTools/shovel");
                    break;
                case Tool.Sword:
                    tags.Add("effectiveTools/sword");
                    break;
                case Tool.Pickaxe:
                    tags.Add("effectiveTools/pickaxe");
                    break;
                case Tool.Axe:
                    tags.Add("effectiveTools/axe");
                    break;
            }
        }

        public bool ToolIsCorrectMaterial(Material toolMaterial)
        {
            //Check if the effective material is supported by the tool
            if (effectiveMaterial != null)
            {
                switch (effectiveMaterial)
                {
                    case Material.Wood:
                        return toolMaterial == Material.Wood
                            || toolMaterial == Material.Stone
                            || toolMaterial == Material.Tin
                            || toolMaterial == Material.Iron
                            || toolMaterial == Material.Diamond;

                    case Material.Stone:
                        return toolMaterial == Material.Stone
                            || toolMaterial == Material.Tin
                            || toolMaterial == Material.Iron
                            || toolMaterial == Material.Diamond;

                    case Material.Tin:
                        return toolMaterial == Material.Tin
                            || toolMaterial == Material.Iron
                            || toolMaterial == Material.Diamond;

                    case Material.Iron:
                        return toolMaterial == Material.Iron
                            || toolMaterial == Material.Diamond;

                    case Material.Diamond:
                        return toolMaterial == Material.Diamond;

                    default:
                        return false;
                }
            }
            else
            {
                return true;
            }
        }

        //-- Event Handlers --//
        public void HandleMouseEnter()
        {

        }

        public void HandleMouseLeave()
        {
            //Stop a possible block modification progress
            tmrBreak.Stop();
            tmrHammer.Stop();

        }

        public void HandleMouseLeftDown()
        {
            //Stop possible hammer process
            tmrHammer.Stop();

            //If the block is in range, check if it has a foreground block or not and check if that block is breakable before starting the break animation
            if (IsInRange())
            {
                if (foregroundBlock != null)
                {
                    if (foregroundBlock.isBreakable)
                    {
                        if (Game.world.gamemode == Gamemode.Creative || foregroundBlock.breakTime == 0)
                        {
                            Game.world.clickHandler.DoLeftClick(this);
                        }
                        else
                        {
                            //Default breakpower to 1. If the right tool is selected, apply that breakpower
                            double breakPower = 1;
                            if (Game.world.player.inventory.GetSelectedItem() is ToolItem tool && foregroundBlock.effectiveTool == tool.type && ToolIsCorrectMaterial(tool.material))
                            {
                                breakPower = tool.breakPower;
                            }

                            tmrBreak.Interval = (int)(foregroundBlock.breakTime / breakPower);
                            tmrBreak.Start();
                        }
                    }
                }
                else
                {
                    if (isBreakable)
                    {
                        if (Game.world.gamemode == Gamemode.Creative || breakTime == 0)
                        {
                            Game.world.clickHandler.DoLeftClick(this);
                        }
                        else
                        {
                            //Default breakpower to 1. If the right tool is selected, apply that breakpower
                            double breakPower = 1;
                            if (Game.world.player.inventory.GetSelectedItem() is ToolItem tool && effectiveTool == tool.type && ToolIsCorrectMaterial(tool.material))
                            {
                                breakPower = tool.breakPower;
                            }

                            tmrBreak.Interval = (int)(breakTime / breakPower);
                            tmrBreak.Start();
                        }
                    }
                }
            }

        }

        public void HandleMouseLeftUp()
        {
            //Stop a possible block modification progress
            tmrBreak.Stop();
        }

        public void HandleMouseRightDown()
        {
            //Stop possible breaking process
            tmrBreak.Stop();

            if (Game.world.player.inventory.GetSelectedItem() != null && Game.world.player.inventory.GetSelectedItem() is ToolItem tool && tool.type == Tool.Hammer)
            {
                //If the player holds a hammer, is in gamemode survival, the block is in range and doesn't have a foreground block
                if (Game.world.gamemode == Gamemode.Survival && IsInRange() && foregroundBlock == null && canBeMovedToBackground)
                {
                    //Get the break power from the selected tool
                    double breakPower = tool.breakPower;

                    //Start the timer for the hammer
                    tmrHammer.Interval = (int)(breakTime / breakPower); //TO-DO: Include tool efficiency
                    tmrHammer.Start();
                    return;
                }
            }

            //If all of the checks above fail, handle it the normal way
            Game.world.clickHandler.DoRightClick(this);
        }

        public void HandleMouseRightUp()
        {
            //Stop a possible block modification progress
            tmrHammer.Stop();
        }


        private void tmrBreak_Tick(object sender, EventArgs e)
        { }//TODO

        private void tmrHammer_Tick(object sender, EventArgs e)
        {//TODO
        }

    }

    public enum Tool
{
    Pickaxe,
    Axe,
    Shovel,
    Scythe,
    Sword,
    Hammer,
    None
}

public enum Material
{
    Wood,
    Stone,
    Tin,
    Iron,
    Diamond
}
}