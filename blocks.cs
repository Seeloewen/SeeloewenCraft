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

namespace SeeloewenCraft
{
    public class Block
    {
        wndGame wndGame;
        public ImageBrush image = new ImageBrush();
        public BlockContainer blockContainer;
        public Chunk chunk;
        public Item item;
        public Inventory blockInventory;
        public int xPos;
        public int yPos;
        public bool isSolid = true;
        public bool isBreakable = true;
        public bool hasInventory = false;

        public Block(wndGame wndGame, int xPos, int yPos, Chunk chunk, Item item)
        {
            //Set the attributes
            this.wndGame = wndGame;
            this.xPos = xPos;
            this.yPos = yPos;
            this.item = item;
            this.chunk = chunk;
        }

        public void SetContainer(BlockContainer blockContainer)
        {
            this.blockContainer = blockContainer;
            this.blockContainer.cvsBlock.Background = image;
            this.blockContainer.cvsBlock.MouseLeftButtonDown += cvsBlock_MouseLeftButtonDown;
            this.blockContainer.cvsBlock.MouseRightButtonDown += cvsBlock_MouseRightButtonDown;
            this.blockContainer.cvsBlock.MouseEnter += cvsBlock_MouseEnter;
            this.blockContainer.cvsBlock.MouseLeave += cvsBlock_MouseLeave;
        }

        public void RemoveHandlersFromContainer()
        {
            blockContainer.cvsBlock.MouseLeftButtonDown -= cvsBlock_MouseLeftButtonDown;
            blockContainer.cvsBlock.MouseRightButtonDown -= cvsBlock_MouseRightButtonDown;
            blockContainer.cvsBlock.MouseEnter -= cvsBlock_MouseEnter;
            blockContainer.cvsBlock.MouseLeave -= cvsBlock_MouseLeave;
        }

        private bool isCollidingWithPlayer(object element)
        {
            if (element is Canvas canvas)
            {
                //Check for collision
                if (wndGame.GetRectangle(wndGame.player.cvsPlayerHitbox).IntersectsWith(wndGame.GetRectangle(blockContainer.cvsBlock)))
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
            //Show block info
            blockContainer.cvsBlock.Children.Add(new TextBlock { Text = string.Format("x: {0} y:{1}\n{2}\n{3}", xPos, yPos, GetType().ToString().Replace("SeeloewenCraft.", "").Replace("Block", ""), chunk.index) });
        }

        public void HideBlockInfo()
        {
            //Hide the block info
            blockContainer.cvsBlock.Children.Clear();
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
                //Remove the block from the chunks blocklist and add an airblock
                
                chunk.blockList.Remove(this);
                Block block = new AirItem(wndGame, 0).GenerateBlock(xPos, yPos, chunk);
                chunk.blockList.Add(block);
                chunk.SetBlock(block, xPos, yPos);

                //Add the block's item to the inventory
                wndGame.player.inventory.AddItem(item);
            }
        }

        private void cvsBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Check if the block is in range, not solid and doesn't collide with the player
            if (IsInRange() == true && isSolid == false && isCollidingWithPlayer(sender) == false)
            {
                //Go through each hotbar slot
                foreach (HotbarSlot slot in wndGame.player.inventory.hotbarSlotList)
                {
                    //Check if the slot is selected annd has an item
                    if (slot.isSelected == true && slot.slot.items.Count > 0)
                    {
                        if (slot.slot.items[slot.slot.items.Count - 1].block == null)
                        {
                            slot.slot.items[slot.slot.items.Count - 1].block = slot.slot.items[slot.slot.items.Count - 1].GenerateBlock(0, 0, chunk);
                        }
                        //Check if the slots item is placable
                        if (slot.slot.items[slot.slot.items.Count - 1].isPlacable == true)
                        {
                            //Remove the non-solid block that is currently there and add the new selected block
                            chunk.blockList.Remove(this);
                            chunk.blockList.Add(slot.slot.items[slot.slot.items.Count - 1].block);
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
                Canvas.SetTop(wndGame.player.inventory.grdInventory, -10);
                wndGame.player.inventory.ShowInventory();
                blockInventory.ShowInventory();
            }
            MessageBox.Show("run!");
        }
       
    }

    public class GrassBlock : Block
    {
        public GrassBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            image = wndGame.images.GrassBlock;
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            image = wndGame.images.StoneBlock;
        }
    }

    public class DirtBlock : Block
    {
        public DirtBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            image = wndGame.images.DirtBlock;
        }
    }

    public class AirBlock : Block
    {
        public AirBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            isBreakable = false;
            isSolid = false;
            image = wndGame.images.AirBlock;
        }
    }

    public class BedrockBlock : Block
    {
        public BedrockBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            isBreakable = false;
            image = wndGame.images.BedrockBlock;
        }
    }

    public class CoalOreBlock : Block
    {
        public CoalOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            image = wndGame.images.CoalOreBlock;
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            image = wndGame.images.IronOreBlock;
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            image = wndGame.images.IronOreBlock;
        }
    }

    public class OakLogBlock : Block
    {
        public OakLogBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            image = wndGame.images.OakLogBlock;
        }
    }

    public class OakLeavesBlock : Block
    {
        public OakLeavesBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            image = wndGame.images.OakLeavesBlock;
        }
    }

    public class ChestBlock : Block
    {
        public ChestBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            hasInventory = true;
            blockInventory = new Inventory(wndGame, item.id, false);
            Canvas.SetTop(blockInventory.grdInventory, 410);
            wndGame.inventoryList.Add(blockInventory);
            image = wndGame.images.ChestBlock;
        }
    }

    public class MagmaBlock : Block
    {
        public MagmaBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            image = wndGame.images.MagmaBlock;
        }
    }
}
