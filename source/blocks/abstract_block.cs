using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;
using System;

namespace SeeloewenCraft
{
    public abstract class Block
    {
        //references
        private HighPrecisionTimer.MultimediaTimer tmrBreak = new HighPrecisionTimer.MultimediaTimer();
        private HighPrecisionTimer.MultimediaTimer tmrHammer = new HighPrecisionTimer.MultimediaTimer();
        public List<string> tags = new List<string>();
        public World world;
        public ImageBrush image = new ImageBrush();
        public BlockContainer blockContainer;
        public Chunk chunk;
        public Item item;
        public Inventory blockInventory;
        public Block foregroundBlock;
        public List<Block> connectedBlocks = new List<Block>();
        public Block baseBlock;
        public LootTable lootTable;
        public Gui gui;
        public CraftingHandler craftingHandler;
        private Random rnd;
        static int offset;

        //block type info
        public string name;
        public string id;
        public bool canBeMovedToBackground = true;
        public bool isReplacable = false;
        public bool isBreakable = true;
        public bool hasInventory = false;
        public bool isLightSource = false;
        public bool isBase = false;
        public bool hasRightClickAction = false;
        public int breakTime = 150;
        public Collision collision;

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

        public Block(World world, bool isBackground)
        {
            rnd = new Random(DateTime.Now.Millisecond + offset);
            offset++;

            collision = new EntireBlockCollision();

            //Set the attributes
            this.world = world;
            this.isBackground = isBackground;

            tmrBreak.Elapsed += tmrBreak_Tick;
            tmrHammer.Elapsed += tmrHammer_Tick;
        }

        //-- Custom Methods --//

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

            writer.WriteEndObject();
        }


