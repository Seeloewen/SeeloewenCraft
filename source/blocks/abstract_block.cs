using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;
using System;
using SeeloewenCraft.entity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text;
using System.Runtime.CompilerServices;
using SeeloewenCraft.gl_rendering;
using OpenTK.Graphics.OpenGL;

namespace SeeloewenCraft
{
    public abstract partial class Block
    {
        //references
        private HighPrecisionTimer.MultimediaTimer tmrBreak = new HighPrecisionTimer.MultimediaTimer();
        private HighPrecisionTimer.MultimediaTimer tmrHammer = new HighPrecisionTimer.MultimediaTimer();
        public List<string> tags = new List<string>();
        public ImageBrush image = new ImageBrush();
        public SealImage sImage;
        public BlockContainer blockContainer;
        public Chunk chunk;
        public Inventory blockInventory;
        private Block foregroundBlock;
        public List<(int xOffset, int yOffset, string blockId)> connectedBlocks = new List<(int, int, string)>();
        public (int? xOffset, int? yOffset) baseBlock;
        public LootTable lootTable;
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
        public int dropAmountMin = 1;
        public int dropAmountMax = 1;
        public Collision collision;
        public Tool effectiveTool;
        public Material? effectiveMaterial;
        public bool needsGround = false;
        public bool willFall;

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
        string state = "";

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
            var info = new BlockRenderInfo(xPos + chunk.index * 8, yPos, id, state, isBackground);
            if(foregroundBlock != null) info.AddForegroundBlock(foregroundBlock.id, foregroundBlock.state);
            return info;
        }

