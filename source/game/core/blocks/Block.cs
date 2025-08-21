using SeeloewenCraft.game.core.crafting;
using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeeloewenCraft.game.core.blocks
{
    public abstract partial class Block : IDebugMenuTargetable
    {
        //references
        private List<string> tags = new List<string>();
        public Chunk chunk;
        public Inventory inventory;
        private Block foregroundBlock;
        public List<(int xOffset, int yOffset, string blockId)> connectedBlocks = new List<(int, int, string)>();
        public (int? xOffset, int? yOffset) baseBlock;
        public (LootTable lootTable, int minRolls, int maxRolls) lootTable;
        public CraftingHandler craftingHandler;

        //block type info
        public string name;
        public string id;
        public string itemId;
        public int breakTime = 150; //Milliseconds
        public int breakTimeTicks { get => breakTime * 12 / 100; } //TODO: 12 is legacy code from old timer, might need adaption
        public List<(string id, int min, int max)> drops = new List<(string, int, int)>(); //Can be empty, means that item id will drop x1
        public Collision collision;
        public Tool effectiveTool;
        public Material? effectiveMaterial;
        public (bool doesNeed, string tag) needsGround = (false, "");

        //variables
        public int xPos;
        public int yPos;
        public bool isBase = false;
        public bool isSolid = true;
        public bool isBackground = false;
        public int lightLevel = 7;
        public bool isAirLightSource;
        public bool isForeground = false;
        public (int x, int y) baseBlockOffset;
        public string state = "";
        public bool breaking;
        public double breakProgress;
        public bool hammering;
        public int hammerProgress;

        //Water
        public int waterLevel = 0; //constant depending on block type
        public bool isWaterSource = false; //variables
        public int waterSourceXPos = 0;
        public int waterSourceYPos = 0;
        public int waterSourceChunkIndex = 0;
        public bool hasWaterSource = false;

        //Temporary, only important during generation
        public bool isSurface = false;

        public Block(bool isBackground = false)
        {
            collision = new EntireBlockCollision();

            this.isBackground = isBackground;
        }

        #region lighting (dev)

        double lightTopRight = 1.0;
        double lightTopLeft = 1.0;
        double lightBotLeft = 1.0;
        double lightBotRight = 1.0;

        double lightFactor = 0.3;

        void ResetLight()
        {
            lightBotLeft = 0f;
            lightBotRight = 0f;
            lightTopLeft = 0f;
            lightTopRight = 0f;
        }


        void SetLightTopRight(double light)
        {
            double sqrt2 = Math.Sqrt(2.0);



        }


        #endregion

        #region tags

        public void WriteTag(string tag)
        { 
            if(!HasTag(tag)) tags.Add(tag);
        }

        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }

        public void RemoveTag(string tag)
        {
            tags.Remove(tag);
        }

        #endregion

        public bool IsLightSource() => HasTag(BlockTags.LIGHTSOURCE) || isAirLightSource;

        public void DoUpdate() //Gets run every tick
        {
            if (breaking)
            {
                hammering = false;
                UpdateBreaking();
            }
            else if (hammering)
            {
                breaking = false;
                UpdateHammering();
            }

            LightHandler.UpdateLighting(this);
        }

        public BlockRenderInfo GetBlockRenderInfo()
        {
            int breakAnimation = (int)(breaking || hammering ? (6 * breakProgress) / breakTimeTicks : 0);
            var info = new BlockRenderInfo(xPos + chunk.index * 8, yPos, id, state, isBackground, breakAnimation, hammering, lightLevel);
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

            if (HasTag(BlockTags.HAS_INVENTORY))
            {
                writer.WritePropertyName("inventory");
                inventory.SaveToJson(writer);
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
                    writer.WriteValue(craftingHandler.currentRecipe.id);
                    writer.WritePropertyName("recipe_progress");
                    writer.WriteValue(craftingHandler.recipeProgress);
                    writer.WritePropertyName("recipe_amount");
                    writer.WriteValue(craftingHandler.recipeAmount);
                }
                else if (craftingHandler.recipeClaimable)
                {
                    writer.WriteValue(true);
                    writer.WritePropertyName("recipe_id");
                    writer.WriteValue(craftingHandler.currentRecipe.id);
                    writer.WritePropertyName("recipe_progress");
                    writer.WriteValue(craftingHandler.currentRecipe.requiredTime - 100);
                    writer.WritePropertyName("recipe_amount");
                    writer.WriteValue(craftingHandler.recipeAmount);
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

            if (block.HasTag(BlockTags.HAS_INVENTORY))
            {
                JsonToken invToken = blockToken.GetToken("/inventory");
                Inventory inventory = Inventory.LoadFromJson(invToken, false);

                inventory.block = block;
                block.inventory = inventory;
                Game.world.inventoryList.Add(block.inventory);
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
                    CraftingRecipe recipe = CraftingHandler.GetRecipe(blockToken.GetString("/recipe_id"));
                    int progress = blockToken.GetInt("/recipe_progress");
                    int amount = blockToken.GetInt("/recipe_amount");
                    block.craftingHandler.currentRecipe = recipe;
                    block.craftingHandler.recipeAmount = amount;
                    block.craftingHandler.BeginCrafting(recipe, false);
                    block.craftingHandler.recipeProgress = progress;
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



        public bool IsCollidingWithPlayer(int x, int y) //TODO: Make work again
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

            Block blockAbove = GetBlockAbove();
            if (blockAbove != null && blockAbove.HasTag(BlockTags.CAN_FALL))
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
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"isLightSource");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"isBase", $"{isBase}");
            //DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"isSurface", $"{isSurface}");
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
                string s = tags.Count != 0 ? $"{tags[0]}" : "None";
                for (int i = 1; i < tags.Count; i++)
                {
                    s += ";" + tags[i];
                }
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, "Tags", s);

            }
        }


        public virtual void UpdateDebugMenu()
        {
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "isSolid", $"{isSolid} ");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "isBackground", $"{isBackground} ");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "lightLevel", $"{lightLevel} ");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, $"isLightSource", $"{IsLightSource()}");
        }


        #endregion

        public static List<Block> GetBlocksInRange(Block block, int range)
        {
            List<Block> blocksInRange = new List<Block>();

            //Gets all blocks above 
            for (int yOffset = 0; yOffset < range; yOffset++)
            {
                for (int xOffset = -yOffset; xOffset <= yOffset; xOffset++)
                {
                    blocksInRange.Add(GetBlockFromOffset(block, xOffset, range - yOffset));
                    blocksInRange.Add(GetBlockFromOffset(block, xOffset, -range + yOffset));
                }
            }

            for (int xOffset = 1; xOffset < range + 1; xOffset++)
            {
                blocksInRange.Add(GetBlockFromOffset(block, xOffset, 0));
                blocksInRange.Add(GetBlockFromOffset(block, -xOffset, 0));
            }

            return blocksInRange;
        }

        private static Block GetBlockFromOffset(Block block, int xOffset, int yOffset)
        {
            //Get the new block based from the offset from this block

            //If total y is above 74 or below 0, there isn't any available block
            if (yOffset + block.yPos < 0 || yOffset + block.yPos > 74)
            {
                return null;
            }

            //If total x is below 0, get the chunk to the left
            if (block.xPos + xOffset < 0)
            {
                Chunk chunk = Game.world.GetLoadedChunk(block.chunk.index - 1);

                if (chunk != null)
                {
                    return chunk.blockList.Get(block.xPos + xOffset + 8, block.yPos + yOffset);
                }
                else
                {
                    return null;
                }
            }
            //If total x is above 7, get the chunk to the right
            else if (block.xPos + xOffset > 7)
            {
                Chunk chunk = Game.world.GetLoadedChunk(block.chunk.index + 1);

                if (chunk != null)
                {
                    return chunk.blockList.Get(block.xPos + xOffset - 8, block.yPos + yOffset);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //Else just get the block from the current chunk
                return block.chunk.blockList.Get(block.xPos + xOffset, block.yPos + yOffset);
            }
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
            if (HasTag(BlockTags.DOESNT_DROP)) return;

            if ((Game.world.player.inventory.GetSelectedItem() is ToolItem tool && tool.type == effectiveTool && ToolIsCorrectMaterial(tool.material) && HasTag(BlockTags.TOOL_SPECIFIC) || !HasTag(BlockTags.TOOL_SPECIFIC)))
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
                    if (!foregroundBlock.HasTag(BlockTags.UNBREAKABLE) || skipBreakableCheck)
                    {
                        if (dropItems)
                        {
                            foregroundBlock.Drop();

                            if (foregroundBlock.HasTag(BlockTags.HAS_INVENTORY))
                            {
                                foregroundBlock.inventory.Drop((xPos + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, yPos * 1000 + 500 - ItemEntity.itemSizeY / 2);
                            }
                        }

                        RemoveForegroundBlock();

                        Block blockAbove = GetBlockAbove();
                        if (blockAbove.HasTag(BlockTags.CAN_FALL))
                        {
                            blockAbove.BreakBlock(true, true, false);
                            Game.world.AddEntity(new FallingBlockEntity(xPos + 8 * chunk.index, yPos, id));
                        }
                    }
                }
                //If it has no foreground block, check if the normal block is breakable
                else if (!HasTag(BlockTags.UNBREAKABLE) || skipBreakableCheck)
                {
                    //Remove the block from the chunks blocklist and add an airblock
                    Block block = new AirBlock(false);
                    SetBlock(block);

                    if (dropItems)
                    {
                        Drop();

                        if (HasTag(BlockTags.HAS_INVENTORY))
                        {
                            inventory.Drop((xPos + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, yPos * 1000 + 500 - ItemEntity.itemSizeY / 2);
                        }
                    }

                    Block blockAbove = GetBlockAbove();
                    if (blockAbove.HasTag(BlockTags.CAN_FALL))
                    {
                        blockAbove.BreakBlock(true, true, false);
                        Game.world.AddEntity(new FallingBlockEntity(xPos + 8 * chunk.index, yPos - 1, blockAbove.id));
                    }
                }
            }
        }

        public void SetBlock(Block block)
        {
            //Add the block to the chunk
            chunk.SetBlock(block, xPos, yPos);

            if (block.isBackground)
            {
                block.MoveToBackground();
            }
            else
            {
                block.MoveToNormal();
            }

            Block blockBelow = GetBlockBelow();
            if (block.HasTag(BlockTags.CAN_FALL)
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
            if (chunk == null) return null;

            return chunk.GetBlock(xPos, yPos + 1);
        }

        public Block GetBlockAbove()
        {
            if (chunk == null) return null;

            return chunk.GetBlock(xPos, yPos - 1);
        }

        public Block GetBlockRight()
        {
            if (chunk == null) return null;

            return chunk.GetBlock(xPos + 1, yPos);
        }

        public Block GetBlockLeft()
        {
            if (chunk == null) return null;

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
                inventory.AddItem(item.id, 1, item.tag);
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
                inventory.AddItem(item.id, 1, item.tag);
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

        public void SetCoords(int xPos, int yPos, Chunk chunk) //TODO: This seems sketchy, rework it
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
            hammering = false;
            breaking = false;
        }

        public void HandleMouseLeftDown()
        {
            breakProgress = 0;
            breaking = true;
        }

        public void HandleMouseLeftUp()
        {
            breaking = false;
        }

        public void HandleMouseRightDown()
        {
            breakProgress = 0;

            if (Game.world.player.inventory.GetSelectedItem() is ToolItem tool && tool.type == Tool.Hammer)
            {
                hammering = true;
            }
            else
            {
                Game.world.clickHandler.DoRightClick(this);
            }
        }

        public void HandleMouseRightUp()
        {
            hammering = false;
        }

        private void UpdateBreaking()
        {
            if (!IsInRange() || HasTag(BlockTags.UNBREAKABLE)) return;

            Block effectiveBlock = foregroundBlock ?? this; //The block which the break get's performed on

            if (Game.world.gamemode == Gamemode.Creative || breakTime == 0)
            {
                //Instantly perform the break when in creative or when time is 0
                Game.world.clickHandler.DoLeftClick(this);
            }
            else
            {
                //Default breakpower to 1. If the right tool is selected, apply that breakpower
                double breakPower = 1;
                if (Game.world.player.inventory.GetSelectedItem() is ToolItem tool && effectiveBlock.effectiveTool == tool.type && effectiveBlock.ToolIsCorrectMaterial(tool.material))
                {
                    breakPower = tool.breakPower == 0 ? 1 : tool.breakPower;
                }

                effectiveBlock.breakProgress += 1 * breakPower;

                if (effectiveBlock.breakProgress >= effectiveBlock.breakTimeTicks) Game.world.clickHandler.DoLeftClick(effectiveBlock);
            }
        }

        public void UpdateHammering()
        {
            if (Game.world.player.inventory.GetSelectedItem() is ToolItem tool && tool.type == Tool.Hammer)
            {
                //If the player holds a hammer, is in gamemode survival, the block is in range and doesn't have a foreground block
                if (IsInRange() && foregroundBlock == null && !HasTag(BlockTags.CANT_BE_BACKGROUND))
                {
                    //Get the break power from the selected tool
                    double breakPower = tool.breakPower == 0 ? 1 : tool.breakPower;
                    breakProgress += 1 * breakPower;

                    if (breakProgress >= breakTimeTicks || Game.world.gamemode == Gamemode.Creative)
                    {
                        Game.world.clickHandler.DoRightClick(this);
                        hammering = false;
                    }
                }
            }
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