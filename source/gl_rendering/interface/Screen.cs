
namespace SeeloewenCraft.gl_rendering
{
    public class Screen
    {


        GameScreen gameScreen;

        enum ScreenMode {GAME, INVENTORY, MENU}

        ScreenMode currentMode = ScreenMode.GAME;

        public void Update()
        {
            switch(currentMode)
            {
                case ScreenMode.GAME:
                    gameScreen.Update();
                    break;
            }
        }

        internal void Render(PrimitiveRenderer renderer)
        {
            switch (currentMode)
            {
                case ScreenMode.GAME:
                    gameScreen.Render(renderer);


                    break;
            }
        }

        public Screen(GameCamera cam)
        {
            gameScreen = new GameScreen(cam);
        }

    }
}
