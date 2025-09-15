using SeeloewenCraft.game.core.blocks;

namespace SeeloewenCraft.game.core.world
{
    public partial class Chunk
    {
        public enum Biome
        {
            None,
            Plains,
            Forest,
            SpruceForest,
            Desert
        }

        private void GenerateTerrain()
        {
            //Set floorheight
            switch (index)
            {
                case < 0: //Right-To-Left chunk
                    floorHeight = World.Get().GetChunk(index + 1).floorHeightLeft;
                    break;
                case 0: //Initial chunk (Left-To-Right)
                    floorHeight = seededRnd.Next(12, 15);
                    break;
                case > 0: //Left-To-Right chunk
                    floorHeight = World.Get().GetChunk(index - 1).floorHeightRight;
                    break;
            }

            //Generate the chunk
            if (index >= 0)
            {
                //Begin generating terrain from left to right
                for (int x = 0; x <= 7; x++)
                {
                    switch (biome)
                    {
                        case Biome.Plains:
                            GeneratePlains(x);
                            break;
                        case Biome.Desert:
                            GenerateDesert(x);
                            break;
                        case Biome.SpruceForest:
                            GeneratePlains(x); //Uses plains mechanism, only structures vary
                            break;
                        case Biome.Forest:
                            GeneratePlains(x); //Uses plains mechanism, only structures vary
                            break;
                    }
                }
            }
            else if (index < 0)
            {
                //Begin generating terrain from right to left
                for (int x = 7; x >= 0; x--)
                {
                    switch (biome)
                    {
                        case Biome.Plains:
                            GeneratePlains(x);
                            break;
                        case Biome.Desert:
                            GenerateDesert(x);
                            break;
                        case Biome.SpruceForest:
                            GeneratePlains(x); //Uses plains mechanism, only structures vary
                            break;
                        case Biome.Forest:
                            GeneratePlains(x); //Uses plains mechanism, only structures vary
                            break;
                    }
                }
            }
        }

        private void GeneratePlains(int x)
        {
            //Go through all 8 columns in the chunk and generate a number to determine if the floor height should change
            int floorHeightChange = seededRnd.Next(0, 100);
            if (floorHeightChange >= 80 && floorHeightChange <= 100 && floorHeight >= 10)
            {
                floorHeight--;
            }
            else if (floorHeightChange >= 60 && floorHeightChange < 80 && floorHeight <= 18)
            {
                floorHeight++;
            }

            //Go through each row
            for (int y = 0; y <= 74; y++)
            {
                if (y == floorHeight)
                {
                    //If the block is exactly on floor height add a grass block
                    Block b = BlockRegister.Get("sc:grass_block");
                    b.isSurface = true;
                    blockList.Add(b, x, y);

                    //If it's at one of the corners, set the left or right floor height variable
                    if (x == 0) floorHeightLeft = floorHeight;
                    if (x == 7) floorHeightRight = floorHeight;
                }
                else if (y == floorHeight - 1 && seededRnd.Next(1, 3) == 1)
                {
                    //If it's 1 above, potentially add crop, flower or grass
                    int random = seededRnd.Next(0, 100);
                    if (random >= 0 && random <= 5) //Crop
                    {
                        blockList.Add(GetRandomCrop(), x, y);
                    }
                    else if (random > 5 && random <= 25) //Flower
                    {
                        blockList.Add(GetRandomFlower(), x, y);
                    }
                    else //Grass 
                    {
                        blockList.Add(BlockRegister.Get("sc:grass"), x, y);
                    }
                }
                else if (y == floorHeight + 1 || y == floorHeight + 2)
                {
                    //If it's 1 or 2 blocks below the floor height, add dirt
                    blockList.Add(BlockRegister.Get("sc:dirt_block"), x, y);
                }
                else if (y == floorHeight + 3)
                {
                    //If it's 3 blocks below the floor height, it has an additional chance to generate dirt
                    if (seededRnd.Next(1, 3) == 1) blockList.Add(BlockRegister.Get("sc:dirt_block"), x, y);
                    else blockList.Add(BlockRegister.Get("sc:stone_block"), x, y);
                }
                else if (y > floorHeight + 3 && y < 74)
                {
                    //If it's 3 blocks below floor height and above y 75, set stone blocks
                    blockList.Add(BlockRegister.Get("sc:stone_block"), x, y);
                }
                else if (y == 74)
                {
                    //If it's exactly at bottom layer y 75, set bedrock block
                    blockList.Add(BlockRegister.Get("sc:bedrock_block"), x, y);
                }
                else
                {
                    //If it's above floor height, generate air
                    blockList.Add(BlockRegister.Get("sc:air_block"), x, y);
                }
            }
        }