        public virtual void Init(string name, string id, int breakTime, string? itemId, Tool effectiveTool, SealImage sImage)
        {
            this.name = name;
            this.id = id;
            this.breakTime = breakTime;
            this.itemId = itemId;
            this.effectiveTool = effectiveTool;
            this.sImage = sImage;
            SetTexture();
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


        public void SetContainer(BlockContainer blockContainer)
        {
            this.blockContainer = blockContainer;
            this.blockContainer.cvsBlock.Background = image;
            this.blockContainer.cvsBlock.MouseLeftButtonDown += cvsBlock_MouseLeftButtonDown;
            this.blockContainer.cvsBlock.MouseLeftButtonUp += cvsBlock_MouseLeftButtonUp;
            this.blockContainer.cvsBlock.MouseRightButtonUp += cvsBlock_MouseRightButtonUp;
            this.blockContainer.cvsBlock.MouseRightButtonDown += cvsBlock_MouseRightButtonDown;
            this.blockContainer.cvsBlock.MouseEnter += cvsBlock_MouseEnter;
            this.blockContainer.cvsBlock.MouseLeave += cvsBlock_MouseLeave;

            //Add image and events to the container
            if (canBeMovedToBackground)
            {
                if (isBackground)
                {
                    MoveToBackground();
                }
                else
                {
                    MoveToNormal();
                }
            }
            else
            {
                this.blockContainer.HideDarkRectangle();
            }
        }

        public void RemoveHandlersFromContainer()
        {
            //Remove the events from the container
            if (blockContainer != null)
            {
                blockContainer.cvsBlock.MouseLeftButtonDown -= cvsBlock_MouseLeftButtonDown;
                blockContainer.cvsBlock.MouseLeftButtonUp -= cvsBlock_MouseLeftButtonUp;
                blockContainer.cvsBlock.MouseRightButtonUp -= cvsBlock_MouseRightButtonUp;
                blockContainer.cvsBlock.MouseRightButtonDown -= cvsBlock_MouseRightButtonDown;
                blockContainer.cvsBlock.MouseEnter -= cvsBlock_MouseEnter;
                blockContainer.cvsBlock.MouseLeave -= cvsBlock_MouseLeave;
            }
        }

        public bool IsCollidingWithPlayer(object element)
        {
            if (element is Canvas)
            {
                //Check for collision
                if (Game.world.wndGame.GetRectangle(Game.world.player.texture).IntersectsWith(Game.world.wndGame.GetRectangle(blockContainer.cvsBlock)))
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
            }
        }

        public void MoveToBackground()
        {
            isBackground = true;

            if (blockContainer != null)
            {
                blockContainer.ShowDarkRectangle();
            }

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

            if (blockContainer != null)
            {
                blockContainer.HideDarkRectangle();
            }
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



        public void RemoveForegroundBlock()
        {
            if (blockContainer != null)
            {
                //First unrender the block, then set the foreground block to null and finally update the lighting
                blockContainer.UnrenderForegroundBlock();
                foregroundBlock = null;
                blockContainer.UpdateLighting();
            }
            else
            {
                foregroundBlock = null;
            }
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

        public virtual void SetTexture()
        {
            image = sImage.GetTexture();
        }

        public virtual void RightClickAction(object sender)
        {
            return;
        }

        public virtual void ShowAdditionalDebugInfo()
        {
            return;
        }

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

        protected virtual void Drop(bool dropForeground)
        {
            //Get the block that should drop
            Block block = dropForeground ? foregroundBlock : this;

            if ((Game.world.player.inventory.GetSelectedItem() is ToolItem tool && tool.type == block.effectiveTool && ToolIsCorrectMaterial(tool.material) && !block.dropsOnWrongTool) || block.dropsOnWrongTool)
            {
                //Get the amount of times the item gets dropped
                int rolls = Game.rnd.Next(block.dropAmountMin, block.dropAmountMax + 1);

                for (int i = 0; i < rolls; i++)
                {
                    //If the block has a loot table, roll an entry and give the items to player
                    if (block.lootTable != null)
                    {
                        List<Item> items = block.lootTable.RollEntry().RollItems();
                        foreach (Item item in items)
                        {
                            Game.world.AddEntity(new ItemEntity(item, item.tag, //item type
                                (xPos + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, //posX
                                yPos * 1000 + 500 - ItemEntity.itemSizeY / 2, //posY
                                Game.rnd.Next(-6000, 6000), Game.rnd.Next(-15000, -10000))); //velX and velY 
                        }
                    }
                    //If it has only an item, only give that item
                    else if (block.GetItem() != null)
                    {
                        Item item = block.GetItem();

                        Game.world.AddEntity(new ItemEntity(item, item.tag, //item type
                                (xPos + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, //posX
                                yPos * 1000 + 500 - ItemEntity.itemSizeY / 2, //posY
                                Game.rnd.Next(-6000, 6000), Game.rnd.Next(-15000, -10000))); //velX and velY 
                    }
                }
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
                            Drop(true);

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
                        Drop(false);

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
            block.MoveToNormal();

            Block blockBelow = block.GetBlockFromOffset(0, 1);
            if (block.willFall
                && (blockBelow is AirBlock || blockBelow is WaterBlock || blockBelow.isBackground))
            {
                block.BreakBlock(true, true, false);
                Game.world.AddEntity(new FallingBlockEntity(xPos + 8 * chunk.index, yPos, block.id));
            }

            //Send the data on the network if it's multiplayer
            NetworkHandler.SendData(MultiplayerPacketType.SET_BLOCK, $"{block.id};{chunk.index};{block.xPos};{block.yPos}");
        }

        public Block GetBlockBelow()
        {
            return chunk.GetBlock(xPos, yPos + 1);
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

            if (blockContainer != null)
            {
                blockContainer.RenderForegroundBlock(block);
            }
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

        public void DisplayDebugInformation()
        {
            //Show the debug information for the block in debug menu
            if (Game.world.debugMenu.isEnabled)
            {
                Game.world.debugMenu.tblBlockStats.Text = "";
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, "Selected Block:");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"id={id}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"name={name}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"xPos={xPos}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"yPos={yPos}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"chunk={chunk.index}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"isSolid={isSolid}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"isBackground={isBackground}");
                if (foregroundBlock != null)
                {
                    Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"foregroundBlock={foregroundBlock.id}");
                }
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"lightLevel={lightLevel}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"isLightSource={isLightSource}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"rangeToNearestLightSource={rangeToNearestLightSource}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"hasRightClickAction={hasRightClickAction}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"hasInventory={hasInventory}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"isBase={isBase}");
                Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"isSurface={isSurface}");
                if (GetBaseBlock() != null)
                {
                    Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, $"baseBlock={GetBaseBlock().id} at x{xPos + baseBlock.xOffset} y{yPos + baseBlock.yOffset}");
                }

                //Try to show the additional debug information
                try
                {
                    ShowAdditionalDebugInfo();
                }
                catch (NotImplementedException)
                {
                    //No additional debug info to show
                }

                if (tags.Count > 0)
                {
                    Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, "Tags:");
                    foreach (string tag in tags)
                    {
                        Game.world.debugMenu.AddLine(Game.world.debugMenu.tblBlockStats, tag);
                    }
                }
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
        private void cvsBlock_MouseEnter(object sender, EventArgs e)
        {
            //Display the debug information of the block
            DisplayDebugInformation();

            //Checks if the block is in range and breakable
            if (IsInRange() == true)
            {
                //Show border that indicates that the block can be broken or placed a specific location
                blockContainer.bdrBlock.BorderThickness = new Thickness(1, 1, 1, 1);
                blockContainer.bdrBlock.BorderBrush = new SolidColorBrush(Colors.Black);
            }
        }

        private void cvsBlock_MouseLeave(object sender, EventArgs e)
        {
            //Remove the border from the block
            if (blockContainer != null)
            {
                blockContainer.bdrBlock.BorderThickness = new Thickness(0, 0, 0, 0);
                blockContainer.bdrBlock.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }

            //Stop a possible block modification progress
            tmrBreak.Stop();
            blockContainer.SetBreakState(0);
            tmrHammer.Stop();
            blockContainer.SetHammerState(0);
        }

        private void cvsBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                            Game.world.clickHandler.DoLeftClick(this, sender);
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
                            Game.world.clickHandler.DoLeftClick(this, sender);
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

        private void cvsBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Stop a possible block modification progress
            tmrBreak.Stop();
            blockContainer.SetBreakState(0);
        }

        private void cvsBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
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
            Game.world.clickHandler.DoRightClick(this, sender);
        }

        private void cvsBlock_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Stop a possible block modification progress
            tmrHammer.Stop();
            blockContainer.SetHammerState(0);
        }

        private void tmrBreak_Tick(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                //If the block is broken, drop it
                if (blockContainer.breakState == 5)
                {
                    Game.world.clickHandler.DoLeftClick(this, sender);
                    blockContainer.SetBreakState(0);
                    tmrBreak.Stop();
                    return;
                }

                //Increase the broken state
                blockContainer.SetBreakState(blockContainer.breakState + 1);
            }));
        }

        private void tmrHammer_Tick(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                //If the hammer is done, do right-click
                if (blockContainer.hammerState == 5)
                {
                    Game.world.clickHandler.DoRightClick(this, sender);
                    blockContainer.SetHammerState(0);
                    tmrHammer.Stop();
                    return;
                }

                //Increase the hammer state
                blockContainer.SetHammerState(blockContainer.hammerState + 1);
            }));
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
