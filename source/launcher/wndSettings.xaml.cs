using Newtonsoft.Json;
using SeeloewenCraft.game;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using JsonToken = SeeloewenCraft.game.util.JsonToken;
using JsonWriter = SeeloewenCraft.game.util.JsonWriter;

namespace SeeloewenCraft.launcher
{
    public partial class wndSettings : Window
    {
        wndMenu wndMenu;

        //-- Constructor --//

        public wndSettings(wndMenu wndMenu, bool firstStart)
        {
            this.wndMenu = wndMenu;
            InitializeComponent();

            //Add items to comboboxes
            cbxMode.Items.Add("Fullscreen");
            cbxMode.Items.Add("Windowed");
            cbxMode.Items.Add("Borderless");
            cbxMode.SelectedItem = "Fullscreen";

            cbxResolution.Items.Add("320x180"); //Warning: WILL lead to problems
            cbxResolution.Items.Add("640x360"); //Warning: Can lead to problems
            cbxResolution.Items.Add("1280x720");
            cbxResolution.Items.Add("1920x1080");
            cbxResolution.Items.Add("2560x1440");
            cbxResolution.Items.Add("3840x2160");
            cbxResolution.Items.Add("Custom");
            cbxResolution.SelectedItem = "1280x720";

            //If the settings file exists, load it
            if (File.Exists($"{FolderUtil.gameFolder}\\clientSettings.json"))
            {
                JsonToken documentToken = JsonUtil.ReadFile($"{FolderUtil.gameFolder}\\clientSettings.json");

                LoadSettings(documentToken, firstStart);
            }
            else
            {
                Log.Write("No settings file was found, creating a new one!", LogType.GENERAL, LogLevel.INFO);

                //If not, create a new one
                using (JsonWriter writer = JsonWriter.Create())
                {
                    writer.Formatting = Formatting.Indented;
                    SaveSettings(writer, true);
                    writer.WriteToFile($"{FolderUtil.gameFolder}\\clientSettings.json");
                }
            }
        }

        //-- Custom Methods --//
        private void SaveSettings(JsonWriter writer, bool suppressConfirmation)
        {
            //Save the settings locally
            Settings.saveLogOnExit = Convert.ToBoolean(cbSaveLogOnExit.IsChecked);
            Log.Write($"Saved setting saveLogOnExit as {Settings.saveLogOnExit}", LogType.GENERAL, LogLevel.INFO);

            Settings.saveWorldOnClose = Convert.ToBoolean(cbSaveWorldWhenClosing.IsChecked);
            Log.Write($"Saved setting saveWorldOnClose as {Settings.saveWorldOnClose}", LogType.GENERAL, LogLevel.INFO);

            Settings.showNotifications = Convert.ToBoolean(cbShowNotifications.IsChecked);
            Log.Write($"Saved setting showNotifications as {Settings.showNotifications}", LogType.GENERAL, LogLevel.INFO);

            Settings.enableMobs = Convert.ToBoolean(cbEnableMobs.IsChecked);
            Log.Write($"Saved setting enableMobs as {Settings.enableMobs}", LogType.GENERAL, LogLevel.INFO);

            Settings.enableAutoSave = Convert.ToBoolean(cbAutoSave.IsChecked);
            Log.Write($"Saved setting enableAutoSave as {Settings.enableAutoSave}", LogType.GENERAL, LogLevel.INFO);

            Settings.showAutoSaveNotification = Convert.ToBoolean(cbAutoSaveNotification.IsChecked);
            Log.Write($"Saved setting showAutoSaveNotification as {Settings.showAutoSaveNotification}", LogType.GENERAL, LogLevel.INFO);

            Settings.resolution = Convert.ToString(cbxResolution.SelectedItem);
            Log.Write($"Saved setting resolution as {Settings.resolution}", LogType.GENERAL, LogLevel.INFO);

            Settings.videoMode = Convert.ToString(cbxMode.SelectedItem);
            Log.Write($"Saved setting videoMode as {Settings.videoMode}", LogType.GENERAL, LogLevel.INFO);

            Settings.customResX = Convert.ToInt32(tbWidth.Text);
            Log.Write($"Saved setting customResX as {Settings.customResX}", LogType.GENERAL, LogLevel.INFO);

            Settings.customResY = Convert.ToInt32(tbHeight.Text);
            Log.Write($"Saved setting customResY as {Settings.customResY}", LogType.GENERAL, LogLevel.INFO);

            Settings.autoSaveInterval = Convert.ToInt32(tbAutosave.Text);
            Log.Write($"Saved setting autoSaveInterval as {Settings.autoSaveInterval}", LogType.GENERAL, LogLevel.INFO);

            Settings.texturepack = cbxTexturepack.Text;
            Log.Write($"Saved setting texturepack as {Settings.texturepack}", LogType.GENERAL, LogLevel.INFO);

            Settings.nickname = tbNickname.Text;
            Log.Write($"Saved setting nickname as {Settings.nickname}", LogType.GENERAL, LogLevel.INFO);

            //Save the log settings
            Settings.logEntities = Convert.ToBoolean(cbLogEntities.IsChecked);
            Log.Write($"Saved setting logEntities as {Settings.logEntities}", LogType.GENERAL, LogLevel.INFO);

            Settings.logGeneral = Convert.ToBoolean(cbLogGeneral.IsChecked);
            Log.Write($"Saved setting logGeneral as {Settings.logGeneral}", LogType.GENERAL, LogLevel.INFO);

            Settings.logNetwork = Convert.ToBoolean(cbLogNetwork.IsChecked);
            Log.Write($"Saved setting logNetwork as {Settings.logNetwork}", LogType.GENERAL, LogLevel.INFO);

            Settings.logRendering = Convert.ToBoolean(cbLogRendering.IsChecked);
            Log.Write($"Saved setting logRendering as {Settings.logRendering}", LogType.GENERAL, LogLevel.INFO);

            Settings.logStructureGeneration = Convert.ToBoolean(cbLogStructureGeneration.IsChecked);
            Log.Write($"Saved setting logStructureGeneration as {Settings.logStructureGeneration}", LogType.GENERAL, LogLevel.INFO);

            Settings.logWorldGeneration = Convert.ToBoolean(cbLogWorldGeneration.IsChecked);
            Log.Write($"Saved setting logWorldGeneration as {Settings.logWorldGeneration}", LogType.GENERAL, LogLevel.INFO);

            //Save the settings to file
            Settings.Save(writer);

            if (Game.world != null) //Game.world.wndGame.ApplyVideoSettings();

                if (!suppressConfirmation)
                {
                    MessageBox.Show("The settings have been saved successfully!", "Saved settings", MessageBoxButton.OK, MessageBoxImage.Information);
                }
        }

