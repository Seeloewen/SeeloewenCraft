using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Permissions;
using System.Text;
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


        //-- Constructor --//

        public wndSettings(wndMenu wndMenu)
        {
            this.wndMenu = wndMenu;
            InitializeComponent();
            LoadSettings();
        }

        //-- Custom Methods --//
        private void SaveSettings()
        {
            //Save the log save setting
            Properties.Settings.Default.saveLogOnExit = Convert.ToBoolean(cbSaveLogOnExit.IsChecked);
            wndMenu.log.Write($"Saved setting saveLogOnExit as {Properties.Settings.Default.saveLogOnExit}", "Info");

            //Save the save on world exit settings
            Properties.Settings.Default.saveWorldOnClose = Convert.ToBoolean(cbSaveWorldWhenClosing.IsChecked);
            wndMenu.log.Write($"Saved setting saveWorldOnClose as {Properties.Settings.Default.saveWorldOnClose}", "Info");

            //Save the enable hammer exit settings
            Properties.Settings.Default.enableHammer = Convert.ToBoolean(cbEnableHammer.IsChecked);
            wndMenu.log.Write($"Saved setting enableHammer as {Properties.Settings.Default.enableHammer}", "Info");

            //Save the enable cave generation exit settings
            Properties.Settings.Default.enableCaveGeneration = Convert.ToBoolean(cbEnableCaveGeneration.IsChecked);
            wndMenu.log.Write($"Saved setting enableCaveGeneration as {Properties.Settings.Default.enableCaveGeneration}", "Info");

            //Save the selected texturepack
            Properties.Settings.Default.texturepack = cbxTexturepack.SelectedItem.ToString();
            wndMenu.log.Write($"Saved setting texturepack as {Properties.Settings.Default.texturepack}", "Info");

            //Save the settings
            Properties.Settings.Default.Save();
        }

        private void LoadSettings()
        {
            //Load the log save setting
            cbSaveLogOnExit.IsChecked = Properties.Settings.Default.saveLogOnExit;

            //Load the save on world exit setting
            cbSaveWorldWhenClosing.IsChecked = Properties.Settings.Default.saveWorldOnClose;

            //Load the enable hammer setting
            cbEnableHammer.IsChecked = Properties.Settings.Default.enableHammer;

            //Load the enable cave generation exit setting
            cbEnableCaveGeneration.IsChecked = Properties.Settings.Default.enableCaveGeneration;

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
            if (cbxTexturepack.Items.Contains(Properties.Settings.Default.texturepack))
            {
                cbxTexturepack.SelectedItem = Properties.Settings.Default.texturepack;
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

            if (wndMenu.wndGame != null)
            {
                if (wndMenu.wndGame.finishedLoading)
                {
                    wndMenu.wndGame.RefreshTextures();
                }
            }
        }

        //-- Event Handlers --//

        private void btnSaveClose_Click(object sender, RoutedEventArgs e)
        {
            //Save the settings - WIP: Will get a rework later, currently resets when updating/reinstalling the app
            SaveSettings();
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
    }
}
