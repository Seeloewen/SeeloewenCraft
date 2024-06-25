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
    public partial class Chunk
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
                blockList.SaveToJson(writer);
            }
            File.WriteAllText($"{wndGame.worldDirectory}/chunk{index}/blocks.json", sw.ToString());

            //save settings in settings.json
            sb = new StringBuilder();
            sw = new StringWriter(sb);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                SaveSettingsToJson(writer);
            }
            File.WriteAllText($"{wndGame.worldDirectory}/chunk{index}/settings.json", sb.ToString());
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

        private void Load()
        {
            wndGame.log.Write($"Loading chunk {index}", "Info");

            //load blocklist
            string documentText = File.ReadAllText($"{wndGame.worldDirectory}/chunk{index}/blocks.json");
            JToken documentToken = JToken.Parse(documentText);

            blockList = BlockList.LoadFromJson(documentToken, this, wndGame);

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
                else if(block.isForeground)
                {
                    blockList.Get(block.xPos, block.yPos).PlaceInForeground(block);
                }
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
    }
}
