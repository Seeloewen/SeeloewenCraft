using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Converters;
using System.Diagnostics.Eventing.Reader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Json.Pointer;

namespace SeeloewenCraft
{
    public abstract class Block
    {
        public wndGame wndGame;
        public ImageBrush image = new ImageBrush();
        public BlockContainer blockContainer;
        public Chunk chunk;
        public Item item;
        public Inventory blockInventory;
        public string name;
        public int xPos;
        public int yPos;
        public bool isSolid = true;
        public bool isInBackground = false;
        public bool hasFixedSolidState = false;
        public bool isBreakable = true;
        public bool hasInventory = false;
        public bool isLightSource = false;
        public double lightLevel;
        public Block foregroundBlock;
        public bool isForeground = false;
        public int rangeToNearestLightSource = 100000;

        //-- Constructor --//

        public Block(wndGame wndGame, int xPos, int yPos, Chunk chunk, Item item, bool isInBackground)
        {
            //Set the attributes
            this.wndGame = wndGame;
            this.xPos = xPos;
            this.yPos = yPos;
            this.chunk = chunk;
            this.isInBackground = isInBackground;

            if (item != null)
            {
                this.item = item;
            }
            else
            {
                this.item = null;
            }
        }

        //-- Custom Methods --//


        public void SaveToJson(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("name");
            writer.WriteValue(GetType().ToString().Replace("SeeloewenCraft.", ""));

            writer.WritePropertyName("pos_x");
            writer.WriteValue(xPos);

            writer.WritePropertyName("pos_y");
            writer.WriteValue(yPos);

            writer.WritePropertyName("is_in_background");
            writer.WriteValue(isInBackground);

            writer.WriteEndObject();
        }


        static public Block LoadFromJson(JToken blockToken, Chunk chunk, wndGame wndGame) 
        {
            int posX = (int)new JsonPointer($"/pos_x").Evaluate(blockToken);
            int posY = (int)new JsonPointer($"/pos_y").Evaluate(blockToken);
            bool isInBackground = (bool)new JsonPointer($"/is_in_background").Evaluate(blockToken);

            string name = (string)new JsonPointer($"/name").Evaluate(blockToken);


            switch (name)
            {
                case "GrassBlock":
                    return new GrassBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "DirtBlock":
                    return new DirtBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "StoneBlock":
                    return new StoneBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "AirBlock":
                    return new AirBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "BedrockBlock":
                    return new BedrockBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "DiamondOreBlock":
                    return new DiamondOreBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "IronOreBlock":
                    return new IronOreBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "CoalOreBlock":
                    return new CoalOreBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "OakLogBlock":
                    return new OakLogBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "OakLeavesBlock":
                    return new OakLeavesBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "SpruceLogBlock":
                    return new SpruceLogBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "SpruceLeavesBlock":
                    return new SpruceLeavesBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "ChestBlock":
                    return new ChestBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                case "MagmaBlock":
                    return new MagmaBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
                /*case "TorchBlock":
                    return Add(new TorchBlock(wndGame, posX, posY, this, null, isInBackground));
                    break;*/
                default:
                    return new AirBlock(wndGame, posX, posY, chunk, null, isInBackground);
                    break;
            }
        }
    