        private void LoadSettings(JsonToken fileToken, bool overwriteResolution)
        {
            Settings.Load(fileToken, overwriteResolution);

            //Change the checkboxes and textboxes to the loaded values
            cbSaveLogOnExit.IsChecked = Settings.saveLogOnExit;
            cbSaveWorldWhenClosing.IsChecked = Settings.saveWorldOnClose;
            cbEnableMobs.IsChecked = Settings.enableMobs;
            cbAutoSave.IsChecked = Settings.enableAutoSave;
            cbShowNotifications.IsChecked = Settings.showNotifications;
            cbAutoSaveNotification.IsChecked = Settings.showAutoSaveNotification;
            cbxTexturepack.Text = Settings.texturepack;
            cbxMode.Text = Settings.videoMode;
            cbxResolution.Text = Settings.resolution;
            tbHeight.Text = Settings.customResY.ToString();
            tbWidth.Text = Settings.customResX.ToString();
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

            //Load the texturepacks
            GetTexturepacks();
        }

        private void GetTexturepacks()
        {
            cbxTexturepack.Items.Clear();

            //Add entry for default texturepack
            cbxTexturepack.Items.Add("default");

            //Get all texturepacks
            string[] directories = Directory.GetDirectories(FolderUtil.texturepacksFolder);
            foreach (string directory in directories)
            {
                //Check if the texturepack has a pack file and add it to the list
                if (File.Exists($"{directory}\\pack.txt"))
                {
                    cbxTexturepack.Items.Add(directory.Replace($"{FolderUtil.texturepacksFolder}\\", ""));
                }
            }

            //Try to load default texturepack
            if (cbxTexturepack.Items.Contains(Settings.texturepack))
            {
                cbxTexturepack.SelectedItem = Settings.texturepack;
                ApplyTexturepack();
            }

        }

        private int GetTexturepackVersion(string texturepack)
        {
            //Check if the pack file exists
            if (File.Exists($"{Game.selectedTexturepack}\\pack.txt"))
            {
                string[] fileContent = File.ReadAllLines($"{Game.selectedTexturepack}\\pack.txt");

                if (fileContent.Length > 1)
                {
                    //Try to read the texturepack version
                    try
                    {
                        Log.Write($"Detected texturepack version {fileContent[1].Replace("texturepackVersion=", "")}", LogType.GENERAL, LogLevel.INFO);
                        return Convert.ToInt32(fileContent[1].Replace("texturepackVersion=", ""));
                    }
                    catch (Exception ex)
                    {
                        Log.Write($"Could not get texturepack version: {ex.Message}\n{ex.StackTrace}", LogType.GENERAL, LogLevel.ERROR);
                        return 0;
                    }
                }
                else
                {
                    Log.Write($"Could not get texturepack version because the pack.txt file is empty", LogType.GENERAL, LogLevel.ERROR);
                    return 0;
                }
            }
            else
            {
                Log.Write($"Could not get texturepack version because the pack.txt file does not exist", LogType.GENERAL, LogLevel.ERROR);
                return 0;
            }
        }

