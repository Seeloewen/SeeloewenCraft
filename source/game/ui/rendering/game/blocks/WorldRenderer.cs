

namespace SeeloewenCraft.game.ui
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

            TextureRenderer.SetTexture(textureMap);
            TextureRenderer.Begin();
            foreach (var chunk in chunks)
            {
                var blockList = chunk.blockList.blocks;
                foreach (Block block in blockList)
                {
                    BlockRenderer.Render(block);
                }
            }
            TextureRenderer.End();
        }
    }
}
