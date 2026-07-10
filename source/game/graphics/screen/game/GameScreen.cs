using SeeloewenCraft.game.core.blocks;
using SeeloewenCraft.game.core.world;
using System;

namespace SeeloewenCraft.game.graphics
{
    public static class GameScreen
    {
        static bool pressedLeft, pressedRight;

        private static double leftCD = 0, rightCD = 0;
        private const double CLICK_COOLDOWN = 1 / 5.0;
        
        public static Block targetedBlock = null;

        public static void Update(double dt) //💀💀💀
        {
            leftCD -= dt;
            rightCD -= dt;
            
            float mouseX = InputHandler.mouseXScreen;
            float mouseY = InputHandler.mouseYScreen;

            int newBlockX = (int)Math.Floor((mouseX - GameCamera.blockXAnchor) / GameCamera.blockLength);
            int newBlockY = (int)-((mouseY - GameCamera.blockYAnchor) / (GameCamera.blockLength * Resolution.RATIO));

            PositionData pos = PositionData.FromGlobalX(newBlockX, newBlockY);

            if (!pos.ChunkExists()) return;

            var block = World.Get().GetBlock(pos);
            if (block == null) return;

            if (targetedBlock != block)
            {

                if (targetedBlock != null) targetedBlock.HandleMouseLeave();
                if (block != null) block.HandleMouseEnter();
                if (pressedLeft)
                {
                    if (targetedBlock != null) targetedBlock.HandleMouseLeftUp();
                    if (block != null && leftCD <= 0)
                    {
                        leftCD = CLICK_COOLDOWN;
                        block.HandleMouseLeftDown();
                    }
                }
                else if (pressedRight)
                {
                    if (targetedBlock != null) targetedBlock.HandleMouseRightUp();
                    if (block != null && rightCD <= 0)
                    {
                        rightCD = CLICK_COOLDOWN;
                        block.HandleMouseRightDown();
                    }
                }
                DebugMenu.NewTargeted(block);
                targetedBlock = block;
            }

            bool newPressedLeft = InputHandler.pressedLeft;
            bool newPressedRight = InputHandler.pressedRight;
            if (pressedLeft && !newPressedLeft || pressedLeft && !Screen.allowIngameInputs)
            {
                block.HandleMouseLeftUp();
            }
            if (!pressedLeft && newPressedLeft && Screen.allowIngameInputs)
            {
                if (pressedRight) block.HandleMouseRightUp();
                block.HandleMouseLeftDown();
                leftCD = CLICK_COOLDOWN;
            }
            pressedLeft = newPressedLeft;
            if (!pressedLeft && pressedRight && !newPressedRight || pressedRight && !Screen.allowIngameInputs)
            {
                block.HandleMouseRightUp();
            }
            if (!pressedLeft && !pressedRight && newPressedRight && Screen.allowIngameInputs)
            {
                block.HandleMouseRightDown();
                rightCD = CLICK_COOLDOWN;
            }
            pressedRight = newPressedRight;

        }

        static internal void Render()
        {
            PrimitiveRenderer.Begin();
            if (targetedBlock != null && targetedBlock.IsInRange())
            {
                float x1 = GameCamera.blockXAnchor + GameCamera.blockLength * (targetedBlock.posX + targetedBlock.chunk.index * 8);
                float y1 = GameCamera.blockYAnchor - GameCamera.blockLength * targetedBlock.posY * Resolution.RATIO;
                float x2 = x1 + GameCamera.blockLength;
                float y2 = y1 - GameCamera.blockLength * Resolution.RATIO;
                float lx = GameCamera.blockLength / 18;
                float ly = lx * Resolution.RATIO;

                PrimitiveRenderer.DrawRectangle(x1, y1, x2, y1 - ly, 0.0f, 0.0f, 0.0f);
                PrimitiveRenderer.DrawRectangle(x1, y1, x1 + lx, y2, 0.0f, 0.0f, 0.0f);
                PrimitiveRenderer.DrawRectangle(x2, y1, x2 - lx, y2, 0.0f, 0.0f, 0.0f);
                PrimitiveRenderer.DrawRectangle(x1, y2, x2, y2 + ly, 0.0f, 0.0f, 0.0f);
            }
            PrimitiveRenderer.End();
        }





    }
}
