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
        KeyConverter keyConverter = new KeyConverter();

        //Settings
        public bool saveLogOnExit = false;
        public bool saveWorldOnClose = true;
        public bool enableHammer = true;
        public bool enableCaveGeneration = true;
        public bool enableLighting = true;
        public string texturepack;

        //Keybinds
        public Key cMoveRight = Key.D;
        public Key cMoveLeft = Key.A;
        public Key cShowInv = Key.E;
        public Key cToggleDebug = Key.F3;
        public Key cJump = Key.Space;

        //-- Constructor --//

        public wndSettings(wndMenu wndMenu)
        {
            this.wndMenu = wndMenu;
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
            saveLogOnExit = Convert.ToBoolean(cbSaveLogOnExit.IsChecked);
            wndMenu.log.Write($"Saved setting saveLogOnExit as {saveLogOnExit}", "Info");

            writer.WritePropertyName("save_world_on_close");
            writer.WriteValue(cbSaveWorldWhenClosing.IsChecked);
            saveWorldOnClose = Convert.ToBoolean(cbSaveWorldWhenClosing.IsChecked);
            wndMenu.log.Write($"Saved setting saveWorldOnClose as {saveWorldOnClose}", "Info");

            writer.WritePropertyName("enable_hammer");
            writer.WriteValue(cbEnableHammer.IsChecked);
            enableHammer = Convert.ToBoolean(cbEnableHammer.IsChecked);
            wndMenu.log.Write($"Saved setting enableHammer as {enableHammer}", "Info");

            writer.WritePropertyName("enable_cave_generation");
            writer.WriteValue(cbEnableCaveGeneration.IsChecked);
            enableCaveGeneration = Convert.ToBoolean(cbEnableCaveGeneration.IsChecked);
            wndMenu.log.Write($"Saved setting enableCaveGeneration as {enableCaveGeneration}", "Info");

            writer.WritePropertyName("enable_lighting");
            writer.WriteValue(cbEnableLighting.IsChecked);
            enableLighting = Convert.ToBoolean(cbEnableLighting.IsChecked);
            wndMenu.log.Write($"Saved setting enableLighting as {enableLighting}", "Info");

            writer.WritePropertyName("texturepack");
            writer.WriteValue(cbxTexturepack.Text);
            texturepack = cbxTexturepack.Text;
            wndMenu.log.Write($"Saved setting texturepack as {texturepack}", "Info");

            writer.WriteEndObject();

            writer.WritePropertyName("keybinds");

            //Save all the keybinds
            writer.WriteStartObject();
            writer.WritePropertyName("move_right");
            writer.WriteValue(tbMoveRight.Text);
            cMoveRight = keyConverter.StringToKey(tbMoveRight.Text);
            writer.WritePropertyName("move_left");
            writer.WriteValue(tbMoveLeft.Text);
            cMoveLeft = keyConverter.StringToKey(tbMoveLeft.Text);
            writer.WritePropertyName("jump");
            writer.WriteValue(tbJump.Text);
            cJump = keyConverter.StringToKey(tbJump.Text);
            writer.WritePropertyName("show_inventory");
            writer.WriteValue(tbOpenInventory.Text);
            cShowInv = keyConverter.StringToKey(tbOpenInventory.Text);
            writer.WritePropertyName("toggle_debug_menu");
            writer.WriteValue(tbToggleDebugMenu.Text);
            cToggleDebug = keyConverter.StringToKey(tbToggleDebugMenu.Text);
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

                saveLogOnExit = settingsToken.GetBool("/save_log_on_exit");
                saveWorldOnClose = settingsToken.GetBool("/save_world_on_close");
                enableHammer = settingsToken.GetBool("/enable_hammer");
                enableCaveGeneration = settingsToken.GetBool($"/enable_cave_generation");
                enableLighting = settingsToken.GetBool($"/enable_lighting");
                texturepack = settingsToken.GetString($"/texturepack");

                cMoveRight = keyConverter.StringToKey(keybindsToken.GetString($"/move_right"));
                cMoveLeft = keyConverter.StringToKey(keybindsToken.GetString($"/move_left"));
                cJump = keyConverter.StringToKey(keybindsToken.GetString($"/jump"));
                cShowInv = keyConverter.StringToKey(keybindsToken.GetString($"/show_inventory"));
                cToggleDebug = keyConverter.StringToKey(keybindsToken.GetString($"/toggle_debug_menu"));
            }
            catch (Exception ex)
            {
                wndMenu.log.Write($"Error loading settings from file: {ex}", "Error");
            }

            //Change the checkboxes and textboxes to the loaded values
            cbSaveLogOnExit.IsChecked = saveLogOnExit;
            cbSaveWorldWhenClosing.IsChecked = saveWorldOnClose;
            cbEnableHammer.IsChecked = enableHammer;
            cbEnableCaveGeneration.IsChecked = enableCaveGeneration;
            cbEnableLighting.IsChecked = enableLighting;
            cbxTexturepack.Text = texturepack;

            tbMoveRight.Text = keyConverter.KeyToString(cMoveRight);
            tbMoveLeft.Text = keyConverter.KeyToString(cMoveLeft);
            tbJump.Text = keyConverter.KeyToString(cJump);
            tbOpenInventory.Text = keyConverter.KeyToString(cShowInv);
            tbToggleDebugMenu.Text = keyConverter.KeyToString(cToggleDebug);

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
            if (cbxTexturepack.Items.Contains(texturepack))
            {
                cbxTexturepack.SelectedItem = texturepack;
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
    }
}
