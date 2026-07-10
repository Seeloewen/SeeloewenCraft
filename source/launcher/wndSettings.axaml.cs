using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeeloewenCraft.game;
using SeeloewenCraft.game.core.events;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace SeeloewenCraft.launcher
{
    internal class TexturepackDisplay : Canvas
    {
        internal readonly string id;

        private readonly TextBlock tblName = new TextBlock() { FontSize = 18, FontWeight = FontWeight.DemiBold };
        private readonly TextBlock tblDescription = new TextBlock() { FontSize = 16 };

        internal TexturepackDisplay(string name, string desc, string id)
        {
            this.id = id;
            Height = 50;

            tblName.Text = name;
            tblDescription.Text = desc;
            tblDescription.MaxWidth = 175;
            tblDescription.MaxHeight = 30;

            SetTop(tblName, 2);
            SetLeft(tblName, 5);

            SetTop(tblDescription, 25);
            SetLeft(tblDescription, 5);

            Children.Add(tblDescription);
            Children.Add(tblName);
        }

        internal void SetAsOutdated()
        {
            tblDescription.Text = "Outdated Texturepack";
            tblDescription.Foreground = new SolidColorBrush(Colors.Red);
            Log.Write($"Detected texturepack {id} as outdated! Be careful, loading it may not work as intended", LogType.RENDERING, LogLevel.WARNING);
        }
    }

    public partial class wndSettings : Window
    {
        private ListBox focusedList;

        private ObservableCollection<TexturepackDisplay> disabledTexturepacks = new();
        private ObservableCollection<TexturepackDisplay> enabledTexturepacks = new();

        private bool texturepackChanged; //Used to determine whether to reload texture at the end 

        public wndSettings(bool firstStart)
        {
            InitializeComponent();

            lbDisabled.ItemsSource = disabledTexturepacks;
            lbEnabled.ItemsSource = enabledTexturepacks;

            //If the settings file exists, load it
            if (File.Exists(Path.Combine(FolderUtil.gameFolder, "clientSettings.json")))
            {
                JObject settingsObj = JObject.Parse(File.ReadAllText(Path.Combine(FolderUtil.gameFolder, "clientSettings.json")));
                LoadSettings(settingsObj);
            }
            else
            {
                Log.Write("No settings file was found, creating a new one!", LogType.GENERAL, LogLevel.INFO);
                SaveSettings(true);
            }
        }

        //-- Custom Methods --//
        private void SaveSettings(bool suppressConfirmation)
        {
            //Save the normal settings
            Settings.saveLogOnExit = Convert.ToBoolean(cbSaveLogOnExit.IsChecked);
            Settings.saveWorldOnClose = Convert.ToBoolean(cbSaveWorldWhenClosing.IsChecked);
            Settings.showNotifications = Convert.ToBoolean(cbShowNotifications.IsChecked);
            Settings.enableMobs = Convert.ToBoolean(cbEnableMobs.IsChecked);
            Settings.enableAutoSave = Convert.ToBoolean(cbAutoSave.IsChecked);
            Settings.showAutoSaveNotification = Convert.ToBoolean(cbAutoSaveNotification.IsChecked);
            Settings.autoSaveInterval = Convert.ToInt32(tbAutosave.Text);
            Settings.nickname = tbNickname.Text;

            //Save the log settings
            Settings.logEntities = Convert.ToBoolean(cbLogEntities.IsChecked);
            Settings.logGeneral = Convert.ToBoolean(cbLogGeneral.IsChecked);
            Settings.logNetwork = Convert.ToBoolean(cbLogNetwork.IsChecked);
            Settings.logRendering = Convert.ToBoolean(cbLogRendering.IsChecked);
            Settings.logStructureGeneration = Convert.ToBoolean(cbLogStructureGeneration.IsChecked);
            Settings.logWorldGeneration = Convert.ToBoolean(cbLogWorldGeneration.IsChecked);

            //Save keybinds
            KeyBinds.Bind(KeyBinds.ToGLFWKey(tbMoveRight.Text), KeyBinds.MOVE_RIGHT);
            KeyBinds.Bind(KeyBinds.ToGLFWKey(tbMoveLeft.Text), KeyBinds.MOVE_LEFT);
            KeyBinds.Bind(KeyBinds.ToGLFWKey(tbOpenInventory.Text), KeyBinds.SHOW_INV);
            KeyBinds.Bind(KeyBinds.ToGLFWKey(tbToggleDebugMenu.Text), KeyBinds.TOGGLE_DEBUG);
            KeyBinds.Bind(KeyBinds.ToGLFWKey(tbJump.Text), KeyBinds.JUMP);
            KeyBinds.Bind(KeyBinds.ToGLFWKey(tbShowNotificationList.Text), KeyBinds.NOTIFICATIONS);
            KeyBinds.Bind(KeyBinds.ToGLFWKey(tbThrowItem.Text), KeyBinds.THROW_ITEM);
            KeyBinds.Bind(KeyBinds.ToGLFWKey(tbSneak.Text), KeyBinds.SNEAK);
            KeyBinds.Bind(KeyBinds.ToGLFWKey(tbSprint.Text), KeyBinds.SPRINT);

            Log.Write($"Successfully saved settings", LogType.GENERAL, LogLevel.INFO);

            //Save the settings to file
            JObject settingsObj = Settings.Save();
            File.WriteAllText(Path.Combine(FolderUtil.gameFolder, "clientSettings.json"), settingsObj.ToString());

            //Apply settings
            if (texturepackChanged && Game.glInitialized) TextureManager.Load();
            if (GameEventHandler.isInitialized) GameEventHandler.Register(new AutoSaveEvent(Settings.autoSaveInterval * 60000));

            if (!suppressConfirmation)
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Saved settings", "The settings have been saved successfully!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
                box.ShowWindowAsync();
            }
        }

        private void LoadSettings(JObject obj)
        {
            Settings.Load(obj);

            //Change the checkboxes and textboxes to the loaded values
            cbSaveLogOnExit.IsChecked = Settings.saveLogOnExit;
            cbSaveWorldWhenClosing.IsChecked = Settings.saveWorldOnClose;
            cbEnableMobs.IsChecked = Settings.enableMobs;
            cbAutoSave.IsChecked = Settings.enableAutoSave;
            cbShowNotifications.IsChecked = Settings.showNotifications;
            cbAutoSaveNotification.IsChecked = Settings.showAutoSaveNotification;
            tbAutosave.Text = Settings.autoSaveInterval.ToString();
            tbAutosave.IsEnabled = Settings.enableAutoSave;
            tbNickname.Text = Settings.nickname;

            cbLogGeneral.IsChecked = Settings.logGeneral;
            cbLogWorldGeneration.IsChecked = Settings.logWorldGeneration;
            cbLogStructureGeneration.IsChecked = Settings.logStructureGeneration;
            cbLogNetwork.IsChecked = Settings.logNetwork;
            cbLogEntities.IsChecked = Settings.logEntities;
            cbLogRendering.IsChecked = Settings.logRendering;

            /*tbMoveRight.Text = KeyConverter.KeyToString(Settings.cMoveRight);
            tbMoveRight.Text = KeyBinds.
            tbMoveLeft.Text = KeyConverter.KeyToString(Settings.cMoveLeft);
            tbJump.Text = KeyConverter.KeyToString(Settings.cJump);
            tbOpenInventory.Text = KeyConverter.KeyToString(Settings.cShowInv);
            tbToggleDebugMenu.Text = KeyConverter.KeyToString(Settings.cToggleDebug);
            tbShowNotificationList.Text = KeyConverter.KeyToString(Settings.cNotifications);
            tbThrowItem.Text = KeyConverter.KeyToString(Settings.cThrowItem);
            tbSneak.Text = KeyConverter.KeyToString(Settings.cSneak);
            tbSprint.Text = KeyConverter.KeyToString(Settings.cSprint);*/

            //Load and display the texturepacks
            InitTexturepackDisplays();
            Settings.texturepacks.ForEach(EnableTexturepack);
            texturepackChanged = false;
        }

        private void InitTexturepackDisplays()
        {
            //Get all texturepacks
            string[] directories = Directory.GetDirectories(FolderUtil.texturepacksFolder);
            foreach (string directory in directories)
            {
                string packFile = Path.Combine(directory, "pack.json");
                if (File.Exists(packFile))
                {
                    //Load the texture pack properties and add it to the list, disabled by default
                    try
                    {
                        JObject pack = JObject.Parse(File.ReadAllText(packFile));
                        string name = (pack["name"] ?? "Unknown").ToString();
                        string description = (pack["description"] ?? "").ToString();
                        int ver = Convert.ToInt32(pack["version"] ?? 0);

                        TexturepackDisplay display = new TexturepackDisplay(name, description, directory);
                        if (ver < Game.TEXTUREPACK_VERSION) display.SetAsOutdated();
                        disabledTexturepacks.Add(display);
                    }
                    catch (Exception e)
                    {
                        Log.Write($"Could not parse pack.json for datapack {directory}:\n{e.StackTrace}", LogType.RENDERING, LogLevel.ERROR);
                    }
                }
            }
        }

        private void EnableTexturepack(string texturepack)
        {
            //Fetch the corresponding display and move it to the enabled box
            TexturepackDisplay display = disabledTexturepacks.Get<TexturepackDisplay>(e => e.id == texturepack);
            if (display != null)
            {
                disabledTexturepacks.Remove(display);
                enabledTexturepacks.Add(display);

                //Also update the settings accordingly
                if (!Settings.texturepacks.Contains(texturepack))
                {
                    Settings.texturepacks.Add(texturepack);
                    texturepackChanged = true;
                }
            }
        }

        private void DisableTexturepack(string texturepack)
        {
            //Fetch the corresponding display and move it to the disabled box
            TexturepackDisplay display = enabledTexturepacks.Get<TexturepackDisplay>(e => e.id == texturepack);
            if (display != null)
            {
                enabledTexturepacks.Remove(display);
                disabledTexturepacks.Add(display);

                Settings.texturepacks.Remove(texturepack);
                texturepackChanged = true;
            }
        }

        private void btnSaveClose_Click(object sender, RoutedEventArgs e)
        {
            //Save the settings
            Close();
            SaveSettings(false);
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            //Show the about window
            wndAbout wndAbout = new wndAbout();
            wndAbout.ShowDialog(this);
        }

        private void btnOpenLog_Click(object sender, RoutedEventArgs e)
        {
            //Show the log
            Log.Show();
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            string keyText = e.Key.ToString();
            ((TextBox)sender).Text = KeyBinds.ToGLFWKey(e.Key).ToString();
        }

        private void tbAutosave_PreviewTextInput(object sender, TextInputEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cbAutoSave_Click(object sender, RoutedEventArgs e)
        {
            tbAutosave.IsEnabled = (bool)cbAutoSave.IsChecked;
        }

        private void tbAutosave_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(tbAutosave.Text, out int interval))
            {
                if (interval == 1)
                {
                    cbAutoSave.Content = "Auto-Save the game every               minute";
                }
                else
                {
                    cbAutoSave.Content = "Auto-Save the game every               minutes";
                }
            }
        }

        private void btnEnable_Click(object sender, RoutedEventArgs e)
        {
            TexturepackDisplay display = (TexturepackDisplay)lbDisabled.SelectedItem;
            if (display != null) EnableTexturepack(display.id);
        }

        private void btnDisable_Click(object sender, RoutedEventArgs e)
        {
            TexturepackDisplay display = (TexturepackDisplay)lbEnabled.SelectedItem;
            if (display != null) DisableTexturepack(display.id);
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            //Get the selected item and the one below and swap them
            if (focusedList != null)
            {
                var collection = (ObservableCollection<TexturepackDisplay>)focusedList.ItemsSource;
                TexturepackDisplay selectedItem = (TexturepackDisplay)focusedList.SelectedItem;
                int selectedIndex = collection.IndexOf(selectedItem);

                if (selectedIndex == collection.Count - 1) return; //If it's the bottom most one do nothing
                TexturepackDisplay itemBelow = collection[selectedIndex + 1];
                texturepackChanged = true;

                if (focusedList.Tag.ToString() == "enabled") Settings.texturepacks.Swap<string>(selectedItem.id, itemBelow.id); //When dealing with the enabled list, also update the actual texturepack list  
                collection.Swap<TexturepackDisplay>(selectedItem, itemBelow);
            }

        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            //Get the selected item and the one below and swap them
            if (focusedList != null)
            {
                var collection = (ObservableCollection<TexturepackDisplay>)focusedList.ItemsSource;
                TexturepackDisplay selectedItem = (TexturepackDisplay)focusedList.SelectedItem;
                int selectedIndex = collection.IndexOf(selectedItem);

                if (selectedIndex == 0) return; //If it's the bottom most one do nothing
                TexturepackDisplay itemAbove = collection[selectedIndex - 1];
                texturepackChanged = true;

                if (focusedList.Tag.ToString() == "enabled") Settings.texturepacks.Swap<string>(selectedItem.id, itemAbove.id); //When dealing with the enabled list, also update the actual texturepack list  
                collection.Swap<TexturepackDisplay>(selectedItem, itemAbove);
            }
        }

        private void lb_GotFocus(object sender, FocusChangedEventArgs e) => focusedList = sender as ListBox;
    }
}
