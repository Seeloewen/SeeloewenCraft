using Microsoft.Json.Pointer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace SeeloewenCraft
{
    public class Chunk
    {
        public BlockList blockList;
        public List<Structure> structureList = new List<Structure>();
        public BlockContainerList blockContainerList;
        public Grid grdChunk = new Grid();
        private Random rnd = new Random(DateTime.Now.Millisecond);
        wndGame wndGame;
        public int index;
        int floorHeight; //Only used while generating
        public int floorHeightRight;
        public int floorHeightLeft;
        public string chunkDirectory;

        //-- Constructor --//

        public Chunk(wndGame wndGame, int index)
        {
            //Set the attributes
            this.wndGame = wndGame;
            this.index = index;

            //Begin loading the chunk
            chunkDirectory = string.Format("{0}/chunk{1}", wndGame.worldDirectory, index);
            Init();
        }

        //-- Custom Methods --//
        public void HideBlockInfo()
        {
            foreach (Block block in blockList.blocks)
            {
                block.HideBlockInfo();
            }
            wndGame.log.Write("Block info is now hidden!", "Info");
        }

        public void ShowBlockInfo()
        {
            foreach (Block block in blockList.blocks)
            {
                block.ShowBlockInfo();
            }
            wndGame.log.Write("Block info is now shown!", "Info");
        }

        public void Save()
        {
            //Check if the chunk directory already exists and create it otherwise
            if (!Directory.Exists(chunkDirectory))
            {
                Directory.CreateDirectory(chunkDirectory);
                wndGame.log.Write($"Created chunk directory {chunkDirectory}!", "Info");
            }


            //save blocks in blocks.json
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                blockList.saveToJson(writer);
            }
            File.WriteAllText($"{wndGame.worldDirectory}/chunk{index}/blocks.json", sw.ToString());


            //save settings in settings.json
            sb = new StringBuilder();
            sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                saveSettingsToJson(writer);
            }
            File.WriteAllText($"{wndGame.worldDirectory}/chunk{index}/settings.json", sb.ToString());

        }


        private void saveSettingsToJson(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("index");
            writer.WriteValue(index);

            writer.WritePropertyName("floor_height_left");
            writer.WriteValue(floorHeightLeft);

            writer.WritePropertyName("floor_height_right");
            writer.WriteValue(floorHeightRight);

            writer.WriteEndObject();
        }


        public void SetBlock(Block block, int x, int y)
        {
            //Check if the coordinate has a container and place the block into that container if possible
            if (x > 8)
            {
                Chunk chunk = wndGame.GetFromCurrentChunks(index + 1);

                if (chunk != null)
                {
                    chunk.SetBlock(block, x - 8, y);
                }
            }
            else if (x < 1)
            {
                Chunk chunk = wndGame.GetFromCurrentChunks(index - 1);

                if (chunk != null)
                {
                    chunk.SetBlock(block, x + 8, y);
                }
            }
            else
            {
                //Add the block to the block list
                block.xPos = x;
                block.yPos = y;
                block.chunk = this;
                blockList.Add(block);

                if (blockContainerList.GetContainer(x, y) != null)
                {
                    blockContainerList.GetContainer(x, y).SetBlock(block);
                }
                else
                {
                    Console.WriteLine($"[Error] Could not find container at x{x} y{y} for block {block.name}");
                }
            }
        }

        public void SetContainerList()
        {
            //Get the container list
            foreach (BlockContainerList containerList in wndGame.blockContainerList)
            {
                if (containerList.IsAvailable())
                {
                    blockContainerList = containerList;
                    blockContainerList.AssignToChunk(this);
                    break;
                }
            }
        }

        public void Init()
        {
            wndGame.log.Write($"Beginning to initialize chunk {index}", "Info");

            //Clear the chunk
            grdChunk.Children.Clear();

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
            SetContainerList();

            //Check if the chunk doesn't already exist
            if (Directory.Exists($"{wndGame.worldDirectory}/chunk{index}"))
            {
                Load();
            }
            else
            {
                Generate();
            }


            //render chunk
            try
            {
                RenderChunk();
                wndGame.log.Write($"Successfully initialized chunk {index}", "Info");
            }
            catch (Exception ex)
            {
                wndGame.log.Write($"Could not initialize chunk: {ex.Message}", "Error");
            }
        }


        private void Generate()
        {
            wndGame.log.Write($"Generating chunk {index}", "Info");

            blockList = new BlockList();

            //If it doesn't exist, create the file
            chunkDirectory = string.Format("{0}/chunk{1}", wndGame.worldDirectory, index);

            //Generate terrain & structure
            GenerateTerrain();
            GenerateTrees();
            GenerateOres();
            if (Properties.Settings.Default.enableCaveGeneration) GenerateCaves();
            ContinueStructureGeneration();

        }


        private void Load()
        {
            wndGame.log.Write($"Loading chunk {index}", "Info");

            //load blocklist
            string documentText = File.ReadAllText($"{wndGame.worldDirectory}/chunk{index}/blocks.json");
            JToken documentToken = JToken.Parse(documentText);

            blockList = BlockList.loadFromJson(documentToken, this, wndGame);


            //load settings
            documentText = File.ReadAllText($"{wndGame.worldDirectory}/chunk{index}/settings.json");
            documentToken = JToken.Parse(documentText);

            index = (int)new JsonPointer("/index").Evaluate(documentToken);
            floorHeightLeft = (int)new JsonPointer("/floor_height_left").Evaluate(documentToken);
            floorHeightRight = (int)new JsonPointer("/floor_height_right").Evaluate(documentToken);

            //Load the inventories of the blocks in the chunk (like chests)
            //LoadInventories();
        }



        public void RenderChunk()
        {
            foreach (Block block in blockList.blocks)
            {
                SetBlock(block, block.xPos, block.yPos);
            }
        }

        public Block GetBlock(int x, int y)
        {
            if (x > 8)
            {
                if (wndGame.GetFromCurrentChunks(index + 1) != null)
                {
                    return wndGame.GetFromCurrentChunks(index + 1).GetBlock(x - 8, y);
                }
                else
                {
                    return null;
                }    
            }
            else if (x < 1)
            {
                if(wndGame.GetFromCurrentChunks(index - 1) != null)
                {
                    return wndGame.GetFromCurrentChunks(index - 1).GetBlock(x + 8, y);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return blockList.Get(x, y);
            }
        }

        public void LoadInventories()
        {
            //Go through each block
            foreach (Block block in blockList.blocks)
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
                    floorHeight = wndGame.GetFromCurrentChunks(index - 1).floorHeightRight;
                }
            }
            else if (index < 0)
            {
                floorHeight = wndGame.GetFromCurrentChunks(index + 1).floorHeightLeft;
            }

            //Actually generate the terrain
            if (index >= 0)
            {
                for (int x = 1; x <= 8; x++)
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
                    for (int y = 1; y <= 75; y++)
                    {

                        if (y == floorHeight)
                        {
                            //If the block is exactly on floor height add a grass block
                            blockList.Add(new GrassBlock(wndGame, x, y, this, null, false));

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
                            blockList.Add(new DirtBlock(wndGame, x, y, this, null, false));
                        }
                        else if (y == floorHeight + 3)
                        {
                            //If it's 3 blocks below the floor height, it has an additional chance to generate dirt
                            int random = rnd.Next(1, 3);
                            if (random == 1)
                            {
                                blockList.Add(new DirtBlock(wndGame, x, y, this, null, false));
                            }
                            else
                            {
                                blockList.Add(new StoneBlock(wndGame, x, y, this, null, false));
                            }
                        }
                        else if (y > floorHeight + 3 && y < 75)
                        {
                            //If it's 3 blocks below floor height and above y 75, set stone blocks
                            blockList.Add(new StoneBlock(wndGame, x, y, this, null, false));
                        }
                        else if (y < floorHeight)
                        {
                            //If it's above floor height, generate air
                            blockList.Add(new AirBlock(wndGame, x, y, this, null, false));
                        }
                        else if (y == 75)
                        {
                            //If it's exactly at bottom layer y 75, set bedrock block
                            blockList.Add(new BedrockBlock(wndGame, x, y, this, null, false));
                        }
                    }
                }
            }

            //Generate the chunk from right to left
            else
            {
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
                            blockList.Add(new GrassBlock(wndGame, x, y, this, null, false));

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
                            blockList.Add(new DirtBlock(wndGame, x, y, this, null, false));
                        }
                        else if (y == floorHeight + 3)
                        {
                            //If it's 3 blocks below the floor height, it has an additional chance to generate dirt
                            int random = rnd.Next(1, 3);
                            if (random == 1)
                            {
                                blockList.Add(new DirtBlock(wndGame, x, y, this, null, false));
                            }
                            else
                            {
                                blockList.Add(new DirtBlock(wndGame, x, y, this, null, false));
                            }
                        }
                        else if (y > floorHeight + 3 && y < 75)
                        {
                            //If it's 3 blocks below floor height and above y 75, set stone blocks
                            blockList.Add(new StoneBlock(wndGame, x, y, this, null, false));
                        }
                        else if (y < floorHeight)
                        {
                            //If it's above floor height, generate air
                            blockList.Add(new AirBlock(wndGame, x, y, this, null, false));
                        }
                        else if (y == 75)
                        {
                            //If it's exactly at bottom layer y 75, set bedrock block
                            blockList.Add(new BedrockBlock(wndGame, x, y, this, null, false));
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
                    int xPos = rnd.Next(0, 9);
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
                        structureList.Add(new SpruceTreeStructure(wndGame, xPos, yPos, index, true, this, false));
                    }
                    else
                    {
                        structureList.Add(new OakTreeStructure(wndGame, xPos, yPos, index, true, this, false));
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
                    int xPos = rnd.Next(0, 9);
                    int yPos = 0;
                    foreach (Block block in blockList.blocks)
                    {
                        if (block != null && block.xPos == xPos && block is GrassBlock)
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
            int random = rnd.Next(1, 9);
            if (random == 1)
            {
                for (int i = 0; i < 1; i++)
                {
                    int xPos = rnd.Next(0, 9);
                    int yPos = 0;
                    foreach (Block block in blockList.blocks)
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

        private void ContinueStructureGeneration()
        {
            //Continue Structure Generation by adding a continuation strucutre, which contains the structure components that were previously cut off
            if (index > 0)
            {
                foreach (Structure structure in wndGame.GetFromCurrentChunks(index - 1).structureList)
                {
                    if (structure.isCutOff)
                    {
                        structureList.Add(new ContinuationStructure(structure.cutOffComponents, wndGame, 1, structure.yBase, index, true, this, structure.widthRemaining, structure.canFloat, structure.canReplaceSolidBlocks));
                    }
                }
            }
            else if (index < 0)
            {
                foreach (Structure structure in wndGame.GetFromCurrentChunks(index + 1).structureList)
                {
                    if (structure.isCutOff)
                    {
                        structureList.Add(new ContinuationStructure(structure.cutOffComponents, wndGame, 8, structure.yBase, index, true, this, structure.widthRemaining, structure.canFloat, structure.canReplaceSolidBlocks));
                    }
                }
            }
        }
    }
}
