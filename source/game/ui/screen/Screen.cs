

using SeeloewenCraft.game.ui.ui_lib;
using Windows.Devices.PointOfService.Provider;

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


        static UIRoot escapeMenuUIRoot;



        public static void Init()
        {
            EscapeMenuScreen.Init();
            InventoryScreen.Init();
            escapeMenuUIRoot = new UIRoot(() => new EscapeMenu());
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
                escapeMenuUIRoot.Update();
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
                if(showEscapeMenu)
                {
                    escapeMenuUIRoot.Show();
                } else
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
                //EscapeMenuScreen.Render();
                escapeMenuUIRoot.Render();
            }
        }

    }
}
