using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace SeeloewenCraft
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
                Log.Write("No settings file was found, creating a new one...", "Info");

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
            Log.Write($"Saved setting saveLogOnExit as {Settings.saveLogOnExit}", "Info");

            Settings.saveWorldOnClose = Convert.ToBoolean(cbSaveWorldWhenClosing.IsChecked);
            Log.Write($"Saved setting saveWorldOnClose as {Settings.saveWorldOnClose}", "Info");

            Settings.enableHammer = Convert.ToBoolean(cbEnableHammer.IsChecked);
            Log.Write($"Saved setting enableHammer as {Settings.enableHammer}", "Info");

            Settings.enableCaveGeneration = Convert.ToBoolean(cbEnableCaveGeneration.IsChecked);
            Log.Write($"Saved setting enableCaveGeneration as {Settings.enableCaveGeneration}", "Info");

            Settings.enableLighting = Convert.ToBoolean(cbEnableLighting.IsChecked);
            Log.Write($"Saved setting enableLighting as {Settings.enableLighting}", "Info");

            Settings.showNotifications = Convert.ToBoolean(cbShowNotifications.IsChecked);
            Log.Write($"Saved setting enableLighting as {Settings.showNotifications}", "Info");

            Settings.enableHealth = Convert.ToBoolean(cbEnableHealth.IsChecked);
            Log.Write($"Saved setting enableHealth as {Settings.enableHealth}", "Info");

            Settings.resolution = Convert.ToString(cbxResolution.SelectedItem);
            Log.Write($"Saved setting resolution as {Settings.resolution}", "Info");

            Settings.videoMode = Convert.ToString(cbxMode.SelectedItem);
            Log.Write($"Saved setting videoMode as {Settings.videoMode}", "Info");

            Settings.customResX = Convert.ToInt32(tbWidth.Text);
            Log.Write($"Saved setting customResX as {Settings.customResX}", "Info");

            Settings.customResY = Convert.ToInt32(tbHeight.Text);
            Log.Write($"Saved setting customResY as {Settings.customResY}", "Info");

            Settings.texturepack = cbxTexturepack.Text;
            Log.Write($"Saved setting texturepack as {Settings.texturepack}", "Info");

            //Save the keybinds locally
            Settings.cMoveRight = KeyConverter.StringToKey(tbMoveRight.Text);
            Settings.cMoveLeft = KeyConverter.StringToKey(tbMoveLeft.Text);
            Settings.cJump = KeyConverter.StringToKey(tbJump.Text);
            Settings.cShowInv = KeyConverter.StringToKey(tbOpenInventory.Text);
            Settings.cToggleDebug = KeyConverter.StringToKey(tbToggleDebugMenu.Text);
            Settings.cNotifications = KeyConverter.StringToKey(tbShowNotificationList.Text);

            //Save the settings to file
            Settings.Save(writer);

            if (wndMenu.world != null) wndMenu.world.wndGame.ApplyVideoSettings();
            
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
            cbEnableHammer.IsChecked = Settings.enableHammer;
            cbEnableCaveGeneration.IsChecked = Settings.enableCaveGeneration;
            cbEnableLighting.IsChecked = Settings.enableLighting;
            cbEnableHealth.IsChecked = Settings.enableHealth;
            cbShowNotifications.IsChecked = Settings.showNotifications;
            cbxTexturepack.Text = Settings.texturepack;
            cbxMode.Text = Settings.videoMode;
            cbxResolution.Text = Settings.resolution;
            tbHeight.Text = Settings.customResY.ToString();
            tbWidth.Text = Settings.customResX.ToString();

            tbMoveRight.Text = KeyConverter.KeyToString(Settings.cMoveRight);
            tbMoveLeft.Text = KeyConverter.KeyToString(Settings.cMoveLeft);
            tbJump.Text = KeyConverter.KeyToString(Settings.cJump);
            tbOpenInventory.Text = KeyConverter.KeyToString(Settings.cShowInv);
            tbToggleDebugMenu.Text = KeyConverter.KeyToString(Settings.cToggleDebug);
            tbShowNotificationList.Text = KeyConverter.KeyToString(Settings.cNotifications);

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
                        Log.Write($"Detected texturepack version {fileContent[1].Replace("texturepackVersion=", "")}", "Info");
                        return Convert.ToInt32(fileContent[1].Replace("texturepackVersion=", ""));
                    }
                    catch (Exception ex)
                    {
                        Log.Write($"Could not get texturepack version: {ex.Message}", "Error");
                        return 0;
                    }
                }
                else
                {
                    Log.Write($"Could not get texturepack version because the file is empty", "Error");
                    return 0;
                }
            }
            else
            {
                Log.Write($"Could not get texturepack version because the pack.txt file does not exist", "Error");
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
                    Log.Write($"The texture pack you are trying to load ({FolderUtil.texturepacksFolder}\\{cbxTexturepack.SelectedItem}) is outdated", "Warning");
                    MessageBox.Show("Warning: The texturepack that you are trying to load is outdated. This may lead to issues.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                wndMenu.selectedTexturepack = $"{FolderUtil.texturepacksFolder}\\{cbxTexturepack.SelectedItem}";
                Log.Write($"Successfully applied texturepack ({FolderUtil.texturepacksFolder}\\{cbxTexturepack.SelectedItem})", "Warning");
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
            Log.Show();
        }

        private void tbMoveRight_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = KeyConverter.KeyToString(e.Key);
            tbMoveRight.Text = keyText;
        }

        private void tbMoveLeft_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = KeyConverter.KeyToString(e.Key);
            tbMoveLeft.Text = keyText;
        }

        private void tbJump_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = KeyConverter.KeyToString(e.Key);
            tbJump.Text = keyText;
        }

        private void tbOpenInventory_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = KeyConverter.KeyToString(e.Key);
            tbOpenInventory.Text = keyText;
        }

        private void tbToggleDebugMenu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = KeyConverter.KeyToString(e.Key);
            tbToggleDebugMenu.Text = keyText;
        }

        private void tbShowNotificationList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Display key in textbox
            e.Handled = true;
            string keyText = KeyConverter.KeyToString(e.Key);
            tbShowNotificationList.Text = keyText;
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
    }
}
