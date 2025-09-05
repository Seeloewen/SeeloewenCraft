using System;

namespace SeeloewenCraft.game.graphics
{
    internal static class WorldRenderer
    {

        static TextureMap textureMap;

        internal static void Init()
        {
            textureMap = new TextureMap("blocks");
        }


        internal static void Render()
        {
            var chunks = Game.world.loadedChunkList;

            Renderer.PushDebugGroup("blocks");
            TextureRenderer.SetTexture(textureMap);
            TextureRenderer.Begin();
            foreach (var chunk in chunks)
            {
                var blockList = chunk.blockList.blocks;

                int start = Math.Max(0, ((Game.world.player.posY / 1000) - 9) * 8);
                int end = Math.Min(blockList.Length, ((Game.world.player.posY / 1000) + 9) * 8);

                for(int i = start; i < end; i++)
                {
                    BlockRenderer.Render(blockList[i]);
                }
            }
            TextureRenderer.End();

            Renderer.PopDebugGroup();
            Renderer.PushDebugGroup("shadow");
            
            //Lighting - This is only a sucffed fix because rendering has no z-index. TODO: Add z-index and remove this crap
            PrimitiveRenderer.Begin();
            foreach (var chunk in chunks)
            {
                var blockList = chunk.blockList.blocks;

                int start = Math.Max(0, ((Game.world.player.posY / 1000) - 9) * 8);
                int end = Math.Min(blockList.Length, ((Game.world.player.posY / 1000) + 9) * 8);

                for (int i = start; i < end; i++)
                {
                    BlockRenderer.RenderLighting(blockList[i].GetBlockRenderInfo());
                }
            }
            PrimitiveRenderer.End();
            
            Renderer.PopDebugGroup();
        }
    }
}