        public Block GetRandomCrop()
        {
            CropBlock b = (CropBlock)(BlockRegister.Get(seededRnd.Next(0, 6) switch
            {
                0 => "sc:berry_bush_crop_block",
                1 => "sc:carrot_crop_block",
                2 => "sc:pumpkin_crop_block",
                3 => "sc:cabbage_crop_block",
                4 => "sc:tomato_crop_block",
                5 => "sc:cucumber_crop_block",
                _ => "sc:grass"
            }));

            b.progress = int.MaxValue;
            b.needsGround = (true, BlockTags.GROUNDS_DIRT);

            return b;
        }

        public Block GetRandomFlower()
        {
            return BlockRegister.Get(seededRnd.Next(0, 2) switch
            {
                0 => "sc:yellow_flower_block",
                1 => "sc:blue_flower_block",
                _ => "sc:grass_block"
            });
        }

        private void GenerateDesert(int x)
        {
            //Go through all 8 columns in the chunk and generate a number to determine if the floor height should change
            int floorHeightChange = seededRnd.Next(0, 100);
            if (floorHeightChange >= 80 && floorHeightChange <= 100 && floorHeight >= 10)
            {
                floorHeight--;
            }
            else if (floorHeightChange >= 60 && floorHeightChange < 80 && floorHeight <= 18)
            {
                floorHeight++;
            }

            //Go through each row
            for (int y = 0; y <= 74; y++)
            {
                if (y == floorHeight)
                {
                    //If the block is exactly on floor height add a sand block
                    Block b = BlockRegister.Get("sc:sand_block");
                    b.isSurface = true;
                    blockList.Add(b, x, y);

                    //If it's at one of the corners, set the left or right floor height variable
                    if (x == 0) floorHeightLeft = floorHeight;
                    if (x == 7) floorHeightRight = floorHeight;
                }
                else if (y == floorHeight - 1 && seededRnd.Next(1, 5) == 1)
                {
                    //If it's 1 above, potentially add grass or a flower
                    blockList.Add(BlockRegister.Get("sc:dead_bush_block"), x, y);
                }
                else if (y == floorHeight + 1)
                {
                    //If it's 1 or 2 blocks below the floor height, add sand
                    blockList.Add(BlockRegister.Get("sc:sand_block"), x, y);
                }
                else if (y == floorHeight + 2)
                {
                    //If it's 3 blocks below the floor height, it has an additional chance to generate sand
                    if (seededRnd.Next(1, 3) == 1) blockList.Add(BlockRegister.Get("sc:sand_block"), x, y);
                    else blockList.Add(BlockRegister.Get("sc:sand_stone_block"), x, y);
                }
                else if (y > floorHeight + 2 && y <= floorHeight + 10)
                {
                    //If it's between 3 and 10 blocks below floor height, set sandstone blocks
                    blockList.Add(BlockRegister.Get("sc:sand_stone_block"), x, y);
                }
                else if (y > floorHeight + 10)
                {
                    //If it's 11 blocks below floor height and above y 75, set stone blocks
                    blockList.Add(BlockRegister.Get("sc:stone_block"), x, y);
                }
                else if (y == 74)
                {
                    //If it's exactly at bottom layer y 75, set bedrock block
                    blockList.Add(BlockRegister.Get("sc:bedrock_block"), x, y);
                }
                else
                {
                    //If it's above floor height, generate air
                    blockList.Add(BlockRegister.Get("sc:air_block"), x, y);
                }
            }
        }
    }
}
