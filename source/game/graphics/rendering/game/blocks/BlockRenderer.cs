namespace SeeloewenCraft.game.graphics
{
    internal static class BlockRenderer
    {
        internal static void Render(Block block)
        {
            var info = block.GetBlockRenderInfo();

            DrawBlock(info.GetTextureID(), info.x, info.y, info.isBackground);
            if (info.hasForegroundBlock) DrawBlock(info.GetForegroundTextureID(), info.x, info.y, false);
            if(info.breakAnimation > 0)
            {
                string type = info.hammering ? "hammering" : "breaking";
                string id = $"sc:{type}_{info.breakAnimation}";
                DrawBlock(id, info.x, info.y, false);
            }

        }
        static void DrawBlock(string blockID, int blockX, int blockY, bool isBackground)
        {

            float x1 = GameCamera.blockXAnchor + GameCamera.blockLength * blockX;
            float y1 = GameCamera.blockYAnchor - GameCamera.blockLength * blockY * Resolution.RATIO;
            float x2 = x1 + GameCamera.blockLength;
            float y2 = y1 - GameCamera.blockLength * Resolution.RATIO;

            float g = isBackground ? 0.69f : 1.0f;

            TextureRenderer.Draw(blockID, x1, y1, x2, y2, g);
        }
    

    }
}
