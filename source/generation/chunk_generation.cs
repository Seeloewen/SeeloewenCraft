using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public partial class Chunk
    {
        private void Generate()
        {
            world.log.Write($"Generating chunk {index}", "Info");

            blockList = new BlockList(this);

            //If it doesn't exist, create the file
            chunkDirectory = string.Format("{0}/chunk{1}", world.worldDirectory, index);

            //Generate terrain & structure
            GenerateTerrain();
            GenerateTrees();
            GenerateOres();
            if (world.settings.enableCaveGeneration) GenerateCaves();
            ContinueStructureGeneration();
        }

        private void GenerateTerrain()
        {

            //Set the floorheight based on the chunk index
            if (index >= 0)
            {
                if (index == 0)
                {
                    //If it's the first chunk, set the floor hight
                    floorHeight = rnd.Next(12, 15);
                }
                else
                {
                    //If it's not the first chunk, get the most right floor height from the chunk to the left
                    floorHeight = world.GetFromCurrentChunks(index - 1).floorHeightRight;
                }
            }
            else if (index < 0)
            {
                floorHeight = world.GetFromCurrentChunks(index + 1).floorHeightLeft;
            }

            //Actually generate the terrain
            if (index >= 0)
            {
                for (int x = 0; x <= 7; x++)
                {
                    //Go through all 8 columns in the chunk and generate a number to determine if the floor height should change
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
                    for (int y = 0; y <= 74; y++)
                    {

                        if (y == floorHeight)
                        {
                            //If the block is exactly on floor height add a grass block
                            blockList.Add(new GrassBlock(world, false), x, y);

                            //If it's at one of the corners, set the left or right floor height variable
                            if (x == 0)
                            {
                                floorHeightLeft = floorHeight;
                            }
                            if (x == 7)
                            {
                                floorHeightRight = floorHeight;
                            }
                        }
                        else if (y == floorHeight + 1 || y == floorHeight + 2)
                        {
                            //If it's 1 or 2 blocks below the floor height, add dirt
                            blockList.Add(new DirtBlock(world, false), x, y);
                        }
                        else if (y == floorHeight + 3)
                        {
                            //If it's 3 blocks below the floor height, it has an additional chance to generate dirt
                            int random = rnd.Next(1, 3);
                            if (random == 1)
                            {
                                blockList.Add(new DirtBlock(world, false), x, y);
                            }
                            else
                            {
                                blockList.Add(new StoneBlock(world, false), x, y);
                            }
                        }
                        else if (y > floorHeight + 3 && y < 74)
                        {
                            //If it's 3 blocks below floor height and above y 75, set stone blocks
                            blockList.Add(new StoneBlock(world, false), x, y);
                        }
                        else if (y < floorHeight)
                        {
                            //If it's above floor height, generate air
                            blockList.Add(new AirBlock(world, false), x, y);
                        }
                        else if (y == 74)
                        {
                            //If it's exactly at bottom layer y 75, set bedrock block
                            blockList.Add(new BedrockBlock(world, false), x, y);
                        }
                    }
                }
            }

            //Generate the chunk from right to left
            else
            {
                for (int x = 7; x >= 0; x--)
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
                    for (int y = 0; y <= 74; y++)
                    {

                        if (y == floorHeight)
                        {
                            //If the block is exactly on floor height add a grass block
                            blockList.Add(new GrassBlock(world, false), x, y);

                            //If it's at one of the corners, set the left or right floor height variable
                            if (x == 0)
                            {
                                floorHeightLeft = floorHeight;
                            }
                            if (x == 7)
                            {
                                floorHeightRight = floorHeight;
                            }
                        }
                        else if (y == floorHeight + 1 || y == floorHeight + 2)
                        {
                            //If it's 1 or 2 blocks below the floor height, add dirt
                            blockList.Add(new DirtBlock(world, false), x, y);
                        }
                        else if (y == floorHeight + 3)
                        {
                            //If it's 3 blocks below the floor height, it has an additional chance to generate dirt
                            int random = rnd.Next(1, 3);
                            if (random == 1)
                            {
                                blockList.Add(new DirtBlock(world, false), x, y);
                            }
                            else
                            {
                                blockList.Add(new DirtBlock(world, false), x, y);
                            }
                        }
                        else if (y > floorHeight + 3 && y < 74)
                        {
                            //If it's 3 blocks below floor height and above y 75, set stone blocks
                            blockList.Add(new StoneBlock(world, false), x, y);
                        }
                        else if (y < floorHeight)
                        {
                            //If it's above floor height, generate air
                            blockList.Add(new AirBlock(world, false), x, y);
                        }
                        else if (y == 74)
                        {
                            //If it's exactly at bottom layer y 75, set bedrock block
                            blockList.Add(new BedrockBlock(world, false), x, y);
                        }
                    }
                }
            }
        }

        private void GenerateTrees()
        {
            //Generate up to 3 trees
            for (int i = 0; i < 3; i++)
            {
                int random = rnd.Next(0, 3);
                if (random == 0)
                {
                    int xPos = rnd.Next(0, 8);
                    int yPos = 0;
                    foreach (Block block in blockList.blocks)
                    {
                        if (block.xPos == xPos && block is GrassBlock)
                            yPos = block.yPos - 1;
                    }

                    //Decide which tree to generate, mostly generate oak trees, rarely spruce
                    int random2 = rnd.Next(0, 6);
                    if (random2 == 0)
                    {
                        structureList.Add(new SpruceTreeStructure(world, xPos, yPos, index, true, this, false));
                    }
                    else
                    {
                        structureList.Add(new OakTreeStructure(world, xPos, yPos, index, true, this, false));
                    }

                }
            }
        }

        private void GenerateOres()
        {
            //Generate up to 15 ores
            for (int i = 0; i < 15; i++)
            {
                int random = rnd.Next(0, 3);
                if (random == 0)
                {
                    int xPos = rnd.Next(0, 8);
                    int yPos = 0;
                    foreach (Block block in blockList.blocks)
                    {
                        if (block != null && block.xPos == xPos && block is GrassBlock)
                        {
                            yPos = rnd.Next(block.yPos + 5, 70);
                        }
                    }
                    structureList.Add(new OreStructure(world, xPos, yPos, index, true, this, true));
                }
            }
        }

        private void GenerateCaves()
        {
            //Generate up to 1 cave
            int random = rnd.Next(1, 9);
            if (random == 1)
            {
                for (int i = 0; i < 1; i++)
                {
                    int xPos = rnd.Next(0, 8);
                    int yPos = 0;
                    foreach (Block block in blockList.blocks)
                    {
                        if (block.xPos == xPos && block is GrassBlock)
                        {
                            yPos = rnd.Next(block.yPos + 15, 70);
                        }
                    }
                    structureList.Add(new Cave(world, xPos, yPos, index, true, this, true));
                }
            }
        }

        private void ContinueStructureGeneration()
        {
            //Continue Structure Generation by adding a continuation strucutre, which contains the structure components that were previously cut off
            if (index > 0)
            {
                foreach (Structure structure in world.GetFromCurrentChunks(index - 1).structureList)
                {
                    if (structure.isCutOff)
                    {
                        structureList.Add(new ContinuationStructure(structure.cutOffComponents, world, 0, structure.yBase, index, true, this, structure.widthRemaining, structure.canFloat, structure.canReplaceSolidBlocks, structure.name));
                    }
                }
            }
            else if (index < 0)
            {
                foreach (Structure structure in world.GetFromCurrentChunks(index + 1).structureList)
                {
                    if (structure.isCutOff)
                    {
                        structureList.Add(new ContinuationStructure(structure.cutOffComponents, world, 7, structure.yBase, index, true, this, structure.widthRemaining, structure.canFloat, structure.canReplaceSolidBlocks, structure.name));
                    }
                }
            }
        }
    }
}
