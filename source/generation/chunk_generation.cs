using System.Windows;

namespace SeeloewenCraft
{
    public partial class Chunk
    {
        private void Generate()
        {
            blockList = new BlockList(this);
            chunkDirectory = string.Format("{0}/chunk{1}", Game.world.worldDirectory, index);

            //Determine the biome
            switch (index)
            {
                //Get the biome based on the previously generated chunk (except for chunk 0)
                case 0:
                    biome = GetNewBiome(Biome.None);
                    break;
                case > 0:
                    biome = GetNewBiome(Game.world.GetChunk(index - 1).biome);
                    break;
                case < 0:
                    biome = GetNewBiome(Game.world.GetChunk(index + 1).biome);
                    break;
            }

            Log.Write($"Generating chunk {index} with biome {biome}", LogType.WORLD_GENERATION, LogLevel.INFO);

            //Generate terrain & structure
            GenerateTerrain();
            GenerateStructues();
        }

        public Biome GetNewBiome(Biome adjacentBiome)
        {
            Biome newBiome = Biome.None;
            bool generateNewBiome = false;

            if (adjacentBiome != Biome.None)
            {
                //1 in 10 chance to generate a new biome
                if (seededRnd.Next(1, 10) == 1)
                {
                    generateNewBiome = true;
                }
            }

            if (generateNewBiome)
            {
                while (newBiome == Biome.None || newBiome == adjacentBiome)
                {
                    newBiome = GetRandomBiome();
                }

                Log.Write($"Determined new biome {newBiome} for chunk {index}", LogType.WORLD_GENERATION, LogLevel.INFO);
                return newBiome;
            }
            else if (!generateNewBiome && adjacentBiome != Biome.None)
            {
                Log.Write($"Determined biome {adjacentBiome} for chunk {index} based on adjacent chunk", LogType.WORLD_GENERATION, LogLevel.INFO);
                return adjacentBiome;
            }

            //Default biome
            return Biome.Plains;
        }

        public Biome GetRandomBiome()
        {
            //Generate a new biome based on random value
            switch (seededRnd.Next(1, 5))
            {
                case 1:
                    return Biome.Plains;
                case 2:
                    return Biome.Desert;
                case 3:
                    return Biome.Forest;
                case 4:
                    return Biome.SpruceForest;
                default:
                    return Biome.Plains;
            }
        }

        private void GenerateStructues()
        {
            if (index != 0) //Honestly no idea why I excluded chunk 0 - but don't fix something that's working, right?
            {
                //Generate structures
                if (biome != Biome.Desert) GenerateLakes();
                if (biome != Biome.Desert) GenerateTrees();
                if (biome == Biome.Desert) GenerateCacti();
                if (biome == Biome.SpruceForest) GenerateCottonFields();
                if (biome == Biome.Desert) GeneratePyramids();
                GenerateAbandonedFarm();
                //if (biome == Biome.Plains) GenerateAbandonedFarm();
                GenerateOres();
                GenerateCaves();
                if (biome == Biome.Desert) GenerateFossil();
                if (biome != Biome.Desert) GeneratePlainsDungeon();
                ContinueStructureGeneration("");
            }
        }

        private (int x, int y) GetCoordinates(int minX, int maxX, int minY, int maxY)
        {
            //Generate random coordinates in the specified range
            int x = seededRnd.Next(minX, maxX + 1);
            int y = seededRnd.Next(minY, maxY + 1);
            return (x, y);
        }

        private (int x, int y) GetCoordinatesOnSurface(int minX, int maxX, bool useFallBack)
        {
            //Generate random x coordinate
            int x = seededRnd.Next(minX, maxX + 1);
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

            if (useFallBack)
            {
                for (int i = 0; i < 74; i++)
                {
                    Block block = blockList.Get(x, i);

                    if (block.tags.Contains("canBeFloor"))
                    {
                        y = i;
                        break;
                    }
                }
            }

            return (x, y);
        }

        private void GenerateAbandonedFarm()
        {
            ContinueStructureGeneration("Abandoned Farm");

            //Generate up to 1 Abandoned Farm
            if (seededRnd.Next(0, 15) == 0)
            {
                (int x, int y) = GetCoordinatesOnSurface(0, 7, false);

                if (y != 0)
                {
                    structureList.Add(new AbandonedFarm(x, y, index, true, this, false));
                }
            }
        }

        private void GenerateCottonFields()
        {
            ContinueStructureGeneration("Cotton Field");

            //Generate up to 1 Cotton field
            if (seededRnd.Next(0, 15) == 0)
            {
                (int x, int y) = GetCoordinatesOnSurface(0, 7, false);

                if (y != 0)
                {
                    structureList.Add(new CottonFieldStructure(x, y + 1, index, true, this, false));
                }
            }
        }

