using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeeloewenCraft.game;
using SeeloewenCraft.game.core.events;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.util;
using SeeloewenCraft.game.util.logging;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using JsonToken = SeeloewenCraft.game.util.JsonToken;
using JsonWriter = SeeloewenCraft.game.util.JsonWriter;

namespace SeeloewenCraft.launcher
{
    internal class TexturepackDisplay : Canvas
    {
        internal readonly string id;

        private readonly TextBlock tblName = new TextBlock() { FontSize = 18, FontWeight = FontWeights.DemiBold };
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
        private wndMenu wndMenu;
        private ListBox focusedList;

        private ObservableCollection<TexturepackDisplay> disabledTexturepacks = new();
        private ObservableCollection<TexturepackDisplay> enabledTexturepacks = new();

        private bool texturepackChanged; //Used to determine whether to reload texture at the end 

        public wndSettings(wndMenu wndMenu, bool firstStart)
        {
            this.wndMenu = wndMenu;
            InitializeComponent();

            lbDisabled.ItemsSource = disabledTexturepacks;
            lbEnabled.ItemsSource = enabledTexturepacks;

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
            //Save the normal settings
            Settings.saveLogOnExit = Convert.ToBoolean(cbSaveLogOnExit.IsChecked);
            Settings.saveWorldOnClose = Convert.ToBoolean(cbSaveWorldWhenClosing.IsChecked);
            Settings.showNotifications = Convert.ToBoolean(cbShowNotifications.IsChecked);
            Settings.enableMobs = Convert.ToBoolean(cbEnableMobs.IsChecked);
            Settings.enableAutoSave = Convert.ToBoolean(cbAutoSave.IsChecked);
            Settings.showAutoSaveNotification = Convert.ToBoolean(cbAutoSaveNotification.IsChecked);
            Settings.resolution = Convert.ToString(cbxResolution.SelectedItem);
            Settings.videoMode = Convert.ToString(cbxMode.SelectedItem);
            Settings.customResX = Convert.ToInt32(tbWidth.Text);
            Settings.customResY = Convert.ToInt32(tbHeight.Text);
            Settings.autoSaveInterval = Convert.ToInt32(tbAutosave.Text);
            Settings.nickname = tbNickname.Text;

            //Save the log settings
            Settings.logEntities = Convert.ToBoolean(cbLogEntities.IsChecked);
            Settings.logGeneral = Convert.ToBoolean(cbLogGeneral.IsChecked);
            Settings.logNetwork = Convert.ToBoolean(cbLogNetwork.IsChecked);
            Settings.logRendering = Convert.ToBoolean(cbLogRendering.IsChecked);
            Settings.logStructureGeneration = Convert.ToBoolean(cbLogStructureGeneration.IsChecked);
            Settings.logWorldGeneration = Convert.ToBoolean(cbLogWorldGeneration.IsChecked);

            Log.Write($"Successfully saved settings", LogType.GENERAL, LogLevel.INFO);

            //Save the settings to file
            Settings.Save(writer);

            //Apply settings
            if (texturepackChanged) TextureManager.Load();
            GameEventHandler.Register(new AutoSaveEvent(Settings.autoSaveInterval * 60000));

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
                string packFile = $"{directory}\\pack.json";
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
            using (JsonWriter writer = JsonWriter.Create())
            {
                writer.Formatting = Formatting.Indented;
                SaveSettings(writer, false);
                writer.WriteToFile($"{FolderUtil.gameFolder}\\clientSettings.json");
            }

            Close();
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

        private void lb_GotFocus(object sender, RoutedEventArgs e) => focusedList = sender as ListBox;
    }
}
