using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public partial class Chunk
    {
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

                //Begin generating terrain from left to right
                for (int x = 0; x <= 7; x++)
                {
                    switch (biome)
                    {
                        case Biome.Plains:
                            GeneratePlains(x);
                            break;
                    }
                }
            }
            else if (index < 0)
            {
                floorHeight = world.GetFromCurrentChunks(index + 1).floorHeightLeft;

                //Begin generating terrain from right to left
                for (int x = 7; x >= 0; x--)
                {
                    switch (biome)
                    {
                        case Biome.Plains:
                            GeneratePlains(x);
                            break;
                    }
                }
            }
        }

        private void GeneratePlains(int x)
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
                    blockList.Add(new GrassBlock(world, false) { isSurface = true }, x, y);

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
}