        private void GeneratePyramids()
        {
            ContinueStructureGeneration("Pyramid");

            //Generate up to 1 Pyramid
            if (seededRnd.Next(0, 15) == 0)
            {
                (int x, int y) = GetCoordinatesOnSurface(0, 7, false);

                if (y != 0)
                {
                    structureList.Add(new PyramidStructure(x, y - 1, index, true, this, false));
                }
            }
        }

        private void GenerateTrees()
        {
            ContinueStructureGeneration("Spruce Tree");
            ContinueStructureGeneration("Oak Tree");

            //Generate up to 3 trees
            for (int i = 0; i < (biome == Biome.Plains ? 1 : 3); i++)
            {
                if (seededRnd.Next(0, biome == Biome.Plains ? 8 : 3) == 0)
                {
                    (int x, int y) = GetCoordinatesOnSurface(0, 7, false);

                    //Decide which tree to generate, depending on biome
                    if (y != 0)
                    {
                        if (biome == Biome.Plains || biome == Biome.Forest)
                        {
                            structureList.Add(new OakTreeStructure(x, y - 1, index, true, this, false));
                        }
                        else if (biome == Biome.SpruceForest)
                        {
                            structureList.Add(new SpruceTreeStructure(x, y - 1, index, true, this, false));
                        }
                    }
                }
            }
        }

        private void GenerateLakes()
        {
            ContinueStructureGeneration("Lake");

            //Generate up to 1 lake
            if (seededRnd.Next(0, 6) == 0)
            {
                (int x, int y) = GetCoordinatesOnSurface(0, 7, false);

                if (y != 0)
                {
                    int depth = seededRnd.Next(3, 8);
                    structureList.Add(new Lake(x, y + depth, index, true, this, true, depth));
                }
            }
        }

        private void GenerateCacti()
        {
            ContinueStructureGeneration("Cactus");

            //Generate up to 1 cactus
            if (seededRnd.Next(0, 2) == 0)
            {
                (int x, int y) = GetCoordinatesOnSurface(0, 7, false);

                if (y != 0)
                {
                    structureList.Add(new CactusStructure(x, y - 1, index, true, this, false));
                }
            }
        }

        private void GenerateFossil()
        {
            ContinueStructureGeneration("Fossil");

            //Generate up to 1 fossil
            if (seededRnd.Next(0, 10) == 0)
            {
                (int x, int y) = GetCoordinatesOnSurface(0, 7, false);

                if (y != 0)
                {
                    structureList.Add(new FossilStructure(x, y + seededRnd.Next(20, 50), index, true, this, false));
                }
            }
        }

        private void GeneratePlainsDungeon()
        {
            ContinueStructureGeneration("Plains Dungeon");

            //Generate up to 1 plains dungeon
            if (seededRnd.Next(0, 25) == 0)
            {
                (int x, int y) = GetCoordinatesOnSurface(0, 7, false);

                if (y != 0)
                {
                    structureList.Add(new PlainsDungeon(x, seededRnd.Next(62, 72), index, true, this, true));
                }
            }
        }

