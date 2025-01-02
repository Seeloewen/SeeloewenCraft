

using System;
using Windows.Services.Maps.OfflineMaps;

namespace SeeloewenCraft.gl_rendering
{
    public class GameScreen
    {


        public static int blockX, blockY;
        static bool pressedLeft, pressedRight;

        static internal void Render(PrimitiveRenderer renderer)
        {
            if (Game.world.GetBlock(blockX, blockY).IsInRange())
            {
                float x1 = GameCamera.blockXAnchor + GameCamera.blockLength * blockX;
                float y1 = GameCamera.blockYAnchor - GameCamera.blockLength * blockY * Resolution.RATIO;
                float x2 = x1 + GameCamera.blockLength;
                float y2 = y1 - GameCamera.blockLength * Resolution.RATIO;
                float lx = GameCamera.blockLength / 15;
                float ly = lx * Resolution.RATIO;

                renderer.DrawRectangle(x1, y1, x2, y1 - ly, 0.0f, 0.0f, 0.0f);
                renderer.DrawRectangle(x1, y1, x1 + lx, y2, 0.0f, 0.0f, 0.0f);
                renderer.DrawRectangle(x2, y1, x2 - lx, y2, 0.0f, 0.0f, 0.0f);
                renderer.DrawRectangle(x1, y2, x2, y2 + ly, 0.0f, 0.0f, 0.0f);
            }
        }

        public static void Update()
        {
            float mouseX = InputHandler.currentMouseX;
            float mouseY = InputHandler.currentMouseY;

            int newBlockX = (int) Math.Floor((mouseX - GameCamera.blockXAnchor) / GameCamera.blockLength);
            int newBlockY = (int)-((mouseY - GameCamera.blockYAnchor) / (GameCamera.blockLength * 16 / 9.0f));

            var block = Game.world.GetBlock(newBlockX, newBlockY);
            if (newBlockX != blockX || newBlockY != blockY)
            {
                var oldBlock = Game.world.GetBlock(blockX, blockY);
                oldBlock.HandleMouseLeave();
                block.HandleMouseEnter();
                if (pressedLeft)
                {
                    oldBlock.HandleMouseLeftUp();
                    block.HandleMouseLeftDown();
                }
                else if (pressedRight)
                {
                    oldBlock.HandleMouseLeftUp();
                    block.HandleMouseLeftDown();
                }
                blockX = newBlockX;
                blockY = newBlockY;
                DebugMenu.NewTargeted(block);
                
            }

            bool newPressedLeft = InputHandler.pressedLeft;
            bool newPressedRight = InputHandler.pressedRight;
            if (pressedLeft && !newPressedLeft)
            {
                block.HandleMouseLeftUp();
            }
            if (!pressedLeft && newPressedLeft)
            {
                block.HandleMouseLeftDown();
                if(pressedRight) block.HandleMouseRightUp();
            }
            pressedLeft = newPressedLeft;
            if(!pressedLeft && pressedRight && !newPressedRight)
            {
                block.HandleMouseRightUp();
            }
            if(!pressedLeft && !pressedRight && newPressedRight)
            {
                block.HandleMouseRightDown();
            }
            pressedRight = newPressedRight;

        }





    }
}
