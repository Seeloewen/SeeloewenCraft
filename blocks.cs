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
        public ImageBrush imageBrush = new ImageBrush();
        public Border bdrBlock = new Border();
        public Canvas cvsBlock = new Canvas();
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

            //Setup the block canvas and border
            bdrBlock.Width = 50;
            bdrBlock.Height = 50;
            bdrBlock.Child = cvsBlock;
            cvsBlock.Background = imageBrush;
            cvsBlock.MouseLeftButtonDown += cvsBlock_MouseLeftButtonDown;
            cvsBlock.MouseRightButtonDown += cvsBlock_MouseRightButtonDown;
            cvsBlock.MouseEnter += cvsBlock_MouseEnter;
            cvsBlock.MouseLeave += cvsBlock_MouseLeave;
        }

        

        private bool isCollidingWithPlayer(object element)
        {
            if(element is Canvas canvas)
            {
                //Check for collision
                if (wndGame.GetRectangle(wndGame.player.cvsPlayerHitbox).IntersectsWith(wndGame.GetRectangle(cvsBlock)))
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
            cvsBlock.Children.Add(new TextBlock { Text = string.Format("x: {0} y:{1}\n{2}\n{3}", xPos, yPos, GetType().ToString().Replace("SeeloewenCraft.", "").Replace("Block", ""), chunk.index) });
        }

        public void HideBlockInfo()
        {
            //Hide the block info
            cvsBlock.Children.Clear();
        }
        public ImageSource GetImageSource(string assemblyName, string resourceName)
        {
            //Get an image from an imagesource from a uri
            Uri imageUri = new Uri(string.Format("pack://application:,,,/{0};component/Resources/{1}", assemblyName, resourceName), UriKind.Absolute);
            return BitmapFrame.Create(imageUri);
        }

        public bool IsInRange()
        {
            //Convert positions to screen coordinates
            Point playerScreenPoint = wndGame.player.cvsPlayer.PointToScreen(new Point(0, 0));
            Point otherScreenPoint = bdrBlock.PointToScreen(new Point(0, 0));

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
                bdrBlock.BorderThickness = new Thickness(1, 1, 1, 1);
                bdrBlock.BorderBrush = new SolidColorBrush(Colors.Black);
            }
        }

        private void cvsBlock_MouseLeave(object sender, EventArgs e)
        {
            //Remove the border from the block
            bdrBlock.BorderThickness = new Thickness(0, 0, 0, 0);
            bdrBlock.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void cvsBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Check if the block is both breakable and in range
            if (isBreakable == true && IsInRange() == true)
            {
                //Remove the block from the chunks blocklist and add an airblock
                chunk.blockList.Remove(this);
                chunk.blockList.Add(new AirItem(wndGame, 0).GenerateBlock(xPos, yPos, chunk));
                chunk.grdChunk.Children.Add(chunk.blockList[chunk.blockList.Count - 1].bdrBlock);
                Grid.SetRow(chunk.blockList[chunk.blockList.Count - 1].bdrBlock, chunk.blockList[chunk.blockList.Count - 1].yPos - 1);
                Grid.SetColumn(chunk.blockList[chunk.blockList.Count - 1].bdrBlock, chunk.blockList[chunk.blockList.Count - 1].xPos - 1);

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
                            slot.slot.items[slot.slot.items.Count - 1].block = slot.slot.items[slot.slot.items.Count - 1].GenerateBlock(0, 0, chunk) ;
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

                            //Remove the border from its parent
                            wndGame.RemoveFromParent(slot.slot.items[slot.slot.items.Count - 1].block.bdrBlock);

                            //Add the border to the chunk
                            chunk.grdChunk.Children.Add(slot.slot.items[slot.slot.items.Count - 1].block.bdrBlock);
                            Grid.SetRow(slot.slot.items[slot.slot.items.Count - 1].block.bdrBlock, slot.slot.items[slot.slot.items.Count - 1].block.yPos - 1);
                            Grid.SetColumn(slot.slot.items[slot.slot.items.Count - 1].block.bdrBlock, slot.slot.items[slot.slot.items.Count - 1].block.xPos - 1);

                            //Remove the item from the inventory
                            wndGame.player.inventory.RemoveItem(slot.slot.items[slot.slot.items.Count - 1]);
                        }
                    }
                }
            }
            else if(IsInRange() == true && isSolid == true && hasInventory == true)
            {
                Canvas.SetTop(wndGame.player.inventory.grdInventory, -10);
                wndGame.player.inventory.ShowInventory();
                blockInventory.ShowInventory();
            }
        }   
    }

    public class GrassBlock : Block
    {
        public GrassBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "GrassBlock.png");
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock(wndGame wndGame,  int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "StoneBlock.png");
        }
    }

    public class DirtBlock : Block
    {
        public DirtBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "DirtBlock.png");
        }
    }

    public class AirBlock : Block
    {
        public AirBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            isBreakable = false;
            isSolid = false;
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "AirBlock.png");
        }
    }

    public class BedrockBlock : Block
    {
        public BedrockBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            isBreakable = false;
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "BedrockBlock.png");
        }
    }

    public class CoalOreBlock : Block
    {
        public CoalOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "CoalOreBlock.png");
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "DiamondOreBlock.png");
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "IronOreBlock.png");
        }
    }

    public class OakLogBlock : Block
    {
        public OakLogBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "OakLogBlock.png");
        }
    }

    public class OakLeavesBlock : Block
    {
        public OakLeavesBlock(wndGame wndGame, int x, int y, Chunk chunk, Item item) : base(wndGame, x, y, chunk, item)
        {
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "OakLeavesBlock.png");
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
            imageBrush.ImageSource = GetImageSource("SeeloewenCraft", "ChestBlock.png");
        }
    }
}
