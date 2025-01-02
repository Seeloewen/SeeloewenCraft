
namespace SeeloewenCraft.gl_rendering
{
    internal class WorldRenderer
    {
        BlockRenderer blockRenderer;

        internal WorldRenderer(TextureManager textureManager)
        {
            var textureMap = new BlockTextureMap(textureManager);
            blockRenderer = new BlockRenderer(textureMap);
        }

        internal void Render()
        {
            blockRenderer.Begin();

            //blockRenderer.DrawBlock(" ", 0, 0);

            foreach (Chunk chunk in Game.world.loadedChunkList)
            {
                foreach (Block block in chunk.blockList.blocks)
                {
                    blockRenderer.DrawBlock(block.GetBlockRenderInfo());
                    //blockRenderer.DrawBlock(block.id, block.xPos + chunk.index * 8, block.yPos);
                }
            }
            blockRenderer.End();
        }

    }
}
