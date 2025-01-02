

namespace SeeloewenCraft.gl_rendering
{
    public static class Screen
    {



        public static bool showGame;
        public static bool showGameOverlay;
        public static bool showDebugMenu;
        public static bool showInventory;
        public static bool showIngameMenu;
        public static bool showEscapeMenu;



        



        enum ScreenMode {GAME, INVENTORY, MENU}

        public static void Init()
        {

        }


        public static void Update()
        {
            GameScreen.Update();
        }

        internal static void Render(PrimitiveRenderer primitiveRenderer, TextRenderer textRenderer)
        {
            GameScreen.Render(primitiveRenderer);

        }


        


    }
}
