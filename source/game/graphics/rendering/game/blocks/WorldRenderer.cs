using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.world;
using System;

namespace SeeloewenCraft.game.graphics
{
    internal static class WorldRenderer
    {


        internal static void Init()
        {
        }


        internal static void Render()
        {
            Renderer.PushDebugGroup("world rendering");
            var chunks = World.Get().loadedChunkList;

            TextureRenderer.SetTexture(TextureManager.textureMaps["blocks"]);
            TextureRenderer.Begin();
            foreach (var chunk in chunks)
            {
                var blockList = chunk.blockList.blocks;

                int start = Math.Max(0, ((Player.Get().posY / 1000) - 9) * 8);
                int end = Math.Min(blockList.Length, ((Player.Get().posY / 1000) + 9) * 8);

                for (int i = start; i < end; i++)
                {
                    BlockRenderer.Render(blockList[i]);
                }
            }

            TextureRenderer.End();

            Renderer.PopDebugGroup();
        }
        
        internal static void RenderShadow() {
            var chunks = World.Get().loadedChunkList;

            Renderer.PushDebugGroup("shadow");
            
            //Lighting - This is only a sucffed fix because rendering has no z-index. TODO: Add z-index and remove this crap
            PrimitiveRenderer.Begin();
            foreach (var chunk in chunks)
            {
                var blockList = chunk.blockList.blocks;

                int start = Math.Max(0, ((Player.Get().posY / 1000) - 9) * 8);
                int end = Math.Min(blockList.Length, ((Player.Get().posY / 1000) + 9) * 8);

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
