

namespace SeeloewenCraft.gl_rendering
{
    public class GameScreen
    {

        GameCamera cam;

        public int blockX, blockY;
        bool pressedLeft;

        internal void Render(PrimitiveRenderer renderer)
        {
            if (Game.world.GetBlock(blockX, blockY).IsInRange())
            {
                float x1 = cam.blockXAnchor + cam.blockLength * blockX;
                float y1 = cam.blockYAnchor - cam.blockLength * blockY * cam.ratio;
                float x2 = x1 + cam.blockLength;
                float y2 = y1 - cam.blockLength * cam.ratio;
                float lx = cam.blockLength / 15;
                float ly = lx * cam.ratio;

                renderer.DrawRectangle(x1, y1, x2, y1 - ly, 0.0f, 0.0f, 0.0f);
                renderer.DrawRectangle(x1, y1, x1 + lx, y2, 0.0f, 0.0f, 0.0f);
                renderer.DrawRectangle(x2, y1, x2 - lx, y2, 0.0f, 0.0f, 0.0f);
                renderer.DrawRectangle(x1, y2, x2, y2 + ly, 0.0f, 0.0f, 0.0f);
            }
        }

        public void Update()
        {
            float mouseX = InputHandler.currentMouseX;
            float mouseY = InputHandler.currentMouseY;

            int newBlockX = (int)((mouseX - cam.blockXAnchor) / cam.blockLength);
            int newBlockY = (int)-((mouseY - cam.blockYAnchor) / (cam.blockLength * 16/9.0f));

            var block = Game.world.GetBlock(newBlockX, newBlockY);
            if (newBlockX != blockX || newBlockY != blockY)
            {
                var oldBlock = Game.world.GetBlock(blockX, blockY);
                oldBlock.HandleMouseLeave();
                block.HandleMouseEnter();
                if(pressedLeft)
                {
                    oldBlock.HandleMouseLeftUp();
                    block.HandleMouseLeftDown();
                }
                blockX = newBlockX;
                blockY = newBlockY;
            }

            bool newPressedLeft = InputHandler.mouseClick;
            if (pressedLeft && !newPressedLeft)
            {
                block.HandleMouseLeftUp();
            }
            if(!pressedLeft && newPressedLeft)
            {
                block.HandleMouseLeftDown();
            }

        }



        public GameScreen(GameCamera cam)
        {
            this.cam = cam;
        }


    }
}
