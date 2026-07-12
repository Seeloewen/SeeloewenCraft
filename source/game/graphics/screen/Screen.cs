using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.graphics.ui_lib;

namespace SeeloewenCraft.game.graphics
{
    internal static class Screen
    {
        public static bool showGame = true;
        public static bool showGameOverlay = true;
        public static bool showDebugMenu = false;
        public static bool showGui = false;
        public static bool showIngameMenu = false;
        public static bool showEscapeMenu = false;
        public static bool showHotbar = true;
        public static bool showChat = false;
        public static bool showNotifications = false;

        public static bool allowIngameInputs { get => !(showEscapeMenu || showIngameMenu || showGui || showChat); }

        internal static GuiHandler guiHandler = new GuiHandler();

        public static UIRoot guiRoot;
        internal static UIRoot escapeMenuUIRoot;
        internal static UIRoot hotbarUIRoot;
        internal static UIRoot gameOverlayUIRoot;
        internal static UIRoot chatRoot;

        public static void Init()
        {
            guiRoot = new UIRoot(() => new GuiScreen());
            escapeMenuUIRoot = new UIRoot(() => new EscapeMenu());
            gameOverlayUIRoot = new UIRoot(() => new GameOverlay());
            hotbarUIRoot = new UIRoot(() => new CHotbar(Player.Get().inventory));
            chatRoot = new UIRoot(() => new ChatScreen());
            gameOverlayUIRoot.Show();
            hotbarUIRoot.Show();
            chatRoot.Show();
        }

        public static void OnResize()
        {
            if (guiRoot.visible)
            {
                guiHandler.ResetGuis();
            }
            if (escapeMenuUIRoot.visible)
            {
                escapeMenuUIRoot.Hide();
                escapeMenuUIRoot.Show();
            }
            if (hotbarUIRoot.visible)
            {
                hotbarUIRoot.Hide();
                hotbarUIRoot.Show();
            }
            if (gameOverlayUIRoot.visible)
            {
                gameOverlayUIRoot.Hide();
                gameOverlayUIRoot.Show();
            }
            if (chatRoot.visible)
            {
                chatRoot.Hide();
                chatRoot.Show();
            }
        }

        public static void Update(double dt)
        {
            HandleInputs();

            if (showGameOverlay)
            {
                gameOverlayUIRoot.Update(dt);
                GameScreen.Update(dt);
            }
            if (showHotbar)
            {
                hotbarUIRoot.Update(dt);
            }
            if (showEscapeMenu)
            {
                escapeMenuUIRoot.Update(dt);
            }
            if (showGui)
            {
                guiRoot.Update(dt);
            }
            if (showChat)
            {
                chatRoot.Update(dt);
            }
        }

        static void HandleInputs()
        {
            if (KeyBinds.CheckPressedFirst(KeyBinds.OPEN_MENU))
            {
                //If any guis are visible, hide them
                if (showGui)
                {
                    guiHandler.HideGuis();
                    return;
                }

                //If not gui is visible, show the escape menu
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
            else if (KeyBinds.CheckPressedFirst(KeyBinds.TOGGLE_DEBUG))
            {
                if (showEscapeMenu) return;

                showDebugMenu = !showDebugMenu;
            }
            else if (KeyBinds.CheckPressedFirst(KeyBinds.CREATIVE_MENU))
            {
                if (showEscapeMenu) return;

                if (showGui)
                {
                    guiHandler.HideGuis();
                }
                else if (World.Get().gamemode == Gamemode.Creative)
                {
                    Player.Get().creativeInventory.ShowGui();
                    Player.Get().inventory.ShowGui();
                }

            }
            else if (KeyBinds.CheckPressedFirst(KeyBinds.CHAT))
            {
                if (showEscapeMenu) return;

                showChat = !showChat;
            }
            else if (KeyBinds.CheckPressedFirst(KeyBinds.SHOW_INV))
            {
                if (showEscapeMenu) return;

                if (showGui) //If currently visible, hide all guis
                {
                    guiHandler.HideGuis();
                }
                else
                {
                    Player.Get().inventory.ShowGui();
                }
            }
        }

        internal static void Render()
        {
            Renderer.PushDebugGroup("screen rendering");

            if (showGame && allowIngameInputs) GameScreen.Render();
            if (showHotbar) hotbarUIRoot.Render();
            if (showDebugMenu) DebugMenu.Render();
            if (showGui) guiRoot.Render();
            if (showEscapeMenu) escapeMenuUIRoot.Render();
            if (showGameOverlay) gameOverlayUIRoot.Render();
            if (showChat) chatRoot.Render();

            Renderer.PopDebugGroup();
        }
    }
}
