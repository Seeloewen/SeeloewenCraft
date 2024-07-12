using Microsoft.Json.Pointer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SeeloewenCraft
{
    public partial class wndSettings : Window
    {
        wndMenu wndMenu;
        Settings settings;
        KeyConverter keyConverter = new KeyConverter();

        //-- Constructor --//

        public wndSettings(wndMenu wndMenu, Settings settings)
        {
            this.wndMenu = wndMenu;
            this.settings = settings;
            InitializeComponent();

            //If the settings file exists, load it
            if(File.Exists($"{wndMenu.gameDirectory}\\clientSettings.json"))
            {
                wndMenu.log.Write("Settings file was found, loading settings...", "Info");
                JsonToken documentToken = JsonUtil.ReadFile($"{wndMenu.gameDirectory}\\clientSettings.json");
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
                    writer.WriteToFile($"{wndMenu.gameDirectory}\\clientSettings.json");
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
            settings.saveLogOnExit = Convert.ToBoolean(cbSaveLogOnExit.IsChecked);
            wndMenu.log.Write($"Saved setting saveLogOnExit as {settings.saveLogOnExit}", "Info");

            writer.WritePropertyName("save_world_on_close");
            writer.WriteValue(cbSaveWorldWhenClosing.IsChecked);
            settings.saveWorldOnClose = Convert.ToBoolean(cbSaveWorldWhenClosing.IsChecked);
            wndMenu.log.Write($"Saved setting saveWorldOnClose as {settings.saveWorldOnClose}", "Info");

            writer.WritePropertyName("enable_hammer");
            writer.WriteValue(cbEnableHammer.IsChecked);
            settings.enableHammer = Convert.ToBoolean(cbEnableHammer.IsChecked);
            wndMenu.log.Write($"Saved setting enableHammer as {settings.enableHammer}", "Info");

            writer.WritePropertyName("enable_cave_generation");
            writer.WriteValue(cbEnableCaveGeneration.IsChecked);
            settings.enableCaveGeneration = Convert.ToBoolean(cbEnableCaveGeneration.IsChecked);
            wndMenu.log.Write($"Saved setting enableCaveGeneration as {settings.enableCaveGeneration}", "Info");

            writer.WritePropertyName("enable_lighting");
            writer.WriteValue(cbEnableLighting.IsChecked);
            settings.enableLighting = Convert.ToBoolean(cbEnableLighting.IsChecked);
            wndMenu.log.Write($"Saved setting enableLighting as {settings.enableLighting}", "Info");

            writer.WritePropertyName("show_notifications");
            writer.WriteValue(cbShowNotifications.IsChecked);
            settings.showNotifications = Convert.ToBoolean(cbShowNotifications.IsChecked);
            wndMenu.log.Write($"Saved setting enableLighting as {settings.showNotifications}", "Info");

            writer.WritePropertyName("enable_health");
            writer.WriteValue(cbEnableHealth.IsChecked);
            enableLighting = Convert.ToBoolean(cbEnableHealth.IsChecked);
            wndMenu.log.Write($"Saved setting enableLighting as {enableHealth}", "Info");

            writer.WritePropertyName("texturepack");
            writer.WriteValue(cbxTexturepack.Text);
            settings.texturepack = cbxTexturepack.Text;
            wndMenu.log.Write($"Saved setting texturepack as {settings.texturepack}", "Info");

            writer.WriteEndObject();

            writer.WritePropertyName("keybinds");

            //Save all the keybinds
            writer.WriteStartObject();
            writer.WritePropertyName("move_right");
            writer.WriteValue(tbMoveRight.Text);
            settings.cMoveRight = keyConverter.StringToKey(tbMoveRight.Text);
            writer.WritePropertyName("move_left");
            writer.WriteValue(tbMoveLeft.Text);
            settings.cMoveLeft = keyConverter.StringToKey(tbMoveLeft.Text);
            writer.WritePropertyName("jump");
            writer.WriteValue(tbJump.Text);
            settings.cJump = keyConverter.StringToKey(tbJump.Text);
            writer.WritePropertyName("show_inventory");
            writer.WriteValue(tbOpenInventory.Text);
            settings.cShowInv = keyConverter.StringToKey(tbOpenInventory.Text);
            writer.WritePropertyName("toggle_debug_menu");
            writer.WriteValue(tbToggleDebugMenu.Text);
            settings.cToggleDebug = keyConverter.StringToKey(tbToggleDebugMenu.Text);
            writer.WritePropertyName("show_notification_list");
            writer.WriteValue(tbShowNotificationList.Text);
            settings.cToggleDebug = keyConverter.StringToKey(tbShowNotificationList.Text);
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

                settings.saveLogOnExit = settingsToken.GetBool("/save_log_on_exit");
                settings.saveWorldOnClose = settingsToken.GetBool("/save_world_on_close");
                settings.enableHammer = settingsToken.GetBool("/enable_hammer");
                settings.enableCaveGeneration = settingsToken.GetBool("/enable_cave_generation");
                settings.enableLighting = settingsToken.GetBool("/enable_lighting");
                settings.enableHealth = settingsToken.GetBool("/enable_health");
                settings.showNotifications = settingsToken.GetBool("/show_notifications");
                settings.texturepack = settingsToken.GetString("/texturepack");

                settings.cMoveRight = keyConverter.StringToKey(keybindsToken.GetString("/move_right"));
                settings.cMoveLeft = keyConverter.StringToKey(keybindsToken.GetString("/move_left"));
                settings.cJump = keyConverter.StringToKey(keybindsToken.GetString("/jump"));
                settings.cShowInv = keyConverter.StringToKey(keybindsToken.GetString("/show_inventory"));
                settings.cToggleDebug = keyConverter.StringToKey(keybindsToken.GetString("/toggle_debug_menu"));
                settings.cNotifications = keyConverter.StringToKey(keybindsToken.GetString("/show_notification_list"));
            }
            catch (Exception ex)
            {
                wndMenu.log.Write($"Error loading settings from file: {ex}", "Error");
            }

            //Change the checkboxes and textboxes to the loaded values
            cbSaveLogOnExit.IsChecked = settings.saveLogOnExit;
            cbSaveWorldWhenClosing.IsChecked = settings.saveWorldOnClose;
            cbEnableHammer.IsChecked = settings.enableHammer;
            cbEnableCaveGeneration.IsChecked = settings.enableCaveGeneration;
            cbEnableLighting.IsChecked = settings.enableLighting;
            cbEnableHealth.IsChecked = settings.enableHealth;
            cbShowNotifications.IsChecked = settings.showNotifications;
            cbxTexturepack.Text = settings.texturepack;

            tbMoveRight.Text = keyConverter.KeyToString(settings.cMoveRight);
            tbMoveLeft.Text = keyConverter.KeyToString(settings.cMoveLeft);
            tbJump.Text = keyConverter.KeyToString(settings.cJump);
            tbOpenInventory.Text = keyConverter.KeyToString(settings.cShowInv);
            tbToggleDebugMenu.Text = keyConverter.KeyToString(settings.cToggleDebug);
            tbShowNotificationList.Text = keyConverter.KeyToString(settings.cNotifications);

            //Load the texturepacks
            GetTexturepacks();
        }

        private void GetTexturepacks()
        {
            cbxTexturepack.Items.Clear();

            //Add entry for default texturepack
            cbxTexturepack.Items.Add("default");

            //Get all texturepacks
            string[] directories = Directory.GetDirectories(wndMenu.texturepackDirectory);
            foreach (string directory in directories)
            {
                //Check if the texturepack has a pack file and add it to the list
                if (File.Exists($"{directory}\\pack.txt"))
                {
                    cbxTexturepack.Items.Add(directory.Replace($"{wndMenu.texturepackDirectory}\\", ""));
                }
            }

            //Try to load default texturepack
            if (cbxTexturepack.Items.Contains(settings.texturepack))
            {
                cbxTexturepack.SelectedItem = settings.texturepack;
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
                if (GetTexturepackVersion($"{wndMenu.texturepackDirectory}\\{cbxTexturepack.SelectedItem}") < wndMenu.texturepackVersion)
                {
                    wndMenu.log.Write($"The texture pack you are trying to load ({wndMenu.texturepackDirectory}\\{cbxTexturepack.SelectedItem}) is outdated", "Warning");
                    MessageBox.Show("Warning: The texturepack that you are trying to load is outdated. This may lead to issues.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                wndMenu.selectedTexturepack = $"{wndMenu.texturepackDirectory}\\{cbxTexturepack.SelectedItem}";
                wndMenu.log.Write($"Successfully applied texturepack ({wndMenu.texturepackDirectory}\\{cbxTexturepack.SelectedItem})", "Warning");
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
                writer.WriteToFile($"{wndMenu.gameDirectory}\\clientSettings.json");
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
