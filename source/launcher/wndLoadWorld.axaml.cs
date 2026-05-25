using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SeeloewenCraft.game;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SeeloewenCraft.launcher
{
    public partial class wndLoadWorld : Window
    {

        public List<string> worldList = new List<string>();
        public wndMenu wndMenu;
        public wndCreateWorld wndCreateWorld;
        private MultiplayerType multiplayerType;

        public wndLoadWorld(wndMenu wndMenu, MultiplayerType multiplayerType)
        {
            InitializeComponent();

            //Set the main menu
            this.wndMenu = wndMenu;
            this.multiplayerType = multiplayerType;

            //Get the world list
            LoadWorlds();
        }

        private void LoadWorlds()
        {
            //Get all worlds
            string[] worlds = Directory.GetDirectories(FolderUtil.worldsFolder);

            //List the worlds in the combobox
            foreach (string world in worlds)
            {
                cbxWorld.Items.Add(world.Replace(FolderUtil.worldsFolder, ""));
            }
        }

        private async void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cbxWorld.Text) == false)
            {
                Game.Create(cbxWorld.Text, 0, false, MultiplayerType.OFFLINE, wndMenu);
                wndMenu.Hide();
                Close();
            }
            else
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Error", "Please select a world you want to play or create a new one");
                await box.ShowAsync();
            }
        }

        private void btnCreateNewWorld_Click(object sender, RoutedEventArgs e)
        {
            //Show the window for creating a new world
            wndCreateWorld = new wndCreateWorld(wndMenu, multiplayerType);
            Close();
            wndCreateWorld.ShowDialog(wndMenu);
        }

        private async Task<ButtonResult> ShowDeleteConfirmation()
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Delete world", "You will not be able to recover deleted worlds. Are you sure you want to delete this world?", ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Warning);
            return await box.ShowAsync();
        }

        private async void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cbxWorld.Text))
            {
                //Ask the user if they want to delete the world
                var result = await ShowDeleteConfirmation();

                switch (result)
                {
                    case ButtonResult.Yes:
                        try
                        {
                            Directory.Delete(Path.Combine(FolderUtil.worldsFolder, cbxWorld.Text), true);
                            var box = MessageBoxManager.GetMessageBoxStandard("Delete world", $"Successfully deleted world {cbxWorld.Text}!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
                            await box.ShowAsync();
                            cbxWorld.Items.Remove(cbxWorld.Text);
                        }
                        catch (Exception ex)
                        {
                            Log.Write($"Could not delete world {cbxWorld.SelectedItem}: {ex.Message}\n{ex.StackTrace}", LogType.GENERAL, LogLevel.ERROR);
                        }
                        Log.Write($"Deleted world {cbxWorld.SelectedItem}", LogType.GENERAL, LogLevel.INFO);
                        break;
                }
            }
            else
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Delete world", "Please select the world that you want to delete!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            }
        }
    }
}
