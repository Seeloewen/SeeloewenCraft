
namespace SeeloewenCraft
{
    public partial class Chunk
    {
        private void Generate()
        {
            blockList = new BlockList(this);
            chunkDirectory = string.Format("{0}/chunk{1}", world.worldDirectory, index);

            //Determine the biome
            switch (index)
            {
                //Get the biome based on the previously generated chunk (except for chunk 0)
                case 0:
                    biome = GetNewBiome(Biome.None);
                    break;
                case > 0:
                    biome = GetNewBiome(world.GetLoadedChunk(index - 1).biome);
                    break;
                case < 0:
                    biome = GetNewBiome(world.GetLoadedChunk(index + 1).biome);
                    break;
            }

            world.log.Write($"Generating chunk {index} with biome {biome}", "Info");

            //Generate terrain & structure
            GenerateTerrain();
            GenerateStructues();
        }

        public Biome GetNewBiome(Biome adjacentBiome)
        {
            bool newBiome = true;

            if (adjacentBiome != Biome.None)
            {
                //1 in 10 chance to generate a new biome
                int random = rnd.Next(1, 11);
                if (random == 0)
                {
                    newBiome = true;
                }
            }

            if (newBiome)
            {
                //Generate a new biome based on random value
                int random = rnd.Next(1, 2);
                switch (random)
                {
                    case 1:
                        return Biome.Plains;
                }
            }
            else
            {
                return adjacentBiome;
            }

            //Default biome
            return Biome.Plains;
        }

        private void GenerateStructues()
        {
            if (index != 0)
            {
                //Generate structures
                GenerateLakes();
                GenerateTrees();
                GenerateOres();
                if (Settings.enableCaveGeneration) GenerateCaves();
                ContinueStructureGeneration();
            }
        }

        private (int x, int y) GetCoordinates(int minX, int maxX, int minY, int maxY)
        {
            //Generate random coordinates in the specified range
            int x = rnd.Next(minX, maxX + 1);
            int y = rnd.Next(minY, maxY + 1);
            return (x, y);
        }

        private (int x, int y) GetCoordinatesOnSurface(int minX, int maxX)
        {
            //Generate random x coordinate
            int x = rnd.Next(minX, maxX + 1);
            int y = 0;

            //Get y coordinate of first surface airblock
            for (int i = 0; i < 74; i++)
            {
                if (blockList.Get(x, i).isSurface)
                {
                    y = i;
                    break;
                }
            }
            return (x, y);
        }

        private void GenerateTrees()
        {
            //Generate up to 3 trees
            for (int i = 0; i < 3; i++)
            {
                if (rnd.Next(0, 3) == 0)
                {
                    (int x, int y) = GetCoordinatesOnSurface(0, 7);

                    //Decide which tree to generate, mostly generate oak trees, rarely spruce
                    if (rnd.Next(0, 6) == 0)
                    {
                        structureList.Add(new SpruceTreeStructure(world, x, y - 1, index, true, this, false));
                    }
                    else
                    {
                        structureList.Add(new OakTreeStructure(world, x, y - 1, index, true, this, false));
                    }

                }
            }
        }

        private void GenerateLakes()
        {
            //Generate up to 1 lake
            if (rnd.Next(0, 5) == 0)
            {
                (int x, int y) = GetCoordinatesOnSurface(0, 7);

                int floorHeight = rnd.Next(3, 8);
                structureList.Add(new Lake(world, x, y + floorHeight, index, true, this, true, floorHeight));
            }
        }


        private void GenerateOres()
        {
            //Generate up to 15 ores
            for (int i = 0; i < 15; i++)
            {
                if (rnd.Next(0, 3) == 0)
                {
                    (int x, int y) = GetCoordinatesOnSurface(0, 7);

                    structureList.Add(new OreStructure(world, x, y + rnd.Next(10, 70), index, true, this, true));
                }
            }
        }

        private void GenerateCaves()
        {
            //Generate up to 1 cave
            if (rnd.Next(0, 8) == 0)
            {
                (int x, int y) = GetCoordinatesOnSurface(0, 7);

                structureList.Add(new Cave(world, x, y + 15, index, true, this, true));
            }
        }

        private void ContinueStructureGeneration()
        {
            //Continue Structure Generation by adding a continuation strucutre, which contains the structure components that were previously cut off
            if (index != 0)
            {
                foreach (Structure structure in world.GetLoadedChunk(index + (index < 0 ? 1 : -1)).structureList)
                {
                    if (structure.isCutOff)
                    {
                        structureList.Add(new ContinuationStructure(structure.cutOffComponents, world, index < 0 ? 7 : 0, structure.yBase, index, true, this, structure.widthRemaining, structure.canFloat, structure.canReplaceSolidBlocks, structure.name));
                    }
                }
            }
        }
    }
}