        private void GenerateOres()
        {
            //Continue generation of previous veines
            ContinueStructureGeneration("Coal Ore Vein");
            ContinueStructureGeneration("Iron Ore Vein");
            ContinueStructureGeneration("Diamond Ore Vein");
            ContinueStructureGeneration("Copper Ore Vein");
            ContinueStructureGeneration("Gold Ore Vein");
            ContinueStructureGeneration("Emerald Ore Vein");
            ContinueStructureGeneration("Tungsten Ore Vein");
            ContinueStructureGeneration("Tin Ore Vein");
            ContinueStructureGeneration("Amethyst Ore Vein");

            //Generate up to 4 coal ore veines
            for (int i = 0; i < 4; i++)
            {
                if (seededRnd.Next(0, 2) == 0)
                {
                    int y = 25;
                    (int x, y) = GetCoordinatesOnSurface(0, 7, true);
                    structureList.Add(new CoalOreStructure(x, y + seededRnd.Next(10, 70), index, true, this, true));
                }
            }

            //Generate up to 4 iron ore veines
            for (int i = 0; i < 4; i++)
            {
                if (seededRnd.Next(0, 3) == 0)
                {
                    int y = 25;
                    (int x, y) = GetCoordinatesOnSurface(0, 7, true);
                    structureList.Add(new IronOreStructure(x, y + seededRnd.Next(10, 70), index, true, this, true));
                }
            }

            //Generate up to 2 diamond ore veines
            for (int i = 0; i < 2; i++)
            {
                if (seededRnd.Next(0, 3) == 0)
                {
                    int y = 25;
                    (int x, y) = GetCoordinatesOnSurface(0, 7, true);
                    structureList.Add(new DiamondOreStructure(x, y + seededRnd.Next(50, 70), index, true, this, true));
                }
            }

            //Generate up to 1 amethyst ore veines
            if (seededRnd.Next(0, 3) == 0)
            {
                int y = 25;
                (int x, y) = GetCoordinatesOnSurface(0, 7, true);
                structureList.Add(new AmethystOreStructure(x, y + seededRnd.Next(45, 60), index, true, this, true));
            }


            //Generate up to 1 emerald ore veines
            if (seededRnd.Next(0, 3) == 0)
            {
                int y = 25;
                (int x, y) = GetCoordinatesOnSurface(0, 7, true);
                structureList.Add(new EmeraldOreStructure(x, y + seededRnd.Next(50, 70), index, true, this, true));
            }

            //Generate up to 2 gold ore veines
            for (int i = 0; i < 2; i++)
            {
                if (seededRnd.Next(0, 2) == 0)
                {
                    int y = 25;
                    (int x, y) = GetCoordinatesOnSurface(0, 7, true);
                    structureList.Add(new GoldOreStructure(x, y + seededRnd.Next(40, 70), index, true, this, true));
                }
            }

            //Generate up to 3 tin ore veines
            for (int i = 0; i < 3; i++)
            {
                if (seededRnd.Next(0, 3) == 0)
                {
                    int y = 25;
                    (int x, y) = GetCoordinatesOnSurface(0, 7, true);
                    structureList.Add(new TinOreStructure(x, y + seededRnd.Next(10, 70), index, true, this, true));
                }
            }

            //Generate up to 3 tungsten ore veines
            for (int i = 0; i < 3; i++)
            {
                if (seededRnd.Next(0, 3) == 0)
                {
                    int y = 25;
                    (int x, y) = GetCoordinatesOnSurface(0, 7, true);
                    structureList.Add(new TungstenOreStructure(x, y + seededRnd.Next(30, 50), index, true, this, true));
                }
            }

            //Generate up to 3 copper ore veines
            for (int i = 0; i < 3; i++)
            {
                if (seededRnd.Next(0, 3) == 0)
                {
                    int y = 25;
                    (int x, y) = GetCoordinatesOnSurface(0, 7, true);
                    structureList.Add(new CopperOreStructure(x, y + seededRnd.Next(10, 40), index, true, this, true));
                }
            }
        }

        private void GenerateCaves()
        {
            ContinueStructureGeneration("Cave1");

            //Generate up to 1 cave
            if (seededRnd.Next(0, 8) == 0)
            {
                (int x, int y) = GetCoordinatesOnSurface(0, 7, true);

                structureList.Add(new Cave(x, y + 30, index, true, this, true));
            }
        }

        public void ContinueStructureGeneration(string structureName)
        {
            //If an ID is given, only generate structures with that id
            if (!string.IsNullOrEmpty(structureName))
            {
                //Continue Structure Generation by adding a continuation strucutre, which contains the structure components that were previously cut off
                if (index != 0)
                {
                    foreach (Structure structure in Game.world.GetChunk(index + (index < 0 ? 1 : -1)).structureList)
                    {
                        //Check if the structure in the list is actually cut off and matches the id
                        if (structure.isCutOff && structure.name == structureName && !continuedStructureList.Contains(structure))
                        {
                            structureList.Add(new ContinuationStructure(structure.cutOffComponents, index < 0 ? 7 : 0, structure.yBase, index, true, this, structure.widthRemaining, structure.canFloat, structure.canReplaceSolidBlocks, structure.name));
                            continuedStructureList.Add(structure);
                        }
                    }
                }
            }
            else //If no id is given, generate all remaining structures in the list
            {
                foreach (Structure structure in Game.world.GetChunk(index + (index < 0 ? 1 : -1)).structureList)
                {
                    //Check if the structure in the list is actually cut off and matches the id
                    if (structure.isCutOff && !continuedStructureList.Contains(structure))
                    {
                        structureList.Add(new ContinuationStructure(structure.cutOffComponents, index < 0 ? 7 : 0, structure.yBase, index, true, this, structure.widthRemaining, structure.canFloat, structure.canReplaceSolidBlocks, structure.name));
                        continuedStructureList.Add(structure);
                    }
                }
            }
        }
    }
}
