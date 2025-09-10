using Newtonsoft.Json;
using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.world.generation;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using JsonToken = SeeloewenCraft.game.util.JsonToken;
using JsonWriter = SeeloewenCraft.game.util.JsonWriter;

namespace SeeloewenCraft.game.core.world
{
    public partial class Chunk
    {
        public BlockList blockList;
        public List<Structure> structureList = new List<Structure>();
        public List<Structure> continuedStructureList = new List<Structure>();
        public Grid grdChunk = new Grid();
        public Random seededRnd;

        public int index;
        private int floorHeight; //Only used while generating
        public int floorHeightRight;
        public int floorHeightLeft;
        public int structureNum = 0;
        public string chunkDirectory;
        public Biome biome;
        public int chunkSeed;

        //-- Constructor --//

        public Chunk(int index)
        {
            //Set the attributes
            this.index = index;

            //Seed magic
            Random rnd = new Random(Game.world.seed);
            chunkSeed = rnd.Next() * index;
            seededRnd = new Random(chunkSeed);

            //Begin loading the chunk
            chunkDirectory = string.Format("{0}/chunk{1}", Game.world.worldDirectory, index);
            Init();
        }

        //-- Custom Methods --//

        public void Save()
        {
            //Check if the chunk directory already exists and create it otherwise
            if (!Directory.Exists(chunkDirectory))
            {
                Directory.CreateDirectory(chunkDirectory);
                Log.Write($"Created chunk directory {chunkDirectory}", LogType.WORLD_GENERATION, LogLevel.INFO);
            }

            //save blocks in blocks.json
            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Formatting.Indented;
                blockList.SaveToJson(writer);
                writer.WriteToFile($"{Game.world.worldDirectory}/chunk{index}/blocks.json");
            }

            //save settings in settings.json
            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Formatting.Indented;
                SaveSettingsToJson(writer);
                writer.WriteToFile($"{Game.world.worldDirectory}/chunk{index}/settings.json");
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
                Chunk chunk = Game.world.GetLoadedChunk(index + 1);

                if (chunk != null)
                {
                    chunk.SetBlock(block, x - 8, y);
                }
            }
            else if (x < 0)
            {
                Chunk chunk = Game.world.GetLoadedChunk(index - 1);

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
                block.OnSetBlock();
            }
        }


        public void Init()
        {
            Log.Write($"Initializing chunk {index}...", LogType.WORLD_GENERATION, LogLevel.INFO);

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


            //Check if the chunk doesn't already exist
            if (Directory.Exists($"{Game.world.worldDirectory}/chunk{index}"))
            {
                Load();
            }
            else
            {
                Generate();
            }

            try
            {
                Log.Write($"Successfully initialized chunk {index}", LogType.WORLD_GENERATION, LogLevel.INFO);
            }
            catch (Exception ex)
            {
                Log.Write($"Could not initialize chunk {index}: {ex.Message}\n{ex.StackTrace}", LogType.WORLD_GENERATION, LogLevel.ERROR);
            }
        }

        private void Load()
        {
            Log.Write($"Loading chunk {index} from file...", LogType.WORLD_GENERATION, LogLevel.INFO);

            //load blocklist
            JsonToken documentToken = JsonUtil.ReadFile($"{Game.world.worldDirectory}/chunk{index}/blocks.json");
            blockList = BlockList.LoadFromJson(documentToken, this);

            //load settings
            documentToken = JsonUtil.ReadFile($"{Game.world.worldDirectory}/chunk{index}/settings.json");

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
                if (block.GetForegroundBlock() != null)
                {
                    block.SetForegroundBlock(block.GetForegroundBlock());
                }
            }
        }

        public Block GetBlock(int x, int y)
        {
            if (x > 7)
            {
                if (Game.world.GetLoadedChunk(index + 1) != null)
                {
                    return Game.world.GetLoadedChunk(index + 1).GetBlock(x - 8, y);
                }
                else
                {
                    return null;
                }
            }
            else if (x < 0)
            {
                if (Game.world.GetLoadedChunk(index - 1) != null)
                {
                    return Game.world.GetLoadedChunk(index - 1).GetBlock(x + 8, y);
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
