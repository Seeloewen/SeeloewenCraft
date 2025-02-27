using SeeloewenCraft.game.ui.ui_lib;

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
        public static bool showHotbar = true;

        public static bool allowIngameInputs { get => !(showEscapeMenu || showIngameMenu || showInventory); }


        static UIRoot invUIRoot;
        static UIRoot escapeMenuUIRoot;
        static UIRoot hotbarUIRoot;
        static UIRoot gameOverlayUIRoot;

        public static void Init()
        {
            InventoryScreen.Init();
            invUIRoot = new UIRoot(() => new InvUI());
            escapeMenuUIRoot = new UIRoot(() => new EscapeMenu());
            gameOverlayUIRoot = new UIRoot(() => new GameOverlay()); //least useless code
            hotbarUIRoot = new UIRoot(() => new Hotbar());
            gameOverlayUIRoot.Show();
            hotbarUIRoot.Show();
        }


        public static void Update()
        {
            HandleInputs();

            if (showGameOverlay)
            {
                gameOverlayUIRoot.Update();
                hotbarUIRoot.Update();
                GameScreen.Update();
                HotbarScreen.Update();
            }
            if (showEscapeMenu)
            {
                escapeMenuUIRoot.Update();
            }
            if (showInventory)
            {
                invUIRoot.Update();
            }
        }

        static void HandleInputs()
        {
            if (KeyBinds.checkPressedFirst(KeyBinds.OPEN_MENU))
            {
                showEscapeMenu = !showEscapeMenu;
                if (showEscapeMenu)
                {
                    escapeMenuUIRoot.Show();
                }
                else
                {
                    escapeMenuUIRoot.Hide();
                }
            }
            else if (KeyBinds.checkPressedFirst(KeyBinds.TOGGLE_DEBUG))
            {
                showDebugMenu = !showDebugMenu;
            }
            else if (KeyBinds.checkPressedFirst(KeyBinds.SHOW_INV))
            {
                showInventory = !showInventory;
                showGameOverlay = !showInventory;
                if (showInventory)
                {
                    invUIRoot.Show();
                }
                else
                {
                    invUIRoot.Hide();
                }
            }
        }

        internal static void Render()
        {
            if (showGame && allowIngameInputs) GameScreen.Render();
            if (showHotbar) hotbarUIRoot.Render();
            if (showDebugMenu) DebugMenu.Render();
            if (showInventory) invUIRoot.Render();
            if (showEscapeMenu) escapeMenuUIRoot.Render();
            if (showGameOverlay) gameOverlayUIRoot.Render();
        }
    }
}