        public void ApplyTexturepack()
        {
            //Apply the texturepack
            if (cbxTexturepack.SelectedItem == null || cbxTexturepack.SelectedItem.ToString() == "default")
            {
                //Default texturepack
                Game.selectedTexturepack = "default";
            }
            else
            {
                Game.selectedTexturepack = $"{FolderUtil.texturepacksFolder}\\{cbxTexturepack.SelectedItem}";
                //Check the texturepack version and apply that if possible
                if (GetTexturepackVersion($"{FolderUtil.texturepacksFolder}\\{cbxTexturepack.SelectedItem}") < Game.TEXTUREPACK_VERSION)
                {
                    Log.Write($"The texture pack you are trying to load ({FolderUtil.texturepacksFolder}\\{cbxTexturepack.SelectedItem}) is outdated", LogType.GENERAL, LogLevel.WARNING);
                    MessageBox.Show("Warning: The texturepack that you are trying to load is outdated. This may lead to issues.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                Log.Write($"Successfully applied texturepack ({FolderUtil.texturepacksFolder}\\{cbxTexturepack.SelectedItem})", LogType.GENERAL, LogLevel.INFO);
            }

            if (Game.world != null && Game.world.finishedLoading)
            {
                //TODO: Game.world.RefreshTextures();
            }
        }

        //-- Event Handlers --//

        private void btnSaveClose_Click(object sender, RoutedEventArgs e)
        {
            //Save the settings
            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Formatting.Indented;
                SaveSettings(writer, false);
                writer.WriteToFile($"{FolderUtil.gameFolder}\\clientSettings.json");
            }

            //Apply some settings instantly
            if (Game.world != null)
            {
                // Game.world.gameLoop.autoSaveEvent.UpdateMaxTick(); //TODO: Events
            }

            ApplyTexturepack();
            Close();
        }

        private void btnApplyTexturepack_Click(object sender, RoutedEventArgs e)
        {
            //Apply the texturepack
            ApplyTexturepack();
            MessageBox.Show("The texturepack was applied!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            //Show the about window
            wndAbout wndAbout = new wndAbout(wndMenu);
            wndAbout.ShowDialog();
        }

        private void btnOpenLog_Click(object sender, RoutedEventArgs e)
        {
            //Show the log
            Log.Show();
        }

        private void tbMoveRight_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = e.Key.ToString();
            if (keyText.Equals("LeftCtrl")) keyText = "LeftControl";
            tbMoveRight.Text = e.Key.ToString();

        }

        private void tbMoveLeft_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = e.Key.ToString();
            if (keyText.Equals("LeftCtrl")) keyText = "LeftControl";
            tbMoveLeft.Text = keyText;
        }

        private void tbJump_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = e.Key.ToString();
            if (keyText.Equals("LeftCtrl")) keyText = "LeftControl";
            tbJump.Text = keyText;
        }

        private void tbOpenInventory_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = e.Key.ToString();
            if (keyText.Equals("LeftCtrl")) keyText = "LeftControl";
            tbOpenInventory.Text = keyText;
        }

        private void tbToggleDebugMenu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = e.Key.ToString();
            if (keyText.Equals("LeftCtrl")) keyText = "LeftControl";
            tbToggleDebugMenu.Text = keyText;
        }

        private void tbShowNotificationList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = e.Key.ToString();
            if (keyText.Equals("LeftCtrl")) keyText = "LeftControl";
            tbShowNotificationList.Text = keyText;
        }

        private void tbThrowItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = e.Key.ToString();
            if (keyText.Equals("LeftCtrl")) keyText = "LeftControl";
            tbThrowItem.Text = keyText;
        }

        private void tbSneak_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = e.Key.ToString();
            if (keyText.Equals("LeftCtrl")) keyText = "LeftControl";
            tbSneak.Text = keyText;
        }

        private void tbSprint_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = e.Key.ToString();
            if (keyText.Equals("LeftCtrl")) keyText = "LeftControl";
            tbSprint.Text = keyText;
        }

        private void tbHeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbWidth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cbxResolution_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains("Custom"))
            {
                tbWidth.Visibility = Visibility.Visible;
                tbHeight.Visibility = Visibility.Visible;
                tblWidth.Visibility = Visibility.Visible;
                tblHeight.Visibility = Visibility.Visible;
            }
            else
            {
                tbWidth.Visibility = Visibility.Hidden;
                tbHeight.Visibility = Visibility.Hidden;
                tblWidth.Visibility = Visibility.Hidden;
                tblHeight.Visibility = Visibility.Hidden;
            }
        }

        private void tbAutosave_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cbAutoSave_Click(object sender, RoutedEventArgs e)
        {
            tbAutosave.IsEnabled = (bool)cbAutoSave.IsChecked;
        }

        private void tbAutosave_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
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
    }
}
