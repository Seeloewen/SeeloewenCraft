using SeeloewenCraft.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Windows.Documents;

namespace SeeloewenCraft
{
    public static class NetworkHandler
    {
        public static async void StartServer()
        {
            Game.server = new Server();
            Game.server.Start(5000);
        }

        public static async Task HandleData(string message)
        {
            if (message.Contains("CreateChunk"))
            {
                string[] data = message.Split(';');
                int index = Convert.ToInt32(data[1]);

                Game.world.CreateChunk(index);
                foreach (Block block in Game.world.GetChunk(index).blockList.blocks)
                {
                    if (Game.isServer)
                    {
                        Game.server.SendData($"SetBlock;{block.id};{index};{block.xPos};{block.yPos}");
                    }
                }
            }
            else if (message == "InitialLoad")
            {
                InitialLoad();
            }
            else if (message.Contains("SetBlock"))
            {
                string[] data = message.Split(';');

                Game.world.SetBlockMultiplayer(BlockRegister.GenerateBlock(data[1]), Convert.ToInt32(data[2]), Convert.ToInt32(data[3]), Convert.ToInt32(data[4]));
            }

        }

        public async static void InitialLoad()
        {
            foreach (Chunk chunk in Game.world.loadedChunkList)
            {
                foreach (Block block in chunk.blockList.blocks)
                {
                    if (Game.isServer)
                    {
                        Game.server.SendData($"SetBlock;{block.id};{chunk.index};{block.xPos};{block.yPos}");
                    }
                }
            }
            Game.server.SendData($"CreatePlayer;{Game.world.player.posX},{Game.world.player.posY}");
        }
    }
}
