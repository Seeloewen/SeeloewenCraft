using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.util;

namespace SeeloewenCraft.game.graphics
{
    internal static class PlayerRenderer
    {


        static float originX;
        static float originY;
        static float unitSize;



        static internal void Render()
        {
            Renderer.PushDebugGroup("player rendering");
            var info = Player.Get().playerRenderInfo;

            originX = GameCamera.blockXAnchor + (info.posX / 1000.0f) * GameCamera.blockLength;
            originY = GameCamera.blockYAnchor - (info.posY / 1000.0f) * GameCamera.blockLength * Resolution.RATIO;
            unitSize = GameCamera.blockLength * 0.475f * 0.125f;

            TextureRenderer.SetTexture(TextureManager.textureMaps["player"]);
            TextureRenderer.Begin();

            //TextureRenderer.Draw("dsa ", -0.5f, 0.5f, 0.5f, -0.5f, 1f);

            DrawLegBack(info);
            DrawArmBack(info);
            DrawHead(info);
            DrawLegFront(info);
            DrawBody(info);
            DrawArmFront(info);

            TextureRenderer.End();

            Renderer.PopDebugGroup();
        }


        static void DrawLegBack(PlayerRenderInfo info)
        {
            var topRight = new Vector2f(unitSize * 2, 0);
            var topLeft = new Vector2f(unitSize * -2, 0);
            var botRight = new Vector2f(unitSize * 2, unitSize * -12);
            var botLeft = new Vector2f(unitSize * -2, unitSize * -12);
            topRight = topRight.Rotate(info.legBack).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 20 * unitSize * Resolution.RATIO);
            topLeft = topLeft.Rotate(info.legBack).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 20 * unitSize * Resolution.RATIO);
            botRight = botRight.Rotate(info.legBack).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 20 * unitSize * Resolution.RATIO);
            botLeft = botLeft.Rotate(info.legBack).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 20 * unitSize * Resolution.RATIO);

            TextureRenderer.Draw("sc:leg_back", topLeft, topRight, botLeft, botRight);
        }
        static void DrawLegFront(PlayerRenderInfo info)
        {
            var topRight = new Vector2f(unitSize * 2, 0);
            var topLeft = new Vector2f(unitSize * -2, 0);
            var botRight = new Vector2f(unitSize * 2, unitSize * -12);
            var botLeft = new Vector2f(unitSize * -2, unitSize * -12);
            topRight = topRight.Rotate(info.legFront).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 20 * unitSize * Resolution.RATIO);
            topLeft = topLeft.Rotate(info.legFront).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 20 * unitSize * Resolution.RATIO);
            botRight = botRight.Rotate(info.legFront).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 20 * unitSize * Resolution.RATIO);
            botLeft = botLeft.Rotate(info.legFront).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 20 * unitSize * Resolution.RATIO);

            TextureRenderer.Draw("sc:leg_front", topLeft, topRight, botLeft, botRight);
        }

        static void DrawArmBack(PlayerRenderInfo info)
        {
            var topRight = new Vector2f(unitSize * 2, unitSize * 2);
            var topLeft = new Vector2f(unitSize * -2, unitSize * 2);
            var botRight = new Vector2f(unitSize * 2, unitSize * -10);
            var botLeft = new Vector2f(unitSize * -2, unitSize * -10);
            topRight = topRight.Rotate(info.armBack).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 10 * unitSize * Resolution.RATIO);
            topLeft = topLeft.Rotate(info.armBack).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 10 * unitSize * Resolution.RATIO);
            botRight = botRight.Rotate(info.armBack).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 10 * unitSize * Resolution.RATIO);
            botLeft = botLeft.Rotate(info.armBack).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 10 * unitSize * Resolution.RATIO);

            TextureRenderer.Draw("sc:arm_back", topLeft, topRight, botLeft, botRight);
        }

        static void DrawArmFront(PlayerRenderInfo info)
        {
            var topRight = new Vector2f(unitSize * 2, unitSize * 2);
            var topLeft = new Vector2f(unitSize * -2, unitSize * 2);
            var botRight = new Vector2f(unitSize * 2, unitSize * -10);
            var botLeft = new Vector2f(unitSize * -2, unitSize * -10);
            topRight = topRight.Rotate(info.armFront).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 10 * unitSize * Resolution.RATIO);
            topLeft = topLeft.Rotate(info.armFront).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 10 * unitSize * Resolution.RATIO);
            botRight = botRight.Rotate(info.armFront).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 10 * unitSize * Resolution.RATIO);
            botLeft = botLeft.Rotate(info.armFront).Scale(1, Resolution.RATIO).Add(originX + 4 * unitSize, originY - 10 * unitSize * Resolution.RATIO);

            TextureRenderer.Draw("sc:arm_front", topLeft, topRight, botLeft, botRight);
        }

        static void DrawBody(PlayerRenderInfo info)
        {
            float x1 = originX + 2 * unitSize;
            float y1 = originY - 8 * unitSize * Resolution.RATIO;
            float x2 = x1 + 4 * unitSize;
            float y2 = y1 - 12 * unitSize * Resolution.RATIO;
            TextureRenderer.Draw("sc:body", x1, y1, x2, y2);
        }

        static void DrawHead(PlayerRenderInfo info)
        {
            float x1 = originX;
            float y1 = originY;
            float x2 = originX + 8 * unitSize;
            float y2 = originY - 8 * unitSize * Resolution.RATIO;
            switch (info.direction)
            {
                case Direction.LEFT: TextureRenderer.Draw("sc:head", x1, y1, x2, y2); break;
                case Direction.RIGHT: TextureRenderer.Draw("sc:head", x2, y1, x1, y2); break;
            }
        }



    }
}
