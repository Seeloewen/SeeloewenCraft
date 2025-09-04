using SeeloewenCraft.game.core.blocks;
using System;

namespace SeeloewenCraft.game.graphics
{
    internal static class BlockRenderer
    {
        internal static void Render(Block block)
        {
            var info = block.GetBlockRenderInfo();

            DrawBlock(info.GetTextureID(), info.x, info.y, info.isBackground, info.lighting); //Normal block
            if (info.hasForegroundBlock) DrawBlock(info.GetForegroundTextureID(), info.x, info.y, false, info.lighting); //Foreground block
            if (info.breakAnimation > 0) //Breaking/Hammering animation
            {
                string type = info.hammering ? "hammering" : "breaking";
                string id = $"sc:{type}_{info.breakAnimation}";
                DrawBlock(id, info.x, info.y, false, info.lighting);
            }

        }

        private static void DrawBlock(string blockID, int blockX, int blockY, bool isBackground, float lighting)
        {
            float x1 = GameCamera.blockXAnchor + GameCamera.blockLength * blockX;
            float y1 = GameCamera.blockYAnchor - GameCamera.blockLength * blockY * Resolution.RATIO;
            float x2 = x1 + GameCamera.blockLength;
            float y2 = y1 - GameCamera.blockLength * Resolution.RATIO;

            float g = isBackground ? 0.69f : 1.0f;

            if(lighting != 1f) TextureRenderer.Draw(blockID, x1, y1, x2, y2, g);
        }

        internal static void RenderLighting(BlockRenderInfo block)
        {
            float x1 = GameCamera.blockXAnchor + GameCamera.blockLength * block.x;
            float y1 = GameCamera.blockYAnchor - GameCamera.blockLength * block.y * Resolution.RATIO;
            float x2 = x1 + GameCamera.blockLength;
            float y2 = y1 - GameCamera.blockLength * Resolution.RATIO;

            if(block.lighting != 0f) PrimitiveRenderer.DrawRectangle(new Rectangle(x1, y1, x2, y2), new Color(0f, 0f, 0f, block.lighting));
        }
    }
}
