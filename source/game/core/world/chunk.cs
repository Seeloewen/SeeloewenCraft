using LibNoise;
using LibNoise.Primitive;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.world.generation;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;


namespace SeeloewenCraft.game.core.world
{
    public partial class Chunk
    {
        public BlockList blockList;
        public List<Structure> structureList = new List<Structure>();
        public List<Structure> continuedStructureList = new List<Structure>();
        public Random chunkRnd;

        public int index;
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
            Random rnd = new Random(World.Get().seed);
            chunkSeed = rnd.Next() + index;
            chunkRnd = new Random(chunkSeed);

            //Begin loading the chunk
            chunkDirectory = string.Format("{0}/chunk{1}", World.Get().worldDirectory, index);
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
            JObject blockListObj = new JObject()
            { { "blocks", blockList.ToJson() } };
            blockListObj.ToFile($"{World.Get().worldDirectory}/chunk{index}/blocks.json");

            //save settings in settings.json
            JObject settingsObj = new JObject()
            {
                {"index", index},
                {"chunkSeed", chunkSeed},
            };
            settingsObj.ToFile($"{World.Get().worldDirectory}/chunk{index}/settings.json");
        }

        internal void SetBlock(Block block, int x, int y)
        {
            //Check if the specified coordinates are indeed in this chunk
            if (x > 7 || x < 0)
            {
                throw new IndexOutOfRangeException("Specified coordinates are not in range of possible chunk coordinates");
            }

            //Drop the current block if it gets replaced
            Block curBlock = GetBlock(x, y);

            if (block.isBackground) block.MoveToBackground();

            //Add the block to the block list
            block.posX = x;
            block.posY = y;
            block.chunk = this;
            blockList.Add(block, block.posX, block.posY);

            block.OnSetBlock();
        }

        public void Init()
        {
            Log.Write($"Initializing chunk {index}...", LogType.WORLD_GENERATION, LogLevel.INFO);

            //Check if the chunk doesn't already exist
            if (Directory.Exists($"{World.Get().worldDirectory}/chunk{index}"))
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
            JArray blockListArray = JsonUtil.ArrayFromFile($"{World.Get().worldDirectory}/chunk{index}/blocks.json");
            blockList = BlockList.FromJson(blockListArray, this);

            //load settings
            JObject settingsObj = JsonUtil.ObjectFromFile($"{World.Get().worldDirectory}/chunk{index}/settings.json");
            index = settingsObj.Get<int>("index");
        }


        public void RenderChunk()
        {
            foreach (Block block in blockList.blocks)
            {
                //Render normal blocks
                if (!block.isForeground && !block.isBackground)
                {
                    SetBlock(block, block.posX, block.posY);
                }
                //Render background blocks
                else if (!block.isForeground && block.isBackground)
                {
                    SetBlock(block, block.posX, block.posY);
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
            //Check if the specified coordinates are indeed in this chunk
            if (x > 7 || x < 0)
            {
                throw new IndexOutOfRangeException("Specified coordinates are not in range of possible chunk coordinates");
            }

            return blockList.Get(x, y);
        }
    }
}
