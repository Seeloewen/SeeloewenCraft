

namespace SeeloewenCraft.game.ui
{
    internal static class Screen
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
            InventoryScreen.Init();
        }


        public static void Update()
        {
            HandleInputs();

            if (showGameOverlay)
            {
                GameScreen.Update();
                HotbarScreen.Update();
            }
            if (showEscapeMenu)
            {
                EscapeMenuScreen.Update();
            }
            if (showInventory)
            {
                InventoryScreen.Update();
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
            else if (KeyBinds.checkPressedFirst(KeyBinds.SHOW_INV))
            {
                showInventory = !showInventory;
            }
        }

        internal static void Render()
        {
            if (showGameOverlay)
            {
                if (allowIngameInputs) GameScreen.Render();
                HotbarScreen.Render();
            }
            if (showDebugMenu)
            {

                DebugMenu.Render();

            }
            if (showInventory)
            {
                InventoryScreen.Render();
            }
            if (showEscapeMenu)
            {
                EscapeMenuScreen.Render();
            }
        }

    }
}