        static public Block LoadFromJson(JsonToken blockToken, Chunk chunk, World world)
        {
            int posX = blockToken.GetInt("/pos_x");
            int posY = blockToken.GetInt("/pos_y");
            bool isInBackground = blockToken.GetBool("/is_in_background");
            string id = blockToken.GetString("/id");


            Block block = BlockRegister.GenerateBlock(id, world);

            if (block == null)
            {
                block = new AirBlock(world, false);
            }
            else
            {
                block.isBackground = isInBackground;
            }

            if (block.hasInventory)
            {
                JsonToken invToken = blockToken.GetToken("/inventory");
                Inventory inventory = Inventory.LoadFromJson(invToken, world);

                block.blockInventory = inventory;
                if (block.gui != null && block.gui.inventory != null)
                {
                    block.gui.inventory = inventory;
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
                block.foregroundBlock = Block.LoadFromJson(blockToken.GetToken("/foreground_block"), chunk, world);
            }


            //Set block stats
            block.xPos = posX;
            block.yPos = posY;
            block.chunk = chunk;

            return block;
        }



        public void LoadInventory(Inventory inv)
        {
            blockInventory = inv;
            Canvas.SetTop(inv.grdInventory, 410);
            world.inventoryList.Add(inv);
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
                    MoveToForeground();
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
            blockContainer.cvsBlock.MouseLeftButtonDown -= cvsBlock_MouseLeftButtonDown;
            blockContainer.cvsBlock.MouseLeftButtonUp -= cvsBlock_MouseLeftButtonUp;
            blockContainer.cvsBlock.MouseRightButtonUp -= cvsBlock_MouseRightButtonUp;
            blockContainer.cvsBlock.MouseRightButtonDown -= cvsBlock_MouseRightButtonDown;
            blockContainer.cvsBlock.MouseEnter -= cvsBlock_MouseEnter;
            blockContainer.cvsBlock.MouseLeave -= cvsBlock_MouseLeave;
        }

        public bool IsCollidingWithPlayer(object element)
        {
            if (element is Canvas)
            {
                //Check for collision
                if (world.wndGame.GetRectangle(world.player.texture).IntersectsWith(world.wndGame.GetRectangle(blockContainer.cvsBlock)))
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
            blockContainer.ShowDarkRectangle();
        }

        public void MoveToForeground()
        {
            isBackground = false;
            blockContainer.HideDarkRectangle();
        }

        public bool IsInRange()
        {
            Block playerBlock = world.GetBlock(world.player.posX / 1000, (world.player.posY / 1000) + 1);
            if (playerBlock != null)
            {
                return (GetXRangeToBlock(playerBlock) < 5 && GetYRangeToBlock(playerBlock) < 5);
            }
            else
            {
                return false;
            }
        }

        public bool IsLightSource(bool ignoreAir)
        {
            if (isLightSource && (id != "sc:air_block" || !ignoreAir))
            {
                return true;
            }
            else
            {
                if (foregroundBlock != null && foregroundBlock.isLightSource && (foregroundBlock.id != "sc:air_block" || !ignoreAir))
                {
                    return true;
                }
            }
            return false;
        }

        //Create the item that corresponds to the block
        public virtual void GenerateItem(World world)
        {
            return;
        }

        public virtual void SetTexture()
        {
            throw new Exception("No texture for block was set.");
        }

        public virtual void RightClickAction(object sender)
        {
            return;
        }

        public virtual void ShowAdditionalDebugInfo()
        {
            return;
        }

        public Item GetItem()
        {
            //Generate a new item if necessary and return the item
            if (item == null)
            {
                GenerateItem(world);
            }
            return item;
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

        public void UpdateNearbyBlocks()
        {
            List<Block> blocksInRange = GetBlocksInRange(world.lightRange);

            foreach (Block block in blocksInRange)
            {
                if (block != null)
                {
                    if (isLightSource || (foregroundBlock != null && foregroundBlock.isLightSource))
                    {
                        int range = GetRangeToBlock(block);
                        if (range < block.rangeToNearestLightSource)
                        {
                            block.rangeToNearestLightSource = range;
                            block.SetLightLevel(range);

                            if (block.blockContainer != null)
                            {
                                block.blockContainer.SetLightOpacity();
                            }
                        }
                    }
                    else if (!isLightSource && (foregroundBlock == null || (foregroundBlock != null && !foregroundBlock.isLightSource)))
                    {
                        int range = GetRangeToBlock(block);

                        if (blockContainer.previousBlockWasLightSource || blockContainer.previousForegroundBlockWasLightSource)
                        {
                            if (blockContainer != null && block.blockContainer != null)
                            {
                                block.rangeToNearestLightSource = block.RangeToLightSource();
                                block.SetLightLevel(block.RangeToLightSource());
                                block.blockContainer.SetLightOpacity();
                            }
                        }
                    }
                }
            }
        }

        public int RangeToLightSource()
        {
            List<Block> blocksInRange = GetBlocksInRange(world.lightRange);

            int minRange = world.lightRange + 1;
            foreach (Block block in blocksInRange)
            {
                if (block != null)
                {
                    if (block.isLightSource || (block.foregroundBlock != null && block.foregroundBlock.isLightSource))
                    {
                        int range = GetRangeToBlock(block);
                        SetAsNearestLightSource(range);
                        minRange = Math.Min(minRange, range);
                    }
                }


            }

            return minRange;
        }

        public void SetLightLevel(int range)
        {
            int rangeToLightSource = range;
            if (isLightSource || (foregroundBlock != null && foregroundBlock.isLightSource) || rangeToLightSource == 1 || rangeToLightSource == 2)
            {
                lightLevel = 0;
            }
            else if (rangeToLightSource < world.lightRange)
            {
                lightLevel = 1.0 / (world.lightRange - 3) * rangeToLightSource - 0.75;
            }
            else if (rangeToLightSource == world.lightRange)
            {
                lightLevel = 0.9;
            }
            else
            {
                lightLevel = 1;
            }
        }

        private void SetAsNearestLightSource(int range)
        {
            if (!isLightSource && (foregroundBlock != null && !foregroundBlock.isLightSource))
            {
                //If no nearest lightsource is detected, add block as lightsource
                if (rangeToNearestLightSource == 100000) //Any random giant number that a range can never be
                {
                    rangeToNearestLightSource = range;
                }
                //If a block with a lower range to nearest lightsource is found, delete all current nearest ones and add new one
                else if (range < rangeToNearestLightSource)
                {
                    rangeToNearestLightSource = range;
                }
            }
        }

        private Block GetBlockFromOffset(int xOffset, int yOffset)
        {

            if (yOffset + yPos < 0 || yOffset + yPos > 74)
            {
                return null;
            }

            if (xPos + xOffset < 0)
            {
                Chunk chunk = world.GetLoadedChunk(this.chunk.index - 1);

                if (chunk != null)
                {
                    return chunk.blockList.Get(xPos + xOffset + 8, yPos + yOffset);
                }
                else
                {
                    return null;
                }
            }
            else if (xPos + xOffset > 7)
            {
                Chunk chunk = world.GetLoadedChunk(this.chunk.index + 1);

                if (chunk != null)
                {
                    return chunk.blockList.Get(xPos + xOffset - 8, yPos + yOffset);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return chunk.blockList.Get(xPos + xOffset, yPos + yOffset);
            }
        }

        public void BreakBlock(bool skipRangeCheck, bool skipBreakableCheck)
        {
            //Check if is in range
            if ((IsInRange() || skipRangeCheck))
            {
                //If it has a foreground block, check if that one is breakable
                if (foregroundBlock != null)
                {
                    if (foregroundBlock.isBreakable || skipBreakableCheck)
                    {
                        //Add the foreground block's item to the inventory
                        foregroundBlock.GenerateItem(world);
                        if (foregroundBlock.item != null)
                        {
                            world.AddEntity(new ItemEntity(foregroundBlock.item, //item type
                                (xPos + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, //posX
                                yPos * 1000 + 500 - ItemEntity.itemSizeY / 2, //posY
                                rnd.Next(-6000, 6000), rnd.Next(-15000, -10000), //velX and velY 
                                world));
                        }
                        blockContainer.RemoveForegroundBlock();
                    }

                    if (hasInventory)
                    {
                        blockInventory.Drop(xPos + 8 * chunk.index, yPos);
                    }
                }
                //If it has no foreground block, check if the normal block is breakable
                else if (foregroundBlock == null && (isBreakable || skipBreakableCheck))
                {
                    //Remove the block from the chunks blocklist and add an airblock
                    Block block = new AirBlock(world, false);
                    PlaceNewBlock(block);

                    //Add the block's item to the inventory
                    GenerateItem(world);

                    //If the block has a loot table, roll an entry and give the items to player
                    if (lootTable != null)
                    {
                        List<Item> items = lootTable.RollEntry().RollItems();
                        foreach (Item item in items)
                        {
                            world.AddEntity(new ItemEntity(item, //item type
                                (xPos + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, //posX
                                yPos * 1000 + 500 - ItemEntity.itemSizeY / 2, //posY
                                rnd.Next(-6000, 6000), rnd.Next(-15000, -10000), //velX and velY 
                                world));
                        }
                    }
                    //If has only an item, only give that item
                    else if (item != null)
                    {
                        world.AddEntity(new ItemEntity(item, //item type
                                (xPos + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, //posX
                                yPos * 1000 + 500 - ItemEntity.itemSizeY / 2, //posY
                                rnd.Next(-6000, 6000), rnd.Next(-15000, -10000), //velX and velY 
                                world));
                    }

                    if (hasInventory)
                    {
                        blockInventory.Drop((xPos + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, yPos * 1000 + 500 - ItemEntity.itemSizeY / 2);
                    }
                }
            }
        }

        public void PlaceNewBlock(Block block)
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
            block.MoveToForeground();
        }

        public void PlaceInForeground(Block block)
        {
            blockContainer.RenderForegroundBlock(block);
            block.xPos = xPos;
            block.yPos = yPos;
            block.chunk = chunk;
        }

        public bool ConnectedBlocksHaveEnoughSpace(Block baseBlock, bool isForeground)
        {
            if (!isForeground)
            {
                foreach (Block block in baseBlock.connectedBlocks)
                {
                    int actualXPos = xPos + block.xOffset;
                    int actualYPos = yPos + block.yOffset;

                    if (actualXPos > 8)
                    {
                        Chunk newChunk = world.GetLoadedChunk(chunk.index + 1);
                        block.chunk = newChunk;
                        if (newChunk.GetBlock(actualXPos - 8, actualYPos).isSolid || newChunk.GetBlock(actualXPos - 8, actualYPos).isBackground)
                        {
                            return false;
                        }
                    }
                    else if (actualXPos < 1)
                    {
                        Chunk newChunk = world.GetLoadedChunk(chunk.index - 1);
                        block.chunk = newChunk;
                        if (newChunk.GetBlock(actualXPos + 8, actualYPos).isSolid || newChunk.GetBlock(actualXPos + 8, actualYPos).isBackground)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        block.chunk = chunk;
                        if (chunk.GetBlock(actualXPos, actualYPos).isSolid || chunk.GetBlock(actualXPos, actualYPos).isBackground)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else if (isForeground)
            {
                foreach (Block block in baseBlock.connectedBlocks)
                {
                    int actualXPos = xPos + block.xOffset;
                    int actualYPos = yPos + block.yOffset;

                    if (actualXPos > 8)
                    {
                        Chunk newChunk = world.GetLoadedChunk(chunk.index + 1);
                        block.chunk = newChunk;
                        if (newChunk.GetBlock(actualXPos - 8, actualYPos).foregroundBlock != null || !newChunk.GetBlock(actualXPos - 8, actualYPos).isBackground)
                        {
                            return false;
                        }
                    }
                    else if (actualXPos < 1)
                    {
                        Chunk newChunk = world.GetLoadedChunk(chunk.index - 1);
                        block.chunk = newChunk;
                        if (newChunk.GetBlock(actualXPos + 8, actualYPos).foregroundBlock != null || !newChunk.GetBlock(actualXPos + 8, actualYPos).isBackground)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        block.chunk = chunk;
                        if (chunk.GetBlock(actualXPos, actualYPos).foregroundBlock != null || !chunk.GetBlock(actualXPos, actualYPos).isBackground)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetCoords(int x, int y, Chunk chunk)
        {
            //Warning: Only sets coords inside blocks, not inside chunk/blocklist
            this.chunk = chunk;
            xPos = x;
            yPos = y;
        }

        public void PlaceConnectedForegroundBlocks(Block baseBlock)
        {
            foreach (Block conBlock in baseBlock.connectedBlocks)
            {
                //Get the actual positions for each connected block based on the offset
                int actualXPos = xPos + conBlock.xOffset;
                int actualYPos = yPos + conBlock.yOffset;

                //Since the actual pos is potentially in another chunk, get the pos in that chunk
                if (actualXPos > 8)
                {
                    Chunk newChunk = world.GetLoadedChunk(chunk.index + 1);
                    newChunk.GetBlock(actualXPos - 8, actualYPos).PlaceInForeground(conBlock);
                }
                else if (actualXPos < 1)
                {
                    Chunk newChunk = world.GetLoadedChunk(chunk.index - 1);
                    newChunk.GetBlock(actualXPos + 8, actualYPos).PlaceInForeground(conBlock);
                }
                else
                {
                    chunk.GetBlock(actualXPos, actualYPos).PlaceInForeground(conBlock);
                }
            }
        }

        public void PlaceConnectedBlocks(Block baseBlock)
        {
            foreach (Block conBlock in baseBlock.connectedBlocks)
            {
                //Get the actual positions for each connected block based on the offset
                int actualXPos = xPos + conBlock.xOffset;
                int actualYPos = yPos + conBlock.yOffset;

                //Since the actual pos is potentially in another chunk, get the pos in that chunk
                if (actualXPos > 8)
                {
                    Chunk newChunk = world.GetLoadedChunk(chunk.index + 1);
                    conBlock.chunk = newChunk;
                    newChunk.GetBlock(actualXPos - 8, actualYPos).PlaceNewBlock(conBlock);
                }
                else if (actualXPos < 1)
                {
                    Chunk newChunk = world.GetLoadedChunk(chunk.index - 1);
                    conBlock.chunk = newChunk;
                    newChunk.GetBlock(actualXPos + 8, actualYPos).PlaceNewBlock(conBlock);
                }
                else
                {
                    conBlock.chunk = chunk;
                    chunk.GetBlock(actualXPos, actualYPos).PlaceNewBlock(conBlock);
                }

            }
        }

        public void UpdateAirLightsources(Block block)
        {
            //Update Air Lightsources
            for (int y = yPos + 1; y < 76; y++)
            {
                //Go through each block below the currently placed one
                if (chunk.GetBlock(xPos, y).id == "sc:air_block")
                {
                    //If the block at that position is air, update it accordingly
                    AirBlock newBlock = new AirBlock(world, false);
                    newBlock.rangeToNearestLightSource = chunk.GetBlock(xPos, y).rangeToNearestLightSource;

                    //If the placed block is air, the blocks below should be a lightsource, if not, then no light source
                    if (block.id == "sc:air_block" && block.isLightSource)
                    {
                        newBlock.isLightSource = true;
                    }
                    else
                    {
                        newBlock.isLightSource = false;
                    }

                    chunk.SetBlock(newBlock, xPos, y);
                }
                else
                {
                    //If it's not air, the other blocks below don't matter since that block blocks it.
                    break;
                }
            }
        }

        public bool IsAirLightSource(Block block)
        {
            for (int y = yPos - 1; y >= 0; y--)
            {
                if (chunk.GetBlock(xPos, y).id != "sc:air_block")
                {
                    block.isLightSource = false;
                    return false;
                }
            }
            return true;
        }

        public void DisplayDebugInformation()
        {
            //Show the debug information for the block in debug menu
            if (world.debugMenu.isEnabled)
            {
                world.debugMenu.tblBlockStats.Text = "";
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, "Selected Block:");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"id={id}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"name={name}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"xPos={xPos}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"yPos={yPos}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"chunk={chunk.index}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"isSolid={isSolid}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"isBackground={isBackground}");
                if (foregroundBlock != null)
                {
                    world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"foregroundBlock={foregroundBlock.id}");
                }
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"lightLevel={lightLevel}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"isLightSource={isLightSource}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"rangeToNearestLightSource={rangeToNearestLightSource}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"hasRightClickAction={hasRightClickAction}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"hasInventory={hasInventory}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"isBase={isBase}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"isSurface={isSurface}");
                if (baseBlock != null)
                {
                    world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"baseBlock={baseBlock.id} at x{baseBlock.xPos} y{baseBlock.yPos}");
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
                    world.debugMenu.AddLine(world.debugMenu.tblBlockStats, "Tags:");
                    foreach (string tag in tags)
                    {
                        world.debugMenu.AddLine(world.debugMenu.tblBlockStats, tag);
                    }
                }
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
            blockContainer.bdrBlock.BorderThickness = new Thickness(0, 0, 0, 0);
            blockContainer.bdrBlock.BorderBrush = new SolidColorBrush(Colors.Transparent);

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
                        if (world.gamemode == Gamemode.Creative || foregroundBlock.breakTime == 0)
                        {
                            world.clickHandler.DoLeftClick(this, sender);
                        }
                        else
                        {
                            tmrBreak.Interval = foregroundBlock.breakTime; //Will later also include tool in hand
                            tmrBreak.Start();
                        }
                    }
                }
                else
                {
                    if (isBreakable)
                    {
                        if (world.gamemode == Gamemode.Creative || breakTime == 0)
                        {
                            world.clickHandler.DoLeftClick(this, sender);
                        }
                        else
                        {
                            tmrBreak.Interval = breakTime; //Will later also include tool in hand
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

            if (world.player.inventory.GetSelectedItem() != null && world.player.inventory.GetSelectedItem().tags.Contains("tools/hammer"))
            {
                //If the player holds a hammer, is in gamemode survival, the block is in range and doesn't have a foreground block
                if (world.gamemode == Gamemode.Survival && IsInRange() && foregroundBlock == null && canBeMovedToBackground)
                {
                    //Start the timer for the hammer
                    tmrHammer.Interval = breakTime; //TO-DO: Include tool efficiency
                    tmrHammer.Start();
                    return;
                }
            }

            //If all of the checks above fail, handle it the normal way
            world.clickHandler.DoRightClick(this, sender);
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
                    world.clickHandler.DoLeftClick(this, sender);
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
                    world.clickHandler.DoRightClick(this, sender);
                    blockContainer.SetHammerState(0);
                    tmrHammer.Stop();
                    return;
                }

                //Increase the hammer state
                blockContainer.SetHammerState(blockContainer.hammerState + 1);
            }));
        }
    }
}