        public void SetContainer(BlockContainer blockContainer)
        {
            this.blockContainer = blockContainer;
            this.blockContainer.cvsBlock.Background = image;
            this.blockContainer.cvsBlock.MouseLeftButtonDown += cvsBlock_MouseLeftButtonDown;
            this.blockContainer.cvsBlock.MouseRightButtonDown += cvsBlock_MouseRightButtonDown;
            this.blockContainer.cvsBlock.MouseEnter += cvsBlock_MouseEnter;
            this.blockContainer.cvsBlock.MouseLeave += cvsBlock_MouseLeave;
            //Add image and events to the container
            if (!hasFixedSolidState)
            {
                if (isInBackground)
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
            blockContainer.cvsBlock.MouseRightButtonDown -= cvsBlock_MouseRightButtonDown;
            blockContainer.cvsBlock.MouseEnter -= cvsBlock_MouseEnter;
            blockContainer.cvsBlock.MouseLeave -= cvsBlock_MouseLeave;
        }

        private bool IsCollidingWithPlayer(object element)
        {
            if (element is Canvas)
            {
                //Check for collision
                if (wndGame.GetRectangle(wndGame.player.cvsPlayer).IntersectsWith(wndGame.GetRectangle(blockContainer.cvsBlock)))
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

        public void ShowBlockInfo()
        {
            List<UIElement> removalList = new List<UIElement>();

            //Check for existing block info
            foreach (UIElement uiElement in blockContainer.cvsBlock.Children)
            {
                if (uiElement is TextBlock textBlock)
                {
                    if (textBlock.Tag.ToString() == "BlockInfo")
                    {
                        removalList.Add(uiElement);
                    }
                }
            }

            //Remove the existing block info
            foreach (UIElement uiElement in removalList)
            {
                blockContainer.cvsBlock.Children.Remove(uiElement);
            }

            //Show block info
            blockContainer.cvsBlock.Children.Add(new TextBlock { Text = string.Format("x: {0} y:{1}\n{2}\n{3},{4}", xPos, yPos, GetType().ToString().Replace("SeeloewenCraft.", "").Replace("Block", ""), chunk.index, lightLevel), Tag = "BlockInfo" });
        }

        public void HideBlockInfo()
        {
            List<UIElement> removalList = new List<UIElement>();

            //Check for existing block info
            foreach (UIElement uiElement in blockContainer.cvsBlock.Children)
            {
                if (uiElement is TextBlock textBlock)
                {
                    if (textBlock.Tag.ToString() == "BlockInfo")
                    {
                        removalList.Add(uiElement);
                    }
                }
            }

            //Remove the existing block info
            foreach (UIElement uiElement in removalList)
            {
                blockContainer.cvsBlock.Children.Remove(uiElement);
            }
        }

        public void MoveToBackground()
        {
            isInBackground = true;
            isSolid = false;
            blockContainer.ShowDarkRectangle();
        }

        public void MoveToForeground()
        {
            isInBackground = false;
            isSolid = true;
            blockContainer.HideDarkRectangle();
        }

        public bool IsInRange()
        {
            //Convert positions to screen coordinates
            Point playerScreenPoint = wndGame.player.cvsPlayer.PointToScreen(new Point(0, 0));
            Point otherScreenPoint = blockContainer.bdrBlock.PointToScreen(new Point(0, 0));

            //Convert to coordinates considering scrolling
            Point playerPosition = wndGame.svWorld.TranslatePoint(playerScreenPoint, wndGame.cvsWorld);
            Point otherPosition = wndGame.svWorld.TranslatePoint(otherScreenPoint, wndGame.cvsWorld);

            //Check if the distance between the player and the block is less than 200 pixels
            if ((Math.Abs(playerPosition.X - otherPosition.X) < 200) && (Math.Abs(playerPosition.Y - otherPosition.Y) < 200))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Create the item that corresponds to the block
        public abstract void GenerateItem(wndGame wndGame, int id);

        public abstract void SetTexture();


        public Item GetItem()
        {
            //Generate a new item if necessary and return the item
            if (item == null)
            {
                GenerateItem(wndGame, 0);
            }
            return item;
        }

        public bool IsHolding(string itemName)
        {
            foreach (HotbarSlot slot in wndGame.player.inventory.hotbarSlotList)
            {
                //Check if the slot is selected and has an item
                if (slot.isSelected == true && slot.slot.items.Count > 0)
                {
                    //Check if the item is the one that is being searched
                    if (slot.slot.items[slot.slot.items.Count - 1].itemName == itemName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Block> GetBlocksInRange()
        {
            List<Block> blocksInRange = new List<Block>();

            //Gets all blocks above 
            for (int yOffset = 0; yOffset < 6; yOffset++)
            {
                for (int xOffset = -yOffset; xOffset <= yOffset; xOffset++)
                {
                    blocksInRange.Add(GetBlock(xOffset, 6 - yOffset));
                    blocksInRange.Add(GetBlock(xOffset, -6 + yOffset));
                }
            }

            for (int xOffset = 1; xOffset < 7; xOffset++)
            {
                blocksInRange.Add(GetBlock(xOffset, 0));
                blocksInRange.Add(GetBlock(-xOffset, 0));
            }

            return blocksInRange;
        }

        public int GetRangeToBlock(Block block)
        {
            int xDiff = block.xPos + block.chunk.index * 8 - xPos - chunk.index * 8;
            return Math.Abs(xDiff) + Math.Abs(block.yPos - yPos);
        }

        public void UpdateNearbyBlocks()
        {
            List<Block> blocksInRange = GetBlocksInRange();

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
                            block.rangeToNearestLightSource = block.RangeToLightSource();
                            block.SetLightLevel(block.RangeToLightSource());
                            block.blockContainer.SetLightOpacity();
                        }
                    }
                }
            }
        }       

        public int RangeToLightSource()
        {
            List<Block> blocksInRange = GetBlocksInRange();

            int minRange = 7;
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
            if (isLightSource || (foregroundBlock != null && foregroundBlock.isLightSource) || rangeToLightSource == 1)
            {
                lightLevel = 0;
            }
            else if (rangeToLightSource < 6)
            {
                lightLevel = 0.25 * rangeToLightSource - 0.5;
            }
            else if (rangeToLightSource == 6)
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
                if (rangeToNearestLightSource == 100000)
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

        private Block GetBlock(int xOffset, int yOffset)
        {

            if (yOffset + yPos < 1 || yOffset + yPos > 75)
            {
                return null;
            }

            if (xPos + xOffset < 1)
            {
                Chunk chunk = wndGame.GetFromCurrentChunks(this.chunk.index - 1);

                if (chunk != null)
                {
                    return chunk.blockList.Get(xPos + xOffset + 8, yPos + yOffset);
                }
                else
                {
                    return null;
                }
            }
            else if (xPos + xOffset > 8)
            {
                Chunk chunk = wndGame.GetFromCurrentChunks(this.chunk.index + 1);

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

        //-- Event Handlers --//

        private void cvsBlock_MouseEnter(object sender, EventArgs e)
        {
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
        }

        private void cvsBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Check if the block is both breakable and in range
            if (isBreakable == true && IsInRange() == true)
            {
                if(foregroundBlock != null)
                {
                    //Add the foreground block's item to the inventory
                    foregroundBlock.GenerateItem(wndGame, 0);
                    wndGame.player.inventory.AddItem(foregroundBlock.item);
                    blockContainer.RemoveForegroundBlock();
                }
                else
                {
                    //Remove the block from the chunks blocklist and add an airblock
                    chunk.blockList.Remove(this);
                    Block block = new AirBlock(wndGame, xPos, yPos, chunk, null, false);
                    chunk.blockList.Add(block);
                    chunk.SetBlock(block, xPos, yPos);

                    //Add the block's item to the inventory
                    MoveToForeground();
                    GenerateItem(wndGame, 0);
                    wndGame.player.inventory.AddItem(item);
                }
            }
        }

        private void cvsBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //If the player is holding a hammer, move the item to the background or foreground (if possible)
            if (!hasFixedSolidState && IsInRange() && IsHolding("Hammer"))
            {
                if (isInBackground)
                {
                    MoveToForeground();
                }
                else
                {
                    MoveToBackground();
                }
            }
            else if (IsInRange() && IsHolding("Torch") && foregroundBlock == null && isInBackground)
            {
                //Go through each hotbar slot
                foreach (HotbarSlot slot in wndGame.player.inventory.hotbarSlotList)
                {
                    //Check if the slot is selected and has an item
                    if (slot.isSelected == true && slot.slot.items.Count > 0)
                    {
                        if (slot.slot.items[slot.slot.items.Count - 1].block == null)
                        {
                            slot.slot.items[slot.slot.items.Count - 1].block = slot.slot.items[slot.slot.items.Count - 1].GenerateBlock(0, 0, chunk, false);
                        }

                        blockContainer.SetForegroundBlock(slot.slot.items[slot.slot.items.Count - 1].block);

                        //Remove the item from the inventory
                        wndGame.player.inventory.RemoveItem(slot.slot.items[slot.slot.items.Count - 1]);
                    }
                }
            }

            //Check if the block is in range, not solid and doesn't collide with the player
            else if (IsInRange() && !isSolid && !IsCollidingWithPlayer(sender) && !isInBackground)
            {
                //Go through each hotbar slot
                foreach (HotbarSlot slot in wndGame.player.inventory.hotbarSlotList)
                {
                    //Check if the slot is selected and has an item
                    if (slot.isSelected == true && slot.slot.items.Count > 0)
                    {
                        if (slot.slot.items[slot.slot.items.Count - 1].block == null)
                        {
                            slot.slot.items[slot.slot.items.Count - 1].block = slot.slot.items[slot.slot.items.Count - 1].GenerateBlock(0, 0, chunk, false);
                        }

                        //Check if the slots item is placable
                        if (slot.slot.items[slot.slot.items.Count - 1].isPlacable == true)
                        {
                            //Remove the non-solid block that is currently there and add the new selected block
                            chunk.blockList.Remove(this);
                            chunk.blockList.Add(slot.slot.items[slot.slot.items.Count - 1].block, xPos, yPos);
                            slot.slot.items[slot.slot.items.Count - 1].block.xPos = xPos;
                            slot.slot.items[slot.slot.items.Count - 1].block.yPos = yPos;
                            slot.slot.items[slot.slot.items.Count - 1].block.chunk = chunk;

                            //Add the border to the chunk
                            chunk.SetBlock(slot.slot.items[slot.slot.items.Count - 1].block, xPos, yPos);

                            //Remove the item from the inventory
                            wndGame.player.inventory.RemoveItem(slot.slot.items[slot.slot.items.Count - 1]);
                        }
                    }
                }
            }
            else if (IsInRange() == true && isSolid == true && hasInventory == true)
            {
                //If the block has an inventory, open it as well as the players inventory
                Canvas.SetTop(wndGame.player.inventory.grdInventory, -10);
                wndGame.player.inventory.ShowInventory();
                blockInventory.ShowInventory();
            }
        }
    }

    //-- Blocks --//

    public class GrassBlock : Block
    {
        public GrassBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Grass Block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new GrassItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.GrassBlock;
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Stone Block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new StoneItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.StoneBlock;
        }
    }

    public class DirtBlock : Block
    {
        public DirtBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Dirt";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new DirtItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.DirtBlock;
        }
    }

    public class AirBlock : Block
    {
        public AirBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            isBreakable = false;
            isSolid = false;
            hasFixedSolidState = true;
            SetTexture();
            name = "Air";
            isLightSource = true;
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new AirItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.AirBlock;
        }
    }

    public class BedrockBlock : Block
    {
        public BedrockBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            isBreakable = false;
            hasFixedSolidState = true;
            SetTexture();
            name = "Bedrock";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new BedrockItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.BedrockBlock;
        }
    }

