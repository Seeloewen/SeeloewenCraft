

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

        public static bool allowIngameInputs { get => !(showEscapeMenu || showIngameMenu || showInventory); }






        public static void Init()
        {
            EscapeMenuScreen.Init();
        }


        public static void Update()
        {
            HandleInputs();

            if (showGameOverlay)
            {
                GameScreen.Update();
            }
            if(showEscapeMenu)
            {
                EscapeMenuScreen.Update();
            }
        }

        static void HandleInputs()
        {
            if (KeyBinds.checkPressedFirst(KeyBinds.OPEN_MENU))
            {
                showEscapeMenu = !showEscapeMenu;
            }
            else if (KeyBinds.checkPressedFirst(KeyBinds.TOGGLE_DEBUG))
            {
                showDebugMenu = !showDebugMenu;
            }
        }

        internal static void Render(PrimitiveRenderer primitiveRenderer, TextRenderer textRenderer, ItemRenderer itemRenderer)
        {
            if (showGameOverlay)
            {
                if(allowIngameInputs) GameScreen.Render(primitiveRenderer);
                HotbarRenderer.Render(primitiveRenderer, itemRenderer, textRenderer);
            }
            if (showDebugMenu)
            {

                primitiveRenderer.Begin();
                textRenderer.Begin();

                DebugMenu.Render(textRenderer);

                primitiveRenderer.End();
                textRenderer.End();
            }
            if (showEscapeMenu)
            {
                EscapeMenuScreen.Render(primitiveRenderer, textRenderer);
            }
        }





    }
}
