using OpenTK.Graphics.OpenGL;
using SeeloewenCraft.game.core.entities.inventory;
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

        public static bool allowIngameInputs { get => !(showEscapeMenu || showIngameMenu || showGui); }

        internal static GuiHandler guiHandler = new GuiHandler();

        public static UIRoot guiRoot;
        internal static UIRoot escapeMenuUIRoot;
        internal static UIRoot hotbarUIRoot;
        internal static UIRoot gameOverlayUIRoot;

        public static void Init()
        {
            guiRoot = new UIRoot(() => new GuiScreen());
            escapeMenuUIRoot = new UIRoot(() => new EscapeMenu());
            gameOverlayUIRoot = new UIRoot(() => new GameOverlay());
            hotbarUIRoot = new UIRoot(() => new CHotbar(Game.world.player.inventory));
            gameOverlayUIRoot.Show();
            hotbarUIRoot.Show();
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
        }

        public static void Update(double dt)
        {
            HandleInputs();

            if (showGameOverlay)
            {
                gameOverlayUIRoot.Update(dt);
                hotbarUIRoot.Update(dt);
                GameScreen.Update(dt);
            }
            if (showEscapeMenu)
            {
                escapeMenuUIRoot.Update(dt);
            }
            if (showGui)
            {
                guiRoot.Update(dt);
            }
        }

        static void HandleInputs()
        {
            if (KeyBinds.checkPressedFirst(KeyBinds.OPEN_MENU))
            {
                //If any guis are visible, hide them
                if(showGui)
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
            else if (KeyBinds.checkPressedFirst(KeyBinds.TOGGLE_DEBUG))
            {
                showDebugMenu = !showDebugMenu;
            }
            else if(KeyBinds.checkPressedFirst(KeyBinds.CREATIVE_MENU))
            {
                if (showGui)
                {
                    guiHandler.HideGuis();
                }
                else if(Game.world.gamemode == Gamemode.Creative)
                {
                    Game.world.creativeInventory.ShowGui();
                    Game.world.player.inventory.ShowGui();
                }

            }
            else if (KeyBinds.checkPressedFirst(KeyBinds.SHOW_INV))
            {
                if(showGui) //If currently visible, hide all guis
                {
                    guiHandler.HideGuis();
                }
                else
                {
                    Game.world.player.inventory.ShowGui();
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
            
            Renderer.PopDebugGroup();
        }
    }
}