    public class CoalOreBlock : Block
    {
        public CoalOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Coal Ore";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new CoalOreItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.CoalOreBlock;
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Diamond Ore";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new DiamondOreItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.DiamondOreBlock;
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Iron Ore";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new IronOreItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.IronOreBlock;
        }
    }

    public class OakLogBlock : Block
    {
        public OakLogBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Oak Log";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new OakLogItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.OakLogBlock;
        }
    }

    public class OakLeavesBlock : Block
    {
        public OakLeavesBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Oak Leaves";
            isLightSource = true;
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new OakLeavesItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.OakLeavesBlock;
        }
    }

    public class SpruceLogBlock : Block
    {
        public SpruceLogBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Spruce Log";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new SpruceLogItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.SpruceLogBlock;
        }
    }

    public class SpruceLeavesBlock : Block
    {
        public SpruceLeavesBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            isLightSource = true;
            SetTexture();
            name = "Spruce Leaves";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new SpruceLeavesItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.SpruceLeavesBlock;
        }
    }

    public class ChestBlock : Block
    {
        public ChestBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            hasInventory = true;
            blockInventory = new Inventory(wndGame, item.id, false);
            Canvas.SetTop(blockInventory.grdInventory, 410);
            wndGame.inventoryList.Add(blockInventory);
            SetTexture();
            name = "Chest";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new ChestItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.ChestBlock;
        }
    }

    public class MagmaBlock : Block
    {
        public MagmaBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            SetTexture();
            name = "Magma Block";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new MagmaBlockItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.MagmaBlock;
        }
    }

    public class TorchBlock : Block
    {
        public TorchBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item, bool isInBackground) : base(wndGame, x, y, chunk, item, isInBackground)
        {
            isSolid = false;
            isLightSource = true;
            SetTexture();
            name = "Torch";
        }

        override public void GenerateItem(wndGame wndGame, int id)
        {
            item = new TorchItem(wndGame, id, this);
        }

        public override void SetTexture()
        {
            image = wndGame.images.Torch;
        }
    }
}
