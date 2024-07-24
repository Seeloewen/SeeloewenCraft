using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace SeeloewenCraft
{
    public partial class wndSettings : Window
    {
        wndMenu wndMenu;
        KeyConverter keyConverter = new KeyConverter();

        //-- Constructor --//

        public wndSettings(wndMenu wndMenu)
        {
            this.wndMenu = wndMenu;
            InitializeComponent();

            //If the settings file exists, load it
            if(File.Exists($"{FolderUtil.gameFolder}\\clientSettings.json"))
            {
                JsonToken documentToken = JsonUtil.ReadFile($"{FolderUtil.gameFolder}\\clientSettings.json");
                LoadSettings(documentToken);
            }
            else
            {
                wndMenu.log.Write("No settings file was found, creating a new one...", "Info");

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
            writer.WriteStartObject();
            writer.WritePropertyName("settings");

            //Save all the settings 
            writer.WriteStartObject();
            writer.WritePropertyName("save_log_on_exit");
            writer.WriteValue(cbSaveLogOnExit.IsChecked);
            Settings.saveLogOnExit = Convert.ToBoolean(cbSaveLogOnExit.IsChecked);
            wndMenu.log.Write($"Saved setting saveLogOnExit as {Settings.saveLogOnExit}", "Info");

            writer.WritePropertyName("save_world_on_close");
            writer.WriteValue(cbSaveWorldWhenClosing.IsChecked);
            Settings.saveWorldOnClose = Convert.ToBoolean(cbSaveWorldWhenClosing.IsChecked);
            wndMenu.log.Write($"Saved setting saveWorldOnClose as {Settings.saveWorldOnClose}", "Info");

            writer.WritePropertyName("enable_hammer");
            writer.WriteValue(cbEnableHammer.IsChecked);
            Settings.enableHammer = Convert.ToBoolean(cbEnableHammer.IsChecked);
            wndMenu.log.Write($"Saved setting enableHammer as {Settings.enableHammer}", "Info");

            writer.WritePropertyName("enable_cave_generation");
            writer.WriteValue(cbEnableCaveGeneration.IsChecked);
            Settings.enableCaveGeneration = Convert.ToBoolean(cbEnableCaveGeneration.IsChecked);
            wndMenu.log.Write($"Saved setting enableCaveGeneration as {Settings.enableCaveGeneration}", "Info");

            writer.WritePropertyName("enable_lighting");
            writer.WriteValue(cbEnableLighting.IsChecked);
            Settings.enableLighting = Convert.ToBoolean(cbEnableLighting.IsChecked);
            wndMenu.log.Write($"Saved setting enableLighting as {Settings.enableLighting}", "Info");

            writer.WritePropertyName("show_notifications");
            writer.WriteValue(cbShowNotifications.IsChecked);
            Settings.showNotifications = Convert.ToBoolean(cbShowNotifications.IsChecked);
            wndMenu.log.Write($"Saved setting enableLighting as {Settings.showNotifications}", "Info");

            writer.WritePropertyName("enable_health");
            writer.WriteValue(cbEnableHealth.IsChecked);
            Settings.enableHealth = Convert.ToBoolean(cbEnableHealth.IsChecked);
            wndMenu.log.Write($"Saved setting enableLighting as {Settings.enableHealth}", "Info");

            writer.WritePropertyName("texturepack");
            writer.WriteValue(cbxTexturepack.Text);
            Settings.texturepack = cbxTexturepack.Text;
            wndMenu.log.Write($"Saved setting texturepack as {Settings.texturepack}", "Info");

            writer.WriteEndObject();

            writer.WritePropertyName("keybinds");

            //Save all the keybinds
            writer.WriteStartObject();
            writer.WritePropertyName("move_right");
            writer.WriteValue(tbMoveRight.Text);
            Settings.cMoveRight = keyConverter.StringToKey(tbMoveRight.Text);
            writer.WritePropertyName("move_left");
            writer.WriteValue(tbMoveLeft.Text);
            Settings.cMoveLeft = keyConverter.StringToKey(tbMoveLeft.Text);
            writer.WritePropertyName("jump");
            writer.WriteValue(tbJump.Text);
            Settings.cJump = keyConverter.StringToKey(tbJump.Text);
            writer.WritePropertyName("show_inventory");
            writer.WriteValue(tbOpenInventory.Text);
            Settings.cShowInv = keyConverter.StringToKey(tbOpenInventory.Text);
            writer.WritePropertyName("toggle_debug_menu");
            writer.WriteValue(tbToggleDebugMenu.Text);
            Settings.cToggleDebug = keyConverter.StringToKey(tbToggleDebugMenu.Text);
            writer.WritePropertyName("show_notification_list");
            writer.WriteValue(tbShowNotificationList.Text);
            Settings.cToggleDebug = keyConverter.StringToKey(tbShowNotificationList.Text);
            writer.WriteEndObject();

            writer.WriteEndObject();

            if(!suppressConfirmation)
            {
                MessageBox.Show("The settings have been saved successfully!", "Saved settings", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadSettings(JsonToken fileToken)
        {
            //Get the settings from the JSON file
            try
            {
                JsonToken settingsToken = fileToken.GetToken("/settings");
                JsonToken keybindsToken = fileToken.GetToken("/keybinds");

                Settings.saveLogOnExit = settingsToken.GetBool("/save_log_on_exit");
                Settings.saveWorldOnClose = settingsToken.GetBool("/save_world_on_close");
                Settings.enableHammer = settingsToken.GetBool("/enable_hammer");
                Settings.enableCaveGeneration = settingsToken.GetBool("/enable_cave_generation");
                Settings.enableLighting = settingsToken.GetBool("/enable_lighting");
                Settings.enableHealth = settingsToken.GetBool("/enable_health");
                Settings.showNotifications = settingsToken.GetBool("/show_notifications");
                Settings.texturepack = settingsToken.GetString("/texturepack");

                Settings.cMoveRight = keyConverter.StringToKey(keybindsToken.GetString("/move_right"));
                Settings.cMoveLeft = keyConverter.StringToKey(keybindsToken.GetString("/move_left"));
                Settings.cJump = keyConverter.StringToKey(keybindsToken.GetString("/jump"));
                Settings.cShowInv = keyConverter.StringToKey(keybindsToken.GetString("/show_inventory"));
                Settings.cToggleDebug = keyConverter.StringToKey(keybindsToken.GetString("/toggle_debug_menu"));
                Settings.cNotifications = keyConverter.StringToKey(keybindsToken.GetString("/show_notification_list"));
            }
            catch (Exception ex)
            {
                wndMenu.log.Write($"Error loading settings from file: {ex}", "Error");
            }

            //Change the checkboxes and textboxes to the loaded values
            cbSaveLogOnExit.IsChecked = Settings.saveLogOnExit;
            cbSaveWorldWhenClosing.IsChecked = Settings.saveWorldOnClose;
            cbEnableHammer.IsChecked = Settings.enableHammer;
            cbEnableCaveGeneration.IsChecked = Settings.enableCaveGeneration;
            cbEnableLighting.IsChecked = Settings.enableLighting;
            cbEnableHealth.IsChecked = Settings.enableHealth;
            cbShowNotifications.IsChecked = Settings.showNotifications;
            cbxTexturepack.Text = Settings.texturepack;

            tbMoveRight.Text = keyConverter.KeyToString(Settings.cMoveRight);
            tbMoveLeft.Text = keyConverter.KeyToString(Settings.cMoveLeft);
            tbJump.Text = keyConverter.KeyToString(Settings.cJump);
            tbOpenInventory.Text = keyConverter.KeyToString(Settings.cShowInv);
            tbToggleDebugMenu.Text = keyConverter.KeyToString(Settings.cToggleDebug);
            tbShowNotificationList.Text = keyConverter.KeyToString(Settings.cNotifications);

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
            if (File.Exists($"{wndMenu.selectedTexturepack}\\pack.txt"))
            {
                string[] fileContent = File.ReadAllLines($"{wndMenu.selectedTexturepack}\\pack.txt");

                if (fileContent.Length > 1)
                {
                    //Try to read the texturepack version
                    try
                    {
                        wndMenu.log.Write($"Detected texturepack version {fileContent[1].Replace("texturepackVersion=", "")}", "Info");
                        return Convert.ToInt32(fileContent[1].Replace("texturepackVersion=", ""));
                    }
                    catch (Exception ex)
                    {
                        wndMenu.log.Write($"Could not get texturepack version: {ex.Message}", "Error");
                        return 0;
                    }
                }
                else
                {
                    wndMenu.log.Write($"Could not get texturepack version because the file is empty", "Error");
                    return 0;
                }
            }
            else
            {
                wndMenu.log.Write($"Could not get texturepack version because the pack.txt file does not exist", "Error");
                return 0;
            }
        }

        public void ApplyTexturepack()
        {
            //Apply the texturepack
            if (cbxTexturepack.SelectedItem.ToString() == "default")
            {
                //Default texturepack
                wndMenu.selectedTexturepack = "default";
            }
            else
            {
                //Check the texturepack version and apply that if possible
                if (GetTexturepackVersion($"{FolderUtil.texturepacksFolder}\\{cbxTexturepack.SelectedItem}") < wndMenu.texturepackVersion)
                {
                    wndMenu.log.Write($"The texture pack you are trying to load ({FolderUtil.texturepacksFolder}\\{cbxTexturepack.SelectedItem}) is outdated", "Warning");
                    MessageBox.Show("Warning: The texturepack that you are trying to load is outdated. This may lead to issues.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                wndMenu.selectedTexturepack = $"{FolderUtil.texturepacksFolder}\\{cbxTexturepack.SelectedItem}";
                wndMenu.log.Write($"Successfully applied texturepack ({FolderUtil.texturepacksFolder}\\{cbxTexturepack.SelectedItem})", "Warning");
            }

            if (wndMenu.world != null)
            {
                if (wndMenu.world.finishedLoading)
                {
                    wndMenu.world.RefreshTextures();
                }
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
            wndMenu.log.Show();
        }

        private void tbMoveRight_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = keyConverter.KeyToString(e.Key);
            tbMoveRight.Text = keyText;
        }

        private void tbMoveLeft_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = keyConverter.KeyToString(e.Key);
            tbMoveLeft.Text = keyText;
        }

        private void tbJump_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = keyConverter.KeyToString(e.Key);
            tbJump.Text = keyText;
        }

        private void tbOpenInventory_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = keyConverter.KeyToString(e.Key);
            tbOpenInventory.Text = keyText;
        }

        private void tbToggleDebugMenu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = keyConverter.KeyToString(e.Key);
            tbToggleDebugMenu.Text = keyText;
        }

        private void tbShowNotificationList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = keyConverter.KeyToString(e.Key);
            tbShowNotificationList.Text = keyText;
        }
    }
}
