using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace SeeloewenCraft
{
    public class Chunk
    {
        public List<Block> blockList = new List<Block>();
        public List<Structure> structureList = new List<Structure>();
        public blockContainerList blockContainerList;
        public Grid grdChunk = new Grid();
        private Random rnd = new Random(DateTime.Now.Millisecond);
        wndGame wndGame;
        public int index;
        public int floorHeightRight;
        public int floorHeightLeft;
        string chunkDirectory;

        public Chunk(wndGame wndGame, int index)
        {
            //Set the attributes
            this.wndGame = wndGame;
            this.index = index;

            //Begin loading the chunk
            chunkDirectory = string.Format("{0}/chunk{1}", wndGame.worldDirectory, index);

            LoadChunk();
        }

        public void SetBlock(Block block, int x, int y)
        {
            if(blockContainerList.GetContainer(x,y) != null)
            {
                blockContainerList.GetContainer(x, y).SetBlock(block);
            }
        }

        public void SaveChunk()
        {
            //Check if the chunk directory already exists and create it otherwise
            if (!Directory.Exists(chunkDirectory))
            {
                Directory.CreateDirectory(chunkDirectory);
            }
            File.WriteAllText(string.Format("{0}/chunk{1}/blocks.txt", wndGame.worldDirectory, index), "");
            foreach (Block block in blockList)
            {
                if (block.hasInventory == true)
                {
                    block.blockInventory.SaveInventory(chunkDirectory);
                }

                //Write all blocks in the chunks blocklist into a file
                File.AppendAllText(string.Format("{0}/chunk{1}/blocks.txt", wndGame.worldDirectory, index), string.Format("{0};{1};{2};{3}\n", block.GetType().ToString().Replace("SeeloewenCraft.", ""), block.xPos, block.yPos, block.item.id));
            }
            //Write the chunk settings into a file
            File.WriteAllText(string.Format("{0}/chunk{1}/settings.txt", wndGame.worldDirectory, index), string.Format("{0};{1};{2}", index, floorHeightLeft, floorHeightRight));
        }

        public void LoadChunk()
        {
            //Clear the chunk
            grdChunk.Children.Clear();
            blockList.Clear();

            //Setup the chunk
            grdChunk.Width = 400;
            grdChunk.Height = 3750;

            for (int i = 0; i < 8; i++)
            {
                grdChunk.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < 75; i++)
            {
                grdChunk.RowDefinitions.Add(new RowDefinition());
            }

            //Get the container list
            foreach (blockContainerList containerList in wndGame.blockContainerList)
            {
                if (containerList.IsAvailable())
                {
                    blockContainerList = containerList;
                    blockContainerList.AssignToChunk(this);
                    break;
                }
            }

            //Check if the chunk doesn't already exist
            if (!Directory.Exists(string.Format("{0}/chunk{1}", wndGame.worldDirectory, index)))
            {
                //If it doesn't exist, create the file
                chunkDirectory = string.Format("{0}/chunk{1}", wndGame.worldDirectory, index);

                //Generate terrain & structure
                GenerateTerrain();
                GenerateTrees();
                GenerateOres();
                GenerateCaves();
                ContinueStructureGeneration();


                //Go through each block and add it to the chunk
                try
                {
                    foreach (Block block in blockList)
                    {
                        SetBlock(block, block.xPos, block.yPos);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while loading chunk: {ex}");
                }


                //Save the chunk
                SaveChunk();
            }
            else
            {
                //Read the chunk from saved file
                string[] blocks = File.ReadAllLines(string.Format("{0}/chunk{1}/blocks.txt", wndGame.worldDirectory, index));

                //Read all blocks from the file
                foreach (string block in blocks)
                {
                    //This goes through every line and converts it to a block
                    string[] blockSplit = block.Split(';');

                    if (blockSplit[0] == "GrassBlock")
                    {
                        blockList.Add(new GrassItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                    else if (blockSplit[0] == "DirtBlock")
                    {
                        blockList.Add(new DirtItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                    else if (blockSplit[0] == "StoneBlock")
                    {
                        blockList.Add(new StoneItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                    else if (blockSplit[0] == "AirBlock")
                    {
                        blockList.Add(new AirItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                    else if (blockSplit[0] == "BedrockBlock")
                    {
                        blockList.Add(new BedrockItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                    else if (blockSplit[0] == "DiamondOreBlock")
                    {
                        blockList.Add(new DiamondOreItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                    else if (blockSplit[0] == "IronOreBlock")
                    {
                        blockList.Add(new IronOreItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                    else if (blockSplit[0] == "CoalOreBlock")
                    {
                        blockList.Add(new CoalOreItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                    else if (blockSplit[0] == "OakLogBlock")
                    {
                        blockList.Add(new OakLogItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                    else if (blockSplit[0] == "OakLeavesBlock")
                    {
                        blockList.Add(new OakLeavesItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                    else if (blockSplit[0] == "ChestBlock")
                    {
                        blockList.Add(new ChestItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                    else
                    {
                        blockList.Add(new AirItem(wndGame, Convert.ToInt32(blockSplit[3])).GenerateBlock(Convert.ToInt32(blockSplit[1]), Convert.ToInt32(blockSplit[2]), this));
                    }
                }

                //Load the inventories of the blocks in the chunk (like chests)
                LoadInventories();

                //Add all the blocks to the chunk
                try
                {
                    foreach (Block block in blockList)
                    {
                        SetBlock(block, block.xPos, block.yPos);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while loading chunk: {ex}");
                }

                //Read the chunk settings from the file
                string[] settings = File.ReadAllText(string.Format("{0}/chunk{1}/settings.txt", wndGame.worldDirectory, index)).Split(';');
                index = Convert.ToInt32(settings[0]);
                floorHeightLeft = Convert.ToInt32(settings[1]);
                floorHeightRight = Convert.ToInt32(settings[2]);
            }
        }
        public Block GetBlock(int x, int y)
        {
            //Go through each block and return the block that matches the coords
            foreach (Block block in blockList)
            {
                if (block.xPos == x && block.yPos == y)
                {
                    return block;
                }
            }
            return null;
        }

        public void SetBlock(int x, int y, Block block)
        {
            //Remove the block that is currently there
            blockList.Remove(GetBlock(x, y));
            blockList.Add(block);
        }
        public void LoadInventories()
        {

            //Go through each block
            foreach (Block block in blockList)
            {
                foreach (string dir in Directory.GetDirectories(chunkDirectory))
                {
                    //Check for each directory if the inventory id matches the block id
                    if (block.item.id == Convert.ToInt32(dir.Replace("inventory", "").Replace(chunkDirectory, "").Replace("\\", "")))
                    {
                        block.blockInventory.LoadInventory(chunkDirectory, block.item.id);
                    }
                }
            }
        }

        private void GenerateTerrain()
        {
            int floorHeight;

            //Generate the chunk from left to right
            if (index >= 0)
            {
                if (wndGame.chunkList.Count == 0)
                {
                    //If it's the first chunk, set the floor hight
                    floorHeight = rnd.Next(12, 15);
                }
                else
                {
                    //If it's not the first chunk, get the most right floor height from the chunk to the left
                    floorHeight = wndGame.GetChunk(index - 1).floorHeightRight;
                }
                for (int x = 1; x <= 8; x++)
                {
                    //Go through all 8 columns in the chunk and generate a number to determine if the floor height should c hange
                    int floorHeightChange = rnd.Next(0, 100);
                    if (floorHeightChange >= 80 && floorHeightChange <= 100)
                    {
                        //Only decrease the floor height if it's currently 10 or higher
                        if (floorHeight >= 10)
                        {
                            floorHeight--;
                        }
                    }
                    else if (floorHeightChange >= 60 && floorHeightChange < 80)
                    {
                        //Only increase the floor height if it's currently 18 or below
                        if (floorHeight <= 18)
                        {
                            floorHeight++;
                        }
                    }

                    //Go through each row
                    for (int y = 1; y <= 75; y++)
                    {

                        if (y == floorHeight)
                        {
                            //If the block is exactly on floor height add a grass block
                            blockList.Add(new GrassItem(wndGame, 0).GenerateBlock(x, y, this));

                            //If it's at one of the corners, set the left or right floor height variable
                            if (x == 1)
                            {
                                floorHeightLeft = floorHeight;
                            }
                            if (x == 8)
                            {
                                floorHeightRight = floorHeight;
                            }
                        }
                        else if (y == floorHeight + 1 || y == floorHeight + 2)
                        {
                            //If it's 1 or 2 blocks below the floor height, add dirt
                            blockList.Add(new DirtItem(wndGame, 0).GenerateBlock(x, y, this));
                        }
                        else if (y == floorHeight + 3)
                        {
                            //If it's 3 blocks below the floor height, it has an additional chance to generate dirt
                            int random = rnd.Next(1, 3);
                            if (random == 1)
                            {
                                blockList.Add(new DirtItem(wndGame, 0).GenerateBlock(x, y, this));
                            }
                            else
                            {
                                blockList.Add(new StoneItem(wndGame, 0).GenerateBlock(x, y, this));
                            }
                        }
                        else if (y > floorHeight + 3 && y < 75)
                        {
                            //If it's 3 blocks below floor height and above y 75, set stone blocks
                            blockList.Add(new StoneItem(wndGame, 0).GenerateBlock(x, y, this));
                        }
                        else if (y < floorHeight)
                        {
                            //If it's above floor height, generate air
                            blockList.Add(new AirItem(wndGame, 0).GenerateBlock(x, y, this));
                        }
                        else if (y == 75)
                        {
                            //If it's exactly at bottom layer y 75, set bedrock block
                            blockList.Add(new BedrockItem(wndGame, 0).GenerateBlock(x, y, this));
                        }
                    }
                }
            }

            //Generate the chunk from right to left
            else
            {
                floorHeight = wndGame.GetChunk(index + 1).floorHeightLeft;

                for (int x = 8; x > 0; x--)
                {
                    int floorHeightChange = rnd.Next(0, 100);
                    if (floorHeightChange >= 80 && floorHeightChange <= 100)
                    {
                        if (floorHeight >= 10)
                        {
                            floorHeight--;
                        }
                    }
                    else if (floorHeightChange >= 60 && floorHeightChange < 80)
                    {
                        if (floorHeight <= 18)
                        {
                            floorHeight++;

                        }
                    }

                    //Go through each row
                    for (int y = 1; y <= 75; y++)
                    {

                        if (y == floorHeight)
                        {
                            //If the block is exactly on floor height add a grass block
                            blockList.Add(new GrassItem(wndGame, 0).GenerateBlock(x, y, this));

                            //If it's at one of the corners, set the left or right floor height variable
                            if (x == 1)
                            {
                                floorHeightLeft = floorHeight;
                            }
                            if (x == 8)
                            {
                                floorHeightRight = floorHeight;
                            }
                        }
                        else if (y == floorHeight + 1 || y == floorHeight + 2)
                        {
                            //If it's 1 or 2 blocks below the floor height, add dirt
                            blockList.Add(new DirtItem(wndGame, 0).GenerateBlock(x, y, this));
                        }
                        else if (y == floorHeight + 3)
                        {
                            //If it's 3 blocks below the floor height, it has an additional chance to generate dirt
                            int random = rnd.Next(1, 3);
                            if (random == 1)
                            {
                                blockList.Add(new DirtItem(wndGame, 0).GenerateBlock(x, y, this));
                            }
                            else
                            {
                                blockList.Add(new StoneItem(wndGame, 0).GenerateBlock(x, y, this));
                            }
                        }
                        else if (y > floorHeight + 3 && y < 75)
                        {
                            //If it's 3 blocks below floor height and above y 75, set stone blocks
                            blockList.Add(new StoneItem(wndGame, 0).GenerateBlock(x, y, this));
                        }
                        else if (y < floorHeight)
                        {
                            //If it's above floor height, generate air
                            blockList.Add(new AirItem(wndGame, 0).GenerateBlock(x, y, this));
                        }
                        else if (y == 75)
                        {
                            //If it's exactly at bottom layer y 75, set bedrock block
                            blockList.Add(new BedrockItem(wndGame, 0).GenerateBlock(x, y, this));
                        }
                    }
                }
            }
        }


        private void GenerateTrees()
        {
            //Generate up to 3 trees
            //WIP - Structure Rework
            for (int i = 0; i < 3; i++)
            {
                int random = rnd.Next(0, 3);
                if (random == 0)
                {
                    int xPos = rnd.Next(0, 9);
                    int yPos = 0;
                    foreach (Block block in blockList)
                    {
                        if (block.xPos == xPos && block is GrassBlock)
                            yPos = block.yPos - 1;
                    }
                    structureList.Add(new TreeStructure(wndGame, xPos, yPos, index, true, this, false));
                }
            }
        }

        private void GenerateOres()
        {
            //Generate up to 15 ores
            //WIP - Structure Rework
            for (int i = 0; i < 15; i++)
            {
                int random = rnd.Next(0, 3);
                if (random == 0)
                {
                    int xPos = rnd.Next(0, 9);
                    int yPos = 0;
                    foreach (Block block in blockList)
                    {
                        if (block.xPos == xPos && block is GrassBlock)
                        {
                            yPos = rnd.Next(block.yPos + 5, 70);
                        }
                    }
                    structureList.Add(new OreStructure(wndGame, xPos, yPos, index, true, this, true));
                }
            }
        }

        private void GenerateCaves()
        {
            //Generate up to 1 cave
            //WIP - Structure Rework
            int random = rnd.Next(1, 9);
            if (random == 1)
            {
                for (int i = 0; i < 1; i++)
                {
                    int random2 = rnd.Next(2, 3);
                    if (random2 == 2)
                    {
                        int xPos = rnd.Next(0, 9);
                        int yPos = 0;
                        foreach (Block block in blockList)
                        {
                            if (block.xPos == xPos && block is GrassBlock)
                            {
                                yPos = rnd.Next(block.yPos + 15, 70);
                            }
                        }
                        structureList.Add(new Cave(wndGame, xPos, yPos, index, true, this, true));
                    }
                }
            }
        }

        private void ContinueStructureGeneration()
        {
            //Continue Structure Generation
            //WIP - Structure Rework
            if (index > 0)
            {
                foreach (Structure structure in wndGame.GetChunk(index - 1).structureList)
                {
                    if (structure.isCutOff)
                    {
                        structureList.Add(new ContinuationStructure(structure.cutOffComponents, wndGame, 1, structure.yBase, index, true, this, structure.widthRemaining, structure.canFloat));
                    }
                }

            }
            else if (index < 0)
            {
                foreach (Structure structure in wndGame.GetChunk(index + 1).structureList)
                {
                    if (structure.isCutOff)
                    {
                        structureList.Add(new ContinuationStructure(structure.cutOffComponents, wndGame, 8, structure.yBase, index, true, this, structure.widthRemaining, structure.canFloat));
                    }
                }
            }
        }
    }
}
