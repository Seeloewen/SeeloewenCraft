using SeeloewenCraft.game.graphics.ui_lib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

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

        public static List<CGui> guis = new List<CGui>(); //(Currently) supports two guis being open
        public static ObservableCollection<IGuiData> guiData = new ObservableCollection<IGuiData>();

        public static UIRoot guiRoot;
        internal static UIRoot escapeMenuUIRoot;
        internal static UIRoot hotbarUIRoot;
        internal static UIRoot gameOverlayUIRoot;

        public static void Init()
        {
            guiData.CollectionChanged += GuiData_CollectionChanged;

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
                ResetGuis();
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
        
        public static void Update()
        {
            HandleInputs();

            if (showGameOverlay)
            {
                gameOverlayUIRoot.Update();
                hotbarUIRoot.Update();
                GameScreen.Update();
            }
            if (showEscapeMenu)
            {
                escapeMenuUIRoot.Update();
            }
            if (showGui)
            {
                guiRoot.Update();
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
                showGui = !showGui;

                if (!showGui)
                {
                    for (int i = guiData.Count - 1; i >= 0; i--) guiData[i].Hide(); //Hide all guis
                }
                else
                {
                    ((IGuiData)Game.world.player.inventory).Show();
                }

                if (showGui)
                {
                    guiRoot.Show();
                }
                else
                {
                    guiRoot.Hide();
                }
            }
        }

        internal static void Render()
        {
            if (showGame && allowIngameInputs) GameScreen.Render();
            if (showHotbar) hotbarUIRoot.Render();
            if (showDebugMenu) DebugMenu.Render();
            if (showGui) guiRoot.Render();
            if (showEscapeMenu) escapeMenuUIRoot.Render();
            if (showGameOverlay) gameOverlayUIRoot.Render();
        }

        internal static void ResetGuis()
        {
            //Update guis when the gui data collection changes
            guis.Clear();

            foreach (IGuiData data in guiData)
            {
                CGui gui = GetGui(data);
                guis.Add(gui);
            }

            guiRoot.Hide();
            guiRoot.Show();
        }

        private static void GuiData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ResetGuis();
        }

        public static CGui GetGui(IGuiData data) => data.guiId switch
        {
            "inventory" => new CInventory(data),
            _ => null
        };
    }
}
