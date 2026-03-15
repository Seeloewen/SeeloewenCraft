using LibNoise;
using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.modules.util;
using System;
using System.Collections.Generic;

namespace SeeloewenCraft.game.core.world
{
    public partial class Chunk
    {
        public static readonly Biome[,] biomeTable = // ^ humidity < temperature
        {
            { Biome.Forest,  Biome.SpruceForest},
            { Biome.Desert,  Biome.Plains}
        };

        public readonly Dictionary<Biome, int> amplitudeMap = new()
        {
            { Biome.Forest, 5 },
            { Biome.SpruceForest, 10 },
            { Biome.Plains, 3 },
            { Biome.Desert, 5 },
        };

        public enum Biome
        {
            None,
            Plains,
            Forest,
            SpruceForest,
            Desert
        }

        private static Biome GetBiomeAt(int x)
        {
            float temperature = World.Get().temperatureNoise.GetValue((x) * 0.01f);
            float humidity = World.Get().humidityNoise.GetValue((x) * 0.01f);
            int t = (int)Math.Round(SMath.NormalizePerlin(temperature));
            int h = (int)Math.Round(SMath.NormalizePerlin(humidity));

            return biomeTable[h, t];
        }

        private void GenerateTerrain()
        {
            //Generate the chunk
            //Begin generating terrain from left to right
            for (int x = 0; x <= 7; x++)
            {
                biome = GetBiomeAt(index * 8 + x);

                //Check how far away to the left the last biome is
                Biome leftBiome = biome;
                int leftX = x - 1;
                while (leftBiome == biome)
                {
                    leftBiome = GetBiomeAt(leftX);
                    leftX--;
                }

                //Calculate how strong the blending should be
                float blendFactor = Math.Clamp((x - leftX) / 5f, 0f, 1f);
                float amplitude = Libnoise.Lerp(amplitudeMap[leftBiome], amplitudeMap[biome], blendFactor);

                switch (biome)
                {
                    case Biome.Plains:
                        GeneratePlains(x, amplitude);
                        break;
                    case Biome.Desert:
                        GenerateDesert(x, amplitude);
                        break;
                    case Biome.SpruceForest:
                        GeneratePlains(x, amplitude); //Uses plains mechanism, only structures vary
                        break;
                    case Biome.Forest:
                        GeneratePlains(x, amplitude); //Uses plains mechanism, only structures vary
                        break;
                }
            }
        }

        private void GeneratePlains(int x, float amplitude)
        {
            float noise = 0.5f * World.Get().heightNoise.GetValue((index * 8 + x) * 0.02f, 0);
            int floorHeight = 12 + (int)Math.Floor(noise * amplitude);

            //Go through each row
            for (int y = 0; y <= 74; y++)
            {
                if (y == floorHeight)
                {
                    //If the block is exactly on floor height add a grass block
                    Block b = BlockRegister.Get("sc:grass_block");
                    b.isSurface = true;
                    blockList.Add(b, x, y);
                }
                else if (y == floorHeight - 1 && chunkRnd.Next(1, 3) == 1)
                {
                    //If it's 1 above, potentially add crop, flower or grass
                    int random = chunkRnd.Next(0, 100);
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
                    if (chunkRnd.Next(1, 3) == 1) blockList.Add(BlockRegister.Get("sc:dirt_block"), x, y);
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
            CropBlock b = (CropBlock)(BlockRegister.Get(chunkRnd.Next(0, 6) switch
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
            return BlockRegister.Get(chunkRnd.Next(0, 2) switch
            {
                0 => "sc:yellow_flower_block",
                1 => "sc:blue_flower_block",
                _ => "sc:grass_block"
            });
        }

        private void GenerateDesert(int x, float amplitude)
        {
            float noise = 0.5f * World.Get().heightNoise.GetValue((index * 8 + x) * 0.02f, 0);
            int floorHeight = 12 + (int)Math.Floor(noise * amplitude);

            //Go through each row
            for (int y = 0; y <= 74; y++)
            {
                if (y == floorHeight)
                {
                    //If the block is exactly on floor height add a sand block
                    Block b = BlockRegister.Get("sc:sand_block");
                    b.isSurface = true;
                    blockList.Add(b, x, y);
                }
                else if (y == floorHeight - 1 && chunkRnd.Next(1, 5) == 1)
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
                    if (chunkRnd.Next(1, 3) == 1) blockList.Add(BlockRegister.Get("sc:sand_block"), x, y);
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
