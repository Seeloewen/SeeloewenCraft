using System;

namespace SeeloewenCraft.game.ui
{
    public static class GameScreen
    {


        static bool pressedLeft, pressedRight;

        public static Block targetedBlock = null;

        public static void Update() //💀💀💀
        {
            float mouseX = InputHandler.mouseXScreen;
            float mouseY = InputHandler.mouseYScreen;

            int newBlockX = (int)Math.Floor((mouseX - GameCamera.blockXAnchor) / GameCamera.blockLength);
            int newBlockY = (int)-((mouseY - GameCamera.blockYAnchor) / (GameCamera.blockLength * 16 / 9.0f));

            var block = Game.world.GetBlock(newBlockX, newBlockY);
            if (targetedBlock != block)
            {

                if (targetedBlock != null) targetedBlock.HandleMouseLeave();
                if (block != null) block.HandleMouseEnter();
                if (pressedLeft)
                {
                    if (targetedBlock != null) targetedBlock.HandleMouseLeftUp();
                    if (block != null) block.HandleMouseLeftDown();
                }
                else if (pressedRight)
                {
                    if (targetedBlock != null) targetedBlock.HandleMouseRightUp();
                    if (block != null) block.HandleMouseRightDown();
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
            }
            pressedLeft = newPressedLeft;
            if (!pressedLeft && pressedRight && !newPressedRight || pressedRight && !Screen.allowIngameInputs)
            {
                block.HandleMouseRightUp();
            }
            if (!pressedLeft && !pressedRight && newPressedRight && Screen.allowIngameInputs)
            {
                block.HandleMouseRightDown();
            }
            pressedRight = newPressedRight;

        }

        static internal void Render()
        {
            PrimitiveRenderer.Begin();
            if (targetedBlock != null && targetedBlock.IsInRange())
            {
                float x1 = GameCamera.blockXAnchor + GameCamera.blockLength * (targetedBlock.xPos + targetedBlock.chunk.index * 8);
                float y1 = GameCamera.blockYAnchor - GameCamera.blockLength * targetedBlock.yPos * Resolution.RATIO;
                float x2 = x1 + GameCamera.blockLength;
                float y2 = y1 - GameCamera.blockLength * Resolution.RATIO;
                float lx = GameCamera.blockLength / 15;
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
