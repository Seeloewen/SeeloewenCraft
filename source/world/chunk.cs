using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace SeeloewenCraft
{
    public enum Biome
    {
        None,
        Plains
    }

    public partial class Chunk
    {
        public BlockList blockList;
        public List<Structure> structureList = new List<Structure>();
        public BlockContainerList blockContainerList;
        public Grid grdChunk = new Grid();
        private Random rnd = new Random(DateTime.Now.Millisecond);
        World world;
        public int index;
        private int floorHeight; //Only used while generating
        public int floorHeightRight;
        public int floorHeightLeft;
        //static int o = 0;
        public string chunkDirectory;
        public Biome biome;

        //-- Constructor --//

        public Chunk(World world, int index)
        {
            //Set the attributes
            this.world = world;
            this.index = index;
            //rnd = new Random(DateTime.Now.Millisecond);
            //o++;

            //Begin loading the chunk
            chunkDirectory = string.Format("{0}/chunk{1}", world.worldDirectory, index);
            Init();
        }

        //-- Custom Methods --//

        public void Save()
        {
            //Check if the chunk directory already exists and create it otherwise
            if (!Directory.Exists(chunkDirectory))
            {
                Directory.CreateDirectory(chunkDirectory);
                world.log.Write($"Created chunk directory {chunkDirectory}!", "Info");
            }

            //save blocks in blocks.json
            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Formatting.Indented;
                blockList.SaveToJson(writer);
                writer.WriteToFile($"{world.worldDirectory}/chunk{index}/blocks.json");
            }

            //save settings in settings.json
            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Formatting.Indented;
                SaveSettingsToJson(writer);
                writer.WriteToFile($"{world.worldDirectory}/chunk{index}/settings.json");
            }

        }

        private void SaveSettingsToJson(JsonWriter writer)
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
            if (x > 7)
            {
                Chunk chunk = world.GetLoadedChunk(index + 1);

                if (chunk != null)
                {
                    chunk.SetBlock(block, x - 8, y);
                }
            }
            else if (x < 0)
            {
                Chunk chunk = world.GetLoadedChunk(index - 1);

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
                blockList.Add(block, block.xPos, block.yPos);

                if (blockContainerList.GetContainer(x, y) != null)
                {
                    blockContainerList.GetContainer(x, y).RenderBlock(block);
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
            foreach (BlockContainerList containerList in world.blockContainerList)
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
            world.log.Write($"Beginning to initialize chunk {index}", "Info");

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
            if (Directory.Exists($"{world.worldDirectory}/chunk{index}"))
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
                foreach (Block block in blockList.blocks)
                {
                    if (block.id == "sc:air_block")
                    {
                        block.isLightSource = block.IsAirLightSource(block);
                    }
                }
                RenderChunk();

                world.log.Write($"Successfully initialized chunk {index}", "Info");
            }
            catch (Exception ex)
            {
                world.log.Write($"Could not initialize chunk: {ex.Message}", "Error");
            }
        }

        private void Load()
        {
            world.log.Write($"Loading chunk {index}", "Info");

            //load blocklist
            JsonToken documentToken = JsonUtil.ReadFile($"{world.worldDirectory}/chunk{index}/blocks.json");
            blockList = BlockList.LoadFromJson(documentToken, this, world);

            //load settings
            documentToken = JsonUtil.ReadFile($"{world.worldDirectory}/chunk{index}/settings.json");

            index = documentToken.GetInt("/index");
            floorHeightLeft = documentToken.GetInt("/floor_height_left");
            floorHeightRight = documentToken.GetInt("/floor_height_right");
        }


        public void RenderChunk()
        {
            foreach (Block block in blockList.blocks)
            {
                //Render normal blocks
                if (!block.isForeground && !block.isBackground)
                {
                    SetBlock(block, block.xPos, block.yPos);
                }
                //Render background blocks
                else if (!block.isForeground && block.isBackground)
                {
                    SetBlock(block, block.xPos, block.yPos);
                    block.MoveToBackground();
                }

                //Render foreground blocks
                if (block.foregroundBlock != null)
                {
                    block.PlaceInForeground(block.foregroundBlock);
                }
            }
        }

        public Block GetBlock(int x, int y)
        {
            if (x > 7)
            {
                if (world.GetLoadedChunk(index + 1) != null)
                {
                    return world.GetLoadedChunk(index + 1).GetBlock(x - 8, y);
                }
                else
                {
                    return null;
                }
            }
            else if (x < 0)
            {
                if (world.GetLoadedChunk(index - 1) != null)
                {
                    return world.GetLoadedChunk(index - 1).GetBlock(x + 8, y);
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
    }
}
