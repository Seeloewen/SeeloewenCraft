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
            //Check and save the auto stepup setting
            if (cbEnableAutoStepup.IsChecked == true)
            {
                Properties.Settings.Default.enableAutoStepup = true;
            }
            else
            {
                Properties.Settings.Default.enableAutoStepup = false;
            }

            //Check and save the save on world exit settings
            if (cbSaveWorldWhenClosing.IsChecked == true)
            {
                Properties.Settings.Default.saveWorldOnClose = true;
            }
            else
            {
                Properties.Settings.Default.saveWorldOnClose = false;
            }


            //Save the selected texturepack
            Properties.Settings.Default.texturepack = cbxTexturepack.SelectedItem.ToString();

            //Save the settings
            Properties.Settings.Default.Save();
        }

        private void LoadSettings()
        {
            //Check and load the auto stepup setting
            if (Properties.Settings.Default.enableAutoStepup == true)
            {
                cbEnableAutoStepup.IsChecked = true;
            }
            else
            {
                cbEnableAutoStepup.IsChecked = false;
            }

            //Check and load the save on world exit setting
            if (Properties.Settings.Default.saveWorldOnClose == true)
            {
                cbSaveWorldWhenClosing.IsChecked = true;
            }
            else
            {
                cbSaveWorldWhenClosing.IsChecked = false;
            }

            //Load the texturepacks
            GetTexturepacks();
        }

        private void GetTexturepacks()
        {
            cbxTexturepack.Items.Clear();

            //Add entry for default texturepack
            cbxTexturepack.Items.Add("default");

            //Check if the texturepack directory exists, create it otherwise
            if (!Directory.Exists($"{wndMenu.gameDirectory}texturepacks"))
            {
                Directory.CreateDirectory($"{wndMenu.gameDirectory}texturepacks");
            }
            wndMenu.texturepackDirectory = $"{wndMenu.gameDirectory}texturepacks";

            //Get all texturepacks
            string[] directories = Directory.GetDirectories(wndMenu.texturepackDirectory);
            foreach (string directory in directories)
            {
                //Check if the texturepack has a pack file and add it to the list
                if (File.Exists($"{directory}/pack.txt"))
                {
                    cbxTexturepack.Items.Add(directory.Replace($"{wndMenu.texturepackDirectory}\\",""));
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
            if (File.Exists($"{wndMenu.selectedTexturepack}/pack.txt"))
            {
                string[] fileContent = File.ReadAllLines($"{wndMenu.selectedTexturepack}/pack.txt");

                if (fileContent.Length > 1)
                {
                    //Try to read the texturepack version
                    try
                    {
                        return Convert.ToInt32(fileContent[1].Replace("texturepackVersion=", ""));
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            return 0;
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
                if (GetTexturepackVersion($"{wndMenu.texturepackDirectory}/{cbxTexturepack.SelectedItem}") < wndMenu.texturepackVersion)
                {
                    MessageBox.Show("Warning: The texturepack that you are trying to load is outdated. This may lead to issues.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                wndMenu.selectedTexturepack = $"{wndMenu.texturepackDirectory}/{cbxTexturepack.SelectedItem}";
            }

            if(wndMenu.wndGame != null)
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
    }
}
