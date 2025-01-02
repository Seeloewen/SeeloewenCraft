

namespace SeeloewenCraft.gl_rendering
{
    public static class Screen
    {



        public static bool showGame = true;
        public static bool showGameOverlay = true;
        public static bool showDebugMenu = false;
        public static bool showInventory = false;
        public static bool showIngameMenu = false;
        public static bool showEscapeMenu = false;



        




        public static void Init()
        {

        }


        public static void Update()
        {
            HandleInputs();

            if (showGameOverlay)
            {
                GameScreen.Update();
            }
        }

        static void HandleInputs()
        {
            if (KeyBinds.checkPressedFirst(KeyBinds.TOGGLE_DEBUG))
            {
                showDebugMenu = !showDebugMenu;
            }
        }

        internal static void Render(PrimitiveRenderer primitiveRenderer, TextRenderer textRenderer)
        {
            if (showGameOverlay)
            {
                primitiveRenderer.Begin();
                GameScreen.Render(primitiveRenderer);
                primitiveRenderer.End();
            }
            if (showDebugMenu)
            {

                primitiveRenderer.Begin();
                textRenderer.Begin();

                DebugMenu.Render(textRenderer);

                primitiveRenderer.End();
                textRenderer.End();
            }
        }


        


    }
}
